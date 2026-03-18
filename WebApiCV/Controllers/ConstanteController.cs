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
    [ApiController][Authorize]
    public class ConstanteController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository _repository;
        public ConstanteController(ConstanteRepository _repository, bdLegajosContext context, IConfiguration configuration)
        {
            this._repository = _repository;
            this.context = context;
            this.config = configuration;
        }

        [HttpGet("{pnTipo}", Name = "ConstanteCombos")]
        public ActionResult<List<Constante>> Get(int pnTipo)
        {
            try
            {
                var lConstante = this._repository.GetConstanteByTipo(pnTipo);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                    (lConstante.Result.Count > 0 ? "Se ha encontrado " + lConstante.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), lConstante.Result));
            }catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpGet("/api/modulos/{pcPerCodigo}")]
        public ActionResult<List<Constante>> GetModulos(String pcPerCodigo)
        {
            try
            {
                var obj = this.context.
                //    Constantes.Where(x => x.NConCodigo == 4026  && x.NConValor != 0).Select(x => new Constante
                //{
                //    NConCodigo = x.NConCodigo,
                //    NConValor = x.NConValor,
                //    CConDescripcion = x.CConDescripcion,
                //    ModulosTD = (ICollection<ModulosTD>)x.ModulosTD.Where(x => x.BModEstado == true)
                //    .Select(x => new ModulosTD
                //    {
                //        NModCodigo = x.NModCodigo,
                //        NConValor = x.NConValor,
                //        NConCodigo = x.NConCodigo,
                //        CModDescripcion = x.CModDescripcion,
                //        CModRuta = x.CModRuta,
                //        BModEstado = x.BModEstado,
                //        CUsuRegistro = x.CUsuRegistro,
                //        DFechaRegistro = x.DFechaRegistro,
                //        CUsuModifica = x.CUsuModifica,
                //        DFechaModifica = x.DFechaModifica,
                //        vPermisosModulos = (ICollection<PermisosModulo>)x.
                        PermisosModulo.Where(x => x.BPrmModEstado == true && x.CPerCodigo == pcPerCodigo)
                         .Select(x => new PermisosModulo
                         {
                             NPrmModCodigo = x.NPrmModCodigo,
                             NModCodigo = x.NModCodigo,
                             CPerCodigo = x.CPerCodigo,
                             BPrmModAdministrador = x.BPrmModAdministrador,
                             BPrmModAlcance = x.BPrmModAlcance,
                             BPrmModEstado = x.BPrmModEstado,
                             CUsuRegistro = x.CUsuRegistro,
                             DFechaRegistro = x.DFechaRegistro,
                             CUsuModifica = x.CUsuModifica,
                             DFechaModifica = x.DFechaModifica,
                             vModulos = new ModulosTD(x.vModulos.NModCodigo, x.vModulos.CModDescripcion, x.vModulos.vModulo),
                             
                             vPermisosTareas = (ICollection<PermisosTarea>)x.vPermisosTareas.Where(x => x.BPrmEstado == true)
                             .Select(x => new PermisosTarea
                             {
                                 NPrmTarCodigo = x.NPrmTarCodigo,
                                 NPrmModCodigo = x.NPrmModCodigo,
                                 NTarModCodigo = x.NTarModCodigo,
                                 vTareaModulo = x.vTareaModulo
                             })
                    //      })
                    //}),
                }).ToList();

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                    (obj.Count > 0 ? "Se ha encontrado " + obj.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpGet("/api/get_idioma", Name = "ConstanteCombosIdiomas")]
        public ActionResult<List<Constante>> GetDatosIdioma()
        {
            try
            {
                var lConstante = this._repository.GetConstanteDatosGetByNConCodigoNConValor(1109, 10000);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                    (lConstante.Result.Count > 0 ? "Se ha encontrado " + lConstante.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), lConstante.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }
    }
}
