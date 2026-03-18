using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using WebApiCV.Contexts;
using WebApiCV.Data;
using WebApiCV.Entity;

namespace WebApiCV.Controllers
{
    //[Route("api/convocatoria")]
    [ApiController]
    // [Authorize]
    public class ConvocatoriaController : ControllerBase
    {
        private readonly bdLegajosContext context;

        private readonly ConvocatoriaRepository _repository;

        private readonly IConfiguration config;

        public ConvocatoriaController(
            bdLegajosContext context,
            IConfiguration configuration,
            ConvocatoriaRepository _repository
        )
        {
            this.context = context;
            this._repository = _repository;
            this.config = configuration;
        }

        [HttpGet("api/convocatoria/{pPrdCodigo}", Name = "CargarConvocatorias")]
        public ActionResult<List<Convocatoria>> Get(int pPrdCodigo)
        {
            try
            {
                var lConvocatoria =
                    this._repository.ObtenerListaConvocatoria(pPrdCodigo);
                return new JsonResult(new Mensajes(HttpContext
                            .Response
                            .StatusCode,
                        true,
                        (
                        lConvocatoria.Result.Count > 0
                            ? "Se ha encontrado " +
                            lConvocatoria.Result.Count.ToString() +
                            " registro(s)."
                            : "No se ha encontrado registros."
                        ),
                        lConvocatoria.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }
    }
}
