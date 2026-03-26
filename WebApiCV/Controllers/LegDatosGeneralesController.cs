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
using DinkToPdf;
using DinkToPdf.Contracts;
using WebApiCV.TemplatePDF;
using WebApiCV.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.Drawing;
using System.IO.Compression;

namespace WebApiCV.Controllers
{
    [Route("api/legajo")]
    [ApiController]
    public class LegDatosGeneralesController : ControllerBase
    {
        private readonly bdLegajosContext context;
        private readonly IConfiguration config;
        private IConverter _converter;
        private readonly LegDatosGeneralesRepository _repository;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        private readonly ReporteLegajosRepository repositorylegajos;
        FuncionesGenerales funcion;
        public LegDatosGeneralesController(
            bdLegajosContext context,
                                           IConfiguration configuration,
                                           IConverter converter,
                                           LegDatosGeneralesRepository _repository,
                                           ConstanteRepository repositorycons,
                                           InterfaceRepository repositoryinterf,
                                           PersonaRepository repositorypersona,
                                           ReporteLegajosRepository repositorylegajos)
        {
            this.context = context;
            this.config = configuration;
            this._converter = converter;
            this._repository = _repository;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            this.repositorylegajos = repositorylegajos;
            this.funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);

        }
        
       

        //
        // ------------------- EDGAR BARRETO SANDOVAL --------------------------------------------------------------------------------
        //

        [HttpGet("/api/evadocentescargalectiva/{cPrdNombre}", Name = "evadocentescargalectiva")]
        public ActionResult<List<EvaluacionDocentesCargaLectiva>> GetEvaDocentesCargaLectiva(string cPrdNombre)
        {
            try
            {
                var objs = this.repositorylegajos.GetEvaluacionDocentesCargaLectiva(cPrdNombre);
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
               (objs.Result.Count > 0 ? "Se ha encontrado " + objs.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), objs.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpGet("/api/docentescargalectiva/", Name = "docentescargalectiva")]
        public ActionResult<List<EvaluacionDocentesCargaLectiva>> GetDocentesCargaLectiva()
        {
            try
            {
                var objs = this.repositorylegajos.GetDocentesCargaLectiva();
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
               (objs.Result.Count > 0 ? "Se ha encontrado " + objs.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), objs.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpGet("/api/evarenovacionratificacion/", Name = "evarenovacionratificacion")]
        public ActionResult<List<Leg_EvaDoc_RenovacionRatificacion>> GetLeg_EvaDoc_Renovacion_Ratificacion()
        {
            try
            {
                var objs = this.repositorylegajos.GetLeg_EvaDoc_Renovacion_Ratificacion();

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
               (objs.Result.Count > 0 ? "Se ha encontrado " + objs.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), objs.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpGet("/api/lst_legajos/{pnPrdActividad}", Name = "ListarRepLegajos")]
        public ActionResult<IEnumerable<ReporteLegajos>> GetList(int pnPrdActividad)
        {
            var obj = this.repositorylegajos.GetReporteLegajosCount(pnPrdActividad);

            if (obj == null)
            {
                return NotFound();
            }

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, (obj.Result.Count > 0 ? "Se ha encontrado " + obj.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj.Result));
        }

        [HttpGet("/api/lst_capinvlegajos/{pnPrdActividad}", Name = "ListarCapInvLegajos")]
        public ActionResult<IEnumerable<ReporteCapInvLegajos>> GetListInvCap(int pnPrdActividad)
        {
            var obj = this.repositorylegajos.GetReporteInvCapLegajos(pnPrdActividad);

            if (obj == null)
            {
                return NotFound();
            }

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, (obj.Result.Count > 0 ? "Se ha encontrado " + obj.Result.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj.Result));
        }

        //
        // ------------------- EDGAR BARRETO SANDOVAL --------------------------------------------------------------------------------
        //

        [HttpGet("/api/legajoaux/{parcCodigo}", Name = "ObtenerLegajoAux")]
        [Authorize]
        public ActionResult<LegDatosGenerales> GetAux(string parcCodigo)
        {

            var cantleg = context.LegDatosGenerales.Where(x => x.cPerCodigo == parcCodigo).Count();

            if (cantleg == 0)
            {
                var objs = _repository.GetDatosSEUSS(parcCodigo).Result;

                if (objs == null)
                {
                    return new JsonResult(new Mensajes(NotFound().StatusCode, false, "No se encontró datos generales en Legajos, proceda a registrar.", objs));
                }
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Legajo no tiene datos generales, se cargaron datos de SEUSS, proceda a completar los datos y guardar.", objs));
            }

            var obj = funcion.LegDatosReturn(parcCodigo);
            var codigodg = obj.NLegDatCodigo;
            obj.LegAdminitrativaCarga = funcion.LegDatosCargaAdministrativa(codigodg);
            obj.LegCapacitaciones = funcion.LegDatosCapacitaciones(codigodg);
            obj.LegCapacitacionInternas = funcion.LegDatosCapacitacionInterna(codigodg);
            obj.LegCategoriaDocente = funcion.LegDatosCategoriaDocente(codigodg);
            obj.LegDocenciaUniv = funcion.LegDatosDocenciaUniv(codigodg);
            obj.LegGradoTitulo = funcion.LegDatosGradoTitulo(codigodg);
            obj.LegIdiomaOfimatica = funcion.LegDatosIdiomaOfimatica(codigodg);
            obj.LegInvestigador = funcion.LegDatosInvestigador(codigodg);
            obj.LegParticipacionCongSem = funcion.LegDatosParticipacion(codigodg);
            obj.LegProduccionCiencia = funcion.LegDatosProduccionCiencia(codigodg);
            obj.LegProfesNoDocente = funcion.LegDatosExperienciaNoDocente(codigodg);
            obj.LegProyeccionSocial = funcion.LegDatosProyeccionSocial(codigodg);
            obj.LegReconocimiento = funcion.LegDatosReconocimiento(codigodg);
            obj.LegRegimenDedicacion = funcion.LegDatosRegimenDedicacion(codigodg);
            obj.LegTesisAseJur = funcion.LegDatosTesisAsJur(codigodg);
            obj.LegContratos = funcion.LegDatosContrato(codigodg);
            obj.LegResoluciones = funcion.LegDatosResolucion(codigodg);
            obj.LegEvaluacionDesemp = funcion.LegDatosEvaluacionDesemp(codigodg);
            obj.LegSeleccion = funcion.LegDatosSeleccion(codigodg);
            obj.LegOrdinarizacion = funcion.LegDatosOrdinarizacion(codigodg);
            obj.LegDeclaracionJurada = funcion.LegDatosDeclaracionJurada(codigodg);
            obj.LegDocumentacionInterna = funcion.LegDatosDocumentacionInterna(codigodg);
            //obj.CLegDatFoto = obj.CLegDatFoto.Trim() == string.Empty ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDatFoto, true)));
            obj.cLegDatSunedu = obj.cLegDatSunedu.Trim() == string.Empty ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.cLegDatSunedu, true)));
            obj.cLegDatFirma = obj.cLegDatFirma.Trim() == string.Empty ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.cLegDatFirma, true)));
            obj.cLegDatPolicial = obj.cLegDatPolicial.Trim() == string.Empty ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.cLegDatPolicial, true)));
            obj.cLegDatJudicial = obj.cLegDatJudicial.Trim() == string.Empty ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.cLegDatJudicial, true)));

            /* EBS - 01/2026 ---------------> */
            obj.LegLicenciaProfesional = funcion.LegDatosLicenciaProfesional(codigodg);
            obj.LegMembresia = funcion.LegDatosMembresia(codigodg);
            /* EBS - 01/2026 <--------------- */


            foreach (LegAdminitrativaCarga f in obj.LegAdminitrativaCarga)
            {
                f.CLegAdmArchivo = f.CLegAdmArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegAdmArchivo, true))) : String.Empty;
            }
            foreach (LegCapacitaciones f in obj.LegCapacitaciones)
            {
                f.CLegCapArchivo = f.CLegCapArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegCapArchivo, true))) : String.Empty;
            }
            foreach (LegCategoriaDocente f in obj.LegCategoriaDocente)
            {
                f.CLegCatArchivo = f.CLegCatArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegCatArchivo, true))) : String.Empty;
            }
            foreach (LegDocenciaUniv f in obj.LegDocenciaUniv)
            {
                f.CLegDocArchivo = f.CLegDocArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDocArchivo, true))) : String.Empty;
            }
            foreach (LegGradoTitulo f in obj.LegGradoTitulo)
            {
                f.CLegGraArchivo = f.CLegGraArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegGraArchivo, true))) : String.Empty;
            }
            foreach (LegIdiomaOfimatica f in obj.LegIdiomaOfimatica)
            {
                f.CLegIdOfArchivo = f.CLegIdOfArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegIdOfArchivo, true))) : String.Empty;
            }
            foreach (LegInvestigador f in obj.LegInvestigador)
            {
                f.CLegInvArchivo = f.CLegInvArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegInvArchivo, true))) : String.Empty;
            }
            foreach (Entity.LegParticipacionCongSem f in obj.LegParticipacionCongSem)
            {
                f.CLegParArchivo = f.CLegParArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegParArchivo, true))) : String.Empty;
            }
            foreach (LegProduccionCiencia f in obj.LegProduccionCiencia)
            {
                f.CLegProdArchivo = f.CLegProdArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegProdArchivo, true))) : String.Empty;
            }
            foreach (LegProfesNoDocente f in obj.LegProfesNoDocente)
            {
                f.CLegProArchivo = f.CLegProArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegProArchivo, true))) : String.Empty;
            }
            foreach (LegProyeccionSocial f in obj.LegProyeccionSocial)
            {
                f.CLegProyArchivo = f.CLegProyArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegProyArchivo, true))) : String.Empty;
            }
            foreach (LegReconocimiento f in obj.LegReconocimiento)
            {
                f.CLegRecArchivo = f.CLegRecArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegRecArchivo, true))) : String.Empty;
            }
            foreach (LegRegimenDedicacion f in obj.LegRegimenDedicacion)
            {
                f.CLegRegArchivo = f.CLegRegArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegRegArchivo, true))) : String.Empty;
            }
            foreach (LegTesisAseJur f in obj.LegTesisAseJur)
            {
                f.CLegTesArchivo = f.CLegTesArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegTesArchivo, true))) : String.Empty;
            }

            foreach (LegCapacitacionInterna f in obj.LegCapacitacionInternas)
            {
                f.CLegCiarchivo = f.CLegCiarchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegCiarchivo, true))) : String.Empty;
            }

            foreach (LegContrato f in obj.LegContratos)
            {
                f.CLegConArchivo = f.CLegConArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegConArchivo, true))) : String.Empty;
            }

            foreach (LegResolucion f in obj.LegResoluciones)
            {
                f.CLegResArchivo = f.CLegResArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegResArchivo, true))) : String.Empty;
            }

            foreach (LegEvaluacionDesemp f in obj.LegEvaluacionDesemp)
            {
                f.CLegEvalArchivo = f.CLegEvalArchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegEvalArchivo, true))) : String.Empty;
            }

            foreach (LegSeleccion f in obj.LegSeleccion)
            {
                f.CLegSelEvaluacionCv = f.CLegSelEvaluacionCv.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegSelEvaluacionCv, true))) : String.Empty;
                f.CLegSelClaseModelo = f.CLegSelClaseModelo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegSelClaseModelo, true))) : String.Empty;
                f.CLegSelEvaluacionPsico = f.CLegSelEvaluacionPsico.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegSelEvaluacionPsico, true))) : String.Empty;
                f.CLegSelEntrevistaPers = f.CLegSelEntrevistaPers.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegSelEntrevistaPers, true))) : String.Empty;
            }

            foreach (LegOrdinarizacion f in obj.LegOrdinarizacion)
            {
                f.CLegOrdFichaInscripcion = f.CLegOrdFichaInscripcion.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegOrdFichaInscripcion, true))) : String.Empty;
                f.CLegOrdEvaluacionCv = f.CLegOrdEvaluacionCv.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegOrdEvaluacionCv, true))) : String.Empty;
                f.CLegOrdClaseModelo = f.CLegOrdClaseModelo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegOrdClaseModelo, true))) : String.Empty;
                f.CLegOrdEvaluacionPsico = f.CLegOrdEvaluacionPsico.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegOrdEvaluacionPsico, true))) : String.Empty;
                f.CLegOrdEntrevistaPers = f.CLegOrdEntrevistaPers.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegOrdEntrevistaPers, true))) : String.Empty;
            }

            foreach (LegDeclaracionJurada f in obj.LegDeclaracionJurada)
            {
                //f.CLegDjanexo2 = f.CLegDjanexo2.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjanexo2, true))) : String.Empty;
                //f.CLegDjanexo6 = f.CLegDjanexo6.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjanexo6, true))) : String.Empty;
                //f.CLegDjanexo7 = f.CLegDjanexo7.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjanexo7, true))) : String.Empty;

                f.CLegDjanexo2 = String.Empty;
                f.CLegDjanexo6 = String.Empty;
                f.CLegDjanexo7 = String.Empty;

                // ------------------ EDGAR_BS-2025---------------------------------------->
                f.CLegDjanexo1 = !string.IsNullOrEmpty(f.CLegDjanexo1) ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjanexo1, true))) : string.Empty;
                f.CLegDjanexo2_2 = !string.IsNullOrEmpty(f.CLegDjanexo2_2) ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjanexo2_2, true))) : string.Empty;
                f.CLegDjanexo3 = !string.IsNullOrEmpty(f.CLegDjanexo3) ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjanexo3, true))) : string.Empty;
                f.CLegDjanexo4 = !string.IsNullOrEmpty(f.CLegDjanexo4) ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjanexo4, true))) : string.Empty;
                f.CLegDjanexo5 = !string.IsNullOrEmpty(f.CLegDjanexo5) ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjanexo5, true))) : string.Empty;
                f.CLegDjanexo6_2 = !string.IsNullOrEmpty(f.CLegDjanexo6_2) ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjanexo6_2, true))) : string.Empty;
                f.CLegDjDNI = !string.IsNullOrEmpty(f.CLegDjDNI) ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjDNI, true))) : string.Empty;
                f.CLegDjDNI_DH = !string.IsNullOrEmpty(f.CLegDjDNI_DH) ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjDNI_DH, true))) : string.Empty;
                f.CLegDjFotoCarnet = !string.IsNullOrEmpty(f.CLegDjFotoCarnet) ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjFotoCarnet, true))) : string.Empty;
                f.CLegDjNumCta = !string.IsNullOrEmpty(f.CLegDjNumCta) ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjNumCta, true))) : string.Empty;
                f.CLegDjConsJubilacion = !string.IsNullOrEmpty(f.CLegDjConsJubilacion) ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjConsJubilacion, true))) : string.Empty;
                f.CLegDjConsAfiliacionOnpAfp = !string.IsNullOrEmpty(f.CLegDjConsAfiliacionOnpAfp) ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDjConsAfiliacionOnpAfp, true))) : string.Empty;
                // ------------------ EDGAR_BS-2025---------------------------------------->
            }

            foreach (LegDocumentacionInterna f in obj.LegDocumentacionInterna)
            {
                f.CLegDiarchivo = f.CLegDiarchivo.Trim() != string.Empty ? Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDiarchivo, true))) : String.Empty;
            }

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }


        [HttpGet("{parnId}", Name = "ObtenerLegajo")]
        [Authorize]
        public ActionResult<LegDatosGenerales> Get(Int64 parnId)
        {
            var obj = context.LegDatosGenerales
                .Include(i => i.vTipoDoc)
                .Include(i => i.vSexo)
                .Include(i => i.vEstadoCivil)
                .Include(i => i.vPais)
                .Include(i => i.vGradoAcad)
                .Include(i => i.vCondicionColeg)
                .Include(i => i.vTipoDomicilio)
                .Include(i => i.vNacimiento)
                .Include(i => i.CLegDatColegioProfNavigation)
                .Include(i => i.LegAdminitrativaCarga.Where(p => p.CLegAdmEstado == true))
                    .ThenInclude(cs => cs.vCargo)
                    .Include(i => i.LegAdminitrativaCarga)
                    .ThenInclude(cs => cs.CLegAdmInstitucionNavigation)
                .Include(i => i.LegCapacitaciones.Where(p => p.CLegCapEstado == true))
                    .ThenInclude(cs => cs.vInstitucion)
                    .Include(i => i.LegCapacitaciones)
                    .ThenInclude(cs => cs.vTipo)
                    .Include(i => i.LegCapacitaciones)
                    .ThenInclude(cs => cs.vTipoEsp)
                .Include(i => i.LegCategoriaDocente.Where(p => p.CLegCatEstado == true))
                    .ThenInclude(cs => cs.vCategoria)
                    .Include(i => i.LegCategoriaDocente)
                    .ThenInclude(cs => cs.CLegCatInstitucionNavigation)
                .Include(i => i.LegDocenciaUniv.Where(p => p.CLegDocEstado == true))
                    .ThenInclude(cs => cs.vCategoria)
                    .Include(i => i.LegDocenciaUniv)
                    .ThenInclude(cs => cs.vRegimen)
                    .Include(i => i.LegDocenciaUniv)
                    .ThenInclude(cs => cs.CLegDocUniversidadNavigation)
                .Include(i => i.LegGradoTitulo.Where(p => p.CLegGraEstado == true))
                    .ThenInclude(cs => cs.vGradoAcad)
                    .Include(i => i.LegGradoTitulo)
                    .ThenInclude(cs => cs.vPais)
                    .Include(i => i.LegGradoTitulo)
                    .ThenInclude(cs => cs.CLegGraInstitucionNavigation)
                .Include(i => i.LegIdiomaOfimatica.Where(p => p.CLegIdOfEstado == true))
                    .ThenInclude(cs => cs.vCodigoDesc)
                    .Include(i => i.LegIdiomaOfimatica)
                    .ThenInclude(cs => cs.vNivel)
                .Include(i => i.LegInvestigador.Where(p => p.CLegInvEstado == true))
                    .ThenInclude(cs => cs.vCentroRegistro)
                .Include(i => i.LegInvestigador.Where(p => p.CLegInvEstado == true))
                    .ThenInclude(cs => cs.vNivelRenacyt)
                .Include(i => i.LegParticipacionCongSem.Where(p => p.CLegParEstado == true))
                    .ThenInclude(cs => cs.vAmbito)
                    .Include(i => i.LegParticipacionCongSem)
                    .ThenInclude(cs => cs.vRol)
                    .Include(i => i.LegParticipacionCongSem)
                    .ThenInclude(cs => cs.CLegParInstitucionNavigation)
                .Include(i => i.LegProduccionCiencia.Where(p => p.CLegProdEstado == true))
                    .ThenInclude(cs => cs.vTipo)
                .Include(i => i.LegProfesNoDocente.Where(p => p.CLegProEstado == true))
                    .ThenInclude(cs => cs.vCargo)
                    .Include(i => i.LegProfesNoDocente)
                    .ThenInclude(cs => cs.CLegProInstitucionNavigation)
                .Include(i => i.LegProyeccionSocial.Where(p => p.CLegProyEstado == true))
                    .ThenInclude(cs => cs.vTipo)
                    .Include(i => i.LegProyeccionSocial)
                    .ThenInclude(cs => cs.CLegProyInstitucionNavigation)
                .Include(i => i.LegReconocimiento.Where(p => p.CLegRecEstado == true))
                    .ThenInclude(cs => cs.vTipo)
                    .Include(i => i.LegReconocimiento)
                    .ThenInclude(cs => cs.vDocumento)
                    .Include(i => i.LegReconocimiento)
                    .ThenInclude(cs => cs.CLegRecInstitucionNavigation)
                .Include(i => i.LegRegimenDedicacion.Where(p => p.CLegRegEstado == true))
                    .ThenInclude(cs => cs.vDedicacion)
                .Include(i => i.LegTesisAseJur.Where(p => p.CLegTesEstado == true))
                    .ThenInclude(cs => cs.vTipo)
                    .Include(i => i.LegTesisAseJur)
                    .ThenInclude(cs => cs.vNivel)

                // EBS - 01/2026 ---------------------- >
                // Licencias Profesionales
                .Include(i => i.LegLicenciaProfesional.Where(p => p.CLegLicEstado == true))
                    .ThenInclude(cs => cs.vCondicion)
                    .Include(i => i.LegLicenciaProfesional)
                    .ThenInclude(cs => cs.vPais)
                    .Include(i => i.LegLicenciaProfesional)
                    .ThenInclude(cs => cs.CLegLicInstitucionNavigation)
                // Membresias
                .Include(i => i.LegMembresia.Where(p => p.CLegMemEstado == true))
                    .Include(i => i.LegMembresia)
                    .ThenInclude(cs => cs.vPais)
                    .Include(i => i.LegMembresia)
                    .ThenInclude(cs => cs.CLegMemInstitucionNavigation)
                // EBS - 01/2026  <----------------------

                .FirstOrDefault(x => x.NLegDatCodigo == parnId);
            if (obj == null)
            {
                return NotFound();
            }
            obj.CLegDatFoto = obj.CLegDatFoto.Trim() == string.Empty ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDatFoto, true)));
            obj.cLegDatSunedu = obj.cLegDatSunedu.Trim() == string.Empty ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.cLegDatSunedu, true)));
            foreach (LegAdminitrativaCarga f in obj.LegAdminitrativaCarga)
            {
                f.CLegAdmArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegAdmArchivo, true)));
            }
            foreach (LegCapacitaciones f in obj.LegCapacitaciones)
            {
                f.CLegCapArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegCapArchivo, true)));
            }
            foreach (LegCategoriaDocente f in obj.LegCategoriaDocente)
            {
                f.CLegCatArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegCatArchivo, true)));
            }
            foreach (LegDocenciaUniv f in obj.LegDocenciaUniv)
            {
                f.CLegDocArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDocArchivo, true)));
            }
            foreach (LegGradoTitulo f in obj.LegGradoTitulo)
            {
                f.CLegGraArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegGraArchivo, true)));
            }
            foreach (LegIdiomaOfimatica f in obj.LegIdiomaOfimatica)
            {
                f.CLegIdOfArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegIdOfArchivo, true)));
            }
            foreach (LegInvestigador f in obj.LegInvestigador)
            {
                f.CLegInvArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegInvArchivo, true)));
            }
            foreach (Entity.LegParticipacionCongSem f in obj.LegParticipacionCongSem)
            {
                f.CLegParArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegParArchivo, true)));
            }
            foreach (LegProduccionCiencia f in obj.LegProduccionCiencia)
            {
                f.CLegProdArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegProdArchivo, true)));
            }
            foreach (LegProfesNoDocente f in obj.LegProfesNoDocente)
            {
                f.CLegProArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegProArchivo, true)));
            }
            foreach (LegProyeccionSocial f in obj.LegProyeccionSocial)
            {
                f.CLegProyArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegProyArchivo, true)));
            }
            foreach (LegReconocimiento f in obj.LegReconocimiento)
            {
                f.CLegRecArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegRecArchivo, true)));
            }
            foreach (LegRegimenDedicacion f in obj.LegRegimenDedicacion)
            {
                f.CLegRegArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegRegArchivo, true)));
            }
            foreach (LegTesisAseJur f in obj.LegTesisAseJur)
            {
                f.CLegTesArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegTesArchivo, true)));
            }
            foreach (LegCapacitacionInterna f in obj.LegCapacitacionInternas)
            {
                f.CLegCiarchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegCiarchivo, true)));
            }

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", obj));
        }

        [HttpGet("/api/legajopercodigo/{parcCodigo}", Name = "ObtenerLegajoCodigoPer")]
        [Authorize]
        public ActionResult<LegDatosGenerales> GetPerCodigo(string parcCodigo)
        {
            var objx = context.LegDatosGenerales.FirstOrDefault(x => x.cPerCodigo == parcCodigo);

            if (objx == null)
            {
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, false, "", null)); ;
            }

            var obj = context.LegDatosGenerales
                .Include(i => i.vGradoAcad)
                .Include(i => i.vCondicionColeg)
                .Include(i => i.vTipoDomicilio)
                .Include(i => i.vTipoDoc)
                .Include(i => i.vSexo)
                .Include(i => i.vEstadoCivil)
                .Include(i => i.vNacimiento)
                .Include(i => i.CLegDatColegioProfNavigation)
                .Include(i => i.LegAdminitrativaCarga.Where(p => p.CLegAdmEstado == true))
                    .ThenInclude(cs => cs.vCargo)
                    .Include(i => i.LegAdminitrativaCarga)
                    .ThenInclude(cs => cs.CLegAdmInstitucionNavigation)
                .Include(i => i.LegCapacitaciones.Where(p => p.CLegCapEstado == true))
                    .ThenInclude(cs => cs.vInstitucion)
                    .Include(i => i.LegCapacitaciones)
                    .ThenInclude(cs => cs.vTipo)
                    .Include(i => i.LegCapacitaciones)
                    .ThenInclude(cs => cs.vTipoEsp)
                .Include(i => i.LegCategoriaDocente.Where(p => p.CLegCatEstado == true))
                    .ThenInclude(cs => cs.vCategoria)
                    .Include(i => i.LegCategoriaDocente)
                    .ThenInclude(cs => cs.CLegCatInstitucionNavigation)
                .Include(i => i.LegDocenciaUniv.Where(p => p.CLegDocEstado == true))
                    .ThenInclude(cs => cs.vCategoria)
                    .Include(i => i.LegDocenciaUniv)
                    .ThenInclude(cs => cs.vRegimen)
                    .Include(i => i.LegDocenciaUniv)
                    .ThenInclude(cs => cs.CLegDocUniversidadNavigation)
                .Include(i => i.LegGradoTitulo.Where(p => p.CLegGraEstado == true))
                    .ThenInclude(cs => cs.vGradoAcad)
                    .Include(i => i.LegGradoTitulo)
                    .ThenInclude(cs => cs.vPais)
                    .Include(i => i.LegGradoTitulo)
                    .ThenInclude(cs => cs.CLegGraInstitucionNavigation)
                .Include(i => i.LegIdiomaOfimatica.Where(p => p.CLegIdOfEstado == true))
                    .ThenInclude(cs => cs.vCodigoDesc)
                    .Include(i => i.LegIdiomaOfimatica)
                    .ThenInclude(cs => cs.vNivel)
                .Include(i => i.LegInvestigador.Where(p => p.CLegInvEstado == true))
                    .ThenInclude(cs => cs.vCentroRegistro)
                .Include(i => i.LegInvestigador.Where(p => p.CLegInvEstado == true))
                    .ThenInclude(cs => cs.vNivelRenacyt)
                .Include(i => i.LegParticipacionCongSem.Where(p => p.CLegParEstado == true))
                    .ThenInclude(cs => cs.vAmbito)
                    .Include(i => i.LegParticipacionCongSem)
                    .ThenInclude(cs => cs.vRol)
                    .Include(i => i.LegParticipacionCongSem)
                    .ThenInclude(cs => cs.CLegParInstitucionNavigation)
                .Include(i => i.LegProduccionCiencia.Where(p => p.CLegProdEstado == true))
                    .ThenInclude(cs => cs.vTipo)
                .Include(i => i.LegProfesNoDocente.Where(p => p.CLegProEstado == true))
                    .ThenInclude(cs => cs.vCargo)
                    .Include(i => i.LegProfesNoDocente)
                    .ThenInclude(cs => cs.CLegProInstitucionNavigation)
                .Include(i => i.LegProyeccionSocial.Where(p => p.CLegProyEstado == true))
                    .ThenInclude(cs => cs.vTipo)
                    .Include(i => i.LegProyeccionSocial)
                    .ThenInclude(cs => cs.CLegProyInstitucionNavigation)
                .Include(i => i.LegReconocimiento.Where(p => p.CLegRecEstado == true))
                    .ThenInclude(cs => cs.vTipo)
                    .Include(i => i.LegReconocimiento)
                    .ThenInclude(cs => cs.vDocumento)
                    .Include(i => i.LegReconocimiento)
                    .ThenInclude(cs => cs.CLegRecInstitucionNavigation)
                .Include(i => i.LegRegimenDedicacion.Where(p => p.CLegRegEstado == true))
                    .ThenInclude(cs => cs.vDedicacion)
                .Include(i => i.LegTesisAseJur.Where(p => p.CLegTesEstado == true))
                    .ThenInclude(cs => cs.vTipo)
                    .Include(i => i.LegTesisAseJur)
                    .ThenInclude(cs => cs.vNivel)

                // EBS - 01/2026 ---------------------- >
                // Licencias Profesionales
                .Include(i => i.LegLicenciaProfesional.Where(p => p.CLegLicEstado == true))
                    .ThenInclude(cs => cs.vCondicion)
                    .Include(i => i.LegLicenciaProfesional)
                    .ThenInclude(cs => cs.vPais)
                    .Include(i => i.LegLicenciaProfesional)
                    .ThenInclude(cs => cs.CLegLicInstitucionNavigation)
                // Membresias
                .Include(i => i.LegMembresia.Where(p => p.CLegMemEstado == true))
                    .Include(i => i.LegMembresia)
                    .ThenInclude(cs => cs.vPais)
                    .Include(i => i.LegMembresia)
                    .ThenInclude(cs => cs.CLegMemInstitucionNavigation)
                // EBS - 01/2026  <----------------------

                .FirstOrDefault(x => x.cPerCodigo == parcCodigo);

            obj.CLegDatFoto = obj.CLegDatFoto.Trim() == string.Empty ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDatFoto, true)));
            obj.cLegDatSunedu = obj.cLegDatSunedu.Trim() == string.Empty ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.cLegDatSunedu, true)));
            foreach (LegAdminitrativaCarga f in obj.LegAdminitrativaCarga)
            {
                f.CLegAdmArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegAdmArchivo, true)));
            }
            foreach (LegCapacitaciones f in obj.LegCapacitaciones)
            {
                f.CLegCapArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegCapArchivo, true)));
            }
            foreach (LegCategoriaDocente f in obj.LegCategoriaDocente)
            {
                f.CLegCatArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegCatArchivo, true)));
            }
            foreach (LegDocenciaUniv f in obj.LegDocenciaUniv)
            {
                f.CLegDocArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegDocArchivo, true)));
            }
            foreach (LegGradoTitulo f in obj.LegGradoTitulo)
            {
                f.CLegGraArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegGraArchivo, true)));
            }
            foreach (LegIdiomaOfimatica f in obj.LegIdiomaOfimatica)
            {
                f.CLegIdOfArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegIdOfArchivo, true)));
            }
            foreach (LegInvestigador f in obj.LegInvestigador)
            {
                f.CLegInvArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegInvArchivo, true)));
            }
            foreach (Entity.LegParticipacionCongSem f in obj.LegParticipacionCongSem)
            {
                f.CLegParArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegParArchivo, true)));
            }
            foreach (LegProduccionCiencia f in obj.LegProduccionCiencia)
            {
                f.CLegProdArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegProdArchivo, true)));
            }
            foreach (LegProfesNoDocente f in obj.LegProfesNoDocente)
            {
                f.CLegProArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegProArchivo, true)));
            }
            foreach (LegProyeccionSocial f in obj.LegProyeccionSocial)
            {
                f.CLegProyArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegProyArchivo, true)));
            }
            foreach (LegReconocimiento f in obj.LegReconocimiento)
            {
                f.CLegRecArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegRecArchivo, true)));
            }
            foreach (LegRegimenDedicacion f in obj.LegRegimenDedicacion)
            {
                f.CLegRegArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegRegArchivo, true)));
            }
            foreach (LegTesisAseJur f in obj.LegTesisAseJur)
            {
                f.CLegTesArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegTesArchivo, true)));
            }

            foreach (LegCapacitacionInterna f in obj.LegCapacitacionInternas)
            {
                f.CLegCiarchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegCiarchivo, true)));
            }

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Datos encontrados", obj));
        }

        [HttpGet]
        [Authorize]
        public ActionResult<IEnumerable<LegDatosGenerales>> Get()
        {
            try
            {
                var obj = context.LegDatosGenerales.Include(i => i.vTipoDoc).Include(i => i.vSexo).Include(i => i.vEstadoCivil).Include(i => i.vPais).Include(i => i.vGradoAcad).ToList();

                if (obj.Count > 0)
                {
                    foreach (LegDatosGenerales x in obj)
                    {
                        x.CLegDatFoto = x.CLegDatFoto.Trim() == string.Empty ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.CLegDatFoto, true)));
                        x.cLegDatSunedu = x.cLegDatSunedu.Trim() == string.Empty ? string.Empty : Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.cLegDatSunedu, true)));
                        foreach (LegGradoTitulo f in x.LegGradoTitulo)
                        {
                            f.CLegGraArchivo = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.CLegGraArchivo, true)));
                        }
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

        [HttpGet("/api/legajo_percodigo/{cPerCodigo}")]
        [Authorize]
        public ActionResult<LegDatosGenerales> GetCodigPer(string cPerCodigo)
        {
            try
            {
                var obj = context.LegDatosGenerales.Select(x => new LegDatosGenerales
                {
                    NLegDatCodigo = x.NLegDatCodigo,
                    NLegDatTipoDoc = x.NLegDatTipoDoc,
                    NClaseTipoDoc = x.NClaseTipoDoc,
                    CLegDatNroDoc = x.CLegDatNroDoc,
                    CLegDatApellidoPaterno = x.CLegDatApellidoPaterno,
                    CLegDatApellidoMaterno = x.CLegDatApellidoMaterno,
                    CLegDatNombres = x.CLegDatNombres,
                    DLegDatFechaNacimiento = x.DLegDatFechaNacimiento,
                    NLegDatSexo = x.NLegDatSexo,
                    NClaseSexo = x.NClaseSexo,
                    NLegDatEstadoCivil = x.NLegDatEstadoCivil,
                    NClaseEstadoCivil = x.NClaseEstadoCivil,
                    cLegDatSunedu = x.cLegDatSunedu,
                    cLegDatFirma = x.cLegDatFirma,
                    cLegDatPolicial = x.cLegDatPolicial,
                    cLegDatJudicial = x.cLegDatJudicial,
                    CLegDatEmail = x.CLegDatEmail,
                    CLegDatTelefono = x.CLegDatTelefono,
                    CLegDatMovil = x.CLegDatMovil,
                    NLegDatGradoAcad = x.NLegDatGradoAcad,
                    NClaseGradoAcad = x.NClaseGradoAcad,
                    NLegDatPais = x.NLegDatPais,
                    NClasePais = x.NClasePais,
                    CLegDatAcerca = x.CLegDatAcerca,
                    NLegDatTipoDomicilio = x.NLegDatTipoDomicilio,
                    NValorTipoDomicilio = x.NValorTipoDomicilio,
                    CLegDatCalleDomicilio = x.CLegDatCalleDomicilio,
                    CLegDatNroDomicilio = x.CLegDatNroDomicilio,
                    CLegDatMzaDomicilio = x.CLegDatMzaDomicilio,
                    CLegDatLtDomicilio = x.CLegDatLtDomicilio,
                    CLegDatDptoDomicilio = x.CLegDatDptoDomicilio,
                    CLegDatReferencia = x.CLegDatReferencia,
                    NLetDatUbigeo = x.NLetDatUbigeo,
                    NClaseUbigeo = x.NClaseUbigeo,
                    NLetDatNacimiento = x.NLetDatNacimiento,
                    NClaseNacimiento = x.NClaseNacimiento,
                    CLegDatColegioProf = x.CLegDatColegioProf,
                    CLegDatNroColegiatura = x.CLegDatNroColegiatura,
                    NLegDatCondicionColeg = x.NLegDatCondicionColeg,
                    NValorCondicionColeg = x.NValorCondicionColeg,
                    DLegDatosFechaEmisionColeg = x.DLegDatosFechaEmisionColeg,
                    DLegDatosFechaExpiraColeg = x.DLegDatosFechaExpiraColeg,
                    CLegDatEstado = x.CLegDatEstado,
                    cPerCodigo = x.cPerCodigo,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,
                    vPais = this.repositoryinterf.GetInterfaceDatos(x.NLegDatPais, x.NClasePais).Result ?? null,
                    vSexo = repositorycons.GetConstanteDatos(x.NLegDatSexo, x.NClaseSexo).Result ?? null,
                    vEstadoCivil = repositorycons.GetConstanteDatos(x.NLegDatEstadoCivil, x.NClaseEstadoCivil).Result ?? null,
                    vTipoDoc = repositoryinterf.GetInterfaceDatos(x.NLegDatTipoDoc, x.NClaseTipoDoc).Result ?? null,
                    vTipoDomicilio = repositorycons.GetConstanteDatos(x.NLegDatTipoDomicilio, x.NValorTipoDomicilio).Result ?? null,
                    vNacimiento = repositoryinterf.GetInterfaceDatos(x.NLetDatNacimiento, x.NClaseNacimiento).Result ?? null,
                    vGradoAcad = repositoryinterf.GetInterfaceDatos(x.NLegDatGradoAcad, x.NClaseGradoAcad).Result ?? null
                }).FirstOrDefault(x => x.cPerCodigo == cPerCodigo);

                if (obj is null)
                {
                    return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, false, "No tiene datos registrados", obj));
                }

                //obj.CLegDatFoto = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDatFoto, true)));
                //obj.cLegDatSunedu = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.cLegDatSunedu, true)));
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Datos de legajo encontrados", obj));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpGet("/api/legajo_pdf/{cPerCodigo}/{bDocente}")]
        public IActionResult CreatePDF(string cPerCodigo, String bDocente)
        {
            FuncionesGenerales funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
            LegDatosGenerales obj = funcion.LegDatosReturn(Encoding.ASCII.GetString(Convert.FromBase64String(cPerCodigo)));
            var codigodg = obj.NLegDatCodigo;
            obj.LegAdminitrativaCarga = funcion.LegDatosCargaAdministrativa(codigodg);
            obj.LegCapacitaciones = funcion.LegDatosCapacitaciones(codigodg);
            //obj.LegCapacitacionInternas = funcion.LegDatosCapacitacionInterna(codigodg);
            obj.LegCategoriaDocente = funcion.LegDatosCategoriaDocente(codigodg);
            obj.LegDocenciaUniv = funcion.LegDatosDocenciaUniv(codigodg);
            obj.LegGradoTitulo = funcion.LegDatosGradoTitulo(codigodg);
            obj.LegIdiomaOfimatica = funcion.LegDatosIdiomaOfimatica(codigodg);
            obj.LegInvestigador = funcion.LegDatosInvestigador(codigodg);
            obj.LegParticipacionCongSem = funcion.LegDatosParticipacion(codigodg);
            obj.LegProduccionCiencia = funcion.LegDatosProduccionCiencia(codigodg);
            obj.LegProfesNoDocente = funcion.LegDatosExperienciaNoDocente(codigodg);
            obj.LegProyeccionSocial = funcion.LegDatosProyeccionSocial(codigodg);
            obj.LegReconocimiento = funcion.LegDatosReconocimiento(codigodg);
            obj.LegRegimenDedicacion = funcion.LegDatosRegimenDedicacion(codigodg);
            obj.LegTesisAseJur = funcion.LegDatosTesisAsJur(codigodg);

            // EBS - 01/2026 ---------------------------->
            /* Listado Virtual para licencias o registros profesionales (colegios, nº colegiatura, condición, fechas, etc.) */
            obj.LegLicenciaProfesional = funcion.LegDatosLicenciaProfesional(codigodg);
            /* Listado Virtual para membresias (colegios, nº colegiatura, fechas, etc.) */
            obj.LegMembresia = funcion.LegDatosMembresia(codigodg);
            // EBS - 01/2026 <----------------------------

            string title = "CV - " + obj.CLegDatApellidoPaterno + " " + obj.CLegDatApellidoMaterno + " " + obj.CLegDatNombres;
            //string foto = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDatFoto, true)));

            //File(imageFileStream, (ext == "png" ? "image/png" : (ext == "jpeg" ? "image/jpeg" : "image/jpg")));
            var globalSettings = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings { Top = 10 },
                DocumentTitle = title
            };
            var objectSettings = new ObjectSettings
            {
                PagesCount = true,
                HtmlContent = LegajoTemplate.GetHTMLLegajo(obj, (Encoding.ASCII.GetString(Convert.FromBase64String(bDocente)) == "2" ? true : false), config),
                WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                //HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                //FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
            };
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSettings,
                Objects = { objectSettings }
            };
            var file = _converter.Convert(pdf);
            return File(file, "application/pdf");
            //return File(file, "application/pdf", title + ".pdf");
        }

        [HttpPost]
        [Authorize]
        public ActionResult PostDatosGenerales([FromForm] LegDatosGenerales datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                //return new JsonResult(new Mensajes(200, false, "PRUEBA", datos));
                String directorio = this.config.GetValue<string>("ServerLegajos"); //D:/Proyectos/USS"

                string path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc); //"LegajosUSS/" 
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
                    ruteFile = this.config.GetValue<string>("PhotoCV") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegDatFoto = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegDatFoto = "";
                }

                if (datos.cFileFirma != null)
                {
                    fileName = Path.GetFileName(datos.cFileFirma.FileName);
                    extension = Path.GetExtension(datos.cFileFirma.FileName);
                    ruteFile = this.config.GetValue<string>("firmadig") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileFirma.CopyTo(stream);
                    }

                    datos.cLegDatFirma = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.cLegDatFirma = "";
                }

                if (datos.cFilePolicial != null)
                {
                    fileName = Path.GetFileName(datos.cFilePolicial.FileName);
                    extension = Path.GetExtension(datos.cFilePolicial.FileName);
                    ruteFile = this.config.GetValue<string>("antpolicial") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFilePolicial.CopyTo(stream);
                    }

                    datos.cLegDatPolicial = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.cLegDatPolicial = "";
                }

                if (datos.cFileJudicial != null)
                {
                    fileName = Path.GetFileName(datos.cFileJudicial.FileName);
                    extension = Path.GetExtension(datos.cFileJudicial.FileName);
                    ruteFile = this.config.GetValue<string>("antjudicial") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileJudicial.CopyTo(stream);
                    }

                    datos.cLegDatJudicial = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.cLegDatJudicial = "";
                }

                if (datos.cFileSunedu != null)
                {
                    fileName = Path.GetFileName(datos.cFileSunedu.FileName);
                    extension = Path.GetExtension(datos.cFileSunedu.FileName);
                    ruteFile = this.config.GetValue<string>("sunedu") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileSunedu.CopyTo(stream);
                    }

                    datos.cLegDatSunedu = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.cLegDatSunedu = "";
                }

                datos.CUsuRegistro = funcion.cPerCodAux;
                datos.DFechaRegistro = DateTime.Now;
                var forcont = 0;

                //DECLARACIÓN JURADA
                foreach (LegDeclaracionJurada objF in datos.LegDeclaracionJurada.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFileDjanexo2 != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjanexo2.FileName);
                        extension = Path.GetExtension(objF.cFileDjanexo2.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoAnx2") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjanexo2.CopyTo(stream);
                        }

                        objF.CLegDjanexo2 = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegDjanexo2 = "";
                    }

                    if (objF.cFileDjanexo6 != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjanexo6.FileName);
                        extension = Path.GetExtension(objF.cFileDjanexo6.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoAnx6") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjanexo6.CopyTo(stream);
                        }

                        objF.CLegDjanexo6 = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegDjanexo6 = "";
                    }

                    if (objF.cFileDjanexo7 != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjanexo7.FileName);
                        extension = Path.GetExtension(objF.cFileDjanexo7.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoAnx7") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjanexo7.CopyTo(stream);
                        }

                        objF.CLegDjanexo7 = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegDjanexo7 = "";
                    }


                    // ------------------ EDGAR_BS-2025---------------------------------------->
                    if (objF.cFileDjanexo1 != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjanexo1.FileName);
                        extension = Path.GetExtension(objF.cFileDjanexo1.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoAnx1") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjanexo1.CopyTo(stream);
                        }
                        objF.CLegDjanexo1 = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else{
                        objF.CLegDjanexo1 = "";
                    }

                    if (objF.cFileDjanexo2_2 != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjanexo2_2.FileName);
                        extension = Path.GetExtension(objF.cFileDjanexo2_2.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoAnx2_2") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjanexo2_2.CopyTo(stream);
                        }

                        objF.CLegDjanexo2_2 = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else{
                        objF.CLegDjanexo2_2 = "";
                    }

                    if (objF.cFileDjanexo3 != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjanexo3.FileName);
                        extension = Path.GetExtension(objF.cFileDjanexo3.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoAnx3") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjanexo3.CopyTo(stream);
                        }

                        objF.CLegDjanexo3 = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else{
                        objF.CLegDjanexo3 = "";
                    }

                    if (objF.cFileDjanexo4 != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjanexo4.FileName);
                        extension = Path.GetExtension(objF.cFileDjanexo4.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoAnx4") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjanexo4.CopyTo(stream);
                        }

                        objF.CLegDjanexo4 = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else{
                        objF.CLegDjanexo4 = "";
                    }

                    if (objF.cFileDjanexo5 != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjanexo5.FileName);
                        extension = Path.GetExtension(objF.cFileDjanexo5.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoAnx5") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjanexo5.CopyTo(stream);
                        }

                        objF.CLegDjanexo5 = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else{
                        objF.CLegDjanexo5 = "";
                    }

                    if (objF.cFileDjanexo6_2 != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjanexo6_2.FileName);
                        extension = Path.GetExtension(objF.cFileDjanexo6_2.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoAnx6_2") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjanexo6_2.CopyTo(stream);
                        }

                        objF.CLegDjanexo6_2 = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else{
                        objF.CLegDjanexo6_2 = "";
                    }

                    if (objF.cFileDjDNI != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjDNI.FileName);
                        extension = Path.GetExtension(objF.cFileDjDNI.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoDNI") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjDNI.CopyTo(stream);
                        }

                        objF.CLegDjDNI = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else{
                        objF.CLegDjDNI = "";
                    }

                    if (objF.cFileDjDNI_DH != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjDNI_DH.FileName);
                        extension = Path.GetExtension(objF.cFileDjDNI_DH.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoDNI_DH") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjDNI_DH.CopyTo(stream);
                        }
                        objF.CLegDjDNI_DH = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else{
                        objF.CLegDjDNI_DH = "";
                    }


                    if (objF.cFileDjFotoCarnet != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjFotoCarnet.FileName);
                        extension = Path.GetExtension(objF.cFileDjFotoCarnet.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoFotoCarnet") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjFotoCarnet.CopyTo(stream);
                        }
                        objF.CLegDjFotoCarnet = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else{
                        objF.CLegDjFotoCarnet = "";
                    }

                    if (objF.cFileDjNumCta != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjNumCta.FileName);
                        extension = Path.GetExtension(objF.cFileDjNumCta.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoNumCta") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjNumCta.CopyTo(stream);
                        }

                        objF.CLegDjNumCta = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else{
                        objF.CLegDjNumCta = "";
                    }


                    if (objF.cFileDjConsJubilacion != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjConsJubilacion.FileName);
                        extension = Path.GetExtension(objF.cFileDjConsJubilacion.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoConsJubilacion") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjConsJubilacion.CopyTo(stream);
                        }

                        objF.CLegDjConsJubilacion = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else{
                        objF.CLegDjConsJubilacion = "";
                    }

                    if (objF.cFileDjConsAfiliacionOnpAfp != null)
                    {
                        fileName = Path.GetFileName(objF.cFileDjConsAfiliacionOnpAfp.FileName);
                        extension = Path.GetExtension(objF.cFileDjConsAfiliacionOnpAfp.FileName);
                        ruteFile = this.config.GetValue<string>("LegajoConsAfiliacionOnpAfp") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFileDjConsAfiliacionOnpAfp.CopyTo(stream);
                        }

                        objF.CLegDjConsAfiliacionOnpAfp = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegDjConsAfiliacionOnpAfp = "";
                    }
                    // ------------------ EDGAR_BS-2025---------------------------------------->


                    forcont++;
                    //return new JsonResult(new Mensajes(200, true, "Currículo ha sido registrado.", objFor.fForDiploma));
                }

                forcont = 0;
                //GRADOS-TITULOS
                foreach (LegGradoTitulo objF in datos.LegGradoTitulo.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo01") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegGraArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegGraArchivo = "";
                    }
                    forcont++;
                    //return new JsonResult(new Mensajes(200, true, "Currículo ha sido registrado.", objFor.fForDiploma));
                }

                forcont = 0;
                //EXPERIENCIA DOCENCIA UNIVERSITARIA
                foreach (LegDocenciaUniv objF in datos.LegDocenciaUniv.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo02") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegDocArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegDocArchivo = "";
                    }
                    forcont++;
                }

                forcont = 0;
                //CATEGORIA DOCENTE
                foreach (LegCategoriaDocente objF in datos.LegCategoriaDocente.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo03") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegCatArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegCatArchivo = "";
                    }
                    forcont++;
                }

                forcont = 0;
                //RÉGIMEN DEDICACIÓN
                foreach (LegRegimenDedicacion objF in datos.LegRegimenDedicacion.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo04") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegRegArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegRegArchivo = "";
                    }
                    forcont++;
                }

                forcont = 0;
                //EXPERIENCIA PROFESIONAL NO DOCENTE
                foreach (LegProfesNoDocente objF in datos.LegProfesNoDocente.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo05") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegProArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegProArchivo = "";
                    }
                    forcont++;
                }

                forcont = 0;
                //IDIOMA o OFIMÁTICA
                foreach (LegIdiomaOfimatica objF in datos.LegIdiomaOfimatica.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo06") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegIdOfArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegIdOfArchivo = "";
                    }
                    forcont++;
                }

                forcont = 0;
                //DOCENTE INVESTIGADOR
                foreach (LegInvestigador objF in datos.LegInvestigador.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo07") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegInvArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegInvArchivo = "";
                    }
                    forcont++;
                }

                forcont = 0;
                // ASESORÍA Y JURADO DE TESIS
                foreach (LegTesisAseJur objF in datos.LegTesisAseJur.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo08") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegTesArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegTesArchivo = "";
                    }
                    forcont++;
                }

                forcont = 0;
                // PRODUCCIÓN CIENTÍFICA, LECTIVA Y DE INVESTIGACIÓN
                foreach (LegProduccionCiencia objF in datos.LegProduccionCiencia.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo09") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegProdArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegProdArchivo = "";
                    }
                    forcont++;
                }

                forcont = 0;
                // PARTICIPACIÓN EN CONGRESOS, TALLERES, SEMINARIOS Y OTROS
                foreach (Entity.LegParticipacionCongSem objF in datos.LegParticipacionCongSem.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo10") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegParArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegParArchivo = "";
                    }
                    forcont++;
                }

                forcont = 0;
                // CARGA ADMINISTRATIVA UNIVERSITARIA
                foreach (LegAdminitrativaCarga objF in datos.LegAdminitrativaCarga.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo11") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegAdmArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegAdmArchivo = "";
                    }
                    forcont++;
                }

                forcont = 0;
                // RECONOCIMIENTO DE OTRAS INSTITUCIONES
                foreach (LegReconocimiento objF in datos.LegReconocimiento.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo12") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegRecArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegRecArchivo = "";
                    }
                    forcont++;
                }

                forcont = 0;
                // CAPACITACIONES 
                foreach (LegCapacitaciones objF in datos.LegCapacitaciones.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo13") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegCapArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegCapArchivo = "";
                    }
                    forcont++;
                }

                forcont = 0;
                // PROYECCIÓN SOCIAL
                foreach (LegProyeccionSocial objF in datos.LegProyeccionSocial.ToList())
                {

                    objF.CUsuRegistro = datos.CUsuRegistro;
                    objF.DFechaRegistro = DateTime.Now;
                    if (objF.cFile != null)
                    {
                        fileName = Path.GetFileName(objF.cFile.FileName);
                        extension = Path.GetExtension(objF.cFile.FileName);
                        ruteFile = this.config.GetValue<string>("Legajo14") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                        {
                            objF.cFile.CopyTo(stream);
                        }

                        objF.CLegProyArchivo = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                    }
                    else
                    {
                        objF.CLegProyArchivo = "";
                    }
                    forcont++;
                }

                datos.CLegDatEstado = true;
                context.LegDatosGenerales.Add(datos);
                //return new JsonResult(new Mensajes(200, false, "PRUEBA", datos));
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerLegajo", new { id = datos.NLegDatCodigo }, datos);
                //return new JsonResult(new Mensajes(200, true, "Currículo ha sido registrado.", curriculo));
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Legajo ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }

        [HttpPost("put/{pnCodigo}")]
        [Authorize]
        public async Task<ActionResult> PutDatosGenerales(int pnCodigo, [FromForm] LegDatosGenerales datos)
        {
            funcion.getPerCodigoToken(User);
            Console.WriteLine(pnCodigo);
            //return new JsonResult(new Mensajes(200, false, "PRUEBA", datos));
            if (!ModelState.IsValid)
                return new JsonResult(new Mensajes(400, false, "Not a valid model", BadRequest("Not a valid model")));

            if (pnCodigo != datos.NLegDatCodigo)
            {
                return new JsonResult(new Mensajes(400, false, "Código no coincide", BadRequest()));
            }


            using var transaction = context.Database.BeginTransaction();
            try
            {

                String directorio = this.config.GetValue<string>("ServerLegajos");
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.NLegDatCodigo == pnCodigo);
                string path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc); //"LegajosUSS/" 
                string fileName = "";
                string extension = "";
                string ruteFile = "";

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                if (datos.cFileFirma != null)
                {
                    fileName = Path.GetFileName(datos.cFileFirma.FileName);
                    extension = Path.GetExtension(datos.cFileFirma.FileName);
                    ruteFile = this.config.GetValue<string>("firmadig") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileFirma.CopyTo(stream);
                    }

                    datos.cLegDatFirma = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.cLegDatFirma = objLG.cLegDatFirma;
                }


                if (datos.cFileConadis != null)
                {
                    fileName = Path.GetFileName(datos.cFileConadis.FileName);
                    extension = Path.GetExtension(datos.cFileConadis.FileName);
                    ruteFile = this.config.GetValue<string>("conadis_") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileConadis.CopyTo(stream);
                    }

                    datos.cLegDatArchivoConadis = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.cLegDatArchivoConadis = objLG.cLegDatArchivoConadis;
                }

                if (datos.cFilePolicial != null)
                {
                    fileName = Path.GetFileName(datos.cFilePolicial.FileName);
                    extension = Path.GetExtension(datos.cFilePolicial.FileName);
                    ruteFile = this.config.GetValue<string>("antpolicial") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFilePolicial.CopyTo(stream);
                    }

                    datos.cLegDatPolicial = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.cLegDatPolicial = objLG.cLegDatPolicial;
                }

                if (datos.cFileJudicial != null)
                {
                    fileName = Path.GetFileName(datos.cFileJudicial.FileName);
                    extension = Path.GetExtension(datos.cFileJudicial.FileName);
                    ruteFile = this.config.GetValue<string>("antjudicial") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileJudicial.CopyTo(stream);
                    }

                    datos.cLegDatJudicial = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.cLegDatJudicial = objLG.cLegDatJudicial;
                }

                if (datos.cFileSunedu != null)
                {
                    fileName = Path.GetFileName(datos.cFileSunedu.FileName);
                    extension = Path.GetExtension(datos.cFileSunedu.FileName);
                    ruteFile = this.config.GetValue<string>("sunedu") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileSunedu.CopyTo(stream);
                    }

                    datos.cLegDatSunedu = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.cLegDatSunedu = objLG.cLegDatSunedu;
                }


                if (datos.cFileBuenaSalud != null)
                {
                    fileName = Path.GetFileName(datos.cFileBuenaSalud.FileName);
                    extension = Path.GetExtension(datos.cFileBuenaSalud.FileName);
                    ruteFile = this.config.GetValue<string>("cFileBuenaSalud") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileBuenaSalud.CopyTo(stream);
                    }

                    datos.cLegDatBuenaSalud = this.config.GetValue<string>("FolderLegajos") + objLG.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.cLegDatBuenaSalud = objLG.cLegDatBuenaSalud;
                }
                datos.CLegDatFoto = "";
                datos.CUsuRegistro = objLG.CUsuRegistro;
                datos.DFechaRegistro = objLG.DFechaRegistro;
                datos.DFechaModifica = Convert.ToDateTime(DateTime.Now);
                datos.CUsuModifica = funcion.cPerCodAux;

                context.Entry(datos).State = EntityState.Modified;
                await context.SaveChangesAsync();

                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerLegajo", new { id = datos.NLegDatCodigo }, datos);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Legajo ha sido actualizado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpPost("notas/{pnCodigo}")]
        [Authorize]
        public ActionResult PostNotas([FromForm] LegDatosGenerales datos)
        {
            funcion.getPerCodigoToken(User);
            using var transaction = context.Database.BeginTransaction();
            try
            {
                //return new JsonResult(new Mensajes(200, false, "PRUEBA", datos));
                String directorio = this.config.GetValue<string>("ServerLegajos"); //D:/Proyectos/USS"

                string path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc); //"LegajosUSS/" 
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
                    ruteFile = this.config.GetValue<string>("PhotoCV") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFile.CopyTo(stream);
                    }

                    datos.CLegDatFoto = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.CLegDatFoto = "";
                }

                if (datos.cFileFirma != null)
                {
                    fileName = Path.GetFileName(datos.cFileFirma.FileName);
                    extension = Path.GetExtension(datos.cFileFirma.FileName);
                    ruteFile = this.config.GetValue<string>("firmadig") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, ruteFile), FileMode.Create))
                    {
                        datos.cFileFirma.CopyTo(stream);
                    }

                    datos.cLegDatFirma = this.config.GetValue<string>("FolderLegajos") + datos.CLegDatNroDoc + "/" + ruteFile;
                }
                else
                {
                    datos.cLegDatFirma = "";
                }



                datos.CLegDatEstado = true;
                context.LegDatosGenerales.Add(datos);
                //return new JsonResult(new Mensajes(200, false, "PRUEBA", datos));
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerLegajo", new { id = datos.NLegDatCodigo }, datos);
                //return new JsonResult(new Mensajes(200, true, "Currículo ha sido registrado.", curriculo));
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Legajo ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                //transaction.Rollback();
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }

        }


        [HttpGet("/api/getphoto/{fileurl}")]
        [Authorize]
        public IActionResult GetPhoto(string fileurl)
        {
            try
            {
                //return new JsonResult(new Mensajes(200, true, "Currículo ha sido registrado.", fileurl));
                String directorio = this.config.GetValue<string>("ServerLegajos");
                fileurl = EncryptHelper.Decrypt(Encoding.ASCII.GetString(Convert.FromBase64String(fileurl)), true);
                String[] filesplit = fileurl.Split(".");
                String ext = filesplit[filesplit.Length - 1];

                var path = Path.Combine(directorio, fileurl);

                var imageFileStream = System.IO.File.OpenRead(path);

                return File(imageFileStream, (ext == "png" ? "image/png" : (ext == "jpeg" ? "image/jpeg" : "image/jpg")));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }

        [HttpGet("/api/getpdf/{fileurl}")]
        public FileStreamResult GetPDF(string fileurl)
        {
            //try
            //{
            //return new JsonResult(new Mensajes(200, true, "Currículo ha sido registrado.", fileurl));
            String directorio = this.config.GetValue<string>("ServerLegajos");
            fileurl = EncryptHelper.Decrypt(Encoding.ASCII.GetString(Convert.FromBase64String(fileurl)), true);
            String[] filesplit = fileurl.Split(".");
            String ext = filesplit[filesplit.Length - 1];

            var path = Path.Combine(directorio, fileurl);

            var pdfFileStream = System.IO.File.OpenRead(path);

            return File(pdfFileStream, "application/pdf");
        }



        [HttpGet("/api/getanexos/{fileurl}")]
        public FileStreamResult GetAnexos(string fileurl)
        {
            string directorio = this.config.GetValue<string>("ServerLegajos");
            fileurl = Encoding.ASCII.GetString(Convert.FromBase64String(fileurl));
            String[] filesplit = fileurl.Split(".");
            String ext = filesplit[filesplit.Length - 1];

            var path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + "Anexos/" + fileurl); //"LegajosUSS/Anexos/archivo.pdf" 

            var pdfFileStream = System.IO.File.OpenRead(path);

            return File(pdfFileStream, "application/pdf");
        }

        [HttpGet("/api/mensajelegajo/{pnLegDatCodigo}/{pbDocente}")]
        [Authorize]
        public ActionResult GetMensaje(int pnLegDatCodigo, Boolean pbDocente)
        {
            try
            {
                var obj = this._repository.GetMensajeLegajos(pnLegDatCodigo, pbDocente).Result;
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,
                "", obj));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(400, false, ex.Message, ex));
            }
        }



        [HttpGet("/api/legajoexp-pdf/{cPerCodigo}/{bDocente}")]
        public FileStreamResult CombineLegajoImgPdf(string cPerCodigo, string bDocente)
        {
            try
            {
                string directorio = this.config.GetValue<string>("ServerLegajos");
                List<string> list_file = new List<string>();
                FuncionesGenerales funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);

                // Decodifica el código y obtiene los datos generales
                LegDatosGenerales obj = funcion.LegDatosReturn(Encoding.ASCII.GetString(Convert.FromBase64String(cPerCodigo)));
                var codigodg = obj.NLegDatCodigo;
                string dirpath = Path.Combine(directorio, "LegajosUSS", obj.CLegDatNroDoc, "legajoexport");

                if (!Directory.Exists(dirpath))
                {
                    Directory.CreateDirectory(dirpath);
                }

                // Agregar PDF base (sección 1)
                string seccion1 = Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_1.pdf");
                if (System.IO.File.Exists(seccion1))
                {
                    list_file.Add(seccion1);
                }
                else
                {
                    Console.WriteLine($"El archivo {seccion1} no existe.");
                }

                // Genera el CV en PDF a partir de HTML
                obj.LegAdminitrativaCarga = funcion.LegDatosCargaAdministrativa(codigodg);
                obj.LegCapacitaciones = funcion.LegDatosCapacitaciones(codigodg);
                obj.LegCategoriaDocente = funcion.LegDatosCategoriaDocente(codigodg);
                obj.LegDocenciaUniv = funcion.LegDatosDocenciaUniv(codigodg);
                obj.LegGradoTitulo = funcion.LegDatosGradoTitulo(codigodg);
                obj.LegIdiomaOfimatica = funcion.LegDatosIdiomaOfimatica(codigodg);
                obj.LegInvestigador = funcion.LegDatosInvestigador(codigodg);
                obj.LegParticipacionCongSem = funcion.LegDatosParticipacion(codigodg);
                obj.LegProduccionCiencia = funcion.LegDatosProduccionCiencia(codigodg);
                obj.LegProfesNoDocente = funcion.LegDatosExperienciaNoDocente(codigodg);
                obj.LegProyeccionSocial = funcion.LegDatosProyeccionSocial(codigodg);
                obj.LegReconocimiento = funcion.LegDatosReconocimiento(codigodg);
                obj.LegRegimenDedicacion = funcion.LegDatosRegimenDedicacion(codigodg);
                obj.LegTesisAseJur = funcion.LegDatosTesisAsJur(codigodg);
                // Se agregó la asignación para las capacitaciones internas (corregido el nombre de la propiedad)
                obj.LegCapacitacionInternas = funcion.LegDatosCapacitacionInterna(codigodg);
                string title = "CV - " + obj.CLegDatApellidoPaterno + " " + obj.CLegDatApellidoMaterno + " " + obj.CLegDatNombres;

                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = title
                };

                var objectSettings = new ObjectSettings
                {
                    PagesCount = true,
                    HtmlContent = LegajoTemplate.GetHTMLLegajo(obj, (Encoding.ASCII.GetString(Convert.FromBase64String(bDocente)) == "2"), config),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },
                };

                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                var file = _converter.Convert(pdf);
                var cv = Path.Combine(directorio, "LegajosUSS", obj.CLegDatNroDoc, "legajoexport", title + ".pdf");
                System.IO.File.WriteAllBytes(cv, file);
                if (System.IO.File.Exists(cv))
                {
                    list_file.Add(cv);
                }
                else
                {
                    Console.WriteLine($"El archivo generado {cv} no se encontró.");
                }

                // Agregar archivo Sunedu, si existe
                string suneduPath = Path.Combine(directorio, obj.cLegDatSunedu.Trim());
                if (System.IO.File.Exists(suneduPath))
                {
                    list_file.Add(suneduPath);
                }
                else
                {
                    Console.WriteLine($"El archivo {suneduPath} no existe y no será procesado.");
                }

                int contimg = 0;

                // Método auxiliar para agregar archivo tras verificar su existencia
                Action<string> agregarArchivo = (rutaArchivo) =>
                {
                    if (System.IO.File.Exists(rutaArchivo))
                    {
                        list_file.Add(rutaArchivo);
                    }
                    else
                    {
                        Console.WriteLine($"El archivo {rutaArchivo} no existe.");
                    }
                };

                // Procesamiento de archivos de cada sección, evaluando explícitamente el bool? a bool
                foreach (LegGradoTitulo f in obj.LegGradoTitulo.Where(x => x.CLegGraValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegGraArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                foreach (LegDocenciaUniv f in obj.LegDocenciaUniv.Where(x => x.CLegDocValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegDocArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                foreach (LegCategoriaDocente f in obj.LegCategoriaDocente.Where(x => x.CLegCatValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegCatArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                foreach (LegRegimenDedicacion f in obj.LegRegimenDedicacion.Where(x => x.CLegRegValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegRegArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                foreach (LegIdiomaOfimatica f in obj.LegIdiomaOfimatica.Where(x => x.CLegIdOfValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegIdOfArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                foreach (LegInvestigador f in obj.LegInvestigador.Where(x => x.CLegInvValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegInvArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                foreach (LegTesisAseJur f in obj.LegTesisAseJur.Where(x => x.CLegTesValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegTesArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                foreach (LegProduccionCiencia f in obj.LegProduccionCiencia.Where(x => x.CLegProdValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegProdArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                foreach (LegParticipacionCongSem f in obj.LegParticipacionCongSem.Where(x => x.CLegParValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegParArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                foreach (LegAdminitrativaCarga f in obj.LegAdminitrativaCarga.Where(x => x.CLegAdmValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegAdmArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                foreach (LegReconocimiento f in obj.LegReconocimiento.Where(x => x.CLegRecValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegRecArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                foreach (LegCapacitaciones f in obj.LegCapacitaciones.Where(x => x.CLegCapValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegCapArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                foreach (LegProyeccionSocial f in obj.LegProyeccionSocial.Where(x => x.CLegProyValida == true))
                {
                    string ruta = Path.Combine(directorio, f.CLegProyArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                // Sección 2: Contratos
                string seccion2 = Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_2.pdf");
                if (System.IO.File.Exists(seccion2))
                    list_file.Add(seccion2);
                else
                    Console.WriteLine($"El archivo {seccion2} no existe.");

                obj.LegContratos = funcion.LegDatosContrato(codigodg);
                foreach (LegContrato f in obj.LegContratos)
                {
                    string ruta = Path.Combine(directorio, f.CLegConArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                // Sección 3: Resoluciones
                string seccion3 = Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_3.pdf");
                if (System.IO.File.Exists(seccion3))
                    list_file.Add(seccion3);
                else
                    Console.WriteLine($"El archivo {seccion3} no existe.");

                obj.LegResoluciones = funcion.LegDatosResolucion(codigodg);
                foreach (LegResolucion f in obj.LegResoluciones)
                {
                    string ruta = Path.Combine(directorio, f.CLegResArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                // Sección 4: Declaraciones juradas
                string seccion4 = Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_4.pdf");
                if (System.IO.File.Exists(seccion4))
                    list_file.Add(seccion4);
                else
                    Console.WriteLine($"El archivo {seccion4} no existe.");

                obj.LegDeclaracionJurada = funcion.LegDatosDeclaracionJurada(codigodg);
                foreach (LegDeclaracionJurada f in obj.LegDeclaracionJurada)
                {
                    string[] anexos = new string[] {
                        f.CLegDjanexo1,
                        f.CLegDjanexo2_2,
                        f.CLegDjanexo3,
                        f.CLegDjanexo4,
                        f.CLegDjanexo5,
                        f.CLegDjanexo6_2,
                        f.CLegDjDNI,
                        f.CLegDjDNI_DH,
                        f.CLegDjFotoCarnet,
                        f.CLegDjNumCta,
                        f.CLegDjConsJubilacion,
                        f.CLegDjConsAfiliacionOnpAfp
                    };

                    foreach (var anexo in anexos)
                    {
                        string ruta = Path.Combine(directorio, anexo.Trim());
                        if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                            ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                        agregarArchivo(ruta);
                    }
                }

                // Sección 5: Capacitaciones internas
                string seccion5 = Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_5.pdf");
                if (System.IO.File.Exists(seccion5))
                    list_file.Add(seccion5);
                else
                    Console.WriteLine($"El archivo {seccion5} no existe.");

                foreach (LegCapacitacionInterna f in obj.LegCapacitacionInternas.Where(x => x.CLegCiarchivo != null))
                {
                    string ruta = Path.Combine(directorio, f.CLegCiarchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                // Sección 6: Evaluaciones de desempeño
                string seccion6 = Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_6.pdf");
                if (System.IO.File.Exists(seccion6))
                    list_file.Add(seccion6);
                else
                    Console.WriteLine($"El archivo {seccion6} no existe.");

                obj.LegEvaluacionDesemp = funcion.LegDatosEvaluacionDesemp(codigodg);
                foreach (LegEvaluacionDesemp f in obj.LegEvaluacionDesemp)
                {
                    string ruta = Path.Combine(directorio, f.CLegEvalArchivo.Trim());
                    if (!ruta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                        ruta = funcion.imgtopdf(directorio, ruta, contimg++, obj.CLegDatNroDoc);
                    agregarArchivo(ruta);
                }

                // Sección 7: Proceso de selección
                string seccion7 = Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_7.pdf");
                if (System.IO.File.Exists(seccion7))
                    list_file.Add(seccion7);
                else
                    Console.WriteLine($"El archivo {seccion7} no existe.");

                obj.LegSeleccion = funcion.LegDatosSeleccion(codigodg);
                foreach (LegSeleccion f in obj.LegSeleccion)
                {
                    string[] rutasSeleccion = new string[] {
                f.CLegSelEvaluacionCv,
                f.CLegSelClaseModelo,
                f.CLegSelEvaluacionPsico,
                f.CLegSelEntrevistaPers
            };

                    foreach (var ruta in rutasSeleccion)
                    {
                        string rutaCompleta = Path.Combine(directorio, ruta.Trim());
                        if (!rutaCompleta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                            rutaCompleta = funcion.imgtopdf(directorio, rutaCompleta, contimg++, obj.CLegDatNroDoc);
                        agregarArchivo(rutaCompleta);
                    }
                }

                // Sección 8: Proceso de ordinización
                string seccion8 = Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_8.pdf");
                if (System.IO.File.Exists(seccion8))
                    list_file.Add(seccion8);
                else
                    Console.WriteLine($"El archivo {seccion8} no existe.");

                obj.LegOrdinarizacion = funcion.LegDatosOrdinarizacion(codigodg);
                foreach (LegOrdinarizacion f in obj.LegOrdinarizacion)
                {
                    string[] rutasOrdinarizacion = new string[] {
                f.CLegOrdEvaluacionCv,
                f.CLegOrdClaseModelo,
                f.CLegOrdEvaluacionPsico,
                f.CLegOrdEntrevistaPers,
                f.CLegOrdFichaInscripcion
            };

                    foreach (var ruta in rutasOrdinarizacion)
                    {
                        string rutaCompleta = Path.Combine(directorio, ruta.Trim());
                        if (!rutaCompleta.EndsWith("pdf", StringComparison.OrdinalIgnoreCase))
                            rutaCompleta = funcion.imgtopdf(directorio, rutaCompleta, contimg++, obj.CLegDatNroDoc);
                        agregarArchivo(rutaCompleta);
                    }
                }

                // Combina los PDFs usando iTextSharp
                var pdfout = Path.Combine(directorio, "LegajosUSS", obj.CLegDatNroDoc,
                    $"{obj.CLegDatApellidoPaterno} {obj.CLegDatApellidoMaterno} {obj.CLegDatNombres}-LEG.pdf");
                string[] fileNames = list_file.ToArray();
                Document document = new Document(PageSize.A4, 0, 0, 0, 0);
                using (FileStream newFileStream = new FileStream(pdfout, FileMode.Create))
                {
                    PdfCopy writer = new PdfCopy(document, newFileStream);
                    document.Open();
                    foreach (string fileName in fileNames)
                    {
                        if (!System.IO.File.Exists(fileName))
                        {
                            Console.WriteLine($"El archivo {fileName} no se encontró para combinar.");
                            continue;
                        }
                        PdfReader reader = new PdfReader(fileName);
                        reader.ConsolidateNamedDestinations();
                        for (int i = 1; i <= reader.NumberOfPages; i++)
                        {
                            PdfImportedPage page = writer.GetImportedPage(reader, i);
                            writer.AddPage(page);
                        }
                        PRAcroForm form = reader.AcroForm;
                        if (form != null)
                        {
                            writer.CopyDocumentFields(reader);
                        }
                        reader.Close();
                    }
                    writer.Close();
                    document.Close();
                }

                var pdfFileStream = System.IO.File.OpenRead(pdfout);
                if (Directory.Exists(dirpath))
                {
                    Directory.Delete(dirpath, true);
                }
                return File(pdfFileStream, "application/pdf");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al combinar PDFs: " + ex.Message);
                throw;
            }
        }




        // ------------------- Edgar Barreto Sandoval 25-02-2022 --------------------------------------------------------------

        [HttpGet("/api/legajos_exp-pdf/{nTipo}/{cPrdNombre}")]
        public IActionResult ExportarLegajosPdf(int nTipo, string cPrdNombre)
        {
            try
            {
                Constante obj_constante = new Constante();
                String url_descarga = "";
                var list_personal = repositorypersona.GetLista_Personal(nTipo, cPrdNombre);

                String directorio = this.config.GetValue<string>("ServerLegajos");
                

                foreach (DatosUsuario item in list_personal.Result)
                {

                    //if (item.bLegajo == true && (item.cPerNroDoc == "25808901" || item.cPerNroDoc == "26678127" || item.cPerNroDoc == "26714482"
                    //|| item.cPerNroDoc == "26718104" || item.cPerNroDoc == "27284029" || item.cPerNroDoc == "27285992" || item.cPerNroDoc == "27729344"
                    //|| item.cPerNroDoc == "27735174" || item.cPerNroDoc == "27750036" || item.cPerNroDoc == "27754095" || item.cPerNroDoc == "27852043"
                    //|| item.cPerNroDoc == "28100013" || item.cPerNroDoc == "28104245" || item.cPerNroDoc == "28308856" || item.cPerNroDoc == "29268940"
                    //|| item.cPerNroDoc == "29537385" || item.cPerNroDoc == "32739776" || item.cPerNroDoc == "32948407" || item.cPerNroDoc == "32981143s")) { 
                    
                    if ((item.bLegajo == true) && (item.cPerNroDoc.ToString() != ""))
                    {

                        List<string> list_file = new List<string>();
                        //String bDocente = "2";
                        //String cPerCodigo = "1000003633";
                        FuncionesGenerales funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
                        LegDatosGenerales obj = funcion.LegDatosReturn(item.cPerCodigo);
                        //LegDatosGenerales obj = funcion.LegDatosReturn(cPerCodigo);

                        var codigodg = obj.NLegDatCodigo;
                        string dirpath = Path.Combine(directorio, "LegajosUSS/Exportacion_Lote/" + item.Area + "/" + obj.CLegDatNroDoc + "/legajoexport/");

                       

                        if (!Directory.Exists(dirpath))
                        {
                            Console.WriteLine();
                            Directory.CreateDirectory(dirpath);
                            url_descarga = Path.Combine(directorio, "LegajosUSS/Exportacion_Lote/");
                        }
                        
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_1.pdf"));
                        obj.LegAdminitrativaCarga = funcion.LegDatosCargaAdministrativa(codigodg);
                        obj.LegCapacitaciones = funcion.LegDatosCapacitaciones(codigodg);
                        //obj.LegCapacitacionInternas = funcion.LegDatosCapacitacionInterna(codigodg);
                        obj.LegCategoriaDocente = funcion.LegDatosCategoriaDocente(codigodg);
                        obj.LegDocenciaUniv = funcion.LegDatosDocenciaUniv(codigodg);
                        obj.LegGradoTitulo = funcion.LegDatosGradoTitulo(codigodg);
                        obj.LegIdiomaOfimatica = funcion.LegDatosIdiomaOfimatica(codigodg);
                        obj.LegInvestigador = funcion.LegDatosInvestigador(codigodg);
                        obj.LegParticipacionCongSem = funcion.LegDatosParticipacion(codigodg);
                        obj.LegProduccionCiencia = funcion.LegDatosProduccionCiencia(codigodg);
                        obj.LegProfesNoDocente = funcion.LegDatosExperienciaNoDocente(codigodg);
                        obj.LegProyeccionSocial = funcion.LegDatosProyeccionSocial(codigodg);
                        obj.LegReconocimiento = funcion.LegDatosReconocimiento(codigodg);
                        obj.LegRegimenDedicacion = funcion.LegDatosRegimenDedicacion(codigodg);
                        obj.LegTesisAseJur = funcion.LegDatosTesisAsJur(codigodg);
                        string title = "CV - " + obj.CLegDatApellidoPaterno + " " + obj.CLegDatApellidoMaterno + " " + obj.CLegDatNombres;
                        //string foto = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDatFoto, true)));

                        var globalSettings = new GlobalSettings
                        {
                            ColorMode = ColorMode.Color,
                            Orientation = Orientation.Portrait,
                            PaperSize = PaperKind.A4,
                            Margins = new MarginSettings { Top = 10 },
                            DocumentTitle = title
                        };


                        if (obj.CLegDatApellidoPaterno == null || obj.CLegDatApellidoPaterno == "" || obj.CLegDatApellidoMaterno == " " || obj.CLegDatApellidoMaterno == null || obj.CLegDatNombres == "" || obj.CLegDatNombres == null)
                        {
                            Console.Write("Error" + obj.cPerCodigo);
                        }

                        var objectSettings = new ObjectSettings
                        {
                            PagesCount = true,
                            HtmlContent = LegajoTemplate.GetHTMLLegajo(obj, (nTipo == 2 ? true : false), config),
                            //HtmlContent = LegajoTemplate.GetHTMLLegajo(obj, (bDocente == "2" ? true : false), config),
                            WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },

                        };
                        var pdf = new HtmlToPdfDocument()
                        {
                            GlobalSettings = globalSettings,
                            Objects = { objectSettings }
                        };
                        var file = _converter.Convert(pdf);
                        var cv = Path.Combine(directorio, "LegajosUSS/Exportacion_Lote/" + item.Area + "/" + obj.CLegDatNroDoc + "/legajoexport/" + title + ".pdf");
                        list_file.Add(cv);

                        System.IO.File.WriteAllBytes(cv, file);
                        MemoryStream stream = new MemoryStream(file);

                        if (obj.cLegDatSunedu == null || obj.cLegDatSunedu.Equals(""))
                        {

                        }
                        else
                        {
                            list_file.Add(Path.Combine(directorio, obj.cLegDatSunedu.Trim()));
                        }

                        int contimg = 0;

                        try
                        {
                        //CONVERTIR IMG A PDF
                        foreach (LegGradoTitulo f in obj.LegGradoTitulo.Where(x => x.CLegGraValida == true && x.CLegGraArchivo!= String.Empty))
                        {
                            if (!f.CLegGraArchivo.EndsWith("pdf") && !f.CLegGraArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegGraArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegGraArchivo.Trim()));
                        }
                        foreach (LegDocenciaUniv f in obj.LegDocenciaUniv.Where(x => x.CLegDocValida == true && x.CLegDocArchivo != String.Empty))
                        {
                            if (!f.CLegDocArchivo.EndsWith("pdf") && !f.CLegDocArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDocArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDocArchivo.Trim()));
                        }
                        foreach (LegCategoriaDocente f in obj.LegCategoriaDocente.Where(x => x.CLegCatValida == true && x.CLegCatArchivo != String.Empty)) 
                        { 
                            if (!f.CLegCatArchivo.EndsWith("pdf") && !f.CLegCatArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegCatArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegCatArchivo.Trim()));
                        }
                        foreach (LegRegimenDedicacion f in obj.LegRegimenDedicacion.Where(x => x.CLegRegValida == true && x.CLegRegArchivo != String.Empty))
                        {
                            if (!f.CLegRegArchivo.EndsWith("pdf") && !f.CLegRegArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegRegArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegRegArchivo.Trim()));
                        }
                        foreach (LegIdiomaOfimatica f in obj.LegIdiomaOfimatica.Where(x => x.CLegIdOfTipo == true && x.CLegIdOfValida == true && x.CLegIdOfArchivo!= String.Empty))
                        {
                            if (!f.CLegIdOfArchivo.EndsWith("pdf") && !f.CLegIdOfArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegIdOfArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegIdOfArchivo.Trim()));
                        }
                        foreach (LegIdiomaOfimatica f in obj.LegIdiomaOfimatica.Where(x => x.CLegIdOfTipo == false && x.CLegIdOfValida == true && x.CLegIdOfArchivo != String.Empty))
                        {
                            if (!f.CLegIdOfArchivo.EndsWith("pdf") && !f.CLegIdOfArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegIdOfArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegIdOfArchivo.Trim()));
                        }
                        foreach (LegInvestigador f in obj.LegInvestigador.Where(x => x.CLegInvValida == true && x.CLegInvArchivo != String.Empty))
                        {
                            if (!f.CLegInvArchivo.EndsWith("pdf") && !f.CLegInvArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegInvArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegInvArchivo.Trim()));
                        }
                        foreach (LegTesisAseJur f in obj.LegTesisAseJur.Where(x => x.CLegTesValida == true && x.CLegTesArchivo!= String.Empty))
                        {
                            if (!f.CLegTesArchivo.EndsWith("pdf") && !f.CLegTesArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegTesArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegTesArchivo.Trim()));
                        }
                        foreach (LegProduccionCiencia f in obj.LegProduccionCiencia.Where(x => x.CLegProdValida == true && x.CLegProdArchivo!= String.Empty))
                        {
                            if (!f.CLegProdArchivo.EndsWith("pdf") && !f.CLegProdArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegProdArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegProdArchivo.Trim()));
                        }
                        foreach (LegParticipacionCongSem f in obj.LegParticipacionCongSem.Where(x => x.CLegParValida == true && x.CLegParArchivo!= String.Empty))
                        {
                            if (!f.CLegParArchivo.EndsWith("pdf") && !f.CLegParArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegParArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegParArchivo.Trim()));
                        }
                        foreach (LegAdminitrativaCarga f in obj.LegAdminitrativaCarga.Where(x => x.CLegAdmValida == true && x.CLegAdmArchivo!= String.Empty))
                        {
                            if (!f.CLegAdmArchivo.EndsWith("pdf") && !f.CLegAdmArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegAdmArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegAdmArchivo.Trim()));
                        }
                        foreach (LegReconocimiento f in obj.LegReconocimiento.Where(x => x.CLegRecValida == true && x.CLegRecArchivo!= String.Empty))
                        {
                            if (!f.CLegRecArchivo.EndsWith("pdf") && !f.CLegRecArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegRecArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegRecArchivo.Trim()));
                        }
                        foreach (LegCapacitaciones f in obj.LegCapacitaciones.Where(x => x.CLegCapValida == true && x.CLegCapArchivo!= String.Empty))
                        {
                            if (!f.CLegCapArchivo.EndsWith("pdf") && !f.CLegCapArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegCapArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegCapArchivo.Trim()));
                        }
                        foreach (LegProyeccionSocial f in obj.LegProyeccionSocial.Where(x => x.CLegProyValida == true && x.CLegProyArchivo!= String.Empty))
                        {
                            if (!f.CLegProyArchivo.EndsWith("pdf") && !f.CLegProyArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegProyArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegProyArchivo.Trim()));
                        }
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_2.pdf")); //CONTRATOS
                        obj.LegContratos = funcion.LegDatosContrato(codigodg);
                        foreach (LegContrato f in obj.LegContratos.Where(x => x.CLegConArchivo!= String.Empty))
                        {
                            if (!f.CLegConArchivo.EndsWith("pdf") && !f.CLegConArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegConArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegConArchivo.Trim()));
                        }
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_3.pdf")); //RESOLUCIONES
                        obj.LegResoluciones = funcion.LegDatosResolucion(codigodg);
                        foreach (LegResolucion f in obj.LegResoluciones.Where(x => x.CLegResArchivo!= String.Empty))
                        {
                            if (!f.CLegResArchivo.EndsWith("pdf") && !f.CLegResArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegResArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegResArchivo.Trim()));
                        }
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_4.pdf")); //DECLARACIONES JURADAS
                        obj.LegDeclaracionJurada = funcion.LegDatosDeclaracionJurada(codigodg);
                        foreach (LegDeclaracionJurada f in obj.LegDeclaracionJurada)
                            {

                            // ------------------ EDGAR_BS-2025---------------------------------------->
                            if (!f.CLegDjanexo1.EndsWith("pdf") && !f.CLegDjanexo1.EndsWith("PDF") && f.CLegDjanexo1 != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjanexo1.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjanexo1.Trim()));

                            if (!f.CLegDjanexo2_2.EndsWith("pdf") && !f.CLegDjanexo2_2.EndsWith("PDF") && f.CLegDjanexo2_2 != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjanexo2_2.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjanexo2_2.Trim()));

                            if (!f.CLegDjanexo3.EndsWith("pdf") && !f.CLegDjanexo3.EndsWith("PDF") && f.CLegDjanexo3 != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjanexo3.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjanexo3.Trim()));

                            if (!f.CLegDjanexo4.EndsWith("pdf") && !f.CLegDjanexo4.EndsWith("PDF") && f.CLegDjanexo4 != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjanexo4.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjanexo4.Trim()));

                            if (!f.CLegDjanexo5.EndsWith("pdf") && !f.CLegDjanexo5.EndsWith("PDF") && f.CLegDjanexo5 != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjanexo5.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjanexo5.Trim()));

                            if (!f.CLegDjanexo6_2.EndsWith("pdf") && !f.CLegDjanexo6_2.EndsWith("PDF") && f.CLegDjanexo6_2 != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjanexo6_2.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjanexo6_2.Trim()));

                            if (!f.CLegDjDNI.EndsWith("pdf") && !f.CLegDjDNI.EndsWith("PDF") && f.CLegDjDNI != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjDNI.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjDNI.Trim()));

                            if (!f.CLegDjDNI_DH.EndsWith("pdf") && !f.CLegDjDNI_DH.EndsWith("PDF") && f.CLegDjDNI_DH != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjDNI_DH.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjDNI_DH.Trim()));

                            if (!f.CLegDjFotoCarnet.EndsWith("pdf") && !f.CLegDjFotoCarnet.EndsWith("PDF") && f.CLegDjFotoCarnet != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjFotoCarnet.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjFotoCarnet.Trim()));

                            if (!f.CLegDjNumCta.EndsWith("pdf") && !f.CLegDjNumCta.EndsWith("PDF") && f.CLegDjNumCta != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjNumCta.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjNumCta.Trim()));

                            if (!f.CLegDjConsJubilacion.EndsWith("pdf") && !f.CLegDjConsJubilacion.EndsWith("PDF") && f.CLegDjConsJubilacion != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjConsJubilacion.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjConsJubilacion.Trim()));

                            if (!f.CLegDjConsAfiliacionOnpAfp.EndsWith("pdf") && !f.CLegDjConsAfiliacionOnpAfp.EndsWith("PDF") && f.CLegDjConsAfiliacionOnpAfp != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjConsAfiliacionOnpAfp.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjConsAfiliacionOnpAfp.Trim()));

                            // ------------------ EDGAR_BS-2025---------------------------------------->



                            }
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_5.pdf")); //CAPACITACIONES INTERNAS
                        obj.LegCapacitacionInternas = funcion.LegDatosCapacitacionInterna(codigodg);
                        foreach (LegCapacitacionInterna f in obj.LegCapacitacionInternas.Where(x => x.CLegCiarchivo!= String.Empty))
                        {
                            if (!f.CLegCiarchivo.EndsWith("pdf") && !f.CLegCiarchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegCiarchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegCiarchivo.Trim()));
                        }

                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_6.pdf")); //EVALUACIONES DE DESEMPEÑO
                        obj.LegEvaluacionDesemp = funcion.LegDatosEvaluacionDesemp(codigodg);
                        foreach (LegEvaluacionDesemp f in obj.LegEvaluacionDesemp.Where(x => x.CLegEvalArchivo!= String.Empty))
                        {
                            if (!f.CLegEvalArchivo.EndsWith("pdf") && !f.CLegEvalArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegEvalArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegEvalArchivo.Trim()));
                        }
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_7.pdf")); //PROCESO DE SELECCIÓN
                        obj.LegSeleccion = funcion.LegDatosSeleccion(codigodg);
                        foreach (LegSeleccion f in obj.LegSeleccion)
                        {
                            if (!f.CLegSelEvaluacionCv.EndsWith("pdf") && !f.CLegSelEvaluacionCv.EndsWith("PDF") && f.CLegSelEvaluacionCv != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegSelEvaluacionCv.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegSelEvaluacionCv.Trim()));

                            if (!f.CLegSelClaseModelo.EndsWith("pdf") && !f.CLegSelClaseModelo.EndsWith("PDF") && f.CLegSelClaseModelo != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegSelClaseModelo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegSelClaseModelo.Trim()));

                            if (!f.CLegSelEvaluacionPsico.EndsWith("pdf") && !f.CLegSelEvaluacionPsico.EndsWith("PDF") && f.CLegSelEvaluacionPsico != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegSelEvaluacionPsico.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegSelEvaluacionPsico.Trim()));

                            if (!f.CLegSelEntrevistaPers.EndsWith("pdf") && !f.CLegSelEntrevistaPers.EndsWith("PDF") && f.CLegSelEntrevistaPers != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegSelEntrevistaPers.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegSelEntrevistaPers.Trim()));
                        }
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_8.pdf")); // PROCESO DE ORDINARIZACIÓN
                        obj.LegOrdinarizacion = funcion.LegDatosOrdinarizacion(codigodg);
                        foreach (LegOrdinarizacion f in obj.LegOrdinarizacion)
                        {
                            if (!f.CLegOrdEvaluacionCv.EndsWith("pdf") && !f.CLegOrdEvaluacionCv.EndsWith("PDF") && f.CLegOrdEvaluacionCv != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegOrdEvaluacionCv.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegOrdEvaluacionCv.Trim()));

                            if (!f.CLegOrdClaseModelo.EndsWith("pdf") && !f.CLegOrdClaseModelo.EndsWith("PDF") && f.CLegOrdClaseModelo != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegOrdClaseModelo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegOrdClaseModelo.Trim()));

                            if (!f.CLegOrdEvaluacionPsico.EndsWith("pdf") && !f.CLegOrdEvaluacionPsico.EndsWith("PDF") && f.CLegOrdEvaluacionPsico != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegOrdEvaluacionPsico.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegOrdEvaluacionPsico.Trim()));

                            if (!f.CLegOrdEntrevistaPers.EndsWith("pdf") && !f.CLegOrdEntrevistaPers.EndsWith("PDF") && f.CLegOrdEntrevistaPers != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegOrdEntrevistaPers.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegOrdEntrevistaPers.Trim()));

                            if (!f.CLegOrdFichaInscripcion.EndsWith("pdf") && !f.CLegOrdFichaInscripcion.EndsWith("PDF") && f.CLegOrdFichaInscripcion != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegOrdFichaInscripcion.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegOrdFichaInscripcion.Trim()));
                        }
                         }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        var pdfout = Path.Combine(directorio, "LegajosUSS/Exportacion_Lote/" + item.Area + "/" + obj.CLegDatNroDoc + "/" + obj.CLegDatApellidoPaterno + " " + obj.CLegDatApellidoMaterno + " " + obj.CLegDatNombres + "-LEG.pdf");



                        //Eliminar Archivo Existente
                        //System.IO.File.Delete(pdfout);


                        string[] fileNames = list_file.ToArray();
                        string outFile = pdfout;
                        // step 1: creation of a document-object
                        Document document = new Document(PageSize.A4, 0, 0, 0, 0);
                        //create newFileStream object which will be disposed at the end
                        using (FileStream newFileStream = new FileStream(outFile, FileMode.Create))
                        {
                            // step 2: we create a writer that listens to the document
                            PdfCopy writer = new PdfCopy(document, newFileStream);
                            //if (writer == null)
                            //{
                            //    return;
                            //}

                            // step 3: we open the document
                            document.Open();
                            foreach (string fileName in fileNames)
                            {
                                //Verificar tamaño del archivo
                                FileInfo file_info = new FileInfo(fileName);
                                if (file_info.Length == 0)
                                {
                                    continue;
                                }

                                // we create a reader for a certain document
                                try
                                {
                                    PdfReader reader = new PdfReader(fileName,null);
                                    PdfReader.unethicalreading = true;
                                    reader.ConsolidateNamedDestinations();

                                // step 4: we add content
                                for (int i = 1; i <= reader.NumberOfPages; i++)
                                {
                                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                                    writer.AddPage(page);
                                }

                               
                                    PRAcroForm form = reader.AcroForm;
                                    if (form != null)
                                    {
                                        writer.CopyDocumentFields(reader);
                                        //writer.AddDocument(reader);
                                    }
                                reader.Close();
                                }
                                catch (Exception)
                                {
                                }

                            }

                            // step 5: we close the document and writer
                            writer.Close();
                            document.Close();
                            //var pdfFileStream = System.IO.File.OpenRead(pdfout);
                            System.IO.Directory.Delete(dirpath, true);
                            //File(pdfFileStream, "application/pdf");
                            
                            newFileStream.Close();
                            //pdfFileStream.Close(); 

                        }//disposes the newFileStream object

                    }

                    //}

                }


                
                obj_constante.CConDescripcion = url_descarga;
                
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Directorio de Descarga", obj_constante));

            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpGet("/api/legajos_exp_zip/{nTipo}/{cPrdNombre}/{nUniOrgCodigo}")]
        public IActionResult ExportarLegajosZip(int nTipo, string cPrdNombre,int nUniOrgCodigo)
        {
            try
            {
                Constante obj_constante = new Constante();
                String url_descarga = "";
                var list_personal = repositorypersona.GetLista_Personal(nTipo, cPrdNombre,nUniOrgCodigo);

                String directorio = this.config.GetValue<string>("ServerLegajos");


                foreach (DatosUsuario item in list_personal.Result)
                {

                   

                    if ((item.bLegajo == true) && (item.cPerNroDoc.ToString() != ""))
                    {

                        List<string> list_file = new List<string>();
                       
                        FuncionesGenerales funcion = new FuncionesGenerales(context, repositorycons, repositoryinterf, repositorypersona);
                        LegDatosGenerales obj = funcion.LegDatosReturn(item.cPerCodigo);
                       

                        var codigodg = obj.NLegDatCodigo;
                        string dirpath = Path.Combine(directorio, "LegajosUSS/Exportacion_Lote/" + item.Area + "/" + obj.CLegDatNroDoc + "/legajoexport/");



                        if (!Directory.Exists(dirpath))
                        {
                            Console.WriteLine();
                            Directory.CreateDirectory(dirpath);
                            url_descarga = Path.Combine(directorio, "LegajosUSS/Exportacion_Lote/");
                        }

                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_1.pdf"));
                        obj.LegAdminitrativaCarga = funcion.LegDatosCargaAdministrativa(codigodg);
                        obj.LegCapacitaciones = funcion.LegDatosCapacitaciones(codigodg);
                        //obj.LegCapacitacionInternas = funcion.LegDatosCapacitacionInterna(codigodg);
                        obj.LegCategoriaDocente = funcion.LegDatosCategoriaDocente(codigodg);
                        obj.LegDocenciaUniv = funcion.LegDatosDocenciaUniv(codigodg);
                        obj.LegGradoTitulo = funcion.LegDatosGradoTitulo(codigodg);
                        obj.LegIdiomaOfimatica = funcion.LegDatosIdiomaOfimatica(codigodg);
                        obj.LegInvestigador = funcion.LegDatosInvestigador(codigodg);
                        obj.LegParticipacionCongSem = funcion.LegDatosParticipacion(codigodg);
                        obj.LegProduccionCiencia = funcion.LegDatosProduccionCiencia(codigodg);
                        obj.LegProfesNoDocente = funcion.LegDatosExperienciaNoDocente(codigodg);
                        obj.LegProyeccionSocial = funcion.LegDatosProyeccionSocial(codigodg);
                        obj.LegReconocimiento = funcion.LegDatosReconocimiento(codigodg);
                        obj.LegRegimenDedicacion = funcion.LegDatosRegimenDedicacion(codigodg);
                        obj.LegTesisAseJur = funcion.LegDatosTesisAsJur(codigodg);

                        // EBS - 01/2026 --------------->
                        // Mostrar las Licencias Profesionales del docente
                        obj.LegLicenciaProfesional = funcion.LegDatosLicenciaProfesional(codigodg);
                        // Mostrar las Membresia del docente
                        obj.LegMembresia = funcion.LegDatosMembresia(codigodg);
                        // EBS - 01/2026 --------------->

                        string title = "CV - " + obj.CLegDatApellidoPaterno + " " + obj.CLegDatApellidoMaterno + " " + obj.CLegDatNombres;
                        //string foto = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(obj.CLegDatFoto, true)));

                        var globalSettings = new GlobalSettings
                        {
                            ColorMode = ColorMode.Color,
                            Orientation = Orientation.Portrait,
                            PaperSize = PaperKind.A4,
                            Margins = new MarginSettings { Top = 10 },
                            DocumentTitle = title
                        };


                        if (obj.CLegDatApellidoPaterno == null || obj.CLegDatApellidoPaterno == "" || obj.CLegDatApellidoMaterno == " " || obj.CLegDatApellidoMaterno == null || obj.CLegDatNombres == "" || obj.CLegDatNombres == null)
                        {
                            Console.Write("Error" + obj.cPerCodigo);
                        }

                        var objectSettings = new ObjectSettings
                        {
                            PagesCount = true,
                            HtmlContent = LegajoTemplate.GetHTMLLegajo(obj, (nTipo == 2 ? true : false), config),
                            
                            WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "assets", "styles.css") },

                        };
                        var pdf = new HtmlToPdfDocument()
                        {
                            GlobalSettings = globalSettings,
                            Objects = { objectSettings }
                        };
                        var file = _converter.Convert(pdf);
                        var cv = Path.Combine(directorio, "LegajosUSS/Exportacion_Lote/" + item.Area + "/" + obj.CLegDatNroDoc + "/legajoexport/" + title + ".pdf");
                        list_file.Add(cv);

                        System.IO.File.WriteAllBytes(cv, file);
                        MemoryStream stream = new MemoryStream(file);

                        if (obj.cLegDatSunedu == null || obj.cLegDatSunedu.Equals(""))
                        {

                        }
                        else
                        {
                            list_file.Add(Path.Combine(directorio, obj.cLegDatSunedu.Trim()));
                        }

                        int contimg = 0;
                        try{
                        //CONVERTIR IMG A PDF
                        foreach (LegGradoTitulo f in obj.LegGradoTitulo.Where(x => x.CLegGraValida == true && x.CLegGraArchivo!= String.Empty))
                        {
                            if (!f.CLegGraArchivo.EndsWith("pdf") && !f.CLegGraArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegGraArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegGraArchivo.Trim()));
                        }
                        foreach (LegDocenciaUniv f in obj.LegDocenciaUniv.Where(x => x.CLegDocValida == true && x.CLegDocArchivo != String.Empty))
                        {
                            if (!f.CLegDocArchivo.EndsWith("pdf") && !f.CLegDocArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDocArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDocArchivo.Trim()));
                        }
                        foreach (LegCategoriaDocente f in obj.LegCategoriaDocente.Where(x => x.CLegCatValida == true && x.CLegCatArchivo != String.Empty)) 
                        { 
                            if (!f.CLegCatArchivo.EndsWith("pdf") && !f.CLegCatArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegCatArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegCatArchivo.Trim()));
                        }
                        foreach (LegRegimenDedicacion f in obj.LegRegimenDedicacion.Where(x => x.CLegRegValida == true && x.CLegRegArchivo != String.Empty))
                        {
                            if (!f.CLegRegArchivo.EndsWith("pdf") && !f.CLegRegArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegRegArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegRegArchivo.Trim()));
                        }
                        foreach (LegIdiomaOfimatica f in obj.LegIdiomaOfimatica.Where(x => x.CLegIdOfTipo == true && x.CLegIdOfValida == true && x.CLegIdOfArchivo!= String.Empty))
                        {
                            if (!f.CLegIdOfArchivo.EndsWith("pdf") && !f.CLegIdOfArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegIdOfArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegIdOfArchivo.Trim()));
                        }
                        foreach (LegIdiomaOfimatica f in obj.LegIdiomaOfimatica.Where(x => x.CLegIdOfTipo == false && x.CLegIdOfValida == true && x.CLegIdOfArchivo != String.Empty))
                        {
                            if (!f.CLegIdOfArchivo.EndsWith("pdf") && !f.CLegIdOfArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegIdOfArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegIdOfArchivo.Trim()));
                        }
                        foreach (LegInvestigador f in obj.LegInvestigador.Where(x => x.CLegInvValida == true && x.CLegInvArchivo != String.Empty))
                        {
                            if (!f.CLegInvArchivo.EndsWith("pdf") && !f.CLegInvArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegInvArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegInvArchivo.Trim()));
                        }
                        foreach (LegTesisAseJur f in obj.LegTesisAseJur.Where(x => x.CLegTesValida == true && x.CLegTesArchivo!= String.Empty))
                        {
                            if (!f.CLegTesArchivo.EndsWith("pdf") && !f.CLegTesArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegTesArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegTesArchivo.Trim()));
                        }
                        foreach (LegProduccionCiencia f in obj.LegProduccionCiencia.Where(x => x.CLegProdValida == true && x.CLegProdArchivo!= String.Empty))
                        {
                            if (!f.CLegProdArchivo.EndsWith("pdf") && !f.CLegProdArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegProdArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegProdArchivo.Trim()));
                        }
                        foreach (LegParticipacionCongSem f in obj.LegParticipacionCongSem.Where(x => x.CLegParValida == true && x.CLegParArchivo!= String.Empty))
                        {
                            if (!f.CLegParArchivo.EndsWith("pdf") && !f.CLegParArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegParArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegParArchivo.Trim()));
                        }
                        foreach (LegAdminitrativaCarga f in obj.LegAdminitrativaCarga.Where(x => x.CLegAdmValida == true && x.CLegAdmArchivo!= String.Empty))
                        {
                            if (!f.CLegAdmArchivo.EndsWith("pdf") && !f.CLegAdmArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegAdmArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegAdmArchivo.Trim()));
                        }
                        foreach (LegReconocimiento f in obj.LegReconocimiento.Where(x => x.CLegRecValida == true && x.CLegRecArchivo!= String.Empty))
                        {
                            if (!f.CLegRecArchivo.EndsWith("pdf") && !f.CLegRecArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegRecArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegRecArchivo.Trim()));
                        }
                        foreach (LegCapacitaciones f in obj.LegCapacitaciones.Where(x => x.CLegCapValida == true && x.CLegCapArchivo!= String.Empty))
                        {
                            if (!f.CLegCapArchivo.EndsWith("pdf") && !f.CLegCapArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegCapArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegCapArchivo.Trim()));
                        }
                        foreach (LegProyeccionSocial f in obj.LegProyeccionSocial.Where(x => x.CLegProyValida == true && x.CLegProyArchivo!= String.Empty))
                        {
                            if (!f.CLegProyArchivo.EndsWith("pdf") && !f.CLegProyArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegProyArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegProyArchivo.Trim()));
                        }
                        
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_2.pdf")); //CONTRATOS
                        obj.LegContratos = funcion.LegDatosContrato(codigodg);
                        foreach (LegContrato f in obj.LegContratos.Where(x => x.CLegConArchivo!= String.Empty))
                        {
                            if (!f.CLegConArchivo.EndsWith("pdf") && !f.CLegConArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegConArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegConArchivo.Trim()));
                        }
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_3.pdf")); //RESOLUCIONES
                        obj.LegResoluciones = funcion.LegDatosResolucion(codigodg);
                        foreach (LegResolucion f in obj.LegResoluciones.Where(x => x.CLegResArchivo!= String.Empty))
                        {
                            if (!f.CLegResArchivo.EndsWith("pdf") && !f.CLegResArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegResArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegResArchivo.Trim()));
                        }
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_4.pdf")); //DECLARACIONES JURADAS
                        obj.LegDeclaracionJurada = funcion.LegDatosDeclaracionJurada(codigodg);
                        foreach (LegDeclaracionJurada f in obj.LegDeclaracionJurada)
                            {
                            // ------------------ EDGAR_BS-2025---------------------------------------->
                            if (!f.CLegDjanexo1.EndsWith("pdf") && !f.CLegDjanexo1.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjanexo1.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjanexo1.Trim()));

                            if (!f.CLegDjanexo2_2.EndsWith("pdf") && !f.CLegDjanexo2_2.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjanexo2_2.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjanexo2_2.Trim()));

                            if (!f.CLegDjanexo3.EndsWith("pdf") && !f.CLegDjanexo3.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjanexo3.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjanexo3.Trim()));

                            if (!f.CLegDjanexo4.EndsWith("pdf") && !f.CLegDjanexo4.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjanexo4.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjanexo4.Trim()));

                            if (!f.CLegDjanexo5.EndsWith("pdf") && !f.CLegDjanexo5.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjanexo5.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjanexo5.Trim()));

                            if (!f.CLegDjanexo6_2.EndsWith("pdf") && !f.CLegDjanexo6_2.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjanexo6_2.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjanexo6_2.Trim()));

                            if (!f.CLegDjDNI.EndsWith("pdf") && !f.CLegDjDNI.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjDNI.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjDNI.Trim()));

                            if (!f.CLegDjDNI_DH.EndsWith("pdf") && !f.CLegDjDNI_DH.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjDNI_DH.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjDNI_DH.Trim()));

                            if (!f.CLegDjFotoCarnet.EndsWith("pdf") && !f.CLegDjFotoCarnet.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjFotoCarnet.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjFotoCarnet.Trim()));

                            if (!f.CLegDjNumCta.EndsWith("pdf") && !f.CLegDjNumCta.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjNumCta.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjNumCta.Trim()));
                        
                            if (!f.CLegDjConsJubilacion.EndsWith("pdf") && !f.CLegDjConsJubilacion.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjConsJubilacion.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjConsJubilacion.Trim()));

                            if (!f.CLegDjConsAfiliacionOnpAfp.EndsWith("pdf") && !f.CLegDjConsAfiliacionOnpAfp.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegDjConsAfiliacionOnpAfp.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegDjConsAfiliacionOnpAfp.Trim()));

                                // ------------------ EDGAR_BS-2025---------------------------------------->


                            }
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_5.pdf")); //CAPACITACIONES INTERNAS
                        obj.LegCapacitacionInternas = funcion.LegDatosCapacitacionInterna(codigodg);
                        foreach (LegCapacitacionInterna f in obj.LegCapacitacionInternas.Where(x => x.CLegCiarchivo!= String.Empty))
                        {
                            if (!f.CLegCiarchivo.EndsWith("pdf") && !f.CLegCiarchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegCiarchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegCiarchivo.Trim()));
                        }

                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_6.pdf")); //EVALUACIONES DE DESEMPEÑO
                        obj.LegEvaluacionDesemp = funcion.LegDatosEvaluacionDesemp(codigodg);
                        foreach (LegEvaluacionDesemp f in obj.LegEvaluacionDesemp.Where(x => x.CLegEvalArchivo!= String.Empty))
                        {
                            if (!f.CLegEvalArchivo.EndsWith("pdf") && !f.CLegEvalArchivo.EndsWith("PDF"))
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegEvalArchivo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegEvalArchivo.Trim()));
                        }
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_7.pdf")); //PROCESO DE SELECCIÓN
                        obj.LegSeleccion = funcion.LegDatosSeleccion(codigodg);
                        foreach (LegSeleccion f in obj.LegSeleccion)
                        {
                            if (!f.CLegSelEvaluacionCv.EndsWith("pdf") && !f.CLegSelEvaluacionCv.EndsWith("PDF") && f.CLegSelEvaluacionCv != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegSelEvaluacionCv.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegSelEvaluacionCv.Trim()));

                            if (!f.CLegSelClaseModelo.EndsWith("pdf") && !f.CLegSelClaseModelo.EndsWith("PDF") && f.CLegSelClaseModelo != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegSelClaseModelo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegSelClaseModelo.Trim()));

                            if (!f.CLegSelEvaluacionPsico.EndsWith("pdf") && !f.CLegSelEvaluacionPsico.EndsWith("PDF") && f.CLegSelEvaluacionPsico != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegSelEvaluacionPsico.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegSelEvaluacionPsico.Trim()));

                            if (!f.CLegSelEntrevistaPers.EndsWith("pdf") && !f.CLegSelEntrevistaPers.EndsWith("PDF") && f.CLegSelEntrevistaPers != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegSelEntrevistaPers.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegSelEntrevistaPers.Trim()));
                        }
                        list_file.Add(Path.Combine(Directory.GetCurrentDirectory(), "assets", "seccion_8.pdf")); // PROCESO DE ORDINARIZACIÓN
                        obj.LegOrdinarizacion = funcion.LegDatosOrdinarizacion(codigodg);
                        foreach (LegOrdinarizacion f in obj.LegOrdinarizacion)
                        {
                            if (!f.CLegOrdEvaluacionCv.EndsWith("pdf") && !f.CLegOrdEvaluacionCv.EndsWith("PDF") && f.CLegOrdEvaluacionCv != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegOrdEvaluacionCv.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegOrdEvaluacionCv.Trim()));

                            if (!f.CLegOrdClaseModelo.EndsWith("pdf") && !f.CLegOrdClaseModelo.EndsWith("PDF") && f.CLegOrdClaseModelo != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegOrdClaseModelo.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegOrdClaseModelo.Trim()));

                            if (!f.CLegOrdEvaluacionPsico.EndsWith("pdf") && !f.CLegOrdEvaluacionPsico.EndsWith("PDF") && f.CLegOrdEvaluacionPsico != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegOrdEvaluacionPsico.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegOrdEvaluacionPsico.Trim()));

                            if (!f.CLegOrdEntrevistaPers.EndsWith("pdf") && !f.CLegOrdEntrevistaPers.EndsWith("PDF") && f.CLegOrdEntrevistaPers != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegOrdEntrevistaPers.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegOrdEntrevistaPers.Trim()));

                            if (!f.CLegOrdFichaInscripcion.EndsWith("pdf") && !f.CLegOrdFichaInscripcion.EndsWith("PDF") && f.CLegOrdFichaInscripcion != String.Empty)
                                list_file.Add(funcion.imgtopdf2(directorio, Path.Combine(directorio, f.CLegOrdFichaInscripcion.Trim()), contimg++, obj.CLegDatNroDoc, item.Area));
                            else
                                list_file.Add(Path.Combine(directorio, f.CLegOrdFichaInscripcion.Trim()));
                        }
                        }
                        catch(Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }

                        var pdfout = Path.Combine(directorio, "LegajosUSS/Exportacion_Lote/" + item.Area + "/" + obj.CLegDatNroDoc + "/" + obj.CLegDatApellidoPaterno + " " + obj.CLegDatApellidoMaterno + " " + obj.CLegDatNombres + "-LEG.pdf");

                        //Eliminar Archivo Existente
                        //System.IO.File.Delete(pdfout);


                        string[] fileNames = list_file.ToArray();
                        string outFile = pdfout;
                        // step 1: creation of a document-object
                        Document document = new Document(PageSize.A4, 0, 0, 0, 0);
                        //create newFileStream object which will be disposed at the end
                        using (FileStream newFileStream = new FileStream(outFile, FileMode.Create))
                        {
                            // step 2: we create a writer that listens to the document
                            PdfCopy writer = new PdfCopy(document, newFileStream);
                            //if (writer == null)
                            //{
                            //    return;
                            //}

                            // step 3: we open the document
                            document.Open();
                            foreach (string fileName in fileNames)
                            {
                                //Verificar tamaño del archivo
                                FileInfo file_info = new FileInfo(fileName);
                                if (file_info.Length == 0)
                                {
                                    continue;
                                }

                                // we create a reader for a certain document
                                PdfReader reader = new PdfReader(fileName);
                                reader.ConsolidateNamedDestinations();

                                // step 4: we add content
                                for (int i = 1; i <= reader.NumberOfPages; i++)
                                {
                                    PdfImportedPage page = writer.GetImportedPage(reader, i);
                                    writer.AddPage(page);
                                }

                                try
                                {
                                    PRAcroForm form = reader.AcroForm;
                                    if (form != null)
                                    {
                                        writer.CopyDocumentFields(reader);
                                        //writer.AddDocument(reader);
                                    }
                                }
                                catch (Exception)
                                {
                                }

                                reader.Close();
                            }

                            // step 5: we close the document and writer
                            writer.Close();
                            document.Close();
                            //var pdfFileStream = System.IO.File.OpenRead(pdfout);
                            System.IO.Directory.Delete(dirpath, true);
                            //File(pdfFileStream, "application/pdf");

                            newFileStream.Close();
                            //pdfFileStream.Close(); 

                        }//disposes the newFileStream object

                    }

                    //}

                }


                ////Comprimir archivo

                DateTime now = DateTime.Now;
                Console.WriteLine();

                string path = Path.Combine(directorio, "LegajosUSS/archivosComprimidos.zip");
                bool result = System.IO.File.Exists(path);
                if (result == true)
                {
                    System.IO.File.Delete(path);
                    ZipFile.CreateFromDirectory(Path.Combine(directorio, "LegajosUSS/Exportacion_Lote"), Path.Combine(directorio, "LegajosUSS/archivosComprimidos.zip"));
                }
                else
                {
                    ZipFile.CreateFromDirectory(Path.Combine(directorio, "LegajosUSS/Exportacion_Lote"), Path.Combine(directorio, "LegajosUSS/archivosComprimidos.zip"));
                }

                /*
                 * DECARCARGAR ARCHIVO
                 */

                


                /*
                 * DESCARGAR ARCHIVO 
                 */
                obj_constante.CConDescripcion = url_descarga;

                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Directorio de Descarga", "archivosComprimidos.zip"));

            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }

        [HttpGet("/api/validarcPerCodigo/{cPerCodigo}", Name = "validarCodigoPersona")]
        [Authorize]
        public ActionResult<LegDatosGenerales> ValidarCodigoPersonaCM(string cPerCodigo)
        {

            String[] vect = cPerCodigo.Split(",");
            Boolean vValidacion = true;
            for (int i = 0; i < vect.Length; i++)
            {
                var obj = this._repository.ValidarCodigoPersonaCargaMasiva(vect[i]);
                if (obj.Result == false)
                {
                    vValidacion = false;
                }
            }
            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "", vValidacion));
        }

        [HttpGet("/api/obtenerdeclaracionjurada/{cPerCodigo}")]
        public ActionResult<LegDatosGenerales> ObtenerDeclaracionJurada(string cPerCodigo)
        {
            try
            {
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.cPerCodigo == cPerCodigo);

                return new JsonResult(new Mensajes(200, true, null, objLG));

            }
            catch (Exception ex)
            {

                return new JsonResult(new Mensajes(200, true, ex.Message, null));

            }

        }



        [HttpPost("/api/declacionjurada/{cPerCodigo}")]
        public async Task<ActionResult> PutAceptarDeclaracion(string cPerCodigo)
        {


            try
            {
                DateTime fecha = DateTime.Now;
                using var transaction = context.Database.BeginTransaction();
                var objLG = context.LegDatosGenerales.AsNoTracking().FirstOrDefault(x => x.cPerCodigo == cPerCodigo);
                if(objLG != null){ 

                objLG.DFechaModifica = fecha;
                objLG.CUsuModifica = objLG.cPerCodigo;
                objLG.declaracionjuradaflag = true;
                objLG.fechadeclaracionjurada = fecha;
                context.Entry(objLG).State = EntityState.Modified;
                Console.WriteLine(objLG);
                await context.SaveChangesAsync();
                transaction.Commit();
                }
                return new JsonResult(new Mensajes(200, true, "Registro ha sido actualizado.", null));
                
            }
            catch (Exception e)
            {

                return new JsonResult(new Mensajes(400, false, e.Message, null));
            }

        }

        [HttpGet("/api/legajoaux/exportar_verificarvalidaciones/{parcCodigo}", Name = "VerificarTieneValidacionesPendientes")]
        //[HttpGet("/api/legajoaux/{parcCodigo}", Name = "ObtenerLegajoAux")]
        [Authorize]
        public ActionResult<LegDatosGenerales> GetVerificarTieneValidacionesPendientes(string parcCodigo)
        {

            var cantleg = context.LegDatosGenerales.Where(x => x.cPerCodigo == parcCodigo).Count();

            if (cantleg == 0)
            {
                var objs = _repository.GetDatosSEUSS(parcCodigo).Result;
                if (objs == null)
                {
                    return new JsonResult(new Mensajes(NotFound().StatusCode, false, "No se encontró datos generales en Legajos, proceda a registrar.", objs));
                }
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, "Legajo no tiene datos generales, se cargaron datos de SEUSS, proceda a completar los datos y guardar.", objs));
            }
            var obj = funcion.LegDatosReturn(parcCodigo);
            var validacion = true;
            var codigodg = obj.NLegDatCodigo;
            var LegAdminitrativaCarga = funcion.LegDatosCargaAdministrativa(codigodg);
            //LegAdminitrativaCarga.Count > 0 &&
            //var d = LegAdminitrativaCarga.Where(CLegAdmValida => false);
            validacion = funcion.LegDatosCargaAdministrativa(codigodg).Count > 0 && funcion.LegDatosCargaAdministrativaSinValidar(codigodg).Count > 0 ? false : validacion;
            validacion = funcion.LegDatosCapacitaciones(codigodg).Count > 0 && funcion.LegDatosCapacitacionesSinValidar(codigodg).Count>0 ? false : validacion;
            validacion = funcion.LegDatosCategoriaDocente(codigodg).Count > 0 && funcion.LegDatosCategoriaDocenteSinValidar(codigodg).Count > 0 ? false : validacion;
            validacion = funcion.LegDatosGradoTitulo(codigodg).Count > 0&&funcion.LegDatosGradoTituloSinValidacion(codigodg).Count > 0 ? false : validacion;
            validacion = funcion.LegDatosIdiomaOfimatica(codigodg).Count > 0 && funcion.LegDatosIdiomaOfimaticaSinValidacion(codigodg).Count > 0 ? false : validacion;
            validacion = funcion.LegDatosInvestigador(codigodg).Count >0 && funcion.LegDatosInvestigadorSinValidar(codigodg).Count > 0 ? false : validacion;
            validacion = funcion.LegDatosParticipacion(codigodg).Count > 0 &&funcion.LegDatosParticipacionSinValidar(codigodg).Count > 0 ? false : validacion;
            validacion = funcion.LegDatosExperienciaNoDocente(codigodg).Count > 0 && funcion.LegDatosExperienciaNoDocenteSinValidar(codigodg).Count > 0 ? false : validacion;
            validacion = funcion.LegDatosProyeccionSocial(codigodg).Count > 0 && funcion.LegDatosProyeccionSocialSinValidar(codigodg).Count > 0 ? false : validacion;
            validacion = funcion.LegDatosReconocimiento(codigodg).Count > 0 && funcion.LegDatosReconocimientoSinValidar(codigodg).Count > 0 ? false : validacion;
            validacion = funcion.LegDatosRegimenDedicacion(codigodg).Count > 0 && funcion.LegDatosRegimenDedicacionSinValidar(codigodg).Count > 0 ? false : validacion;
            validacion = funcion.LegDatosTesisAsJur(codigodg).Count > 0 && funcion.LegDatosTesisAsJurSinValidar(codigodg).Count > 0 ? false : validacion;            
            validacion = funcion.LegDatosDocumentacionInterna(codigodg).Count > 0 && funcion.LegDatosDocumentacionInterna(codigodg).Count > 0 ? false : validacion;

            if (validacion)
            {
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, validacion, "", null));
            }
            else
            {
                return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, validacion, "El personal, tiene datos no validados. ¿Desea continuar?", null));
            }

            
        }
       
        //-------------------------------------------------
        [HttpPost("registrar_permisos", Name = "RegistrarPermisos")]
        [Authorize]
        //public ActionResult PostDatosGenerales([FromForm] LegDatosGenerales datos)
        public ActionResult RegistrarPermisos([FromForm] EPermisos data)
        {
            try
            {
                var obj = _repository.RegistrarPermisos(data.nIntCodigo, data.cPerCodigo);
                Console.WriteLine();
                return new JsonResult(new Mensajes(200, true, "Registro ha sido registrado.", null));

            }
            catch (Exception e)
            {

                return new JsonResult(new Mensajes(400, false, e.Message, null));
            }

        }

        [HttpPost("registrar_accionesxusuario", Name = "RegistrarAccionesXUsuario")]
        //[Authorize]
        //public ActionResult PostDatosGenerales([FromForm] LegDatosGenerales datos)
        public ActionResult RegistrarAcciones([FromForm] EPermisos data)
        {
            try
            {
                var obj = _repository.RegistrarAccionesXUsuario(data.nIntCodigo, data.cPerCodigo);
                Console.WriteLine();
                return new JsonResult(new Mensajes(200, true, "Registro ha sido registrado.", null));

            }
            catch (Exception e)
            {

                return new JsonResult(new Mensajes(400, false, e.Message, null));
            }

        }



    }
}