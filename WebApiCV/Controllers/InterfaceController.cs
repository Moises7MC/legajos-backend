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
    //[Authorize]
    public class InterfaceController : ControllerBase
    {
        private readonly InterfaceRepository _repository;
        public InterfaceController(InterfaceRepository _repository)
        {
            this._repository = _repository;
        }

        [HttpGet("{pnTipo}", Name = "InterfaceCombos")]
        public ActionResult<List<Interface>> Get(int pnTipo)
        {
            try
            {
                var lInterface = this._repository.GetInterfaceByTipo(pnTipo);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                    (lInterface.Result.Count > 0 ? "Se ha encontrado " + lInterface.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), lInterface.Result));
            }
            catch (Exception ex)
            {
                
                return new JsonResult(new Mensajes(ex.Message));
            }
        }
        [HttpGet("{pnTipo}/{pnIntClase}", Name = "InterfaceCombosClass")]
        public ActionResult<Interface> Get(int pnTipo,int pnIntClase)
        {
            try
            {
                var obj = this._repository.GetInterfaceDatosPorTipoClase(pnTipo, pnIntClase);
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Datos Interface", obj.Result));

            }
            catch (Exception ex)
            {
                
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpGet("menu/{cPerUsuCodigo}",Name ="CargarMenu")]
        public ActionResult<List<Interface>> Get(string cPerUsuCodigo)
        {
            try
            {
                var obj = this._repository.GetInterfaceMenu(cPerUsuCodigo,1);
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Datos Interface", obj.Result));
            }
            catch(Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpGet("permisos_leg", Name = "ObtenerPermisosLeg")]
        public ActionResult<List<Interface>> Get()
        {
            try
            {
                var obj = this._repository.GetInterfacePermisos();
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Datos Interface", obj.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        //[HttpGet("obtener_acciones_x_usuario/{cPerUsuCodigo}", Name = "ObtenerAccionesPorUsuario")]
        //public ActionResult<List<Interface>> ObtenerAccionesPorPermiso(string cPerUsuCodigo)
        //{
        //    try
        //    {
        //        var obj = this._repository.GetInterfacePermisosAccionesXUsuario(cPerUsuCodigo);
        //        return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Datos Interface", obj.Result));
        //    }
        //    catch (Exception ex)
        //    {
        //        return new JsonResult(new Mensajes(ex.Message));
        //    }
        //}

    }
}
