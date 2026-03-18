using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using NETCore.Encrypt;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebApiCV.Contexts;
using WebApiCV.Data;
using WebApiCV.Entity;

namespace WebApiCV.Controllers
{
    [Route("api/experiencianodoc")]
    [ApiController][Authorize]
    public class LegProfesNoDocenteController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        FuncionesGenerales funcion;
        public LegProfesNoDocenteController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }

        [HttpGet("{parnId}", Name = "ObtenerExperienciaNoDocente")]
        public ActionResult<LegProfesNoDocente> Get(Int64 parnId)
        {
            var obj = context.LegProfesNoDocente
                .Select(x => new LegProfesNoDocente
                {
                    NLegProCodigo = x.NLegProCodigo,
                    CLegProInstitucion = x.CLegProInstitucion ?? "",
                    CLegProPais = x.CLegProPais ?? "",
                    CLegProOtraInst = x.CLegProOtraInst ?? "",
                    NLegProCargo = x.NLegProCargo,
                    NValorCargo = x.NValorCargo,
                    CLegProCargoProf = x.CLegProCargoProf,
                    DLegProFechaInicio = x.DLegProFechaInicio,
                    DLegProFechaFin = x.DLegProFechaFin,
                    CLegProArchivo = x.CLegProArchivo,
                    NLegProDatCodigo = x.NLegProDatCodigo,
                    CLegProValida = x.CLegProValida,
                    CLegProEstado = x.CLegProEstado,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,
                    vCargo = repositorycons.GetConstanteDatos(x.NLegProCargo ?? 0, x.NValorCargo ?? 0).Result ?? null,
                    CLegProInstitucionNavigation = x.CLegProInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegProInstitucion).Result ?? null,
                })
                .FirstOrDefault(x => x.NLegProCodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            obj.CLegProArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegProArchivo, true)));

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpGet("/api/experiencianodoc_lst/{parnCodigoLegajo}", Name = "ListarExperienciaNoDocente")]
        public ActionResult<IEnumerable<LegProfesNoDocente>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosExperienciaNoDocente(parnCodigoLegajo);

                if (obj.Count > 0)
                {
                    foreach (LegProfesNoDocente x in obj)
                    {
                        x.CLegProArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegProArchivo, true)));
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
        public ActionResult PostNoDocencia(int pnCodigo, [FromForm] LegProfesNoDocente datos)
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

                    ruteFile = this.config.GetValue<string>("Legajo05") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;

                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))

                    {

                        datos.cFile.CopyTo(stream);

                    }



                    datos.CLegProArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;

                }

                else

                {

                    datos.CLegProArchivo = "";

                }

                datos.CUsuRegistro = funcion.cPerCodAux;

                datos.DFechaRegistro = DateTime.Now;

                datos.NLegProDatCodigo = pnCodigo;

                datos.NLegProCargo = datos.NLegProCargo == 0 ? null : datos.NLegProCargo;

                datos.CLegProEstado = true;

                context.LegProfesNoDocente.Add(datos);

                context.SaveChanges();

                transaction.Commit();

                var obj = new CreatedAtRouteResult("ObtenerExperienciaNoDocente", new { id = datos.NLegProDatCodigo }, datos);

                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Experiencia profesional ha sido registrado.", obj.Value));
            }

            catch (Exception ex)
            {
                transaction.Rollback();

                return new JsonResult(new Mensajes(400, false, ex.Message, ex));

            }
        }

        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutNoDocencia(int pnCodigo, [FromForm] LegProfesNoDocente datos)
        {
            funcion.getPerCodigoToken(User);

            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Modelo no válido", BadRequest(ModelState)));

            if (pnCodigo != datos.NLegProCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }

            using var transaction = context.Database.BeginTransaction();
            try
            {
                // 1. Obtener datos originales (AsNoTracking es clave para no chocar con el context.Entry después)
                var objGT = context.LegProfesNoDocente.AsNoTracking().FirstOrDefault(x => x.NLegProCodigo == pnCodigo);
                if (objGT == null) return new JsonResult(new Mensajes(404, false, "No se encontró el registro", null));

                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegProDatCodigo);

                string directorio = this.config.GetValue<string>("ServerLegajos");
                string path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc);

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                // 2. Manejo de Archivo
                if (datos.cFile != null)
                {
                    string extension = Path.GetExtension(datos.cFile.FileName);
                    string ruteFile = this.config.GetValue<string>("Legajo05") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;

                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        await datos.cFile.CopyToAsync(stream);
                    }

                    datos.CLegProArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegProArchivo = objGT.CLegProArchivo;
                }

                // 3. Rehidratación de campos obligatorios (Evita el error de NULL en SQL)
                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.CLegProValida = objGT.CLegProValida ?? false; // 🚩 Solución al error de tu captura
                datos.CLegProEstado = objGT.CLegProEstado;          // Mantiene el estado activo/inactivo
                datos.NLegProDatCodigo = objLG.NLegDatCodigo;

                // 4. Lógica de Cargos
                datos.NLegProCargo = (datos.NValorCargo == 0 || datos.NValorCargo == null) ? null : datos.NValorCargo;

                // 5. Auditoría de Modificación
                datos.DFechaModifica = DateTime.Now;
                datos.CUsuModifica = funcion.cPerCodAux;

                // 6. Guardado
                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();

                transaction.Commit();

                var obj = new CreatedAtRouteResult("ObtenerExperienciaNoDocente", new { id = datos.NLegProDatCodigo }, datos);
                return new JsonResult(new Mensajes(200, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex.InnerException?.Message));
            }
        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchExperienciaNoDoc(int pnCodigo, [FromBody] JsonPatchDocument<LegProfesNoDocente> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegProfesNoDocente.FirstOrDefault(x => x.NLegProCodigo == pnCodigo);

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
                var res = new CreatedAtRouteResult("ObtenerExperienciaNoDocente", new { id = obj.NLegProCodigo }, obj);
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
