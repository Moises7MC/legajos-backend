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
    [Route("api/seleccion")]
    [ApiController]
    [Authorize]
    public class LegSeleccionController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        FuncionesGenerales funcion;
        public LegSeleccionController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }

        [HttpGet("{parnId}", Name = "ObtenerSeleccion")]
        public ActionResult<LegSeleccion> Get(Int64 parnId)
        {
            var obj = context.LegSeleccion
                .Select(x => new LegSeleccion
                {
                    NLegSelDatCodigo = x.NLegSelDatCodigo,
                    NLegSelCodigo = x.NLegSelCodigo,
                    NLegSelArea = x.NLegSelArea,
                    NLegValArea = x.NLegValArea,
                    NLegSelCargo = x.NLegSelCargo,
                    NLegValCargo = x.NLegValCargo,
                    DLegSelFecha = x.DLegSelFecha,
                    CLegSelEvaluacionCv = x.CLegSelEvaluacionCv,
                    CLegSelClaseModelo = x.CLegSelClaseModelo,
                    CLegSelEvaluacionPsico = x.CLegSelEvaluacionPsico,
                    CLegSelEntrevistaPers = x.CLegSelEntrevistaPers,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,
                })
                .FirstOrDefault(x => x.NLegSelCodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            obj.CLegSelEvaluacionCv = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegSelEvaluacionCv, true)));
            obj.CLegSelClaseModelo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegSelClaseModelo, true)));
            obj.CLegSelEvaluacionPsico = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegSelEvaluacionPsico, true)));
            obj.CLegSelEntrevistaPers = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegSelEntrevistaPers, true)));

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpGet("/api/seleccion_lst/{parnCodigoLegajo}", Name = "ListarSeleccion")]
        public ActionResult<IEnumerable<LegSeleccion>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosSeleccion(parnCodigoLegajo);

                if (obj.Count > 0)
                {
                    foreach (LegSeleccion x in obj)
                    {
                        x.CLegSelEvaluacionCv = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegSelEvaluacionCv, true)));
                        x.CLegSelClaseModelo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegSelClaseModelo, true)));
                        x.CLegSelEvaluacionPsico = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegSelEvaluacionPsico, true)));
                        x.CLegSelEntrevistaPers = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegSelEntrevistaPers, true)));
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
        public ActionResult PostSeleccion(int pnCodigo, [FromForm] LegSeleccion datos)
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
                if (datos.cFileEvaluacionCv != null)
                {
                    fileName = Path.GetFileName(datos.cFileEvaluacionCv.FileName);
                    extension = Path.GetExtension(datos.cFileEvaluacionCv.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoSelECV") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileEvaluacionCv.CopyTo(stream);
                    }

                    datos.CLegSelEvaluacionCv = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegSelEvaluacionCv = "";
                }

                if (datos.cFileClaseModelo != null)
                {
                    fileName = Path.GetFileName(datos.cFileClaseModelo.FileName);
                    extension = Path.GetExtension(datos.cFileClaseModelo.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoSelCM") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileClaseModelo.CopyTo(stream);
                    }

                    datos.CLegSelClaseModelo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegSelClaseModelo = "";
                }

                if (datos.cFileEvaluacionPsico != null)
                {
                    fileName = Path.GetFileName(datos.cFileEvaluacionPsico.FileName);
                    extension = Path.GetExtension(datos.cFileEvaluacionPsico.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoSelEPS") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileEvaluacionPsico.CopyTo(stream);
                    }

                    datos.CLegSelEvaluacionPsico = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegSelEvaluacionPsico = "";
                }

                if (datos.cFileEntrevistaPers != null)
                {
                    fileName = Path.GetFileName(datos.cFileEntrevistaPers.FileName);
                    extension = Path.GetExtension(datos.cFileEntrevistaPers.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoSelEPR") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileEntrevistaPers.CopyTo(stream);
                    }

                    datos.CLegSelEntrevistaPers = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegSelEntrevistaPers = "";
                }

                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegSelDatCodigo = pnCodigo;

                datos.BLegSelEstado = true;
                context.LegSeleccion.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerSeleccion", new { id = datos.NLegSelCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Selección ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }

        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutSeleccion(int pnCodigo, [FromForm] LegSeleccion datos)
        {
            funcion.getPerCodigoToken(User);
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegSelCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {
                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objGT = context.LegSeleccion.AsNoTracking().FirstOrDefault(x => x.NLegSelCodigo == pnCodigo);
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegSelDatCodigo);
                string path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc); //"LegajosUSS/" 
                string fileName = "";
                string extension = "";
                string ruteFile = "";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (datos.cFileEvaluacionCv != null)
                {
                    fileName = Path.GetFileName(datos.cFileEvaluacionCv.FileName);
                    extension = Path.GetExtension(datos.cFileEvaluacionCv.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoSelECV") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileEvaluacionCv.CopyTo(stream);
                    }

                    datos.CLegSelEvaluacionCv = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegSelEvaluacionCv = objGT.CLegSelEvaluacionCv;
                }

                if (datos.cFileClaseModelo != null)
                {
                    fileName = Path.GetFileName(datos.cFileClaseModelo.FileName);
                    extension = Path.GetExtension(datos.cFileClaseModelo.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoSelCM") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileClaseModelo.CopyTo(stream);
                    }

                    datos.CLegSelClaseModelo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegSelClaseModelo = objGT.CLegSelClaseModelo;
                }

                if (datos.cFileEvaluacionPsico != null)
                {
                    fileName = Path.GetFileName(datos.cFileEvaluacionPsico.FileName);
                    extension = Path.GetExtension(datos.cFileEvaluacionPsico.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoSelEPS") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileEvaluacionPsico.CopyTo(stream);
                    }

                    datos.CLegSelEvaluacionPsico = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegSelEvaluacionPsico = objGT.CLegSelEvaluacionPsico;
                }

                if (datos.cFileEntrevistaPers != null)
                {
                    fileName = Path.GetFileName(datos.cFileEntrevistaPers.FileName);
                    extension = Path.GetExtension(datos.cFileEntrevistaPers.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoSelEPR") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileEntrevistaPers.CopyTo(stream);
                    }

                    datos.CLegSelEntrevistaPers = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegSelEntrevistaPers = objGT.CLegSelEntrevistaPers;
                }

                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.NLegSelDatCodigo = objLG.NLegDatCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerSeleccion", new { id = datos.NLegSelCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchSeleccion(int pnCodigo, [FromBody] JsonPatchDocument<LegSeleccion> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegSeleccion.FirstOrDefault(x => x.NLegSelCodigo == pnCodigo);

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
                var res = new CreatedAtRouteResult("ObtenerSeleccion", new { id = obj.NLegSelCodigo }, obj);
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
