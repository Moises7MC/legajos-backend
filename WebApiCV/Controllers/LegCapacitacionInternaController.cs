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
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class LegCapacitacionInternaController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        private readonly ReporteCapacitacionesRepository repositorycapacitaciones;
        FuncionesGenerales funcion;
        public LegCapacitacionInternaController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona, ReporteCapacitacionesRepository repositorycapacitaciones)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.repositorycapacitaciones = repositorycapacitaciones;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }

        [HttpGet("{parnId}", Name = "ObtenerCapacitacionInterna")]
        public ActionResult<LegCapacitacionInterna> Get(Int64 parnId)
        {
            var obj = context.LegCapacitacionInterna
                .Select(x => new LegCapacitacionInterna
                {
                    NLegDatCodigo = x.NLegDatCodigo,
                    NCapCodigo = x.NCapCodigo,
                    NLegCicodigo = x.NLegCicodigo,
                    CLegCicompetenciaMejora = x.CLegCicompetenciaMejora,
                    CLegCiarchivo = x.CLegCiarchivo,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,
                    vCapacitacionUSS = x.vCapacitacionUSS
                })
                .FirstOrDefault(x => x.NLegCicodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            obj.CLegCiarchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegCiarchivo, true)));

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }


        [HttpGet("/api/lst_capacitaciones/{pnPrdActividad}/{pnCapCodigo}", Name = "ListarCapacitaciones")]
        public ActionResult<IEnumerable<LegCapacitacionInterna>> GetList(int pnPrdActividad, int pnCapCodigo)
        {
            var obj = this.repositorycapacitaciones.GetCapacitaciones(pnPrdActividad, pnCapCodigo);

            if (obj == null)
            {
                return NotFound();
            }

            if (obj.Result.Count > 0)
            {
                foreach (ReporteCapacitaciones f in obj.Result)
                {
                    f.cLegCIArchivo = f.cLegCIArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.cLegCIArchivo, true))) : String.Empty;
                }
            }

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, (obj.Result.Count > 0 ? "Se ha encontrado " + obj.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj.Result));
        }

        [HttpGet("/api/capacitacioninterna_lst/{parnCodigoLegajo}", Name = "ListarCapacitacionInterna")]
        public ActionResult<IEnumerable<LegCapacitacionInterna>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosCapacitacionInterna(parnCodigoLegajo);

                if (obj.Count > 0)
                {
                    foreach (LegCapacitacionInterna x in obj)
                    {
                        //int codigo = x.NLegDatCodigo;
                        //x.vDatosGenerales = context.LegDatosGenerales.Select(y=>new LegDatosGenerales
                        //{
                        //    CLegDatNroDoc = y.CLegDatNroDoc,
                        //    CLegDatApellidoPaterno = y.CLegDatApellidoPaterno,
                        //    CLegDatApellidoMaterno = y.CLegDatApellidoMaterno,
                        //    CLegDatNombres = y.CLegDatNombres,
                        //    DLegDatFechaNacimiento = y.DLegDatFechaNacimiento,
                        //    vSexo = repositorycons.GetConstanteDatos(y.NLegDatSexo, y.NClaseSexo).Result,
                        //    vTipoDoc = repositoryinterf.GetInterfaceDatos(y.NLegDatTipoDoc, y.NClaseTipoDoc).Result,
                        //    vGradoAcad = repositoryinterf.GetInterfaceDatos(y.NLegDatGradoAcad, y.NClaseGradoAcad).Result,
                        //}).FirstOrDefault(y=>y.NLegDatCodigo == codigo);
                        x.CLegCiarchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegCiarchivo, true)));
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
        public ActionResult PostCapacitacionInterna(int pnCodigo, [FromForm] LegCapacitacionInterna datos)
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
                    ruteFile = this.config.GetValue<string>("LegajoCapInt") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegCiarchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegCiarchivo = "";
                }
                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegDatCodigo = pnCodigo;

                datos.BLegCiestado = true;
                context.LegCapacitacionInterna.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerCapacitacionInterna", new { id = datos.NLegCicodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Capacitación interna ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }

        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutCapacitacionInterna(int pnCodigo, [FromForm] LegCapacitacionInterna datos)
        {
            funcion.getPerCodigoToken(User);
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegCicodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {
                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objGT = context.LegCapacitacionInterna.AsNoTracking().FirstOrDefault(x => x.NLegCicodigo == pnCodigo);
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegDatCodigo);
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
                    ruteFile = this.config.GetValue<string>("LegajoCapInt") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegCiarchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegCiarchivo = objGT.CLegCiarchivo;
                }

                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.NLegDatCodigo = objLG.NLegDatCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerCapacitacionInterna", new { id = datos.NLegDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchCapacitacionInterna(int pnCodigo, [FromBody] JsonPatchDocument<LegCapacitacionInterna> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegCapacitacionInterna.FirstOrDefault(x => x.NLegCicodigo == pnCodigo);

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
                var res = new CreatedAtRouteResult("ObtenerCapacitacionInterna", new { id = obj.NLegCicodigo }, obj);
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