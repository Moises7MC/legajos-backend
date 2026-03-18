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
    [Route("api/tesisasejur")]
    [ApiController][Authorize]
    public class LegTesisAseJurController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        FuncionesGenerales funcion;
        public LegTesisAseJurController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }


        [HttpGet("{parnId}", Name = "ObtenerTesisAseJur")]
        public async Task<ActionResult<LegTesisAseJur>> Get(Int64 parnId)
        {
            try
            {
                var entity = await context.LegTesisAseJur
                    .FirstOrDefaultAsync(x => x.NLegTesCodigo == parnId);

                if (entity == null)
                {
                    return NotFound();
                }

                // Mapear a un nuevo objeto con manejo adecuado de nulos
                var obj = new LegTesisAseJur
                {
                    NLegTesCodigo = entity.NLegTesCodigo,
                    NLegTesTipo = entity.NLegTesTipo,
                    NValorTipo = entity.NValorTipo,
                    NLegTesNivel = entity.NLegTesNivel,
                    NValorNivel = entity.NValorNivel,
                    DLegTesFecha = entity.DLegTesFecha,
                    CLegTesNroResolucion = entity.CLegTesNroResolucion ?? string.Empty,
                    CLegTesArchivo = entity.CLegTesArchivo ?? string.Empty,
                    NLegTesDatCodigo = entity.NLegTesDatCodigo,
                    CLegTesValida = entity.CLegTesValida,
                    CLegTesEstado = entity.CLegTesEstado,
                    CUsuRegistro = entity.CUsuRegistro ?? string.Empty,
                    DFechaRegistro = entity.DFechaRegistro,
                    CUsuModifica = entity.CUsuModifica ?? string.Empty,
                    DFechaModifica = entity.DFechaModifica,

                    // Manejo seguro de navegaciones y relaciones
                    vTipo = entity.NLegTesTipo != 0 && entity.NValorTipo != 0
                        ? await repositoryinterf.GetInterfaceDatos(entity.NValorTipo, entity.NLegTesTipo)
                        : null,

                    vNivel = entity.NLegTesNivel != 0 && entity.NValorNivel != 0
                        ? await repositorycons.GetConstanteDatos(entity.NLegTesNivel, entity.NValorNivel)
                        : null,

                    // Nuevos campos con manejo de nulos - EBS 01/2026
                    NLegTesPais = entity.NLegTesPais,
                    NClasePais = entity.NClasePais,
                    CLegTesInstitucion = entity.CLegTesInstitucion ?? string.Empty,
                    CLegTesOtraInst = entity.CLegTesOtraInst ?? string.Empty,

                    vPais = entity.NLegTesPais.HasValue && entity.NClasePais.HasValue &&
                    entity.NLegTesPais.Value != 0 && entity.NClasePais.Value != 0
                    ? await repositoryinterf.GetInterfaceDatos(entity.NLegTesPais.Value, entity.NClasePais.Value)
                    : null,

                    CLegTesInstitucionNavigation = !string.IsNullOrEmpty(entity.CLegTesInstitucion)
                        ? await repositorypersona.GetPersonaDatos(entity.CLegTesInstitucion)
                        : null
                };

                // Manejar archivo nulo antes de encriptar
                if (!string.IsNullOrEmpty(obj.CLegTesArchivo))
                {
                    obj.CLegTesArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(
                        EncryptHelper.Encrypt(obj.CLegTesArchivo, true)));
                }
                else
                {
                    obj.CLegTesArchivo = string.Empty;
                }

                return new JsonResult(new Mensajes(StatusCodes.Status200OK, true, "", obj));
            }
            catch (Exception ex)
            {
                // Manejo de excepciones para nulos específicos
                if (ex is NullReferenceException || ex is InvalidOperationException)
                {
                    return new JsonResult(new Mensajes(400, false, "Uno o más valores requeridos son nulos en el registro.", ex));
                }

                return new JsonResult(new Mensajes(500, false, "Error interno del servidor al procesar la solicitud.", ex));
            }
        }

        [HttpGet("/api/tesisasejur_lst/{parnCodigoLegajo}", Name = "ListarTesisAseJur")]
        public ActionResult<IEnumerable<LegTesisAseJur>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosTesisAsJur(parnCodigoLegajo);

                if (obj.Count > 0)
                {
                    foreach (LegTesisAseJur x in obj)
                    {
                        x.CLegTesArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegTesArchivo, true)));
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
        public ActionResult PostTesisAseJur(int pnCodigo, [FromForm] LegTesisAseJur datos)
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
                    ruteFile = this.config.GetValue<string>("Legajo08") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegTesArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegTesArchivo = "";
                }
                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegTesDatCodigo = pnCodigo;

                datos.CLegTesEstado = true;
                context.LegTesisAseJur.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerTesisAseJur", new { id = datos.NLegTesDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Asesor o jurado de tesis ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }


        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutTesisAseJur(int pnCodigo, [FromForm] LegTesisAseJur datos)
        {
            funcion.getPerCodigoToken(User);
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegTesCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {
                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objGT = context.LegTesisAseJur.AsNoTracking().FirstOrDefault(x => x.NLegTesCodigo == pnCodigo);

                // Verificar si el registro existe
                if (objGT == null)
                {
                    return new JsonResult(new Mensajes(404, false, "Registro no encontrado", null));
                }

                // Manejar nulos en los nuevos campos
                datos.NLegTesPais = datos.NLegTesPais ?? objGT.NLegTesPais ?? 0;
                datos.NClasePais = datos.NClasePais ?? objGT.NClasePais ?? 0;
                datos.CLegTesInstitucion = datos.CLegTesInstitucion ?? objGT.CLegTesInstitucion ?? string.Empty;
                datos.CLegTesOtraInst = datos.CLegTesOtraInst ?? objGT.CLegTesOtraInst ?? string.Empty;

                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegTesDatCodigo);
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
                    ruteFile = this.config.GetValue<string>("Legajo08") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegTesArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegTesArchivo = objGT.CLegTesArchivo;
                }

                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.NLegTesDatCodigo = objLG.NLegDatCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerTesisAseJur", new { id = datos.NLegTesDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchTesisAseJur(int pnCodigo, [FromBody] JsonPatchDocument<LegTesisAseJur> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegTesisAseJur.FirstOrDefault(x => x.NLegTesCodigo == pnCodigo);

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
                var res = new CreatedAtRouteResult("ObtenerTesisAseJur", new { id = obj.NLegTesCodigo }, obj);
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

