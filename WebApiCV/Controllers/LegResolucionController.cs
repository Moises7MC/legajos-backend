using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebApiCV.Contexts;
using WebApiCV.Entity;
using NETCore.Encrypt;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using WebApiCV.Data;

namespace WebApiCV.Controllers
{
    [Route("api/resolucion")]
    [ApiController]
    [Authorize]
    public class LegResolucionController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        FuncionesGenerales funcion;
        public LegResolucionController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }

        [HttpGet("{parnId}", Name = "ObtenerResolucion")]
        public ActionResult<LegResolucion> Get(Int64 parnId)
        {
            var obj = context.LegResolucion
                .Select(x => new LegResolucion
                {
                    NLegResDatCodigo = x.NLegResDatCodigo,
                    NLegResCodigo = x.NLegResCodigo,
                    NLegResTipo = x.NLegResTipo,
                    NLegValTipo = x.NLegValTipo,
                    CLegResArchivo = x.CLegResArchivo,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,
                    vResolucion = x.vResolucion
                })
                .FirstOrDefault(x => x.NLegResCodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            obj.CLegResArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegResArchivo, true)));

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpGet("/api/resolucion_lst/{parnCodigoLegajo}", Name = "ListarResolucion")]
        public ActionResult<IEnumerable<LegResolucion>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosResolucion(parnCodigoLegajo);

                if (obj.Count > 0)
                {
                    foreach (LegResolucion x in obj)
                    {
                        x.CLegResArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegResArchivo, true)));
                    }
                }

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                (obj.Count > 0 ? "Se ha encontrado " + obj.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }



        [HttpPost("{pnCodigo}")]
        public ActionResult PostResolucion(int pnCodigo, [FromForm] LegResolucion datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == pnCodigo);
                String directorio = this.config.GetValue<string>("ServerLegajos"); //D:/Proyectos/USS"

                string path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc); //"LegajosUSS/" 
                string fileName = "";
                string extension = "";
                string ruteFile = "";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                if (datos.cFile != null)
                {
                    fileName = Path.GetFileName(datos.cFile.FileName);
                    extension = Path.GetExtension(datos.cFile.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoResol") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegResArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegResArchivo = "";
                }
                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegResDatCodigo = pnCodigo;

                datos.BLegResEstado = true;
                context.LegResolucion.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerResolucion", new { id = datos.NLegResCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Resolución ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }

        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutResolucion(int pnCodigo, [FromForm] LegResolucion datos)
        {
            funcion.getPerCodigoToken(User);
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegResCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {
                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objGT = context.LegResolucion.AsNoTracking().FirstOrDefault(x => x.NLegResCodigo == pnCodigo);
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegResDatCodigo);
                string path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc); //"LegajosUSS/" 
                string fileName = "";
                string extension = "";
                string ruteFile = "";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (datos.cFile != null)
                {
                    fileName = Path.GetFileName(datos.cFile.FileName);
                    extension = Path.GetExtension(datos.cFile.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoResol") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegResArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegResArchivo = objGT.CLegResArchivo;
                }

                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.NLegResDatCodigo = objLG.NLegDatCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerResolucion", new { id = datos.NLegResCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchResolucion(int pnCodigo, [FromBody] JsonPatchDocument<LegResolucion> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegResolucion.FirstOrDefault(x => x.NLegResCodigo == pnCodigo);

                if (obj == null)
                {
                    var err = NotFound();
                    return new JsonResult(new Mensajes(err.StatusCode, false, "No existe datos", err));
                }
                obj.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                obj.CUsuModifica = funcion.cPerCodAux;
                datos.ApplyTo(obj, ModelState);
                context.Entry(obj).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var res = new CreatedAtRouteResult("ObtenerResolucion", new { id = obj.NLegResCodigo }, obj);
                return new JsonResult(new Mensajes(Ok().StatusCode, true, "Registro ha sido actualizado.", res.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }


    }
}