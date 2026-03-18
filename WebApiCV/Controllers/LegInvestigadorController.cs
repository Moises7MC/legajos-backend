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
using WebApiCV.Contexts;
using WebApiCV.Entity;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using WebApiCV.Data;

namespace WebApiCV.Controllers
{
    [Route("api/investigador")]
    [ApiController][Authorize]
    public class LegInvestigadorController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        FuncionesGenerales funcion;
        public LegInvestigadorController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }


        [HttpGet("{parnId}", Name = "ObtenerInvestigador")]
        public ActionResult<LegInvestigador> Get(Int64 parnId)
        {
            var obj = context.LegInvestigador
               .Select(x => new LegInvestigador
               {
                   NLegInvCodigo = x.NLegInvCodigo,
                   NLegInvCentroRegistro = x.NLegInvCentroRegistro,
                   NValorCentroRegistro = x.NValorCentroRegistro,
                   NLegInvNivelRenacyt = x.NLegInvNivelRenacyt,
                   NValorNivelRenacyt = x.NValorNivelRenacyt,
                   CLegInvNroRegistro = x.CLegInvNroRegistro,
                   DLegInvFechaInicio = x.DLegInvFechaInicio,
                   DLegInvFechaFin = x.DLegInvFechaFin,
                   CLegInvArchivo = x.CLegInvArchivo,
                   NLegInvDatCodigo = x.NLegInvDatCodigo,
                   CLegInvValida = x.CLegInvValida,
                   CLegInvEstado = x.CLegInvEstado,
                   CUsuRegistro = x.CUsuRegistro,
                   DFechaRegistro = x.DFechaRegistro,
                   CUsuModifica = x.CUsuModifica,
                   DFechaModifica = x.DFechaModifica,
                   vCentroRegistro = repositoryinterf.GetInterfaceDatos(x.NValorCentroRegistro, x.NLegInvCentroRegistro).Result ?? null,
                   vNivelRenacyt = x.NLegInvNivelRenacyt.HasValue && x.NValorNivelRenacyt.HasValue ? repositoryinterf.GetInterfaceDatos(x.NValorNivelRenacyt.Value, x.NLegInvNivelRenacyt.Value).Result
    : null,
               })
                .FirstOrDefault(x => x.NLegInvCodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            obj.CLegInvArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegInvArchivo, true)));

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpGet("/api/investigador_lst/{parnCodigoLegajo}", Name = "ListarInvestigador")]
        public ActionResult<IEnumerable<LegInvestigador>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosInvestigador(parnCodigoLegajo);

                if (obj.Count > 0)
                {
                    foreach (LegInvestigador x in obj)
                    {
                        x.CLegInvArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegInvArchivo, true)));
                    }
                }

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                (obj.Count > 0 ? "Se ha encontrado " + obj.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }

        [HttpPost("{pnCodigo}")]
        public ActionResult PostInvestigador(int pnCodigo, [FromForm] LegInvestigador datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == pnCodigo);
                String directorio = this.config.GetValue<string>("ServerLegajos"); //D:/Proyectos/USS"

                string path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc); //"LegajosUSS/" 
                string fileName = "";
                string extension = "";
                string ruteFile = "";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //return new JsonResult(new Mensajes(200, true, "Currículo ha sido registrado.", curriculo.cCurFile));
                if (datos.cFile != null)
                {
                    fileName = Path.GetFileName(datos.cFile.FileName);
                    extension = Path.GetExtension(datos.cFile.FileName);
                    ruteFile = this.config.GetValue<string>("Legajo07") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegInvArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegInvArchivo = "";
                }
                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegInvDatCodigo = pnCodigo;

                datos.CLegInvEstado = true;
                context.LegInvestigador.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerInvestigador", new { id = datos.NLegInvDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true,  "Docente investigador ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }


        [HttpPost("put/{pnCodigo}")]
        public async Task<ActionResult> PutInvestigador(int pnCodigo, [FromForm] LegInvestigador datos)
        {
            funcion.getPerCodigoToken(User);
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegInvCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {
                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objGT = context.LegInvestigador.AsNoTracking().FirstOrDefault(x => x.NLegInvCodigo == pnCodigo);
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegInvDatCodigo);
                string path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc); //"LegajosUSS/" 
                string fileName = "";
                string extension = "";
                string ruteFile = "";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (datos.cFile != null)
                {
                    fileName = Path.GetFileName(datos.cFile.FileName);
                    extension = Path.GetExtension(datos.cFile.FileName);
                    ruteFile = this.config.GetValue<string>("Legajo07") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegInvArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegInvArchivo = objGT.CLegInvArchivo;
                }

                datos.CUsuRegistro = objGT.CUsuRegistro;
                datos.DFechaRegistro = objGT.DFechaRegistro;
                datos.NLegInvDatCodigo = objLG.NLegDatCodigo;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerInvestigador", new { id = datos.NLegInvDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPatch("{pnCodigo}")]
        public async Task<ActionResult> PatchInvestigador(int pnCodigo, [FromBody] JsonPatchDocument<LegInvestigador> datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var obj = context.LegInvestigador.FirstOrDefault(x => x.NLegInvCodigo == pnCodigo);

                if (obj == null)
                {
                    var err = NotFound();
                    return new JsonResult(new Mensajes(err.StatusCode, false, "No existe datos", err));
                }
                obj.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                obj.CUsuModifica = funcion.cPerCodAux;
                datos.ApplyTo(obj, ModelState);
                context.Entry(obj).State = EntityState.Modified;
                await context.SaveChangesAsync();
                transaction.Commit();
                var res = new CreatedAtRouteResult("ObtenerInvestigador", new { id = obj.NLegInvCodigo }, obj);
                return new JsonResult(new Mensajes(Ok().StatusCode, true, "Registro ha sido actualizado.", res.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }
    }
}
