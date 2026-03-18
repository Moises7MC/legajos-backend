using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using WebApiCV.Entity;
using WebApiCV.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
//using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using WebApiCV.Contexts;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using WebApiCV.TemplatePDF;
using iTextSharp.text.pdf;
using iTextSharp.text;
namespace WebApiCV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DatosUsuarioController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly DatosUsuarioRepository _repository;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        //private readonly PerUsuarioRepository repositoryperusuario;
        private readonly ReporteLegajosRepository repositorylegajos;
        private readonly IConfiguration config;
        FuncionesGenerales funcion;

        public DatosUsuarioController(
            DatosUsuarioRepository _repository,
            IConfiguration configuration,
            bdLegajosContext context,
            ConstanteRepository repositorycons,
            InterfaceRepository repositoryinterf,
            PersonaRepository repositorypersona,
            ReporteLegajosRepository repositorylegajos
            //PerUsuarioRepository repositoryperusuario
            )
        {
            this.context = context;
            this._repository = _repository;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.repositorylegajos = repositorylegajos;
            //this.repositoryperusuario = repositoryperusuario;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }


        [HttpGet("{pcCodigo}", Name = "GetDatosUsuario")]
        [Authorize]
        public ActionResult<DatosUsuario> Get(string pcCodigo)
        {
            try
            {
                var eDatosUsuario = this._repository.GetDatosUsuario(pcCodigo);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Datos de usuario" , eDatosUsuario.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }


        [HttpGet("/api/listausuarios/{nTipo}/{cPrdNombre}", Name = "GetListaUsuarios")]
        [Authorize]
        public ActionResult<List<DatosUsuario>> GetListPersona(int nTipo, string cPrdNombre)
        {
            try
            {
                var obj = this._repository.GetLista_Legajos_by_Periodo(nTipo, cPrdNombre);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                (obj.Result.Count > 0 ? "Se ha encontrado " + obj.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj.Result));
               
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpPost("/api/login/{pcPerUsuCodigo}/{pcPerUsuClave}", Name = "Login")]
        public ActionResult<DatosUsuario> PostLogin(string pcPerUsuCodigo, string pcPerUsuClave)
        {
            try
            {
                DatosUsuario datosusuario = null;
                var passbase64EncodedBytes = System.Convert.FromBase64String(pcPerUsuClave);                
                var eDatosLogin = this._repository.GetLogin(pcPerUsuCodigo, System.Text.Encoding.UTF8.GetString(passbase64EncodedBytes));

                if(eDatosLogin.Result.cPerUsuCodigo == pcPerUsuCodigo)
                {
                    var eDatosUsuario = this._repository.GetDatosUsuario(eDatosLogin.Result.cPerCodigo);
                    //var eDatosUsuario = this._repository.GetDatosUsuario("1000211996");
                    
                    var tokenhandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                    var key = config.GetValue<string>("keysecret");
                    var keys = System.Text.Encoding.ASCII.GetBytes(config.GetValue<string>("keysecret"));
                    var tokendescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
                    {
                        Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                    {
                        new Claim(ClaimTypes.NameIdentifier,eDatosLogin.Result.cPerCodigo.ToString()),
                        new Claim(ClaimTypes.Name,eDatosLogin.Result.cPerUsuCodigo.ToString()),
                        new Claim(ClaimTypes.Role,(eDatosUsuario.Result.nRol == 1 ? "Admin" : "User")),
                        new Claim(ClaimTypes.Version,"v2.1")
                    }),
                        Expires = DateTime.UtcNow.AddHours(3),
                        SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keys), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)

                    };
                    var tokens = tokenhandler.CreateToken(tokendescriptor);
                    datosusuario = eDatosUsuario.Result;
                    datosusuario.cToken = tokenhandler.WriteToken(tokens);
                    datosusuario.cPerUsuClave = null;

                }
                else
                {
                    return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, false, "Credenciales incorrectas", null));
                }


                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Bienvenido: " + datosusuario.cPerNombre + " " + datosusuario.cPerApellido, datosusuario));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }


        [HttpGet("/api/lst_prdacademico/{cPerCodigo}", Name = "Get_Lst_PrdAcademico_By_Jur")]
        
        public ActionResult<List<Periodo>> Get_Lst_PrdAcademico(string cPerCodigo)
        {
            try
            {
                var obj = this._repository.Get_Lst_PrdAcademico_By_Jur(cPerCodigo);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                (obj.Result.Count > 0 ? "Se ha encontrado " + obj.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj.Result));

            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }
    
    
    
        [HttpGet("/api/tipousuario/{cPerCodigo}")]
        [Authorize]
        public ActionResult<List<Periodo>> Get_Lst_TipoUsuario(string cPerCodigo)
        {
            try
            {
                var obj = this._repository.GetComboTipoUsuario(cPerCodigo);

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                (obj.Result.Count > 0 ? "Se ha encontrado " + obj.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj.Result));
            }
            catch(Exception ex) { return new JsonResult(new Mensajes(ex.Message)); }
        }
        
        [HttpGet("/api/excel_usuarios/{nTipo}/{cPrdNombre}", Name = "GetExportarUsuario")]
        //[Authorize]
        public ActionResult<List<LegDatosGenerales>> ExportarExcelListadoUsuario(int nTipo, string cPrdNombre)
        {
            try

            {

                var obj = this._repository.GetLista_Legajos_by_Periodo_Excel(nTipo, cPrdNombre);
                // var datosGuardar = obj.Result;
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
               (obj.Result.Count > 0 ? "Se ha encontrado " + obj.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj.Result));

            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));


            }
        }

        [HttpGet("/api/listar_persona_con_permisos" ,Name = "PersonasConPemisosLegajos")]
        [Authorize]
        public ActionResult<List<PerUsuario>> ListarPersonasConPermisos()
        {
            try
            {
                var obj = this._repository.GetPerUsuariosConPermisos();
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
            (obj.Result.Count > 0 ? "Se ha encontrado " + obj.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj.Result));

            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));


            }
        }


        [HttpGet("/api/listar_persona_con_acciones", Name = "PersonasConAccionesLegajos")]
        //[Authorize]
        public ActionResult<List<PerUsuario>> ListarPersonasConAcciones()
        {
            try
            {
                var obj = this._repository.GetPerUsuariosConAcciones();
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
            (obj.Result.Count > 0 ? "Se ha encontrado " + obj.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj.Result));

            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));


            }
        }

    }
}
