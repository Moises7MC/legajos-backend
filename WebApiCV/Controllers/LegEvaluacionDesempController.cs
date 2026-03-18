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
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Authorization;
using WebApiCV.Data;

namespace WebApiCV.Controllers
{
    [Route("api/evaluaciondesemp")]
    [ApiController]
    [Authorize]
    public class LegEvaluacionDesempController : ControllerBase
    { 
     private readonly bdLegajosContext context;
    private readonly IConfiguration config;
    private readonly ConstanteRepository repositorycons;
    private readonly InterfaceRepository repositoryinterf;
    private readonly PersonaRepository repositorypersona;
    FuncionesGenerales funcion;
    public LegEvaluacionDesempController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
    {
        this.context = context;
        this.config = configuration;
        this.repositorycons = repositorycons;
        this.repositoryinterf = repositoryinterf;
        this.repositorypersona = repositorypersona;
        this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
    }

    [HttpGet("{parnId}", Name = "ObtenerEvaluacionDesemp")]
    public ActionResult<LegEvaluacionDesemp> Get(Int64 parnId)
    {
        var obj = context.LegEvaluacionDesemp
            .Select(x => new LegEvaluacionDesemp
            {
                NLegEvalDatCodigo = x.NLegEvalDatCodigo,
                NLegEvalCodigo = x.NLegEvalCodigo,
                NLegEvalCargo = x.NLegEvalCargo,
                NLegValCargo = x.NLegValCargo,
                NLegEvalArea = x.NLegEvalArea,
                NLegValArea = x.NLegValArea,
                CLegEvalArchivo = x.CLegEvalArchivo,
                BLegEvalEstado = x.BLegEvalEstado,
                NLegEvalPuntaje = x.NLegEvalPuntaje,
                NLegEvalNivel = x.NLegEvalNivel,
                NLegValNivel = x.NLegValNivel,
                CLegEvalAnio = x.CLegEvalAnio,
                CLegEvalSemestre = x.CLegEvalSemestre,
                CUsuRegistro = x.CUsuRegistro,
                DFechaRegistro = x.DFechaRegistro,
                CUsuModifica = x.CUsuModifica,
                DFechaModifica = x.DFechaModifica,
                vArea = x.vArea,
                vCargo = x.vCargo,
                vNivel = x.vNivel
            })
            .FirstOrDefault(x => x.NLegEvalCodigo == parnId);
        if (obj == null)
        {
            return NotFound();
        }
        obj.CLegEvalArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegEvalArchivo, true)));

        return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
    }

    [HttpGet("/api/evaluaciondesemp_lst/{parnCodigoLegajo}", Name = "ListarEvaluacionDesemp")]
    public ActionResult<IEnumerable<LegEvaluacionDesemp>> Get(int parnCodigoLegajo)
    {
        try
        {
            var obj = funcion.LegDatosEvaluacionDesemp(parnCodigoLegajo);

            if (obj.Count > 0)
            {
                foreach (LegEvaluacionDesemp x in obj)
                {
                    x.CLegEvalArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegEvalArchivo, true)));
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
    public ActionResult PostEvaluacionDesemp(int pnCodigo, [FromForm] LegEvaluacionDesemp datos)
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
            if (datos.cFile != null)
            {
                fileName = Path.GetFileName(datos.cFile.FileName);
                extension = Path.GetExtension(datos.cFile.FileName);
                ruteFile = this.config.GetValue<string>("LegajoEvalDesemp") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                {
                    datos.cFile.CopyTo(stream);
                }

                datos.CLegEvalArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
            }
            else
            {
                datos.CLegEvalArchivo = "";
            }
            datos.CUsuRegistro = funcion.cPerCodAux;
            datos.DFechaRegistro = DateTime.Now;
            datos.NLegEvalDatCodigo = pnCodigo;

            datos.BLegEvalEstado = true;
            context.LegEvaluacionDesemp.Add(datos);
            context.SaveChanges();
            transaction.Commit();
            var obj = new CreatedAtRouteResult("ObtenerEvaluacionDesemp", new { id = datos.NLegEvalCodigo }, datos);
            return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Evaluaciòn de desempeño ha sido registrado.", obj.Value));
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new JsonResult(new Mensajes(400, false, ex.Message, ex));
        }

    }

    [HttpPost("put/{pnCodigo}")]
    public async Task<ActionResult> PutEvaluacionDesemp(int pnCodigo, [FromForm] LegEvaluacionDesemp datos)
    {
        funcion.getPerCodigoToken(User);
        if (!ModelState.IsValid)
            return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

        if (pnCodigo != datos.NLegEvalCodigo)
        {
            return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
        }


        using var transaction = context.Database.BeginTransaction();
        try
        {
            String directorio = this.config.GetValue<string>("ServerLegajos");
            var objGT = context.LegEvaluacionDesemp.AsNoTracking().FirstOrDefault(x => x.NLegEvalCodigo == pnCodigo);
            var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == objGT.NLegEvalDatCodigo);
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
                ruteFile = this.config.GetValue<string>("LegajoEvalDesemp") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                {
                    datos.cFile.CopyTo(stream);
                }

                datos.CLegEvalArchivo = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
            }
            else
            {
                datos.CLegEvalArchivo = objGT.CLegEvalArchivo;
            }

            datos.CUsuRegistro = objGT.CUsuRegistro;
            datos.DFechaRegistro = objGT.DFechaRegistro;
            datos.NLegEvalDatCodigo = objLG.NLegDatCodigo;
            datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
            datos.CUsuModifica = funcion.cPerCodAux;

            context.Entry(datos).State = EntityState.Modified;
            await context.SaveChangesAsync();
            transaction.Commit();
            var obj = new CreatedAtRouteResult("ObtenerEvaluacionDesemp", new { id = datos.NLegEvalCodigo }, datos);
            return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Registro ha sido actualizado.", obj.Value));
        }
        catch (Exception ex)
        {
            transaction.Rollback();
            return new JsonResult(new Mensajes(400, false, ex.Message, ex));
        }
    }

    [HttpPatch("{pnCodigo}")]
    public async Task<ActionResult> PatchEvaluacionDesemp(int pnCodigo, [FromBody] JsonPatchDocument<LegEvaluacionDesemp> datos)
    {
        funcion.getPerCodigoToken(User);
        using var transaction = context.Database.BeginTransaction();
        try
        {
            var obj = context.LegEvaluacionDesemp.FirstOrDefault(x => x.NLegEvalCodigo == pnCodigo);

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
            var res = new CreatedAtRouteResult("ObtenerEvaluacionDesemp", new { id = obj.NLegEvalCodigo }, obj);
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
