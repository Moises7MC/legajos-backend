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
    [Route("api/capacitacion")]
    [ApiController][Authorize]
    public class LegCapacitacionesController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        private readonly LegDatosGeneralesRepository repositorydatosgral;
        FuncionesGenerales funcion;
        public LegCapacitacionesController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona, LegDatosGeneralesRepository repositorydatosgral)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }


        [HttpGet("{parnId}", Name = "ObtenerCapacitacion")]
        public ActionResult<LegCapacitaciones> Get(Int64 parnId)
        {
            var obj = context.LegCapacitaciones
                .Select(x => new LegCapacitaciones
                {
                    NLegCapCodigo = x.NLegCapCodigo,
                    NLegCapTipo = x.NLegCapTipo,
                    NLegCapTipoEsp = x.NLegCapTipoEsp,
                    CLegCapNombre = x.CLegCapNombre,
                    NLegCapHoras = x.NLegCapHoras,
                    DLegCapFechaInicio = x.DLegCapFechaInicio,
                    DLegCapFechaFin = x.DLegCapFechaFin,
                    CLegCapArchivo = x.CLegCapArchivo,
                    CLegCapInstitucion = x.CLegCapInstitucion ?? "",
                    CLegCapOtraInst = x.CLegCapOtraInst ?? "",
                    CLegCapPais = x.CLegCapPais ?? "",
                    NLegCapDatCodigo = x.NLegCapDatCodigo,
                    CLegCapValida = x.CLegCapValida,
                    CLegCapEstado = x.CLegCapEstado,
                    NValorTipo = x.NValorTipo,
                    NValorTipoEsp = x.NValorTipoEsp,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,
                    vInstitucion = x.CLegCapInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegCapInstitucion).Result ?? null,
                    vTipo = repositorycons.GetConstanteDatos(x.NLegCapTipo, x.NValorTipo).Result ?? null,
                    vTipoEsp = repositorycons.GetConstanteDatos(x.NLegCapTipoEsp, x.NValorTipoEsp).Result ?? null,
                })
                .FirstOrDefault(x => x.NLegCapCodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            obj.CLegCapArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegCapArchivo, true)));

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpGet("/api/capacitacion_lst/{parnCodigoLegajo}", Name = "ListarCapacitacion")]
        public ActionResult<IEnumerable<LegCapacitaciones>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosCapacitaciones(parnCodigoLegajo);

                if (obj.Count > 0)
                {
                    foreach (LegCapacitaciones x in obj)
                    {
                        x.CLegCapArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegCapArchivo, true)));
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
        public ActionResult PostCapacitacion(int pnCodigo, [FromForm] LegCapacitaciones datos)
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
                    ruteFile = this.config.GetValue<string>("Legajo13") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegCapArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegCapArchivo = "";
                }
                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegCapDatCodigo = pnCodigo;

                datos.CLegCapEstado = true;
                context.LegCapacitaciones.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerCapacitacion", new { id = datos.NLegCapDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Capacitación o diplomado ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }


        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutCapacitacion(int pnCodigo, [FromForm] LegCapacitaciones datos)
        {
            funcion.getPerCodigoToken(User);
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegCapCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {
                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objGT = context.LegCapacitaciones.AsNoTracking().FirstOrDefault(x => x.NLegCapCodigo == pnCodigo);
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegCapDatCodigo);
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
                    ruteFile = this.config.GetValue<string>("Legajo13") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegCapArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegCapArchivo = objGT.CLegCapArchivo;
                }

                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.NLegCapDatCodigo = objLG.NLegDatCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerCapacitacion", new { id = datos.NLegCapDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchCapacitacion(int pnCodigo, [FromBody] JsonPatchDocument<LegCapacitaciones> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegCapacitaciones.FirstOrDefault(x => x.NLegCapCodigo == pnCodigo);

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
                var res = new CreatedAtRouteResult("ObtenerCapacitacion", new { id = obj.NLegCapCodigo }, obj);
                return new JsonResult(new Mensajes(Ok().StatusCode, true, "Registro ha sido actualizado.", res.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

         [HttpPost("reg_masivo_cap")]
        public ActionResult PutCMCapacitacionInterna([FromForm] LegCapacitaciones datos)
        {
            for (var i = 0; i < datos.cFiles.Length; i++)
            {
                string fileName = "";
                string extension = "";
                string ruteFile = "";
                string idPersona = "";

                idPersona = Path.GetFileNameWithoutExtension(datos.cFiles[i].FileName);
                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.cPerCodigo == idPersona);
                string path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc); //"LegajosUSS/" 

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (datos.cFiles[i] != null)
                {
                    fileName = Path.GetFileName(datos.cFiles[i].FileName);
                    extension = Path.GetExtension(datos.cFiles[i].FileName);

                    ruteFile = this.config.GetValue<string>("Legajo10") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFiles[i].CopyTo(stream);
                    }

                    datos.CLegCapArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    //datos.CLegParArchivo = objGT.CLegParArchivo;                                                                                                                                        
                }
                
                //datos.CUsuRegistro = funcion.cPerCodAux != null ? funcion.cPerCodAux : null;
                datos.DFechaRegistro = Convert.ToDateTime(DateTime.Now);
                datos.NLegCapDatCodigo = objLG.NLegDatCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
;
                //datos.CUsuModifica = funcion.cPerCodAux != null ? funcion.cPerCodAux : null;
                //datos.cPerCodigo = idPersona != null ? idPersona : null;
                datos.CLegCapEstado = true;
                context.LegCapacitaciones.Add(datos);
                using var transaction = context.Database.BeginTransaction();
            try
            {

                var r = this.repositorydatosgral.cargaMasivaCapacitacion
                        (
                           datos.NLegCapTipo,
                            datos.NLegCapTipoEsp,
                            datos.CLegCapNombre,
                            datos.NLegCapHoras,
                            datos.DLegCapFechaInicio,
                            datos.DLegCapFechaFin,
                            datos.CLegCapArchivo,
                            datos.CLegCapInstitucion,
                            datos.NLegCapDatCodigo,
                            datos.CLegCapValida,
                            datos.CLegCapEstado,
                            datos.NValorTipo,
                            datos.NValorTipoEsp,
                            datos.CUsuRegistro,
                            datos.DFechaRegistro,
                            datos.CUsuModifica,
                            (DateTime)(datos.DFechaModifica),
                            datos.CLegCapOtraInst,
                            datos.CLegCapPais
                        );
                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
            }
            return new JsonResult(new Mensajes(200, true, "Registro Correcto.", null));

        }
    }
}
