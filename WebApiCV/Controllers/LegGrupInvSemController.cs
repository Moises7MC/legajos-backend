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
    [Route("api/leg_grup_inv_sem")]
    [ApiController]
    
    public class LegGrupInvSemController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly LegGrupInvSemRepository _repository;
        private readonly InterfaceRepository repositoryinterf;
        private readonly ConstanteRepository repositorycons;
        private readonly PersonaRepository repositorypersona;
        FuncionesGenerales funcion;

        public LegGrupInvSemController(
            bdLegajosContext context, 
            IConfiguration configuration, 
            ConstanteRepository repositorycons, 
            InterfaceRepository repositoryinterf,
            LegGrupInvSemRepository _repository,
            PersonaRepository repositorypersona
        )
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this._repository = _repository;
            this.funcion = new FuncionesGenerales(context,repositorycons,repositoryinterf,repositorypersona);
        }

        [HttpGet("list_leg_grup_inv")]
        public ActionResult<List<LegGrupInvSem>> GetListLegGrupInvSem(string cPcPerCodigo)

        {
            try
            {
                var objs = this._repository.GetLegGrupInvSem(cPcPerCodigo);

                Console.WriteLine(objs);
                if(objs.Result.Count > 0)
                {
                    foreach(LegGrupInvSem x in objs.Result)
                    {
                        x.nLegLidGrupInvSemArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.nLegLidGrupInvSemArchivo, true)));
                    }
                }


                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
               (objs.Result.Count > 0 ? "Se ha encontrado " + objs.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), objs.Result));
            }
            catch(Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpPost("registro_leg_grup_inv/{pnCodigo}")]
        [Authorize]
        public ActionResult RegistrarGrupoInvestigacion(String pnCodigo,[FromForm] LegGrupInvSem datos)
        {
            
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.cPerCodigo == pnCodigo);
                String directorio = this.config.GetValue<string>("ServerLegajos"); //D:/Proyectos/USS"
                Console.WriteLine('d');
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
                    ruteFile = this.config.GetValue<string>("Legajo06") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.nLegLidGrupInvSemArchivo= this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.nLegLidGrupInvSemArchivo = "";
                }
                datos.dFechaRegistro = Convert.ToDateTime(DateTime.Now);
                datos.dFechaModifica = Convert.ToDateTime(DateTime.Now);
                Console.WriteLine(datos);
                context.LegGrupInvSem.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerIdiomaOfimatica", new { id = datos.nLegLidGrupInvSem }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro Correcto", obj));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPatch("{nLegLidGrupInvSem}")]
        public async Task<ActionResult> PatchGrupoInvestigacion(int nLegLidGrupInvSem,[FromBody] JsonPatchDocument<LegGrupInvSem> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegGrupInvSem.FirstOrDefault(x=>x.nLegLidGrupInvSem == nLegLidGrupInvSem);
                if (obj==null)
                {
                    var err = NotFound();
                    return new JsonResult(new Mensajes(err.StatusCode, false, "No existe datos", err));
                }
                obj.dFechaModifica = Convert.ToDateTime(DateTime.Now);
                obj.cUsuModifica = funcion.cPerCodAux;
                datos.ApplyTo(obj, ModelState);
                context.Entry(obj).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var res = new CreatedAtRouteResult("LegGrupInvSem", new { id = obj.nLegLidGrupInvSem }, obj);
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
