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
using WebApiCV.Contexts;
using WebApiCV.Entity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using WebApiCV.Data;

namespace WebApiCV.Controllers
{
    [Route("api/licenciaprofesional")]
    [ApiController]
    [Authorize]
    public class LegLicenciaProfesionalController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        FuncionesGenerales funcion;
        public LegLicenciaProfesionalController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }


        [HttpGet("{parnId}", Name = "ObtenerLicenciaProfesional")]
        public ActionResult<LegLicenciaProfesional> Get(Int64 parnId)
        {
            var obj = context.LegLicenciaProfesional
               .Select(x => new LegLicenciaProfesional
               {
                   NLegLicCodigo = x.NLegLicCodigo,
                   NLegLicPais = x.NLegLicPais,
                   NClasePais = x.NClasePais,
                   CLegLicInstitucion = x.CLegLicInstitucion,
                   CLegLicOtraInst = x.CLegLicOtraInst,
                   CLegLicNroRegistro = x.CLegLicNroRegistro,
                   NLegLicCondicion = x.NLegLicCondicion,
                   NClaseCondicion = x.NClaseCondicion,
                   DLegLicFechaEmision = x.DLegLicFechaEmision,
                   DLegLicFechaExpiracion = x.DLegLicFechaExpiracion,
                   NLegLicDatCodigo = x.NLegLicDatCodigo,
                   CLegLicValida = x.CLegLicValida,
                   CLegLicEstado = x.CLegLicEstado,
                   CUsuRegistro = x.CUsuRegistro,
                   DFechaRegistro = x.DFechaRegistro,
                   CUsuModifica = x.CUsuModifica,
                   DFechaModifica = x.DFechaModifica,
                   vCondicion = repositoryinterf.GetInterfaceDatos(x.NLegLicCondicion, x.NClaseCondicion).Result ?? null,
                   vPais = repositoryinterf.GetInterfaceDatos(x.NLegLicPais, x.NClasePais).Result ?? null,
                   CLegLicInstitucionNavigation = x.CLegLicInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegLicInstitucion).Result ?? null,
               })
                .FirstOrDefault(x => x.NLegLicCodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }


        [HttpGet("/api/licenciaprofesional_lst/{parnCodigoLegajo}", Name = "ListarLicenciaProfesional")]
        public ActionResult<IEnumerable<LegLicenciaProfesional>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosLicenciaProfesional(parnCodigoLegajo);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                (obj.Count > 0 ? "Se ha encontrado " + obj.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }

        [HttpPost("{pnCodigo}")]
        public ActionResult PostLicenciaProfesional(int pnCodigo, [FromForm] LegLicenciaProfesional datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == pnCodigo);
                              
                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegLicDatCodigo = pnCodigo;

                datos.CLegLicEstado = true;
                context.LegLicenciaProfesional.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerLicenciaProfesional", new { id = datos.NLegLicDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Docente LicenciaProfesional ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }


        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutLicenciaProfesional(int pnCodigo, [FromForm] LegLicenciaProfesional datos)
        {
            funcion.getPerCodigoToken(User);
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegLicCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {
                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objGT = context.LegLicenciaProfesional.AsNoTracking().FirstOrDefault(x => x.NLegLicCodigo == pnCodigo);
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegLicDatCodigo);

                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.NLegLicDatCodigo = objLG.NLegDatCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerLicenciaProfesional", new { id = datos.NLegLicDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchLicenciaProfesional(int pnCodigo, [FromBody] JsonPatchDocument<LegLicenciaProfesional> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegLicenciaProfesional.FirstOrDefault(x => x.NLegLicCodigo == pnCodigo);

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
                var res = new CreatedAtRouteResult("ObtenerLicenciaProfesional", new { id = obj.NLegLicCodigo }, obj);
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
