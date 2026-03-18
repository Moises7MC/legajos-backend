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
    [ApiController]
    public class UnidadOrganizacionalsController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly LegDatosGeneralesRepository repository;
        public UnidadOrganizacionalsController(
            bdLegajosContext context,
            LegDatosGeneralesRepository repository
        )
        {
            this.context = context;
            this.repository = repository;
        }

        [HttpGet("/api/cargar_escuelas",Name ="Cargar Escuelas")]
        public ActionResult<List<UnidadOrganizacional>> GetActionCargar()
        {
            try
            {
                var objs = this.repository.GetCargarUnidadOrganizacional();
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                  (objs.Result.Count > 0 ? "Se ha encontrado " + objs.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), objs.Result));
            }catch(Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }
    }

}
