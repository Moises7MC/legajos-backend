using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using WebApiCV.Contexts;
using WebApiCV.Data;
using WebApiCV.Entity;

namespace WebApiCV.Controllers
{
    [Route("api/legarchivos")]
    [ApiController]
    [Authorize]
    public class LegArchivosController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        FuncionesGenerales funcion;

        public LegArchivosController(bdLegajosContext context, IConfiguration configuration, LegDatosGeneralesRepository _repository, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }


        [HttpGet("{arxnId}", Name = "ObtenerArchivo")]
        public ActionResult<LegArchivos> Get(int arxnId)
        {
            var obj = context.LegArchivos
                .FirstOrDefault(x => x.NLegArcCodigo == arxnId);
            if (obj == null)
            {
                return NotFound();
            }

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpGet("/api/legarchivos_lst/{cPerCodigo}", Name = "ListaArchivos")]
        public ActionResult<IEnumerable<LegArchivos>> Get(string cPerCodigo)
        {
            try
            {
                var obj = funcion.LegDatosArchivos(cPerCodigo);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                (obj.Count > 0 ? "Se ha encontrado " + obj.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPost]
        public ActionResult Post([FromForm] LegArchivos datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;

                context.LegArchivos.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerArchivo", new { id = datos.NLegArcCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Archivo de Legajo ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutLegArchivos(int pnCodigo, [FromForm] LegArchivos datos)
        {
            funcion.getPerCodigoToken(User);
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegArcCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {
                var objGT = context.LegArchivos.AsNoTracking().FirstOrDefault(x => x.NLegArcCodigo == pnCodigo);
            
                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.NLegArcCodigo = objGT.NLegArcCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerArchivo", new { id = datos.NLegArcCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }
    }
}
