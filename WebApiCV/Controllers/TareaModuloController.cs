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
    public class TareaModuloController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly TareaModuloRepository _repository;
        public TareaModuloController(TareaModuloRepository _repository, bdLegajosContext context, IConfiguration configuration)
        {
            this._repository = _repository;
            this.context = context;
            this.config = configuration;
        }
        [Authorize]
        [HttpGet("{pcPerCodigo}/{pnModCodigo}", Name = "ListaTareaModulo")]
        public ActionResult<List<TareaModulo>> Get(string pcPerCodigo, int pnModCodigo)
        {
            try
            {
                var lConstante = this._repository.GetTareasPermiso(pcPerCodigo, pnModCodigo);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                    (lConstante.Result.Count > 0 ? "Se ha encontrado " + lConstante.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), lConstante.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpGet("/api/tarea_modulo/listar_acciones/{pnModCodigo}", Name ="ListarAcciones")]
        //[Authorize]
        public ActionResult<List<TareaModulo>> ListarAcciones(int pnModCodigo)
        {
            try
            {
                var lConstante = this._repository.ListarTareasModulo(pnModCodigo);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                    (lConstante.Result.Count > 0 ? "Se ha encontrado " + lConstante.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), lConstante.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }


        [HttpGet("obtener_acciones_x_usuario/{cPerUsuCodigo}", Name = "ObtenerAccionesPorUsuario")]
        public ActionResult<List<TareaModulo>> ObtenerAccionesPorPermiso(string cPerUsuCodigo)
        {
            try
            {
                var obj = this._repository.GetTareaModuloPermisosAccionesXUsuario(cPerUsuCodigo);
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Datos Interface", obj.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }
    }
}
