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
    [Route("api/declaracionjurada")]
    [ApiController]
    [Authorize]
    public class LegDeclaracionJuradaController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        private readonly LegDatosGeneralesRepository repositorydatosgral;
        FuncionesGenerales funcion;
        public LegDeclaracionJuradaController(bdLegajosContext context, IConfiguration configuration, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona, LegDatosGeneralesRepository repositorydatosgral)
        {
            this.context = context;
            this.config = configuration;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.repositorydatosgral = repositorydatosgral;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
        }

        [HttpGet("{parnId}", Name = "ObtenerDeclaracionJurada")]
        public ActionResult<LegDeclaracionJurada> Get(Int64 parnId)
        {
            var obj = context.LegDeclaracionJurada
                .Select(x => new LegDeclaracionJurada
                {
                    NLegDjcodigo = x.NLegDjcodigo,
                    NLegDjdatCodigo = x.NLegDjdatCodigo,
                    CLegDjanexo2 = x.CLegDjanexo2,
                    CLegDjanexo6 = x.CLegDjanexo6,
                    CLegDjanexo7 = x.CLegDjanexo7,
                    BLegDjestado = x.BLegDjestado,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,

                    // ------------------ EDGAR_BS-2025---------------------------------------->
                    CLegDjanexo1 = x.CLegDjanexo1,
                    CLegDjanexo2_2 = x.CLegDjanexo2_2,
                    CLegDjanexo3 = x.CLegDjanexo3,
                    CLegDjanexo4 = x.CLegDjanexo4,
                    CLegDjanexo5 = x.CLegDjanexo5,
                    CLegDjanexo6_2 = x.CLegDjanexo6_2,
                    CLegDjDNI = x.CLegDjDNI,
                    CLegDjDNI_DH = x.CLegDjDNI_DH,
                    CLegDjFotoCarnet = x.CLegDjFotoCarnet,
                    CLegDjNumCta = x.CLegDjNumCta,
                    CLegDjConsJubilacion = x.CLegDjConsJubilacion,
                    CLegDjConsAfiliacionOnpAfp = x.CLegDjConsAfiliacionOnpAfp
                    // ------------------ EDGAR_BS-2025---------------------------------------->
                })
                .FirstOrDefault(x => x.NLegDjcodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            
            obj.CLegDjanexo2 = string.Empty;
            obj.CLegDjanexo6 = string.Empty;
            obj.CLegDjanexo7 = string.Empty;

            // ------------------ EDGAR_BS-2025---------------------------------------->
            obj.CLegDjanexo1 = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDjanexo1 ?? string.Empty, true)));
            obj.CLegDjanexo2_2 = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDjanexo2_2 ?? string.Empty, true)));
            obj.CLegDjanexo3 = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDjanexo3 ?? string.Empty, true)));
            obj.CLegDjanexo4 = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDjanexo4 ?? string.Empty, true)));
            obj.CLegDjanexo5 = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDjanexo5 ?? string.Empty, true)));
            obj.CLegDjanexo6_2 = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDjanexo6_2 ?? string.Empty, true)));
            obj.CLegDjDNI = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDjDNI ?? string.Empty, true)));
            obj.CLegDjDNI_DH = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDjDNI_DH ?? string.Empty, true)));
            obj.CLegDjFotoCarnet = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDjFotoCarnet ?? string.Empty, true)));
            obj.CLegDjNumCta = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDjNumCta ?? string.Empty, true)));
            obj.CLegDjConsJubilacion = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDjConsJubilacion ?? string.Empty, true)));
            obj.CLegDjConsAfiliacionOnpAfp = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDjConsAfiliacionOnpAfp ?? string.Empty, true)));
            // ------------------ EDGAR_BS-2025---------------------------------------->
            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpGet("/api/declaracionjurada_lst/{parnCodigoLegajo}", Name = "ListarDeclaracionJurada")]
        public ActionResult<IEnumerable<LegDeclaracionJurada>> Get(int parnCodigoLegajo)
        {
            try
            {
                var obj = funcion.LegDatosDeclaracionJurada(parnCodigoLegajo);

                if (obj.Count > 0)
                {
                    foreach (LegDeclaracionJurada x in obj)
                    {
                        x.CLegDjanexo2 = null;
                        x.CLegDjanexo6 = null;
                        x.CLegDjanexo7 = null;

                        // ------------------ EDGAR_BS-2025---------------------------------------->
                        x.CLegDjanexo1 = !string.IsNullOrEmpty(x.CLegDjanexo1) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptHelper.Encrypt(x.CLegDjanexo1, true))): null;
                        x.CLegDjanexo2_2 = !string.IsNullOrEmpty(x.CLegDjanexo2_2) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptHelper.Encrypt(x.CLegDjanexo2_2, true))): null;
                        x.CLegDjanexo3 = !string.IsNullOrEmpty(x.CLegDjanexo3) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptHelper.Encrypt(x.CLegDjanexo3, true))): null;
                        x.CLegDjanexo4 = !string.IsNullOrEmpty(x.CLegDjanexo4) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptHelper.Encrypt(x.CLegDjanexo4, true))): null;
                        x.CLegDjanexo5 = !string.IsNullOrEmpty(x.CLegDjanexo5)? Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptHelper.Encrypt(x.CLegDjanexo5, true))): null;
                        x.CLegDjanexo6_2 = !string.IsNullOrEmpty(x.CLegDjanexo6_2) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptHelper.Encrypt(x.CLegDjanexo6_2, true))) : null;
                        x.CLegDjDNI = !string.IsNullOrEmpty(x.CLegDjDNI) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptHelper.Encrypt(x.CLegDjDNI, true))): null;
                        x.CLegDjDNI_DH = !string.IsNullOrEmpty(x.CLegDjDNI_DH) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptHelper.Encrypt(x.CLegDjDNI_DH, true))): null;
                        x.CLegDjFotoCarnet = !string.IsNullOrEmpty(x.CLegDjFotoCarnet) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptHelper.Encrypt(x.CLegDjFotoCarnet, true))): null;
                        x.CLegDjNumCta = !string.IsNullOrEmpty(x.CLegDjNumCta) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptHelper.Encrypt(x.CLegDjNumCta, true))): null;
                        x.CLegDjConsJubilacion = !string.IsNullOrEmpty(x.CLegDjConsJubilacion) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptHelper.Encrypt(x.CLegDjConsJubilacion, true))): null;
                        x.CLegDjConsAfiliacionOnpAfp = !string.IsNullOrEmpty(x.CLegDjConsAfiliacionOnpAfp) ? Convert.ToBase64String(Encoding.UTF8.GetBytes(EncryptHelper.Encrypt(x.CLegDjConsAfiliacionOnpAfp, true))) : null;
                        // ------------------ EDGAR_BS-2025---------------------------------------->
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
        public async Task<ActionResult> PostDeclaracionJurada(int pnCodigo, [FromForm] LegDeclaracionJurada datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                var decljur = context.LegDeclaracionJurada.FirstOrDefault(x => x.NLegDjdatCodigo == pnCodigo && x.BLegDjestado == true);
                await this.repositorydatosgral.UpdateStateDJ(pnCodigo, funcion.cPerCodAux);
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

                datos.CLegDjanexo2 = null;
                datos.CLegDjanexo6 = null;
                datos.CLegDjanexo7 = null;

                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegDjdatCodigo = pnCodigo;


                // ------------------ EDGAR_BS-2025---------------------------------------->

                if (datos.cFileDjanexo1 != null)
                {
                    fileName = Path.GetFileName(datos.cFileDjanexo1.FileName);
                    extension = Path.GetExtension(datos.cFileDjanexo1.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoAnx1") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileDjanexo1.CopyTo(stream);
                    }

                    datos.CLegDjanexo1 = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else if (decljur != null)
                {
                    datos.CLegDjanexo1 = decljur.CLegDjanexo1;
                }
                else
                {
                    datos.CLegDjanexo1 = null;
                }

                if (datos.cFileDjanexo2_2 != null)
                {
                    fileName = Path.GetFileName(datos.cFileDjanexo2_2.FileName);
                    extension = Path.GetExtension(datos.cFileDjanexo2_2.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoAnx2_2") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileDjanexo2_2.CopyTo(stream);
                    }

                    datos.CLegDjanexo2_2 = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else if (decljur != null)
                {
                    datos.CLegDjanexo2_2 = decljur.CLegDjanexo2_2;
                }
                else
                {
                    datos.CLegDjanexo2_2 = null;
                }

                if (datos.cFileDjanexo3 != null)
                {
                    fileName = Path.GetFileName(datos.cFileDjanexo3.FileName);
                    extension = Path.GetExtension(datos.cFileDjanexo3.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoAnx3") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileDjanexo3.CopyTo(stream);
                    }

                    datos.CLegDjanexo3 = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else if (decljur != null)
                {
                    datos.CLegDjanexo3 = decljur.CLegDjanexo3;
                }
                else
                {
                    datos.CLegDjanexo3 = null;
                }

                if (datos.cFileDjanexo4 != null)
                {
                    fileName = Path.GetFileName(datos.cFileDjanexo4.FileName);
                    extension = Path.GetExtension(datos.cFileDjanexo4.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoAnx4") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileDjanexo4.CopyTo(stream);
                    }

                    datos.CLegDjanexo4 = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else if (decljur != null)
                {
                    datos.CLegDjanexo4 = decljur.CLegDjanexo4;
                }
                else
                {
                    datos.CLegDjanexo4 = null;
                }

                if (datos.cFileDjanexo5 != null)
                {
                    fileName = Path.GetFileName(datos.cFileDjanexo5.FileName);
                    extension = Path.GetExtension(datos.cFileDjanexo5.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoAnx5") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileDjanexo5.CopyTo(stream);
                    }

                    datos.CLegDjanexo5 = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else if (decljur != null){
                    datos.CLegDjanexo5 = decljur.CLegDjanexo5;
                }
                else{
                    datos.CLegDjanexo5 = null;
                }

                if (datos.cFileDjanexo6_2 != null)
                {
                    fileName = Path.GetFileName(datos.cFileDjanexo6_2.FileName);
                    extension = Path.GetExtension(datos.cFileDjanexo6_2.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoAnx6_2") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileDjanexo6_2.CopyTo(stream);
                    }

                    datos.CLegDjanexo6_2 = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else if (decljur != null){
                    datos.CLegDjanexo6_2 = decljur.CLegDjanexo6_2;
                }
                else{
                    datos.CLegDjanexo6_2 = null;
                }


                if (datos.cFileDjDNI != null)
                {
                    fileName = Path.GetFileName(datos.cFileDjDNI.FileName);
                    extension = Path.GetExtension(datos.cFileDjDNI.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoDNI") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileDjDNI.CopyTo(stream);
                    }

                    datos.CLegDjDNI = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else if (decljur != null)
                {
                    datos.CLegDjDNI = decljur.CLegDjDNI;
                }
                else
                {
                    datos.CLegDjDNI = null;
                }

                if (datos.cFileDjDNI_DH != null)
                {
                    fileName = Path.GetFileName(datos.cFileDjDNI_DH.FileName);
                    extension = Path.GetExtension(datos.cFileDjDNI_DH.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoDNI_DH") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileDjDNI_DH.CopyTo(stream);
                    }

                    datos.CLegDjDNI_DH = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else if (decljur != null)
                {
                    datos.CLegDjDNI_DH = decljur.CLegDjDNI_DH;
                }
                else
                {
                    datos.CLegDjDNI_DH = null;
                }

                if (datos.cFileDjFotoCarnet != null)
                {
                    fileName = Path.GetFileName(datos.cFileDjFotoCarnet.FileName);
                    extension = Path.GetExtension(datos.cFileDjFotoCarnet.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoFotoCarnet") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileDjFotoCarnet.CopyTo(stream);
                    }

                    datos.CLegDjFotoCarnet = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else if (decljur != null)
                {
                    datos.CLegDjFotoCarnet = decljur.CLegDjFotoCarnet;
                }
                else
                {
                    datos.CLegDjFotoCarnet = null;
                }

                if (datos.cFileDjNumCta != null)
                {
                    fileName = Path.GetFileName(datos.cFileDjNumCta.FileName);
                    extension = Path.GetExtension(datos.cFileDjNumCta.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoNumCta") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileDjNumCta.CopyTo(stream);
                    }

                    datos.CLegDjNumCta = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else if (decljur != null)
                {
                    datos.CLegDjNumCta = decljur.CLegDjNumCta;
                }
                else
                {
                    datos.CLegDjNumCta = null;
                }

              
                if (datos.cFileDjConsJubilacion != null)
                {
                    fileName = Path.GetFileName(datos.cFileDjConsJubilacion.FileName);
                    extension = Path.GetExtension(datos.cFileDjConsJubilacion.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoConsJubilacion") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileDjConsJubilacion.CopyTo(stream);
                    }

                    datos.CLegDjConsJubilacion = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else if (decljur != null)
                {
                    datos.CLegDjConsJubilacion = decljur.CLegDjConsJubilacion;
                }
                else
                {
                    datos.CLegDjConsJubilacion = null;
                }


                if (datos.cFileDjConsAfiliacionOnpAfp != null)
                {
                    fileName = Path.GetFileName(datos.cFileDjConsAfiliacionOnpAfp.FileName);
                    extension = Path.GetExtension(datos.cFileDjConsAfiliacionOnpAfp.FileName);
                    ruteFile = this.config.GetValue<string>("LegajoConsAfiliacionOnpAfp") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileDjConsAfiliacionOnpAfp.CopyTo(stream);
                    }

                    datos.CLegDjConsAfiliacionOnpAfp = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else if (decljur != null)
                {
                    datos.CLegDjConsAfiliacionOnpAfp = decljur.CLegDjConsAfiliacionOnpAfp;
                }
                else
                {
                    datos.CLegDjConsAfiliacionOnpAfp = null;
                }
                // ------------------ EDGAR_BS-2025---------------------------------------->

                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                datos.NLegDjdatCodigo = pnCodigo;

                datos.BLegDjestado = true;
                //return new JsonResult(new Mensajes(400, false, "Declaraciones juradas han sido registradas.", datos));
                context.LegDeclaracionJurada.Add(datos);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerDeclaracionJurada", new { id = datos.NLegDjcodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Declaraciones juradas han sido registradas.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }


        public class EliminarDocumentoRequest
        {
            public string TipoDocumento { get; set; }
            public int CodigoDJ { get; set; }
        }

        // Eliminación de documentos para subir (Anexos) - EBS 11/2025   ------------>
        [HttpPost("eliminar_documento_legajo")]
        public async Task<ActionResult> PostEliminarDocumentoAnexoLegajo([FromBody] EliminarDocumentoRequest request)

        {
            try
            {
                // Validar parámetros
                if (string.IsNullOrEmpty(request.TipoDocumento) || request.CodigoDJ <= 0)
                {
                    return new JsonResult(new Mensajes(400, false, "Parámetros inválidos.", null));
                }

                // Obtener el usuario actual
                funcion.getPerCodigoToken(User);

                // Buscar la declaración jurada
                var declJurada = await context.LegDeclaracionJurada
                    .FirstOrDefaultAsync(x => x.NLegDjcodigo == request.CodigoDJ && x.BLegDjestado == true);

                if (declJurada == null)
                {
                    return new JsonResult(new Mensajes(404, false, "Declaración jurada no encontrada.", null));
                }

                // Obtener la ruta del directorio de archivos
                string directorio = config.GetValue<string>("ServerLegajos");
                var objLG = await context.LegDatosGenerales
                    .AsNoTracking()
                    .FirstOrDefaultAsync(x => x.NLegDatCodigo == declJurada.NLegDjdatCodigo);

                if (objLG == null)
                {
                    return new JsonResult(new Mensajes(404, false, "Datos generales no encontrados.", null));
                }

                string path = Path.Combine(directorio, config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc);

                // Determinar qué documento eliminar según el tipo
                string rutaArchivo = null;
                string nombreCampo = null;

                switch (request.TipoDocumento.ToLower())
                {
                    case "anexo1":
                        rutaArchivo = declJurada.CLegDjanexo1;
                        declJurada.CLegDjanexo1 = null;
                        nombreCampo = "Anexo 1";
                        break;
                    case "anexo2_2":
                        rutaArchivo = declJurada.CLegDjanexo2_2;
                        declJurada.CLegDjanexo2_2 = null;
                        nombreCampo = "Anexo 2.2";
                        break;
                    case "anexo3":
                        rutaArchivo = declJurada.CLegDjanexo3;
                        declJurada.CLegDjanexo3 = null;
                        nombreCampo = "Anexo 3";
                        break;
                    case "anexo4":
                        rutaArchivo = declJurada.CLegDjanexo4;
                        declJurada.CLegDjanexo4 = null;
                        nombreCampo = "Anexo 4";
                        break;
                    case "anexo5":
                        rutaArchivo = declJurada.CLegDjanexo5;
                        declJurada.CLegDjanexo5 = null;
                        nombreCampo = "Anexo 5";
                        break;
                    case "anexo6_2":
                        rutaArchivo = declJurada.CLegDjanexo6_2;
                        declJurada.CLegDjanexo6_2 = null;
                        nombreCampo = "Anexo 6.2";
                        break;
                    case "dni":
                        rutaArchivo = declJurada.CLegDjDNI;
                        declJurada.CLegDjDNI = null;
                        nombreCampo = "DNI";
                        break;
                    case "dni_dh":
                        rutaArchivo = declJurada.CLegDjDNI_DH;
                        declJurada.CLegDjDNI_DH = null;
                        nombreCampo = "DNI (Declarante y Herederos)";
                        break;
                    case "fotocarnet":
                        rutaArchivo = declJurada.CLegDjFotoCarnet;
                        declJurada.CLegDjFotoCarnet = null;
                        nombreCampo = "Foto Carnet";
                        break;
                    case "numcta":
                        rutaArchivo = declJurada.CLegDjNumCta;
                        declJurada.CLegDjNumCta = null;
                        nombreCampo = "Número de Cuenta";
                        break;
                    case "consjubilacion":
                        rutaArchivo = declJurada.CLegDjConsJubilacion;
                        declJurada.CLegDjConsJubilacion = null;
                        nombreCampo = "Constancia de Jubilación";
                        break;
                    case "consafiliacion":
                        rutaArchivo = declJurada.CLegDjConsAfiliacionOnpAfp;
                        declJurada.CLegDjConsAfiliacionOnpAfp = null;
                        nombreCampo = "Constancia de Afiliación ONP/AFP";
                        break;
                    default:
                        return new JsonResult(new Mensajes(400, false, "Tipo de documento no válido.", null));
                }

                // Actualizar la información de modificación
                declJurada.CUsuModifica = funcion.cPerCodAux;
                declJurada.DFechaModifica = DateTime.Now;

                // Guardar los cambios en la base de datos
                context.LegDeclaracionJurada.Update(declJurada);
                await context.SaveChangesAsync();

                // Eliminar el archivo físico si existe
                if (!string.IsNullOrEmpty(rutaArchivo))
                {
                    string rutaCompleta = Path.Combine(directorio, rutaArchivo);
                    if (System.IO.File.Exists(rutaCompleta))
                    {
                        System.IO.File.Delete(rutaCompleta);
                    }
                }

                return new JsonResult(new Mensajes(200, true, $"Documento '{nombreCampo}' eliminado correctamente.", null));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(500, false, $"Error al eliminar el documento: {ex.Message}", ex));
            }
        }
        // Eliminación de documentos para subir (Anexos) - EBS 11/2025   ------------>

    }
}
