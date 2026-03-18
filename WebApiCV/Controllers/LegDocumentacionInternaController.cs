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
    [Route("api/documentacioninterna")]
    [ApiController]
    [Authorize]
    public class LegDocumentacionInternaController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        private readonly LegDatosGeneralesRepository repositorydatosgral;
        FuncionesGenerales funcion;
        public LegDocumentacionInternaController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona, LegDatosGeneralesRepository repositorydatosgral)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.repositorydatosgral = repositorydatosgral;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }

        [HttpGet("{parnId}", Name = "ObtenerDocumentacionInterna")]
        public ActionResult<LegDocumentacionInterna> Get(Int64 parnId)
        {
            var obj = context.LegDocumentacionInterna
                .Select(x => new LegDocumentacionInterna
                {
                    NLegDicodigo = x.NLegDicodigo,
                    NLegDidatCodigo = x.NLegDidatCodigo,
                    CLegDiarchivo = x.CLegDiarchivo,
                    CLegDicodigo = x.CLegDicodigo,
                    CLegDidescripcion = x.CLegDidescripcion,
                    NLegDitipoDoc = x.NLegDitipoDoc,
                    NLegValTipoDoc = x.NLegValTipoDoc,
                    BLegDiestado = x.BLegDiestado,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,
                })
                .FirstOrDefault(x => x.NLegDicodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            obj.CLegDiarchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDiarchivo, true)));

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpGet("/api/documentacioninterna_lst/{parnCodigoLegajo}", Name = "ListarDocumentacionInterna")]
        public ActionResult<IEnumerable<LegDocumentacionInterna>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosDocumentacionInterna(parnCodigoLegajo);

                if (obj.Count > 0)
                {
                    foreach (LegDocumentacionInterna x in obj)
                    {
                        x.CLegDiarchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegDiarchivo, true)));
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
        public async Task<ActionResult> PostDocumentacionInterna(int pnCodigo, [FromForm] LegDocumentacionInterna datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var decljur = context.LegDocumentacionInterna.FirstOrDefault(x => x.NLegDidatCodigo == pnCodigo && x.BLegDiestado == true);
                await this.repositorydatosgral.UpdateStateDJ(pnCodigo, funcion.cPerCodAux);
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
                    ruteFile = this.config.GetValue<string>("LegajoDocInt") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegDiarchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegDiarchivo = "";
                }

                

                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegDidatCodigo = pnCodigo;

                datos.BLegDiestado = true;
                //return new JsonResult(new Mensajes(400, false, "Declaraciones juradas han sido registradas.", datos));
                context.LegDocumentacionInterna.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerDocumentacionInterna", new { id = datos.NLegDicodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Documentación interna han sido registrada.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchDocumentacionInterna(int pnCodigo, [FromBody] JsonPatchDocument<LegDocumentacionInterna> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegDocumentacionInterna.FirstOrDefault(x => x.NLegDicodigo == pnCodigo);

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
                var res = new CreatedAtRouteResult("ObtenerDocumentacionInterna", new { id = obj.NLegDicodigo }, obj);
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
