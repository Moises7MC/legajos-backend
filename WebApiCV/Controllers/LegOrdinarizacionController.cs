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
    [Route("api/ordinarizacion")]
    [ApiController]
    [Authorize]
    public class LegOrdinarizacionController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        FuncionesGenerales funcion;
        public LegOrdinarizacionController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }

        [HttpGet("{parnId}", Name = "ObtenerOrdinarizacion")]
        public ActionResult<LegOrdinarizacion> Get(Int64 parnId)
        {
            var obj = context.LegOrdinarizacion
                .Select(x => new LegOrdinarizacion
                {
                    NLegOrdDatCodigo = x.NLegOrdDatCodigo,
                    NLegOrdCodigo = x.NLegOrdCodigo,
                    NLegOrdArea = x.NLegOrdArea,
                    NLegOrdValArea = x.NLegOrdValArea,
                    NLegOrdCargo = x.NLegOrdCargo,
                    NLegValCargo = x.NLegValCargo,
                    DLegOrdFecha = x.DLegOrdFecha,
                    CLegOrdFichaInscripcion = x.CLegOrdFichaInscripcion,
                    CLegOrdEvaluacionCv = x.CLegOrdEvaluacionCv,
                    CLegOrdClaseModelo = x.CLegOrdClaseModelo,
                    CLegOrdEvaluacionPsico = x.CLegOrdEvaluacionPsico,
                    CLegOrdEntrevistaPers = x.CLegOrdEntrevistaPers,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,
                })
                .FirstOrDefault(x => x.NLegOrdCodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            obj.CLegOrdFichaInscripcion = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegOrdFichaInscripcion, true)));
            obj.CLegOrdEvaluacionCv = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegOrdEvaluacionCv, true)));
            obj.CLegOrdClaseModelo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegOrdClaseModelo, true)));
            obj.CLegOrdEvaluacionPsico = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegOrdEvaluacionPsico, true)));
            obj.CLegOrdEntrevistaPers = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegOrdEntrevistaPers, true)));

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpGet("/api/ordinarizacion_lst/{parnCodigoLegajo}", Name = "ListarOrdinarizacion")]
        public ActionResult<IEnumerable<LegOrdinarizacion>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosOrdinarizacion(parnCodigoLegajo);

                if (obj.Count > 0)
                {
                    foreach (LegOrdinarizacion x in obj)
                    {
                        x.CLegOrdFichaInscripcion = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegOrdFichaInscripcion, true)));
                        x.CLegOrdEvaluacionCv = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegOrdEvaluacionCv, true)));
                        x.CLegOrdClaseModelo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegOrdClaseModelo, true)));
                        x.CLegOrdEvaluacionPsico = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegOrdEvaluacionPsico, true)));
                        x.CLegOrdEntrevistaPers = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegOrdEntrevistaPers, true)));
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
        public ActionResult PostOrdinarizacion(int pnCodigo, [FromForm] LegOrdinarizacion datos)
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

                if (datos.cFileFichaInscripcion != null)
                {
                    fileName = Path.GetFileName(datos.cFileFichaInscripcion.FileName);
                    extension = Path.GetExtension(datos.cFileFichaInscripcion.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoOrdFIN") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileFichaInscripcion.CopyTo(stream);
                    }

                    datos.CLegOrdFichaInscripcion = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegOrdFichaInscripcion = "";
                }

                if (datos.cFileEvaluacionCv != null)
                {
                    fileName = Path.GetFileName(datos.cFileEvaluacionCv.FileName);
                    extension = Path.GetExtension(datos.cFileEvaluacionCv.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoOrdECV") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileEvaluacionCv.CopyTo(stream);
                    }

                    datos.CLegOrdEvaluacionCv = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegOrdEvaluacionCv = "";
                }

                if (datos.cFileClaseModelo != null)
                {
                    fileName = Path.GetFileName(datos.cFileClaseModelo.FileName);
                    extension = Path.GetExtension(datos.cFileClaseModelo.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoOrdCM") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileClaseModelo.CopyTo(stream);
                    }

                    datos.CLegOrdClaseModelo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegOrdClaseModelo = "";
                }

                if (datos.cFileEvaluacionPsico != null)
                {
                    fileName = Path.GetFileName(datos.cFileEvaluacionPsico.FileName);
                    extension = Path.GetExtension(datos.cFileEvaluacionPsico.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoOrdEPS") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileEvaluacionPsico.CopyTo(stream);
                    }

                    datos.CLegOrdEvaluacionPsico = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegOrdEvaluacionPsico = "";
                }

                if (datos.cFileEntrevistaPers != null)
                {
                    fileName = Path.GetFileName(datos.cFileEntrevistaPers.FileName);
                    extension = Path.GetExtension(datos.cFileEntrevistaPers.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoOrdEPR") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileEntrevistaPers.CopyTo(stream);
                    }

                    datos.CLegOrdEntrevistaPers = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegOrdEntrevistaPers = "";
                }

                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegOrdDatCodigo = pnCodigo;

                datos.BLegOrdEstado = true;
                context.LegOrdinarizacion.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerOrdinarizacion", new { id = datos.NLegOrdCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Ordinarización ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }

        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutOrdinarizacion(int pnCodigo, [FromForm] LegOrdinarizacion datos)
        {
            funcion.getPerCodigoToken(User);
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegOrdCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {
                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objGT = context.LegOrdinarizacion.AsNoTracking().FirstOrDefault(x => x.NLegOrdCodigo == pnCodigo);
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegOrdDatCodigo);
                string path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc); //"LegajosUSS/" 
                string fileName = "";
                string extension = "";
                string ruteFile = "";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (datos.cFileFichaInscripcion != null)
                {
                    fileName = Path.GetFileName(datos.cFileFichaInscripcion.FileName);
                    extension = Path.GetExtension(datos.cFileFichaInscripcion.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoOrdFIN") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileFichaInscripcion.CopyTo(stream);
                    }

                    datos.CLegOrdFichaInscripcion = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegOrdFichaInscripcion = objGT.CLegOrdFichaInscripcion;
                }

                if (datos.cFileEvaluacionCv != null)
                {
                    fileName = Path.GetFileName(datos.cFileEvaluacionCv.FileName);
                    extension = Path.GetExtension(datos.cFileEvaluacionCv.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoOrdECV") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileEvaluacionCv.CopyTo(stream);
                    }

                    datos.CLegOrdEvaluacionCv = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegOrdEvaluacionCv = objGT.CLegOrdEvaluacionCv;
                }

                if (datos.cFileClaseModelo != null)
                {
                    fileName = Path.GetFileName(datos.cFileClaseModelo.FileName);
                    extension = Path.GetExtension(datos.cFileClaseModelo.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoOrdCM") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileClaseModelo.CopyTo(stream);
                    }

                    datos.CLegOrdClaseModelo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegOrdClaseModelo = objGT.CLegOrdClaseModelo;
                }

                if (datos.cFileEvaluacionPsico != null)
                {
                    fileName = Path.GetFileName(datos.cFileEvaluacionPsico.FileName);
                    extension = Path.GetExtension(datos.cFileEvaluacionPsico.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoOrdEPS") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileEvaluacionPsico.CopyTo(stream);
                    }

                    datos.CLegOrdEvaluacionPsico = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegOrdEvaluacionPsico = objGT.CLegOrdEvaluacionPsico;
                }

                if (datos.cFileEntrevistaPers != null)
                {
                    fileName = Path.GetFileName(datos.cFileEntrevistaPers.FileName);
                    extension = Path.GetExtension(datos.cFileEntrevistaPers.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoOrdEPR") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileEntrevistaPers.CopyTo(stream);
                    }

                    datos.CLegOrdEntrevistaPers = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegOrdEntrevistaPers = objGT.CLegOrdEntrevistaPers;
                }

                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.NLegOrdDatCodigo = objLG.NLegDatCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerOrdinarizacion", new { id = datos.NLegOrdCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchOrdinarizacion(int pnCodigo, [FromBody] JsonPatchDocument<LegOrdinarizacion> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegOrdinarizacion.FirstOrDefault(x => x.NLegOrdCodigo == pnCodigo);

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
                var res = new CreatedAtRouteResult("ObtenerOrdinarizacion", new { id = obj.NLegOrdCodigo }, obj);
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
