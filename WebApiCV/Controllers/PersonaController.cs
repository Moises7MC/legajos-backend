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
using WebApiCV.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApiCV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonaController : ControllerBase
    {
        private readonly PersonaRepository _repository;
        public PersonaController(PersonaRepository _repository)
        {
            this._repository = _repository;
        }

        [HttpGet("{pnTipo}", Name = "PersonaCombos")]
        [Authorize]
        public ActionResult<List<Persona>> Get(int pnTipo)
        {
            try
            {
                var lPersona = this._repository.GetPersonaByTipo(pnTipo);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                    (lPersona.Result.Count > 0 ? "Se ha encontrado " + lPersona.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), lPersona.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpGet("buscar_por_apellido/{cPerNombre}", Name = "BuscarPersonaPorApellidos")]
        public ActionResult<List<Persona>> Get(String cPerNombre)
        {
            try
            {
                var objs = this._repository.GetPersonaDatosXNombres(cPerNombre);
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
               (objs.Result.Count > 0 ? "Se ha encontrado " + objs.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), objs.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }


    }
}
