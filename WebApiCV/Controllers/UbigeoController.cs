using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCV.Contexts;
using WebApiCV.Entity;
using Microsoft.AspNetCore.Authorization;

namespace WebApiCV.Controllers
{
    [Route("api/[controller]")]
    [ApiController][Authorize]
    public class UbigeoController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UbigeoController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ubigeo>> Get()
        {
            var obj = context.Ubigeo.ToList();
            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                (obj.Count > 0 ? "Se ha encontrado " + obj.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj));
        }
    }
}
