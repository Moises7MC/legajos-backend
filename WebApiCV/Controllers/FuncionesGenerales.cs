using iTextSharp.text;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApiCV.Contexts;
using WebApiCV.Data;
using WebApiCV.Entity;


namespace WebApiCV.Controllers
{
    public class FuncionesGenerales
    {
        private readonly bdLegajosContext context;
        private readonly ConstanteRepository repositorycons;
        private readonly InterfaceRepository repositoryinterf;
        private readonly PersonaRepository repositorypersona;
        public String cPerCodAux;
        public FuncionesGenerales(bdLegajosContext context, ConstanteRepository repositorycons, InterfaceRepository repositoryinterf, PersonaRepository repositorypersona)
        {

            this.context = context;
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
            //, ConstanteRepository _repositorycons, InterfaceRepository _repositoryinterf
            //this._repositorycons = _repositorycons;
            //this._repositoryinterf = _repositoryinterf;
        }

       

        public  void  getPerCodigoToken(ClaimsPrincipal user)
        {
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            var identity = user.Identity as ClaimsIdentity;
            IEnumerable<Claim> claims = identity.Claims;
            cPerCodAux =  claims.First().Value;
        }

        public LegDatosGenerales LegDatosBDSipanReturn(string parcCodigo)
        {
            return context.LegDatosGenerales
                .Select(x => new LegDatosGenerales
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
                    NLegDatZona = x.NLegDatZona,
                    NValorZona = x.NValorZona,
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
                    cPerCodigo = parcCodigo
                })
                .FirstOrDefault(x => x.cPerCodigo == parcCodigo);
        }

        public LegDatosGenerales LegDatosReturn(string parcCodigo)
        {
            Console.WriteLine(parcCodigo);

            //var codigo = context.LegDatosGenerales.FirstOrDefault(x => x.cPerCodigo == parcCodigo).NLegDatCodigo;
            return context.LegDatosGenerales
                .Select(x => new LegDatosGenerales
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
                    NLegDatZona = x.NLegDatZona,
                    NValorZona = x.NValorZona,
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
                    cLegDatBuenaSalud = x.cLegDatBuenaSalud,
                    cPerCodigo = x.cPerCodigo,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,

                    // ------------ EdgarBS 2025 --------------------------------

                    // Campos para Régimen Pensionario 
                    NLegDatRegPenAfiliado = x.NLegDatRegPenAfiliado,
                    NValorAfiliado = x.NValorAfiliado,
                    NLegDatRegPenEntidad = x.NLegDatRegPenEntidad,
                    NValorEntidad = x.NValorEntidad,
                    DLegDatRegPenFechaCese = x.DLegDatRegPenFechaCese,

                    // Campos para cuenta de haberes
                    NLegDatCtaHabHaberes = x.NLegDatCtaHabHaberes,
                    NValorHaberes = x.NValorHaberes,
                    NLegDatCtaHabBanco = x.NLegDatCtaHabBanco,
                    NValorBanco = x.NValorBanco,
                    CLegDatCtaHabNumCta = x.CLegDatCtaHabNumCta,
                    CLegDatCtaHabNumCtaCci = x.CLegDatCtaHabNumCtaCci,
                    NLegDatCtaHabBancoAperturar = x.NLegDatCtaHabBancoAperturar,
                    NValorBancoAperturar = x.NValorBancoAperturar,


                    CLegDatMencionEnMayGradAcad = x.CLegDatMencionEnMayGradAcad,
                    CLegDatInstitucionMayGradAcad = x.CLegDatInstitucionMayGradAcad,

                    NLegDatAceptaTerminos = x.NLegDatAceptaTerminos,

                    // ------------ EdgarBS 2025 --------------------------------


                    declaracionjuradaflag = x.declaracionjuradaflag ,
                    fechadeclaracionjurada = x.fechadeclaracionjurada,
                    nLegIdiomaNativo = x.nLegIdiomaNativo,
                    nLegDatDiscapacidad = x.nLegDatDiscapacidad,
                    nLegDatTipoDiscapacidad = x.nLegDatTipoDiscapacidad,
                    cLegDatOtraDiscapcidad = x.cLegDatOtraDiscapcidad,
                    cLegDatArchivoConadis = x.cLegDatArchivoConadis,
                    vPais = this.repositoryinterf.GetInterfaceDatos(x.NLegDatPais, x.NClasePais).Result,
                    vSexo = repositorycons.GetConstanteDatos(x.NLegDatSexo, x.NClaseSexo).Result,
                    vEstadoCivil = repositorycons.GetConstanteDatos(x.NLegDatEstadoCivil, x.NClaseEstadoCivil).Result,
                    vTipoDoc = repositoryinterf.GetInterfaceDatos(x.NLegDatTipoDoc, x.NClaseTipoDoc).Result,
                    vTipoDomicilio = repositorycons.GetConstanteDatos(x.NLegDatTipoDomicilio, x.NValorTipoDomicilio).Result,
                    vZona = repositorycons.GetConstanteDatos(x.NLegDatZona, x.NValorZona).Result,
                    vNacimiento = repositoryinterf.GetInterfaceDatos(x.NLetDatNacimiento, x.NClaseNacimiento).Result,
                    vGradoAcad = repositoryinterf.GetInterfaceDatos(x.NLegDatGradoAcad, x.NClaseGradoAcad).Result,
                    CLegDatColegioProfNavigation = repositorypersona.GetPersonaDatos(x.CLegDatColegioProf).Result,
                    vCondicionColeg = repositorycons.GetConstanteDatos(x.NLegDatCondicionColeg ?? 0, x.NValorCondicionColeg ?? 0).Result,
                    vIdiomaNativo = repositorycons.GetConstanteDatos(x.nLegIdiomaNativo ?? 0, x.nLegIdiomaNativo ?? 0).Result,

                    // ------------ EdgarBS 2025 --------------------------------
                    vAfiliado = repositorycons.GetConstanteDatos(x.NLegDatRegPenAfiliado ?? 0, x.NValorAfiliado ?? 0).Result,
                    vEntidad = repositorycons.GetConstanteDatos(x.NLegDatRegPenEntidad ?? 0, x.NValorEntidad ?? 0).Result,
                    vHaberes = repositorycons.GetConstanteDatos(x.NLegDatCtaHabHaberes ?? 0, x.NValorHaberes ?? 0).Result,
                    vBanco = repositorycons.GetConstanteDatos(x.NLegDatCtaHabBanco ?? 0, x.NValorBanco ?? 0).Result,
                    vBancoAperturar = repositorycons.GetConstanteDatos(x.NLegDatCtaHabBancoAperturar ?? 0, x.NValorBancoAperturar ?? 0).Result,
                    // ------------ EdgarBS 2025 --------------------------------
        })
                .FirstOrDefault(x => x.cPerCodigo == parcCodigo);
        }

        public ICollection<LegAdminitrativaCarga> LegDatosCargaAdministrativa(int parnCodigo)
        {
            return (ICollection<LegAdminitrativaCarga>)context.LegAdminitrativaCarga
                    .Select(x => new LegAdminitrativaCarga
                    {
                        NLegAdmCodigo = x.NLegAdmCodigo,
                        NLegAdmCargo = x.NLegAdmCargo,
                        NClaseCargo = x.NClaseCargo,
                        CLegAdmInstitucion = x.CLegAdmInstitucion ?? "",
                        CLegAdmOtraInst = x.CLegAdmOtraInst ?? "",
                        CLegAdmPais = x.CLegAdmPais ?? "",
                        CLegAdmDocumento = x.CLegAdmDocumento,
                        DLegAdmFechaInicio = x.DLegAdmFechaInicio,
                        DLegAdmFechaFin = x.DLegAdmFechaFin,
                        CLegAdmArchivo = x.CLegAdmArchivo,
                        NLegAdmDatCodigo = x.NLegAdmDatCodigo,
                        CLegAdmValida = x.CLegAdmValida,
                        CLegAdmEstado = x.CLegAdmEstado,
                        CUsuRegistro = x.CLegAdmArchivo,
                        vCargo = repositorycons.GetConstanteDatos(x.NLegAdmCargo, x.NClaseCargo).Result,
                        CLegAdmInstitucionNavigation = x.CLegAdmInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegAdmInstitucion).Result,
                    }).Where(p => p.NLegAdmDatCodigo == parnCodigo && p.CLegAdmEstado == true).ToList();
        }

        public ICollection<LegCapacitaciones> LegDatosCapacitaciones(int parnCodigo)
        {
            return (ICollection<LegCapacitaciones>)context.LegCapacitaciones
                        .Select(x => new LegCapacitaciones
                        {
                            NLegCapCodigo = x.NLegCapCodigo,
                            NLegCapTipo = x.NLegCapTipo,
                            NLegCapTipoEsp = x.NLegCapTipoEsp,
                            CLegCapNombre = x.CLegCapNombre,
                            NLegCapHoras = x.NLegCapHoras,
                            DLegCapFechaInicio = x.DLegCapFechaInicio,
                            DLegCapFechaFin = x.DLegCapFechaFin,
                            CLegCapArchivo = x.CLegCapArchivo,
                            CLegCapInstitucion = x.CLegCapInstitucion ?? "",
                            CLegCapOtraInst = x.CLegCapOtraInst ?? "",
                            CLegCapPais = x.CLegCapPais ?? "",
                            NLegCapDatCodigo = x.NLegCapDatCodigo,                            
                            NValorTipo = x.NValorTipo,
                            NValorTipoEsp = x.NValorTipoEsp,
                            CLegCapValida = x.CLegCapValida,
                            CLegCapEstado = x.CLegCapEstado,
                            CUsuRegistro = x.CLegCapArchivo,
                            vInstitucion = x.CLegCapInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegCapInstitucion).Result,
                            vTipo = repositorycons.GetConstanteDatos(x.NLegCapTipo, x.NValorTipo).Result,
                            vTipoEsp = repositorycons.GetConstanteDatos(x.NLegCapTipoEsp, x.NValorTipoEsp).Result,
                        }).Where(p => p.NLegCapDatCodigo == parnCodigo && p.CLegCapEstado == true).ToList();
        }

        public ICollection<LegCategoriaDocente> LegDatosCategoriaDocente(int parnCodigo)
        {
            return (ICollection<LegCategoriaDocente>)context.LegCategoriaDocente
                        .Select(x => new LegCategoriaDocente
                        {
                            NLegCatCodigo = x.NLegCatCodigo,
                            CLegCatInstitucion = x.CLegCatInstitucion ?? "",
                            CLegCatOtraInst = x.CLegCatOtraInst ?? "",
                            CLegCatPais = x.CLegCatPais ?? "",
                            NLegCatCategoria = x.NLegCatCategoria,
                            NValorCategoria = x.NValorCategoria,
                            DLegCatFechaInicio = x.DLegCatFechaInicio,
                            DLegCatFechaFin = x.DLegCatFechaFin,
                            CLegCatArchivo = x.CLegCatArchivo,
                            NLegCatDatCodigo = x.NLegCatDatCodigo,
                            CLegCatValida = x.CLegCatValida,
                            CLegCatEstado = x.CLegCatEstado,
                            CUsuRegistro = x.CLegCatArchivo,
                            vCategoria = repositorycons.GetConstanteDatos(x.NLegCatCategoria, x.NValorCategoria).Result,
                            CLegCatInstitucionNavigation = x.CLegCatInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegCatInstitucion).Result,
                        }).Where(p => p.NLegCatDatCodigo == parnCodigo && p.CLegCatEstado == true).ToList();
        }

        public ICollection<LegDocenciaUniv> LegDatosDocenciaUniv(int parnCodigo)
        {
            return (ICollection<LegDocenciaUniv>)context.LegDocenciaUniv
                         .Select(x => new LegDocenciaUniv
                         {
                             NLegDocCodigo = x.NLegDocCodigo,
                             CLegDocUniversidad = x.CLegDocUniversidad ?? "",
                             CLegDocOtraInst = x.CLegDocOtraInst ?? "",
                             CLegDocPais = x.CLegDocPais ?? "",
                             NLegDocRegimen = x.NLegDocRegimen,
                             NValorRegimen = x.NValorRegimen,
                             NLegDocCategoria = x.NLegDocCategoria,
                             NValorCategoria = x.NValorCategoria,

                             // 🔹 AGREGA ESTA LÍNEA AQUÍ:
                             cLegDocCargo = x.cLegDocCargo ?? "",

                             DLegDocFechaInicio = x.DLegDocFechaInicio,
                             DLegDocFechaFin = x.DLegDocFechaFin,
                             CLegDocArchivo = x.CLegDocArchivo,
                             NLegDocDatCodigo = x.NLegDocDatCodigo,
                             CLegDocValida = x.CLegDocValida,
                             CLegDocEstado = x.CLegDocEstado,
                             CUsuRegistro = x.CLegDocArchivo,
                             vCategoria = repositorycons.GetConstanteDatos(x.NLegDocCategoria, x.NValorCategoria).Result,
                             vRegimen = repositorycons.GetConstanteDatos(x.NLegDocRegimen, x.NValorRegimen).Result,
                             CLegDocUniversidadNavigation = x.CLegDocUniversidad == null ? null : repositorypersona.GetPersonaDatos(x.CLegDocUniversidad).Result,
                         }).Where(p => p.NLegDocDatCodigo == parnCodigo && p.CLegDocEstado == true).ToList();
        }

        public ICollection<LegGradoTitulo> LegDatosGradoTitulo(int parnCodigo)
        {
            return (ICollection<LegGradoTitulo>)context.LegGradoTitulo
                        .Select(x => new LegGradoTitulo
                        {
                            NLegGraCodigo = x.NLegGraCodigo,
                            NLegGraGradoAcad = x.NLegGraGradoAcad,
                            NClaseGradoAcad = x.NClaseGradoAcad,
                            CLegGraInstitucion = x.CLegGraInstitucion ?? "",
                            CLegGraOtraInst = x.CLegGraOtraInst ?? "",
                            CLegGraCarreraProf = x.CLegGraCarreraProf ?? "",
                            NLegGraPais = x.NLegGraPais,
                            NClasePais = x.NClasePais,
                            NLegGraUbigeo = x.NLegGraUbigeo,
                            NClaseUbigeo = x.NClaseUbigeo,
                            DLegGraFecha = x.DLegGraFecha,
                            CLegGraArchivo = x.CLegGraArchivo,
                            NLegGraDatCodigo = x.NLegGraDatCodigo,
                            CLegGraValida = x.CLegGraValida,
                            CLegGraEstado = x.CLegGraEstado,
                            CUsuRegistro = x.CLegGraArchivo,
                            vGradoAcad = repositoryinterf.GetInterfaceDatos(x.NLegGraGradoAcad, x.NClaseGradoAcad).Result,
                            vPais = repositoryinterf.GetInterfaceDatos(x.NLegGraPais, x.NClasePais).Result,
                            CLegGraInstitucionNavigation = x.CLegGraInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegGraInstitucion).Result,
                        }).Where(p => p.NLegGraDatCodigo == parnCodigo && p.CLegGraEstado == true).ToList();
        }

        public ICollection<LegIdiomaOfimatica> LegDatosIdiomaOfimatica(int parnCodigo)
        {
            return (ICollection<LegIdiomaOfimatica>)context.LegIdiomaOfimatica
                        .Select(x => new LegIdiomaOfimatica
                        {
                            NLegIdOfCodigo = x.NLegIdOfCodigo,
                            NLegIdOfCodigoDesc = x.NLegIdOfCodigoDesc,
                            NValorDesc = x.NValorDesc,
                            CLegIdOfTipo = x.CLegIdOfTipo,
                            NLegIdOfNivel = x.NLegIdOfNivel,
                            NValorNivel = x.NValorNivel,
                            DLegIdOfFecha = x.DLegIdOfFecha,
                            CLegIdOfArchivo = x.CLegIdOfArchivo,
                            NLegIdOfDatCodigo = x.NLegIdOfDatCodigo,
                            CLegIdOfValida = x.CLegIdOfValida,
                            CLegIdOfEstado = x.CLegIdOfEstado,
                            CUsuRegistro = x.CLegIdOfArchivo,
                            vCodigoDesc = repositorycons.GetConstanteDatos(x.NLegIdOfCodigoDesc, x.NValorDesc).Result,
                            vNivel = repositorycons.GetConstanteDatos(x.NLegIdOfNivel, x.NValorNivel).Result,
                        }).Where(p => p.NLegIdOfDatCodigo == parnCodigo && p.CLegIdOfEstado == true).ToList();
        }

        public ICollection<LegInvestigador> LegDatosInvestigador(int parnCodigo)
        {
            return (ICollection<LegInvestigador>)context.LegInvestigador
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
                            CUsuRegistro = x.CLegInvArchivo,
                            vCentroRegistro = repositoryinterf.GetInterfaceDatos(x.NValorCentroRegistro, x.NLegInvCentroRegistro).Result,
                            vNivelRenacyt = repositoryinterf.GetInterfaceDatos(x.NValorNivelRenacyt ?? 0, x.NLegInvNivelRenacyt ?? 0).Result,
                        }).Where(p => p.NLegInvDatCodigo == parnCodigo && p.CLegInvEstado == true).ToList();
        }

        public ICollection<LegParticipacionCongSem> LegDatosParticipacion(int parnCodigo)
        {
            return (ICollection<LegParticipacionCongSem>)context.LegParticipacionCongSem
                        .Select(x => new LegParticipacionCongSem
                        {
                            NLegParCodigo = x.NLegParCodigo,
                            CLegParInstitucion = x.CLegParInstitucion ?? "",
                            CLegParOtraInst = x.CLegParOtraInst ?? "",
                            CLegParPais = x.CLegParPais ?? "",
                            NLegParRol = x.NLegParRol,
                            NValorRol = x.NValorRol,
                            NLegParAmbito = x.NLegParAmbito,
                            NValorAmbito = x.NValorAmbito,
                            CLegParNombre = x.CLegParNombre,
                            DLegParFecha = x.DLegParFecha,
                            DLegParFechaFin = x.DLegParFechaFin,
                            NLegParHoras = x.NLegParHoras,
                            CLegParArchivoOrig = x.CLegParArchivoOrig == null ? "": x.CLegParArchivoOrig,
                            CLegParArchivo = x.CLegParArchivo,
                            NLegParDatCodigo = x.NLegParDatCodigo,
                            CLegParValida = x.CLegParValida,
                            CLegParEstado = x.CLegParEstado,
                            CUsuRegistro = x.CLegParArchivo,
                            vAmbito = repositoryinterf.GetInterfaceDatos(x.NValorAmbito, x.NLegParAmbito).Result,
                            vRol = repositoryinterf.GetInterfaceDatos(x.NValorRol, x.NLegParRol).Result,
                            CLegParInstitucionNavigation = x.CLegParInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegParInstitucion).Result
                        }).Where(p => p.NLegParDatCodigo == parnCodigo && p.CLegParEstado == true).ToList();
        }

        public ICollection<LegArchivos> LegDatosArchivos(string cPerCodigo)
        {
            return (ICollection<LegArchivos>)context.LegArchivos
                        .Select(x => new LegArchivos
                        {

                            NLegArcCodigo = x.NLegArcCodigo,
                            CPerCodigo = x.CPerCodigo,
                            CLegArcNombre = x.CLegArcNombre,
                            NLegArcTipo = x.NLegArcTipo,
                            CUsuRegistro = x.CUsuRegistro,
                        }).Where(p => p.CPerCodigo == cPerCodigo ).ToList();
        }

        public ICollection<LegProduccionCiencia> LegDatosProduccionCiencia(int parnCodigo)
        {
            return (ICollection<LegProduccionCiencia>)context.LegProduccionCiencia
                        .Select(x => new LegProduccionCiencia
                        {
                            NLegProdCodigo = x.NLegProdCodigo,
                            NLegProdTipo = x.NLegProdTipo,
                            NValorTipo = x.NValorTipo,
                            NLegProdPais = x.NLegProdPais,
                            vPais = repositoryinterf.GetInterfaceDatos(x.NLegProdPais ?? 0, 6002).Result,
                            // Asumiendo 1010 es el tipo de interface para países
                            NLegProdParticipacion =x.NLegProdParticipacion,
                            CLegProdTitulo = x.CLegProdTitulo,
                            DLegProdFecha = x.DLegProdFecha,
                            CLegProdNroResolucion = x.CLegProdNroResolucion,
                            CLegProdArchivo = x.CLegProdArchivo,
                            NLegProdDatCodigo = x.NLegProdDatCodigo,
                            CLegProdValida = x.CLegProdValida,
                            CLegProdEstado = x.CLegProdEstado,
                            CUsuRegistro = x.CLegProdArchivo,
                            vTipo = repositoryinterf.GetInterfaceDatos(x.NValorTipo, x.NLegProdTipo).Result,
                        }).Where(p => p.NLegProdDatCodigo == parnCodigo && p.CLegProdEstado == true).ToList();
        }

        public ICollection<LegProfesNoDocente> LegDatosExperienciaNoDocente(int parnCodigo)
        {
            var lista = context.LegProfesNoDocente
                .Where(p => p.NLegProDatCodigo == parnCodigo && p.CLegProEstado == true)
                .ToList();

            foreach (var item in lista)
            {
                if (item.NValorCargo > 0)
                {
                    // 1. Intentamos con el código que tiene el registro (el que sea que tenga)
                    int codigoGuardado = item.NLegProCargo ?? 0;
                    var res = repositorycons.GetConstanteDatos(codigoGuardado, item.NValorCargo.Value).Result;

                    // 2. Si no lo encuentra, probamos en el grupo 1082 (Dedicación)
                    if (res == null || string.IsNullOrEmpty(res.CConDescripcion))
                        res = repositorycons.GetConstanteDatos(1082, item.NValorCargo.Value).Result;

                    // 3. Si sigue sin aparecer, probamos en el grupo 4022 (Categoría)
                    if (res == null || string.IsNullOrEmpty(res.CConDescripcion))
                        res = repositorycons.GetConstanteDatos(4022, item.NValorCargo.Value).Result;

                    item.vCargo = res;
                }

                if (!string.IsNullOrEmpty(item.CLegProInstitucion))
                {
                    item.CLegProInstitucionNavigation = repositorypersona.GetPersonaDatos(item.CLegProInstitucion).Result;
                }
            }
            return lista;
        }

        public ICollection<LegProyeccionSocial> LegDatosProyeccionSocial(int parnCodigo)
        {
            return (ICollection<LegProyeccionSocial>)context.LegProyeccionSocial
                        .Select(x => new LegProyeccionSocial
                        {
                            NLegProyCodigo = x.NLegProyCodigo,
                            CLegProyInstitucion = x.CLegProyInstitucion ?? "",
                            CLegProyOtraInst = x.CLegProyOtraInst ?? "",
                            CLegProyPais = x.CLegProyPais ?? "",
                            NLegProyTipo = x.NLegProyTipo,
                            NValorTipo = x.NValorTipo,
                            CLegProyDescripcion = x.CLegProyDescripcion,
                            DLegProyFechaInicio = x.DLegProyFechaInicio,
                            DLegProyFechaFin = x.DLegProyFechaFin,
                            CLegProyArchivo = x.CLegProyArchivo,
                            NLegProyDatCodigo = x.NLegProyDatCodigo,
                            CLegProyValida = x.CLegProyValida,
                            CLegProyEstado = x.CLegProyEstado,
                            CUsuRegistro = x.CLegProyArchivo,
                            vTipo = repositorycons.GetConstanteDatos(x.NLegProyTipo, x.NValorTipo).Result,
                            CLegProyInstitucionNavigation = x.CLegProyInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegProyInstitucion).Result,
                        }).Where(p => p.NLegProyDatCodigo == parnCodigo && p.CLegProyEstado == true).ToList();
        }

        public ICollection<LegReconocimiento> LegDatosReconocimiento(int parnCodigo)
        {
            return (ICollection<LegReconocimiento>)context.LegReconocimiento
                        .Select(x => new LegReconocimiento
                        {
                            NLegRecCodigo = x.NLegRecCodigo,
                            NLegRecDocumento = x.NLegRecDocumento,
                            NValorDocumento = x.NValorDocumento,
                            NLegRecTipo = x.NLegRecTipo,
                            NValorTipo = x.NValorTipo,
                            CLegRecInstitucion = x.CLegRecInstitucion ?? "",
                            CLegRecOtraInst = x.CLegRecOtraInst ?? "",
                            CLegRecPais = x.CLegRecPais ?? "",
                            DLegRecFecha = x.DLegRecFecha,
                            CLegRecArchivo = x.CLegRecArchivo,
                            NLegRecDatCodigo = x.NLegRecDatCodigo,
                            CLegRecValida = x.CLegRecValida,
                            CLegRecEstado = x.CLegRecEstado,
                            CUsuRegistro = x.CLegRecArchivo,
                            vTipo = repositorycons.GetConstanteDatos(x.NLegRecTipo, x.NValorTipo).Result,
                            vDocumento = repositorycons.GetConstanteDatos(x.NLegRecDocumento, x.NValorDocumento).Result,
                            CLegRecInstitucionNavigation = x.CLegRecInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegRecInstitucion).Result,
                        }).Where(p => p.NLegRecDatCodigo == parnCodigo && p.CLegRecEstado == true).ToList();
        }

        public ICollection<LegRegimenDedicacion> LegDatosRegimenDedicacion(int parnCodigo)
        {
            return (ICollection<LegRegimenDedicacion>)context.LegRegimenDedicacion
                        .Select(x => new LegRegimenDedicacion
                        {
                            NLegRegCodigo = x.NLegRegCodigo,
                            CLegCatInstitucion = x.CLegCatInstitucion ?? "",
                            CLegRegOtraInst = x.CLegRegOtraInst ?? "",
                            CLegRegPais = x.CLegRegPais ?? "",
                            NLegRegDedicacion = x.NLegRegDedicacion,
                            NValorDedicacion = x.NValorDedicacion,
                            DLegRegFechaInicio = x.DLegRegFechaInicio,
                            DLegRegFechaFin = x.DLegRegFechaFin,
                            CLegRegArchivo = x.CLegRegArchivo,
                            NLegRegDatCodigo = x.NLegRegDatCodigo,
                            CLegRegValida = x.CLegRegValida,
                            CLegRegEstado = x.CLegRegEstado,
                            CUsuRegistro = x.CLegRegArchivo,
                            vDedicacion = repositorycons.GetConstanteDatos(x.NLegRegDedicacion, x.NValorDedicacion).Result,
                            CLegCatInstitucionNavigation = x.CLegCatInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegCatInstitucion).Result,
                        }).Where(p => p.NLegRegDatCodigo == parnCodigo && p.CLegRegEstado == true).ToList();
        }

        public ICollection<LegTesisAseJur> LegDatosTesisAsJur(int parnCodigo)
        {
            return (ICollection<LegTesisAseJur>)context.LegTesisAseJur
                        .Select(x => new LegTesisAseJur
                        {
                            NLegTesCodigo = x.NLegTesCodigo,
                            NLegTesTipo = x.NLegTesTipo,
                            NValorTipo = x.NValorTipo,
                            NLegTesNivel = x.NLegTesNivel,
                            NValorNivel = x.NValorNivel,
                            DLegTesFecha = x.DLegTesFecha,
                            CLegTesNroResolucion = x.CLegTesNroResolucion,
                            CLegTesArchivo = x.CLegTesArchivo,
                            NLegTesDatCodigo = x.NLegTesDatCodigo,
                            CLegTesValida = x.CLegTesValida,
                            CLegTesEstado = x.CLegTesEstado,
                            CUsuRegistro = x.CLegTesArchivo,
                            vTipo = repositoryinterf.GetInterfaceDatos(x.NValorTipo, x.NLegTesTipo).Result,
                            vNivel = repositorycons.GetConstanteDatos(x.NLegTesNivel, x.NValorNivel).Result,

                            NLegTesPais = x.NLegTesPais,
                            NClasePais = x.NClasePais,
                            CLegTesInstitucion = x.CLegTesInstitucion,
                            CLegTesOtraInst = x.CLegTesOtraInst,
                            vPais = repositoryinterf.GetInterfaceDatos(x.NLegTesPais ?? 0, x.NClasePais ?? 0).Result


                        }).Where(p => p.NLegTesDatCodigo == parnCodigo && p.CLegTesEstado == true).ToList();
        }

        public ICollection<LegCapacitacionInterna> LegDatosCapacitacionInterna(int parnCodigo)
        {
            return (ICollection<LegCapacitacionInterna>)context.LegCapacitacionInterna
                        .Select(x => new LegCapacitacionInterna
                        {
                            NLegDatCodigo = x.NLegDatCodigo,
                            NCapCodigo = x.NCapCodigo,
                            NLegCicodigo = x.NLegCicodigo,
                            CLegCicompetenciaMejora = x.CLegCicompetenciaMejora,
                            CLegCiarchivo = x.CLegCiarchivo,
                            BLegCiestado = x.BLegCiestado,
                            vCapacitacionUSS = x.vCapacitacionUSS,
                            CUsuRegistro = x.CLegCiarchivo
                        }).Where(p => p.NLegDatCodigo == parnCodigo && p.BLegCiestado == true).ToList();
        }

        public ICollection<LegContrato> LegDatosContrato(int parnCodigo)
        {
            return (ICollection<LegContrato>)context.LegContrato
                        .Select(x => new LegContrato
                        {
                            NLegConDatCodigo = x.NLegConDatCodigo,
                            NLegConCodigo = x.NLegConCodigo,
                            NLegConArea = x.NLegConArea,
                            NLegValArea = x.NLegValArea,
                            NLegConCargo = x.NLegConCargo,
                            NLegValCargo = x.NLegValCargo,
                            CLegConArchivo = x.CLegConArchivo,
                            NLegConSueldo = x.NLegConSueldo,
                            CUsuRegistro = x.CLegConArchivo,
                            BLegConEstado = x.BLegConEstado,
                            DLegConFechaInicio = x.DLegConFechaInicio,
                            DLegConFechaFin = x.DLegConFechaFin,
                            vArea = this.repositoryinterf.GetInterfaceDatos(x.NLegValArea, x.NLegConArea).Result,
                            vCargo = this.repositoryinterf.GetInterfaceDatos(x.NLegValCargo, x.NLegConCargo).Result,
                        }).Where(p => p.NLegConDatCodigo == parnCodigo && p.BLegConEstado == true).ToList();
        }

        public ICollection<LegResolucion> LegDatosResolucion(int parnCodigo)
        {
            return (ICollection<LegResolucion>)context.LegResolucion
                        .Select(x => new LegResolucion
                        {
                            NLegResDatCodigo = x.NLegResDatCodigo,
                            NLegResCodigo = x.NLegResCodigo,
                            NLegResTipo = x.NLegResTipo,
                            NLegValTipo = x.NLegValTipo,
                            CLegResArchivo = x.CLegResArchivo,
                            CUsuRegistro = x.CLegResArchivo,
                            BLegResEstado = x.BLegResEstado,
                            CLegResResuelve = x.CLegResResuelve,
                            CLegResNroResolucion = x.CLegResNroResolucion,
                            DLegResFecha = x.DLegResFecha,
                            vResolucion = this.repositorycons.GetConstanteDatos(x.NLegResTipo, x.NLegValTipo).Result,
                        }).Where(p => p.NLegResDatCodigo == parnCodigo && p.BLegResEstado == true).ToList();
        }

        public ICollection<LegEvaluacionDesemp> LegDatosEvaluacionDesemp(int parnCodigo)
        {
            return (ICollection<LegEvaluacionDesemp>)context.LegEvaluacionDesemp
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
                            CLegEvalAnio = x.CLegEvalAnio ?? "",
                            CLegEvalSemestre = x.CLegEvalSemestre ?? "", 
                            CUsuRegistro = x.CLegEvalArchivo,
                            vArea = this.repositoryinterf.GetInterfaceDatos(x.NLegValArea, x.NLegEvalArea).Result,
                            vCargo = this.repositoryinterf.GetInterfaceDatos(x.NLegValCargo, x.NLegEvalCargo).Result,
                            vNivel = this.repositorycons.GetConstanteDatos(x.NLegEvalNivel, x.NLegValNivel).Result,
                        }).Where(p => p.NLegEvalDatCodigo == parnCodigo && p.BLegEvalEstado == true).ToList();
        }

        public ICollection<LegSeleccion> LegDatosSeleccion(int parnCodigo)
        {
            return (ICollection<LegSeleccion>)context.LegSeleccion
                        .Select(x => new LegSeleccion
                        {
                            NLegSelDatCodigo = x.NLegSelDatCodigo,
                            NLegSelCodigo = x.NLegSelCodigo,
                            NLegSelArea = x.NLegSelArea,
                            NLegValArea = x.NLegValArea,
                            NLegSelCargo = x.NLegSelCargo,
                            NLegValCargo = x.NLegValCargo,
                            DLegSelFecha = x.DLegSelFecha,
                            BLegSelEstado = x.BLegSelEstado,
                            CLegSelEvaluacionCv = x.CLegSelEvaluacionCv,
                            CLegSelClaseModelo = x.CLegSelClaseModelo,
                            CLegSelEvaluacionPsico = x.CLegSelEvaluacionPsico,
                            CLegSelEntrevistaPers = x.CLegSelEntrevistaPers,
                            vArea = this.repositoryinterf.GetInterfaceDatos(x.NLegValArea, x.NLegSelArea).Result,
                            vCargo = this.repositoryinterf.GetInterfaceDatos(x.NLegValCargo, x.NLegSelCargo).Result,
                        }).Where(p => p.NLegSelDatCodigo == parnCodigo && p.BLegSelEstado == true).ToList();
        }

        public ICollection<LegOrdinarizacion> LegDatosOrdinarizacion(int parnCodigo)
        {
            return (ICollection<LegOrdinarizacion>)context.LegOrdinarizacion
                        .Select(x => new LegOrdinarizacion
                        {
                            NLegOrdDatCodigo = x.NLegOrdDatCodigo,
                            NLegOrdCodigo = x.NLegOrdCodigo,
                            NLegOrdArea = x.NLegOrdArea,
                            NLegOrdValArea = x.NLegOrdValArea,
                            NLegOrdCargo = x.NLegOrdCargo,
                            NLegValCargo = x.NLegValCargo,
                            DLegOrdFecha = x.DLegOrdFecha,
                            BLegOrdEstado = x.BLegOrdEstado,
                            CLegOrdFichaInscripcion = x.CLegOrdFichaInscripcion,
                            CLegOrdEvaluacionCv = x.CLegOrdEvaluacionCv,
                            CLegOrdClaseModelo = x.CLegOrdClaseModelo,
                            CLegOrdEvaluacionPsico = x.CLegOrdEvaluacionPsico,
                            CLegOrdEntrevistaPers = x.CLegOrdEntrevistaPers,
                            vArea = this.repositoryinterf.GetInterfaceDatos(x.NLegOrdValArea, x.NLegOrdArea).Result,
                            vCargo = this.repositoryinterf.GetInterfaceDatos(x.NLegValCargo, x.NLegOrdCargo).Result,
                        }).Where(p => p.NLegOrdDatCodigo == parnCodigo && p.BLegOrdEstado == true).ToList();
        }

        public ICollection<LegDocumentacionInterna> LegDatosDocumentacionInterna(int parnCodigo)
        {
            return (ICollection<LegDocumentacionInterna>)context.LegDocumentacionInterna
                        .Select(x => new LegDocumentacionInterna
                        {
                            NLegDicodigo = x.NLegDicodigo,
                            NLegDidatCodigo = x.NLegDidatCodigo,
                            CLegDiarchivo = x.CLegDiarchivo,
                            CLegDicodigo = x.CLegDicodigo ?? "",
                            CLegDidescripcion = x.CLegDidescripcion ?? "",
                            NLegDitipoDoc = x.NLegDitipoDoc,
                            NLegValTipoDoc = x.NLegValTipoDoc,
                            BLegDiestado = x.BLegDiestado,
                            vTipo = this.repositorycons.GetConstanteDatos(x.NLegDitipoDoc, x.NLegValTipoDoc).Result,
                        }).Where(p => p.NLegDidatCodigo == parnCodigo && p.BLegDiestado == true).ToList();
        }

        public ICollection<LegDeclaracionJurada> LegDatosDeclaracionJurada(int parnCodigo)
        {
            return context.LegDeclaracionJurada
                .Where(x => x.NLegDjdatCodigo == parnCodigo && x.BLegDjestado == true)
                .Select(x => new LegDeclaracionJurada
                {
                    NLegDjcodigo = x.NLegDjcodigo,
                    NLegDjdatCodigo = x.NLegDjdatCodigo,
                    CLegDjanexo2 = x.CLegDjanexo2 ?? string.Empty,
                    CLegDjanexo6 = x.CLegDjanexo6 ?? string.Empty,
                    CLegDjanexo7 = x.CLegDjanexo7 ?? string.Empty,
                    BLegDjestado = x.BLegDjestado,
                    CLegDjanexo1 = x.CLegDjanexo1 ?? string.Empty,
                    CLegDjanexo2_2 = x.CLegDjanexo2_2 ?? string.Empty,
                    CLegDjanexo3 = x.CLegDjanexo3 ?? string.Empty,
                    CLegDjanexo4 = x.CLegDjanexo4 ?? string.Empty,
                    CLegDjanexo5 = x.CLegDjanexo5 ?? string.Empty,
                    CLegDjanexo6_2 = x.CLegDjanexo6_2 ?? string.Empty,
                    CLegDjDNI = x.CLegDjDNI ?? string.Empty,
                    CLegDjDNI_DH = x.CLegDjDNI_DH ?? string.Empty,
                    CLegDjFotoCarnet = x.CLegDjFotoCarnet ?? string.Empty,
                    CLegDjNumCta = x.CLegDjNumCta ?? string.Empty,
                    CLegDjConsJubilacion = x.CLegDjConsJubilacion ?? string.Empty,
                    CLegDjConsAfiliacionOnpAfp = x.CLegDjConsAfiliacionOnpAfp ?? string.Empty
                })
                .ToList();
        }


        /** EBS - 01/2026 ---------------------------> */
        /* Funcion para Licencias o registros profesionales (colegios, nº colegiatura, condición, fechas, archivo, etc.) */
        public ICollection<LegLicenciaProfesional> LegDatosLicenciaProfesional(int parnCodigo)
        {
            return (ICollection<LegLicenciaProfesional>)context.LegLicenciaProfesional
                        .Select(x => new LegLicenciaProfesional
                        {
                            NLegLicCodigo = x.NLegLicCodigo,
                            NLegLicPais = x.NLegLicPais,
                            NClasePais = x.NClasePais,
                            CLegLicInstitucion = x.CLegLicInstitucion,
                            CLegLicOtraInst = x.CLegLicOtraInst,
                            CLegLicNroRegistro = x.CLegLicNroRegistro,
                            NLegLicCondicion = x.NLegLicCondicion,
                            NClaseCondicion = x.NClaseCondicion,
                            DLegLicFechaEmision = x.DLegLicFechaEmision,
                            DLegLicFechaExpiracion = x.DLegLicFechaExpiracion,
                            NLegLicDatCodigo = x.NLegLicDatCodigo,
                            CLegLicValida = x.CLegLicValida,
                            CLegLicEstado = x.CLegLicEstado,
                            CUsuRegistro = x.CUsuRegistro,
                            vCondicion = repositoryinterf.GetInterfaceDatos(x.NLegLicCondicion, x.NClaseCondicion).Result,
                            vPais = repositoryinterf.GetInterfaceDatos(x.NLegLicPais, x.NClasePais).Result,
                            CLegLicInstitucionNavigation = x.CLegLicInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegLicInstitucion).Result,

                        }).Where(p => p.NLegLicDatCodigo == parnCodigo && p.CLegLicEstado == true).ToList();
        }

        /* Funcion para Membresias (colegios, nº colegiatura, fechas, etc.) */
        public ICollection<LegMembresia> LegDatosMembresia(int parnCodigo)
        {
            return (ICollection<LegMembresia>)context.LegMembresia
                        .Select(x => new LegMembresia
                        {
                            NLegMemCodigo = x.NLegMemCodigo,
                            NLegMemPais = x.NLegMemPais,
                            NClasePais = x.NClasePais,
                            CLegMemInstitucion = x.CLegMemInstitucion,
                            CLegMemOtraInst = x.CLegMemOtraInst,
                            CLegMemNroRegistro = x.CLegMemNroRegistro,
                            DLegMemFechaEmision = x.DLegMemFechaEmision,
                            DLegMemFechaExpiracion = x.DLegMemFechaExpiracion,
                            NLegMemDatCodigo = x.NLegMemDatCodigo,
                            CLegMemValida = x.CLegMemValida,
                            CLegMemEstado = x.CLegMemEstado,
                            CUsuRegistro = x.CUsuRegistro,
                            vPais = repositoryinterf.GetInterfaceDatos(x.NLegMemPais, x.NClasePais).Result,
                            CLegMemInstitucionNavigation = x.CLegMemInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegMemInstitucion).Result,

                        }).Where(p => p.NLegMemDatCodigo == parnCodigo && p.CLegMemEstado == true).ToList();
        }
        /** EBS - 01/2026 <--------------------------- */


        public ICollection<LegCapacitacionInterna> ListCapacitacionInterna(int parnCapacitacion)
        {
            return (ICollection<LegCapacitacionInterna>)context.LegCapacitacionInterna
                .Select(x => new LegCapacitacionInterna
                {
                    NLegDatCodigo = x.NLegDatCodigo,
                    NCapCodigo = x.NCapCodigo,
                    NLegCicodigo = x.NLegCicodigo,
                    CLegCicompetenciaMejora = x.CLegCicompetenciaMejora,
                    CLegCiarchivo = x.CLegCiarchivo,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,
                    vCapacitacionUSS = x.vCapacitacionUSS
                    }).Where(x => x.NCapCodigo == (parnCapacitacion == 0 ? x.NCapCodigo : parnCapacitacion)).ToList();
        }

        public ICollection<LegContrato> ListContrato(int parnContrato)
        {
            return (ICollection<LegContrato>)context.LegContrato
                .Select(x => new LegContrato
                {
                    NLegConDatCodigo = x.NLegConDatCodigo,
                    NLegConCodigo = x.NLegConCodigo,
                    NLegConCargo = x.NLegConCargo,
                    NLegValCargo = x.NLegValCargo,
                    NLegConArea = x.NLegConArea,
                    NLegValArea = x.NLegValArea,
                    CLegConArchivo = x.CLegConArchivo,
                    DLegConFechaInicio = x.DLegConFechaInicio,
                    DLegConFechaFin = x.DLegConFechaFin,
                    CUsuRegistro = x.CUsuRegistro,
                    DFechaRegistro = x.DFechaRegistro,
                    CUsuModifica = x.CUsuModifica,
                    DFechaModifica = x.DFechaModifica,
                    NLegConSueldo = x.NLegConSueldo
                }).Where(x => x.NLegConCodigo == (parnContrato == 0 ? x.NLegConCodigo : parnContrato)).ToList();
        }




        public string imgtopdf(string directorio, string srcFilename, int num, string numdni)
        {
            Console.WriteLine($"srcFilename recibido: {srcFilename}");

            if (string.IsNullOrWhiteSpace(srcFilename))
                throw new ArgumentException("La ruta de la imagen está vacía o es nula.", nameof(srcFilename));

            if (!File.Exists(srcFilename))
                throw new FileNotFoundException($"La imagen no existe: {srcFilename}");


            // Verifica que el archivo de imagen exista
            if (!System.IO.File.Exists(srcFilename))
            {
                throw new System.IO.FileNotFoundException($"No se encontró la imagen en la ruta: {srcFilename}");
            }

            // Construir el path completo para el PDF de la imagen
            var imgpdf = Path.Combine(directorio, "LegajosUSS", numdni, "legajoexport", $"img_{num}.pdf");

            // Asegurarse de que el directorio de destino exista
            var destDir = Path.GetDirectoryName(imgpdf);
            if (!Directory.Exists(destDir))
            {
                Directory.CreateDirectory(destDir);
            }

            using (var ms = new MemoryStream())
            {
                var documentimg = new iTextSharp.text.Document(PageSize.A4, 15f, 15f, 15f, 0);
                var writer = iTextSharp.text.pdf.PdfWriter.GetInstance(documentimg, ms);
                writer.SetFullCompression();
                documentimg.Open();

                // Cargar la imagen
                var image = iTextSharp.text.Image.GetInstance(srcFilename);
                // Calcular el porcentaje de escalado para ajustar la imagen a la página
                var scalePercent = (((documentimg.PageSize.Width / image.Width) * 100) - 4);
                image.ScalePercent(scalePercent);
                documentimg.SetPageSize(PageSize.A4);
                documentimg.Add(image);
                documentimg.Close();

                // Escribir el PDF generado al sistema de archivos
                System.IO.File.WriteAllBytes(imgpdf, ms.ToArray());
            }
            return imgpdf;
        }


        public String imgtopdf2(String directorio, String srcFilename, int num, String numdni, String area)
        {
            var imgpdf = Path.Combine(directorio, "LegajosUSS/Exportacion_Lote/"+area+"/" + numdni + "/legajoexport/img_" + num.ToString() + ".pdf");
            using (var ms = new MemoryStream())
            {
                var documentimg = new iTextSharp.text.Document(PageSize.A4, 15f, 15f, 15f, 0);
                iTextSharp.text.pdf.PdfWriter.GetInstance(documentimg, ms).SetFullCompression();
                documentimg.Open();
                var image = iTextSharp.text.Image.GetInstance(srcFilename);
                var scalePercent = (((documentimg.PageSize.Width / image.Width) * 100) - 4);
                image.ScalePercent(scalePercent);
                documentimg.SetPageSize(PageSize.A4);
                documentimg.Add(image);

                documentimg.Close();
                System.IO.File.WriteAllBytes(imgpdf, ms.ToArray());
            }
            return imgpdf;
        }

        public ICollection<LegCapacitaciones> LegDatosCapacitacionesSinValidar(int parnCodigo)
        {
            return (ICollection<LegCapacitaciones>)context.LegCapacitaciones
                        .Select(x => new LegCapacitaciones
                        {
                            NLegCapCodigo = x.NLegCapCodigo,
                            NLegCapTipo = x.NLegCapTipo,
                            NLegCapTipoEsp = x.NLegCapTipoEsp,
                            CLegCapNombre = x.CLegCapNombre,
                            NLegCapHoras = x.NLegCapHoras,
                            DLegCapFechaInicio = x.DLegCapFechaInicio,
                            DLegCapFechaFin = x.DLegCapFechaFin,
                            CLegCapArchivo = x.CLegCapArchivo,
                            CLegCapInstitucion = x.CLegCapInstitucion ?? "",
                            CLegCapOtraInst = x.CLegCapOtraInst ?? "",
                            CLegCapPais = x.CLegCapPais ?? "",
                            NLegCapDatCodigo = x.NLegCapDatCodigo,
                            NValorTipo = x.NValorTipo,
                            NValorTipoEsp = x.NValorTipoEsp,
                            CLegCapValida = x.CLegCapValida,
                            CLegCapEstado = x.CLegCapEstado,
                            CUsuRegistro = x.CLegCapArchivo,
                            vInstitucion = x.CLegCapInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegCapInstitucion).Result,
                            vTipo = repositorycons.GetConstanteDatos(x.NLegCapTipo, x.NValorTipo).Result,
                            vTipoEsp = repositorycons.GetConstanteDatos(x.NLegCapTipoEsp, x.NValorTipoEsp).Result,
                        }).Where(p => p.NLegCapDatCodigo == parnCodigo && p.CLegCapEstado == true && p.CLegCapValida == false).ToList();
        }
        public ICollection<LegCategoriaDocente> LegDatosCategoriaDocenteSinValidar(int parnCodigo)
        {
            return (ICollection<LegCategoriaDocente>)context.LegCategoriaDocente
                        .Select(x => new LegCategoriaDocente
                        {
                            NLegCatCodigo = x.NLegCatCodigo,
                            CLegCatInstitucion = x.CLegCatInstitucion ?? "",
                            CLegCatOtraInst = x.CLegCatOtraInst ?? "",
                            CLegCatPais = x.CLegCatPais ?? "",
                            NLegCatCategoria = x.NLegCatCategoria,
                            NValorCategoria = x.NValorCategoria,
                            DLegCatFechaInicio = x.DLegCatFechaInicio,
                            DLegCatFechaFin = x.DLegCatFechaFin,
                            CLegCatArchivo = x.CLegCatArchivo,
                            NLegCatDatCodigo = x.NLegCatDatCodigo,
                            CLegCatValida = x.CLegCatValida,
                            CLegCatEstado = x.CLegCatEstado,
                            CUsuRegistro = x.CLegCatArchivo,
                            vCategoria = repositorycons.GetConstanteDatos(x.NLegCatCategoria, x.NValorCategoria).Result,
                            CLegCatInstitucionNavigation = x.CLegCatInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegCatInstitucion).Result,
                        }).Where(p => p.NLegCatDatCodigo == parnCodigo && p.CLegCatEstado == true && p.CLegCatValida == false).ToList();
        }

        public ICollection<LegGradoTitulo> LegDatosGradoTituloSinValidacion(int parnCodigo)
        {
            return (ICollection<LegGradoTitulo>)context.LegGradoTitulo
                        .Select(x => new LegGradoTitulo
                        {
                            NLegGraCodigo = x.NLegGraCodigo,
                            NLegGraGradoAcad = x.NLegGraGradoAcad,
                            NClaseGradoAcad = x.NClaseGradoAcad,
                            CLegGraInstitucion = x.CLegGraInstitucion ?? "",
                            CLegGraOtraInst = x.CLegGraOtraInst ?? "",
                            CLegGraCarreraProf = x.CLegGraCarreraProf ?? "",
                            NLegGraPais = x.NLegGraPais,
                            NClasePais = x.NClasePais,
                            NLegGraUbigeo = x.NLegGraUbigeo,
                            NClaseUbigeo = x.NClaseUbigeo,
                            DLegGraFecha = x.DLegGraFecha,
                            CLegGraArchivo = x.CLegGraArchivo,
                            NLegGraDatCodigo = x.NLegGraDatCodigo,
                            CLegGraValida = x.CLegGraValida,
                            CLegGraEstado = x.CLegGraEstado,
                            CUsuRegistro = x.CLegGraArchivo,
                            vGradoAcad = repositoryinterf.GetInterfaceDatos(x.NLegGraGradoAcad, x.NClaseGradoAcad).Result,
                            vPais = repositoryinterf.GetInterfaceDatos(x.NLegGraPais, x.NClasePais).Result,
                            CLegGraInstitucionNavigation = x.CLegGraInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegGraInstitucion).Result,
                        }).Where(p => p.NLegGraDatCodigo == parnCodigo && p.CLegGraEstado == true&&p.CLegGraValida==false).ToList();
        }
        public ICollection<LegIdiomaOfimatica> LegDatosIdiomaOfimaticaSinValidacion(int parnCodigo)
        {
            return (ICollection<LegIdiomaOfimatica>)context.LegIdiomaOfimatica
                        .Select(x => new LegIdiomaOfimatica
                        {
                            NLegIdOfCodigo = x.NLegIdOfCodigo,
                            NLegIdOfCodigoDesc = x.NLegIdOfCodigoDesc,
                            NValorDesc = x.NValorDesc,
                            CLegIdOfTipo = x.CLegIdOfTipo,
                            NLegIdOfNivel = x.NLegIdOfNivel,
                            NValorNivel = x.NValorNivel,
                            DLegIdOfFecha = x.DLegIdOfFecha,
                            CLegIdOfArchivo = x.CLegIdOfArchivo,
                            NLegIdOfDatCodigo = x.NLegIdOfDatCodigo,
                            CLegIdOfValida = x.CLegIdOfValida,
                            CLegIdOfEstado = x.CLegIdOfEstado,
                            CUsuRegistro = x.CLegIdOfArchivo,
                            vCodigoDesc = repositorycons.GetConstanteDatos(x.NLegIdOfCodigoDesc, x.NValorDesc).Result,
                            vNivel = repositorycons.GetConstanteDatos(x.NLegIdOfNivel, x.NValorNivel).Result,
                        }).Where(p => p.NLegIdOfDatCodigo == parnCodigo && p.CLegIdOfEstado == true&&p.CLegIdOfValida==false).ToList();
        }

        public ICollection<LegInvestigador> LegDatosInvestigadorSinValidar(int parnCodigo)
        {
            return (ICollection<LegInvestigador>)context.LegInvestigador
                        .Select(x => new LegInvestigador
                        {
                            NLegInvCodigo = x.NLegInvCodigo,
                            NLegInvCentroRegistro = x.NLegInvCentroRegistro,
                            NValorCentroRegistro = x.NValorCentroRegistro,
                            CLegInvNroRegistro = x.CLegInvNroRegistro,
                            DLegInvFechaInicio = x.DLegInvFechaInicio,
                            DLegInvFechaFin = x.DLegInvFechaFin,
                            CLegInvArchivo = x.CLegInvArchivo,
                            NLegInvDatCodigo = x.NLegInvDatCodigo,
                            CLegInvValida = x.CLegInvValida,
                            CLegInvEstado = x.CLegInvEstado,
                            CUsuRegistro = x.CLegInvArchivo,
                            vCentroRegistro = repositoryinterf.GetInterfaceDatos(x.NValorCentroRegistro, x.NLegInvCentroRegistro).Result,
                            vNivelRenacyt = x.NLegInvNivelRenacyt.HasValue && x.NValorNivelRenacyt.HasValue ? repositoryinterf.GetInterfaceDatos(x.NValorNivelRenacyt.Value, x.NLegInvNivelRenacyt.Value).Result: null,
                        }).Where(p => p.NLegInvDatCodigo == parnCodigo && p.CLegInvEstado == true && p.CLegInvValida==false).ToList();
        }
        public ICollection<LegParticipacionCongSem> LegDatosParticipacionSinValidar(int parnCodigo)
        {
            return (ICollection<LegParticipacionCongSem>)context.LegParticipacionCongSem
                        .Select(x => new LegParticipacionCongSem
                        {
                            NLegParCodigo = x.NLegParCodigo,
                            CLegParInstitucion = x.CLegParInstitucion ?? "",
                            CLegParOtraInst = x.CLegParOtraInst ?? "",
                            CLegParPais = x.CLegParPais ?? "",
                            NLegParRol = x.NLegParRol,
                            NValorRol = x.NValorRol,
                            NLegParAmbito = x.NLegParAmbito,
                            NValorAmbito = x.NValorAmbito,
                            CLegParNombre = x.CLegParNombre,
                            DLegParFecha = x.DLegParFecha,
                            DLegParFechaFin = x.DLegParFechaFin,
                            NLegParHoras = x.NLegParHoras,
                            CLegParArchivoOrig = x.CLegParArchivoOrig == null ? "" : x.CLegParArchivoOrig,
                            CLegParArchivo = x.CLegParArchivo,
                            NLegParDatCodigo = x.NLegParDatCodigo,
                            CLegParValida = x.CLegParValida,
                            CLegParEstado = x.CLegParEstado,
                            CUsuRegistro = x.CLegParArchivo,
                            vAmbito = repositoryinterf.GetInterfaceDatos(x.NValorAmbito, x.NLegParAmbito).Result,
                            vRol = repositoryinterf.GetInterfaceDatos(x.NValorRol, x.NLegParRol).Result,
                            CLegParInstitucionNavigation = x.CLegParInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegParInstitucion).Result
                        }).Where(p => p.NLegParDatCodigo == parnCodigo && p.CLegParEstado == true&& p.CLegParValida ==false).ToList();
        }
        
        public ICollection<LegProfesNoDocente> LegDatosExperienciaNoDocenteSinValidar(int parnCodigo)
        {
            return (ICollection<LegProfesNoDocente>)context.LegProfesNoDocente
                        .Select(x => new LegProfesNoDocente
                        {
                            NLegProCodigo = x.NLegProCodigo,
                            CLegProInstitucion = x.CLegProInstitucion ?? "",
                            CLegProPais = x.CLegProPais ?? "",
                            CLegProOtraInst = x.CLegProOtraInst ?? "",
                            NLegProCargo = x.NLegProCargo,
                            NValorCargo = x.NValorCargo,
                            CLegProCargoProf = x.CLegProCargoProf,
                            DLegProFechaInicio = x.DLegProFechaInicio,
                            DLegProFechaFin = x.DLegProFechaFin,
                            CLegProArchivo = x.CLegProArchivo,
                            NLegProDatCodigo = x.NLegProDatCodigo,
                            CLegProValida = x.CLegProValida,
                            CLegProEstado = x.CLegProEstado,
                            CUsuRegistro = x.CLegProArchivo,
                            vCargo = repositorycons.GetConstanteDatos(x.NLegProCargo ?? 0, x.NValorCargo ?? 0).Result,
                            CLegProInstitucionNavigation = x.CLegProInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegProInstitucion).Result,
                        }).Where(p => p.NLegProDatCodigo == parnCodigo && p.CLegProEstado == true&& p.CLegProValida ==false).ToList();
        }

        public ICollection<LegProyeccionSocial> LegDatosProyeccionSocialSinValidar(int parnCodigo)
        {
            return (ICollection<LegProyeccionSocial>)context.LegProyeccionSocial
                        .Select(x => new LegProyeccionSocial
                        {
                            NLegProyCodigo = x.NLegProyCodigo,
                            CLegProyInstitucion = x.CLegProyInstitucion ?? "",
                            CLegProyOtraInst = x.CLegProyOtraInst ?? "",
                            CLegProyPais = x.CLegProyPais ?? "",
                            NLegProyTipo = x.NLegProyTipo,
                            NValorTipo = x.NValorTipo,
                            CLegProyDescripcion = x.CLegProyDescripcion,
                            DLegProyFechaInicio = x.DLegProyFechaInicio,
                            DLegProyFechaFin = x.DLegProyFechaFin,
                            CLegProyArchivo = x.CLegProyArchivo,
                            NLegProyDatCodigo = x.NLegProyDatCodigo,
                            CLegProyValida = x.CLegProyValida,
                            CLegProyEstado = x.CLegProyEstado,
                            CUsuRegistro = x.CLegProyArchivo,
                            vTipo = repositorycons.GetConstanteDatos(x.NLegProyTipo, x.NValorTipo).Result,
                            CLegProyInstitucionNavigation = x.CLegProyInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegProyInstitucion).Result,
                        }).Where(p => p.NLegProyDatCodigo == parnCodigo && p.CLegProyEstado == true && p.CLegProyValida==false).ToList();
        }

        public ICollection<LegReconocimiento> LegDatosReconocimientoSinValidar(int parnCodigo)
        {
            return (ICollection<LegReconocimiento>)context.LegReconocimiento
                        .Select(x => new LegReconocimiento
                        {
                            NLegRecCodigo = x.NLegRecCodigo,
                            NLegRecDocumento = x.NLegRecDocumento,
                            NValorDocumento = x.NValorDocumento,
                            NLegRecTipo = x.NLegRecTipo,
                            NValorTipo = x.NValorTipo,
                            CLegRecInstitucion = x.CLegRecInstitucion ?? "",
                            CLegRecOtraInst = x.CLegRecOtraInst ?? "",
                            CLegRecPais = x.CLegRecPais ?? "",
                            DLegRecFecha = x.DLegRecFecha,
                            CLegRecArchivo = x.CLegRecArchivo,
                            NLegRecDatCodigo = x.NLegRecDatCodigo,
                            CLegRecValida = x.CLegRecValida,
                            CLegRecEstado = x.CLegRecEstado,
                            CUsuRegistro = x.CLegRecArchivo,
                            vTipo = repositorycons.GetConstanteDatos(x.NLegRecTipo, x.NValorTipo).Result,
                            vDocumento = repositorycons.GetConstanteDatos(x.NLegRecDocumento, x.NValorDocumento).Result,
                            CLegRecInstitucionNavigation = x.CLegRecInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegRecInstitucion).Result,
                        }).Where(p => p.NLegRecDatCodigo == parnCodigo && p.CLegRecEstado == true && p.CLegRecValida==false).ToList();
        }

        public ICollection<LegRegimenDedicacion> LegDatosRegimenDedicacionSinValidar(int parnCodigo)
        {
            return (ICollection<LegRegimenDedicacion>)context.LegRegimenDedicacion
                        .Select(x => new LegRegimenDedicacion
                        {
                            NLegRegCodigo = x.NLegRegCodigo,
                            CLegCatInstitucion = x.CLegCatInstitucion ?? "",
                            CLegRegOtraInst = x.CLegRegOtraInst ?? "",
                            CLegRegPais = x.CLegRegPais ?? "",
                            NLegRegDedicacion = x.NLegRegDedicacion,
                            NValorDedicacion = x.NValorDedicacion,
                            DLegRegFechaInicio = x.DLegRegFechaInicio,
                            DLegRegFechaFin = x.DLegRegFechaFin,
                            CLegRegArchivo = x.CLegRegArchivo,
                            NLegRegDatCodigo = x.NLegRegDatCodigo,
                            CLegRegValida = x.CLegRegValida,
                            CLegRegEstado = x.CLegRegEstado,
                            CUsuRegistro = x.CLegRegArchivo,
                            vDedicacion = repositorycons.GetConstanteDatos(x.NLegRegDedicacion, x.NValorDedicacion).Result,
                            CLegCatInstitucionNavigation = x.CLegCatInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegCatInstitucion).Result,
                        }).Where(p => p.NLegRegDatCodigo == parnCodigo && p.CLegRegEstado == true&&p.CLegRegValida==false).ToList();
        }

        public ICollection<LegTesisAseJur> LegDatosTesisAsJurSinValidar(int parnCodigo)
        {
            return (ICollection<LegTesisAseJur>)context.LegTesisAseJur
                        .Select(x => new LegTesisAseJur
                        {
                            NLegTesCodigo = x.NLegTesCodigo,
                            NLegTesTipo = x.NLegTesTipo,
                            NValorTipo = x.NValorTipo,
                            NLegTesNivel = x.NLegTesNivel,
                            NValorNivel = x.NValorNivel,
                            DLegTesFecha = x.DLegTesFecha,
                            CLegTesNroResolucion = x.CLegTesNroResolucion,
                            CLegTesArchivo = x.CLegTesArchivo,
                            NLegTesDatCodigo = x.NLegTesDatCodigo,
                            CLegTesValida = x.CLegTesValida,
                            CLegTesEstado = x.CLegTesEstado,
                            CUsuRegistro = x.CLegTesArchivo,
                            vTipo = repositoryinterf.GetInterfaceDatos(x.NValorTipo, x.NLegTesTipo).Result,
                            vNivel = repositorycons.GetConstanteDatos(x.NLegTesNivel, x.NValorNivel).Result
                        }).Where(p => p.NLegTesDatCodigo == parnCodigo && p.CLegTesEstado == true && p.CLegTesValida == false).ToList();
        }

      
        public ICollection<LegAdminitrativaCarga> LegDatosCargaAdministrativaSinValidar(int parnCodigo)
        {
            return (ICollection<LegAdminitrativaCarga>)context.LegAdminitrativaCarga
                    .Select(x => new LegAdminitrativaCarga
                    {
                        NLegAdmCodigo = x.NLegAdmCodigo,
                        NLegAdmCargo = x.NLegAdmCargo,
                        NClaseCargo = x.NClaseCargo,
                        CLegAdmInstitucion = x.CLegAdmInstitucion ?? "",
                        CLegAdmOtraInst = x.CLegAdmOtraInst ?? "",
                        CLegAdmPais = x.CLegAdmPais ?? "",
                        CLegAdmDocumento = x.CLegAdmDocumento,
                        DLegAdmFechaInicio = x.DLegAdmFechaInicio,
                        DLegAdmFechaFin = x.DLegAdmFechaFin,
                        CLegAdmArchivo = x.CLegAdmArchivo,
                        NLegAdmDatCodigo = x.NLegAdmDatCodigo,
                        CLegAdmValida = x.CLegAdmValida,
                        CLegAdmEstado = x.CLegAdmEstado,
                        CUsuRegistro = x.CLegAdmArchivo,
                        vCargo = repositorycons.GetConstanteDatos(x.NLegAdmCargo, x.NClaseCargo).Result,
                        CLegAdmInstitucionNavigation = x.CLegAdmInstitucion == null ? null : repositorypersona.GetPersonaDatos(x.CLegAdmInstitucion).Result,
                    }).Where(p => p.NLegAdmDatCodigo == parnCodigo && p.CLegAdmEstado == true&&p.CLegAdmValida==false).ToList();
        }


    }
}
