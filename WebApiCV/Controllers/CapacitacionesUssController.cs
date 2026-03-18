using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCV.Contexts;
using WebApiCV.Data;
using WebApiCV.Entity;

namespace WebApiCV.Controllers
{
    [Route("api/[controller]")]
    [ApiController][Authorize]
    public class CapacitacionesUssController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        FuncionesGenerales funcion;
        public CapacitacionesUssController(bdLegajosContext context, LegDatosGeneralesRepository _repository, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
        {
            this.context = context;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }
        [HttpGet]
        public ActionResult<IEnumerable<CapacitacionesUss>> Get()
        {
            var obj = context.CapacitacionesUss.Where(x => x.BCapEstado == true).ToList();
            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                (obj.Count > 0 ? "Se ha encontrado " + obj.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj));
        }

        [HttpGet("{parnId}", Name = "ObtenerCapacitacionUSS")]
        public ActionResult<CapacitacionesUss> Get(Int64 parnId)
        {
            var obj = context.CapacitacionesUss
                .FirstOrDefault(x => x.NCapCodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpPost]
        public ActionResult Post([FromForm] CapacitacionesUss datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {                
                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;

                context.CapacitacionesUss.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerCapacitacionUSS", new { id = datos.NCapCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Capacitación USS ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }

    }
}
