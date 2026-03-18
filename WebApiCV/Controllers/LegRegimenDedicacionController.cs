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
    [Route("api/regimendedicacion")]
    [ApiController][Authorize]
    public class LegRegimenDedicacionController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        FuncionesGenerales funcion;
        public LegRegimenDedicacionController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
        {
            this.context = context;
            this.config = configuration;
             this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
    }


        [HttpGet("{parnId}", Name = "ObtenerRegimenDedica")]
        public ActionResult<LegRegimenDedicacion> Get(Int64 parnId)
        {
            var obj = context.LegRegimenDedicacion
                .Select(x => new LegRegimenDedicacion
                {
                    NLegRegCodigo = x.NLegRegCodigo,
                    CLegCatInstitucion = x.CLegCatInstitucion ?? "",
                    CLegRegOtraInst = x.CLegRegOtraInst ?? "",
                    CLegRegPais = x.CLegRegPais ?? "",
                    NLegRegDedicacion = x.NLegRegDedicacion,
                    NValorDedicacion = x.NValorDedicacion,
                    DLegRegFechaInicio = x.DLegRegFechaInicio,
                    DLegRegFechaFin = x.DLegRegFechaFin,
                    CLegRegArchivo = x.CLegRegArchivo,
                    NLegRegDatCodigo = x.NLegRegDatCodigo,
                    CLegRegValida = x.CLegRegValida,
                    CLegRegEstado = x.CLegRegEstado,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,
                    vDedicacion = repositorycons.GetConstanteDatos(x.NLegRegDedicacion, x.NValorDedicacion).Result ?? null,
                    CLegCatInstitucionNavigation = x.CLegCatInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegCatInstitucion).Result ?? null,
                })
                .FirstOrDefault(x => x.NLegRegCodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            obj.CLegRegArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegRegArchivo, true)));

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpGet("/api/regimendedicacion_lst/{parnCodigoLegajo}", Name = "ListarRegimenDedica")]
        public ActionResult<IEnumerable<LegRegimenDedicacion>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosRegimenDedicacion(parnCodigoLegajo);

                if (obj.Count > 0)
                {
                    foreach (LegRegimenDedicacion x in obj)
                    {
                        x.CLegRegArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegRegArchivo, true)));
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
        public ActionResult PostRegimenDedicacion(int pnCodigo, [FromForm] LegRegimenDedicacion datos)
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
                //return new JsonResult(new Mensajes(200, true, "Currículo ha sido registrado.", curriculo.cCurFile));
                if (datos.cFile != null)
                {
                    fileName = Path.GetFileName(datos.cFile.FileName);
                    extension = Path.GetExtension(datos.cFile.FileName);
                    ruteFile = this.config.GetValue<string>("Legajo04") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegRegArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegRegArchivo = "";
                }
                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegRegDatCodigo = pnCodigo;

                datos.CLegRegEstado = true;
                context.LegRegimenDedicacion.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerRegimenDedica", new { id = datos.NLegRegDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Régimen dedicación ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }


        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutRegimenDedicacion(int pnCodigo, [FromForm] LegRegimenDedicacion datos)
        {
            funcion.getPerCodigoToken(User);
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegRegCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {
                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objGT = context.LegRegimenDedicacion.AsNoTracking().FirstOrDefault(x => x.NLegRegCodigo == pnCodigo);
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegRegDatCodigo);
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
                    ruteFile = this.config.GetValue<string>("Legajo04") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegRegArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegRegArchivo = objGT.CLegRegArchivo;
                }

                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.NLegRegDatCodigo = objLG.NLegDatCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerRegimenDedica", new { id = datos.NLegRegDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchRegimenDedica(int pnCodigo, [FromBody] JsonPatchDocument<LegRegimenDedicacion> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegRegimenDedicacion.FirstOrDefault(x => x.NLegRegCodigo == pnCodigo);

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
                var res = new CreatedAtRouteResult("ObtenerRegimenDedica", new { id = obj.NLegRegCodigo }, obj);
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
