using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCV.Contexts;
using WebApiCV.Entity;

namespace WebApiCV.Controllers
{
    [Route("api/[controller]")]
    [ApiController][Authorize]
    public class GradoAcademicoController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public GradoAcademicoController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<GradoAcademico>> Get()
        {
            var obj = context.GradoAcademico.ToList();
            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                (obj.Count > 0 ? "Se ha encontrado " + obj.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj));
        }
    }
}
