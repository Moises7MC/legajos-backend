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
    [Route("api/participacion")]
    [ApiController][Authorize]
    public class LegParticipacionCongSemController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        private readonly LegDatosGeneralesRepository legDatosGeneralesRepository;
        FuncionesGenerales funcion;
        public LegParticipacionCongSemController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona,LegDatosGeneralesRepository legDatosGeneralesRepository)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.legDatosGeneralesRepository = legDatosGeneralesRepository;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }


        [HttpGet("{parnId}", Name = "ObtenerParticipacion")]
        public ActionResult<LegParticipacionCongSem> Get(Int64 parnId)
        {
            var obj = context.LegParticipacionCongSem
                .Select(x => new LegParticipacionCongSem
                {
                    NLegParCodigo = x.NLegParCodigo,
                    CLegParInstitucion = x.CLegParInstitucion ?? "",
                    CLegParOtraInst = x.CLegParOtraInst ?? "",
                    CLegParPais = x.CLegParPais ?? "",
                    NLegParRol = x.NLegParRol,
                    NValorRol = x.NValorRol,
                    NLegParAmbito = x.NLegParAmbito,
                    NValorAmbito = x.NValorAmbito,
                    CLegParNombre = x.CLegParNombre,
                    DLegParFecha = x.DLegParFecha,

                    DLegParFechaFin = x.DLegParFechaFin,
                    NLegParHoras = x.NLegParHoras,

                    CLegParArchivo = x.CLegParArchivo,
                    NLegParDatCodigo = x.NLegParDatCodigo,
                    CLegParValida = x.CLegParValida,
                    CLegParEstado = x.CLegParEstado,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,
                    vAmbito = repositoryinterf.GetInterfaceDatos(x.NLegParAmbito, x.NValorAmbito).Result ?? null,
                    vRol = repositoryinterf.GetInterfaceDatos(x.NLegParRol, x.NValorRol).Result ?? null,
                    CLegParInstitucionNavigation = x.CLegParInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegParInstitucion).Result ?? null
                })
                .FirstOrDefault(x => x.NLegParCodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            obj.CLegParArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegParArchivo, true)));

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpGet("/api/participacion_lst/{parnCodigoLegajo}", Name = "ListarParticipacion")]
        public ActionResult<IEnumerable<LegParticipacionCongSem>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosParticipacion(parnCodigoLegajo);

                if (obj.Count > 0)
                {
                    foreach (LegParticipacionCongSem x in obj)
                    {
                        x.CLegParArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegParArchivo, true)));
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
        public ActionResult PostParticipacion(int pnCodigo, [FromForm] LegParticipacionCongSem datos)
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
                    ruteFile = this.config.GetValue<string>("Legajo10") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }
                    datos.CLegParArchivoOrig = fileName;
                    datos.CLegParArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegParArchivo = "";
                }
                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegParDatCodigo = pnCodigo;

                datos.CLegParEstado = true;
                context.LegParticipacionCongSem.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerParticipacion", new { id = datos.NLegParDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Partipación en eventos ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }


        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutParticipacion(int pnCodigo, [FromForm] LegParticipacionCongSem datos)
        {
            funcion.getPerCodigoToken(User);
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegParCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {
                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objGT = context.LegParticipacionCongSem.AsNoTracking().FirstOrDefault(x => x.NLegParCodigo == pnCodigo);
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegParDatCodigo);
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
                    ruteFile = this.config.GetValue<string>("Legajo10") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }
                   
                    datos.CLegParArchivoOrig = fileName;
                   
                    
                    datos.CLegParArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegParArchivo = objGT.CLegParArchivo;
                    datos.CLegParArchivoOrig = objGT.CLegParArchivoOrig;
                }

                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.NLegParDatCodigo = objLG.NLegDatCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerParticipacion", new { id = datos.NLegParDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchParticipacion(int pnCodigo, [FromBody] JsonPatchDocument<LegParticipacionCongSem> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegParticipacionCongSem.FirstOrDefault(x => x.NLegParCodigo == pnCodigo);

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
                var res = new CreatedAtRouteResult("ObtenerParticipacion", new { id = obj.NLegParCodigo }, obj);
                return new JsonResult(new Mensajes(Ok().StatusCode, true, "Registro ha sido actualizado.", res.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }
    

    /*ANALISTA: JZ*/
    [HttpPatch("registrarCargaMasiva")]
    public ActionResult registrarCargaMasiva([FromForm] LegParticipacionCongSem datos)
    {
        
        funcion.getPerCodigoToken(User);



        for (int i = 0; i < datos.cFiles.Length; i++)
        {
            string fileName = "";
            string extension = "";
            string ruteFile = "";
            string idPersona = "";
            idPersona = Path.GetFileNameWithoutExtension(datos.cFiles[i].FileName);
            String directorio = this.config.GetValue<string>("ServerLegajos");
            //var objGT = context.LegParticipacionCongSem.AsNoTracking().FirstOrDefault(x => x.NLegParCodigo == 1213);
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

                datos.CLegParArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
            }
            else
            {
                //datos.CLegParArchivo = objGT.CLegParArchivo;                                                                                                                                        
            }

            datos.CUsuRegistro = funcion.cPerCodAux != null ? funcion.cPerCodAux : null;
            datos.DFechaRegistro = Convert.ToDateTime(DateTime.Now);
            datos.NLegParDatCodigo = objLG.NLegDatCodigo;
            datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
            datos.CUsuModifica = funcion.cPerCodAux != null ? funcion.cPerCodAux : null;
            datos.cPerCodigo = idPersona != null ? idPersona : null;
            datos.CLegParEstado = true;
            context.LegParticipacionCongSem.Add(datos);
            using var transaction = context.Database.BeginTransaction();
            try
            {

                var r = this.legDatosGeneralesRepository.cargaMasivaLegParticipacionCongSem
                        (
                           datos.CLegParInstitucion,
                            datos.NLegParRol,
                            datos.NValorRol,
                            datos.NLegParAmbito,
                            datos.NValorAmbito,
                            datos.CLegParNombre,
                            datos.DLegParFecha,
                            datos.CLegParArchivo,
                            datos.NLegParDatCodigo,
                            datos.CLegParEstado,
                            datos.CUsuRegistro,
                            datos.DFechaRegistro,
                            datos.CUsuModifica,
                            datos.DFechaModifica,
                            datos.CLegParOtraInst,
                            datos.CLegParPais,
                            datos.DLegParFechaFin,
                            datos.NLegParHoras,
                            datos.cPerCodigo
                        );
                Console.WriteLine(r);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }

        return new JsonResult(new Mensajes(400, true, "Registro correcto.", null));
    }

}
}