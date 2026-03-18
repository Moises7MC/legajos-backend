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
    [Route("api/membresia")]
    [ApiController]
    [Authorize]
    public class LegMembresiaController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        FuncionesGenerales funcion;
        public LegMembresiaController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }


        [HttpGet("{parnId}", Name = "ObtenerMembresia")]
        public ActionResult<LegMembresia> Get(Int64 parnId)
        {
            var obj = context.LegMembresia
               .Select(x => new LegMembresia
               {
                   NLegMemCodigo = x.NLegMemCodigo,
                   NLegMemPais = x.NLegMemPais,
                   NClasePais = x.NClasePais,
                   CLegMemInstitucion = x.CLegMemInstitucion,
                   CLegMemOtraInst = x.CLegMemOtraInst,
                   CLegMemNroRegistro = x.CLegMemNroRegistro,
                   DLegMemFechaEmision = x.DLegMemFechaEmision,
                   DLegMemFechaExpiracion = x.DLegMemFechaExpiracion,
                   NLegMemDatCodigo = x.NLegMemDatCodigo,
                   CLegMemValida = x.CLegMemValida,
                   CLegMemEstado = x.CLegMemEstado,
                   CUsuRegistro = x.CUsuRegistro,
                   DFechaRegistro = x.DFechaRegistro,
                   CUsuModifica = x.CUsuModifica,
                   DFechaModifica = x.DFechaModifica,
                   vPais = repositoryinterf.GetInterfaceDatos(x.NLegMemPais, x.NClasePais).Result ?? null,
                   CLegMemInstitucionNavigation = x.CLegMemInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegMemInstitucion).Result ?? null,
               })
                .FirstOrDefault(x => x.NLegMemCodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }


        [HttpGet("/api/membresia_lst/{parnCodigoLegajo}", Name = "ListarMembresia")]
        public ActionResult<IEnumerable<LegMembresia>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosMembresia(parnCodigoLegajo);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                (obj.Count > 0 ? "Se ha encontrado " + obj.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }

        [HttpPost("{pnCodigo}")]
        public ActionResult PostMembresia(int pnCodigo, [FromForm] LegMembresia datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == pnCodigo);

                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegMemDatCodigo = pnCodigo;

                datos.CLegMemEstado = true;
                context.LegMembresia.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerMembresia", new { id = datos.NLegMemDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Docente Membresia ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }


        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutMembresia(int pnCodigo, [FromForm] LegMembresia datos)
        {
            funcion.getPerCodigoToken(User);
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegMemCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {
                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objGT = context.LegMembresia.AsNoTracking().FirstOrDefault(x => x.NLegMemCodigo == pnCodigo);
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegMemDatCodigo);

                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.NLegMemDatCodigo = objLG.NLegDatCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerMembresia", new { id = datos.NLegMemDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchMembresia(int pnCodigo, [FromBody] JsonPatchDocument<LegMembresia> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegMembresia.FirstOrDefault(x => x.NLegMemCodigo == pnCodigo);

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
                var res = new CreatedAtRouteResult("ObtenerMembresia", new { id = obj.NLegMemCodigo }, obj);
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
