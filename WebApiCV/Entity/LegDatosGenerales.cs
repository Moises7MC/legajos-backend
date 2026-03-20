using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegDatosGenerales
    {
        public LegDatosGenerales()
        {
            LegAdminitrativaCarga = new HashSet<LegAdminitrativaCarga>();
            LegCapacitacionInternas = new HashSet<LegCapacitacionInterna>();
            LegContratos = new HashSet<LegContrato>();
            LegCapacitaciones = new HashSet<LegCapacitaciones>();
            LegCategoriaDocente = new HashSet<LegCategoriaDocente>();
            LegDocenciaUniv = new HashSet<LegDocenciaUniv>();
            LegGradoTitulo = new HashSet<LegGradoTitulo>();
            LegIdiomaOfimatica = new HashSet<LegIdiomaOfimatica>();
            LegInvestigador = new HashSet<LegInvestigador>();
            LegParticipacionCongSem = new HashSet<LegParticipacionCongSem>();
            LegProduccionCiencia = new HashSet<LegProduccionCiencia>();
            LegProfesNoDocente = new HashSet<LegProfesNoDocente>();
            LegProyeccionSocial = new HashSet<LegProyeccionSocial>();
            LegReconocimiento = new HashSet<LegReconocimiento>();
            LegRegimenDedicacion = new HashSet<LegRegimenDedicacion>();
            LegTesisAseJur = new HashSet<LegTesisAseJur>();
            LegEvaluacionDesemp = new HashSet<LegEvaluacionDesemp>();
            LegResoluciones = new HashSet<LegResolucion>();
            LegSeleccion = new HashSet<LegSeleccion>();
            LegOrdinarizacion = new HashSet<LegOrdinarizacion>();
            LegDeclaracionJurada = new HashSet<LegDeclaracionJurada>();
            LegDocumentacionInterna = new HashSet<LegDocumentacionInterna>();

            // EBS - 01/2026 ---------------------------->
            /* Listado Virtual para licencias o registros profesionales (colegios, nº colegiatura, condición, fechas, etc.) */
            LegLicenciaProfesional = new HashSet<LegLicenciaProfesional>();
            /* Listado Virtual para membresias (colegios, nº colegiatura, fechas, etc.) */
            LegMembresia = new HashSet<LegMembresia>();
            // EBS - 01/2026 <----------------------------
        }

        #region Datos Personales
        public int NLegDatCodigo { get; set; }
        public int NLegDatTipoDoc { get; set; }
        public int NClaseTipoDoc { get; set; }
        public string CLegDatNroDoc { get; set; }
        public string CLegDatApellidoPaterno { get; set; }
        public string CLegDatApellidoMaterno { get; set; }
        public string CLegDatNombres { get; set; }
        public DateTime DLegDatFechaNacimiento { get; set; }
        public int NLegDatSexo { get; set; }
        public int NClaseSexo { get; set; }
        public int NLegDatEstadoCivil { get; set; }
        public int NClaseEstadoCivil { get; set; }
        #endregion

        #region Archivos y Firmas
        [NotMapped] public virtual IFormFile cFile { get; set; }
        public string CLegDatFoto { get; set; }

        [NotMapped] public virtual IFormFile cFileFirma { get; set; }
        public string cLegDatFirma { get; set; }

        [NotMapped] public virtual IFormFile cFileConadis { get; set; }
        public string cLegDatArchivoConadis { get; set; }

        [NotMapped] public virtual IFormFile cFileSunedu { get; set; }
        public string cLegDatSunedu { get; set; }

        [NotMapped] public virtual IFormFile cFilePolicial { get; set; }
        public string cLegDatPolicial { get; set; }

        [NotMapped] public virtual IFormFile cFileJudicial { get; set; }
        public string cLegDatJudicial { get; set; }

        [NotMapped] public virtual IFormFile cFileBuenaSalud { get; set; }
        public string cLegDatBuenaSalud { get; set; }
        #endregion

        #region Declaración Jurada
        public bool declaracionjuradaflag { get; set; }
        public DateTime? fechadeclaracionjurada { get; set; }
        #endregion

        #region Contacto
        public string CLegDatEmail { get; set; }
        public string CLegDatTelefono { get; set; }
        public string CLegDatMovil { get; set; }
        #endregion

        #region Educación / Datos Académicos
        public int NLegDatGradoAcad { get; set; }
        public int NClaseGradoAcad { get; set; }
        public int NLegDatPais { get; set; }
        public int NClasePais { get; set; }
        public string CLegDatAcerca { get; set; }
        public int? nLegIdiomaNativo { get; set; }
        // Nuevos campos (2025)
        public string CLegDatMencionEnMayGradAcad { get; set; }      // Mención en el mayor grado académico
        public string CLegDatInstitucionMayGradAcad { get; set; }    // Institución del mayor grado académico
        #endregion

        #region Domicilio
        public int NLegDatZona { get; set; }
        public int NValorZona { get; set; }
        public int NLegDatTipoDomicilio { get; set; }
        public int NValorTipoDomicilio { get; set; }
        public string CLegDatCalleDomicilio { get; set; }
        public string CLegDatNroDomicilio { get; set; }
        public string CLegDatMzaDomicilio { get; set; }
        public string CLegDatLtDomicilio { get; set; }
        public string CLegDatDptoDomicilio { get; set; }
        public string CLegDatReferencia { get; set; }
        public int NLetDatUbigeo { get; set; }
        public int NClaseUbigeo { get; set; }
        public int NLetDatNacimiento { get; set; }
        public int NClaseNacimiento { get; set; }
        #endregion

        #region Colegio Profesional
        public string CLegDatColegioProf { get; set; }
        public string CLegDatNroColegiatura { get; set; }
        public int? NLegDatCondicionColeg { get; set; }
        public int? NValorCondicionColeg { get; set; }
        public DateTime? DLegDatosFechaEmisionColeg { get; set; }
        public DateTime? DLegDatosFechaExpiraColeg { get; set; }
        #endregion

        #region Discapacidad
        public int? nLegDatDiscapacidad { get; set; }
        public int? nLegDatTipoDiscapacidad { get; set; }
        public string? cLegDatOtraDiscapcidad { get; set; }
        #endregion

        #region Auditoría
        public bool? CLegDatEstado { get; set; }
        public string cPerCodigo { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        #endregion

        #region Régimen Pensionario (EdgarBS 2025)
        public int? NLegDatRegPenAfiliado { get; set; }
        public int? NValorAfiliado { get; set; }
        public int? NLegDatRegPenEntidad { get; set; }
        public int? NValorEntidad { get; set; }
        public DateTime? DLegDatRegPenFechaCese { get; set; }
        #endregion

        #region Cuenta de Haberes (EdgarBS 2025)
        public int? NLegDatCtaHabHaberes { get; set; }
        public int? NValorHaberes { get; set; }
        public int? NLegDatCtaHabBanco { get; set; }
        public int? NValorBanco { get; set; }
        public string CLegDatCtaHabNumCta { get; set; }
        public string CLegDatCtaHabNumCtaCci { get; set; }
        public int? NLegDatCtaHabBancoAperturar { get; set; }
        public int? NValorBancoAperturar { get; set; }
        public bool? NLegDatAceptaTerminos { get; set; }
        #endregion

        #region Relaciones (NotMapped)
        [NotMapped] public virtual Persona CLegDatColegioProfNavigation { get; set; }
        [NotMapped] public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped] public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped] public virtual Constante vCondicionColeg { get; set; }
        [NotMapped] public virtual Interface vPais { get; set; }
        [NotMapped] public virtual Constante vSexo { get; set; }
        [NotMapped] public virtual Constante vEstadoCivil { get; set; }
        [NotMapped] public virtual Interface vTipoDoc { get; set; }
        [NotMapped] public virtual Constante vTipoDomicilio { get; set; }
        [NotMapped] public virtual Constante vZona { get; set; }
        [NotMapped] public virtual Interface vNacimiento { get; set; }
        [NotMapped] public virtual Interface vGradoAcad { get; set; }
        [NotMapped] public virtual Constante vIdiomaNativo { get; set; }
        [NotMapped] public virtual Constante vAfiliado { get; set; }
        [NotMapped] public virtual Constante vEntidad { get; set; }
        [NotMapped] public virtual Constante vHaberes { get; set; }
        [NotMapped] public virtual Constante vBanco { get; set; }
        [NotMapped] public virtual Constante vBancoAperturar { get; set; }
        #endregion

        #region Colecciones
        public virtual ICollection<LegAdminitrativaCarga> LegAdminitrativaCarga { get; set; }
        public virtual ICollection<LegCapacitacionInterna> LegCapacitacionInternas { get; set; }
        public virtual ICollection<LegContrato> LegContratos { get; set; }
        public virtual ICollection<LegCapacitaciones> LegCapacitaciones { get; set; }
        public virtual ICollection<LegCategoriaDocente> LegCategoriaDocente { get; set; }
        public virtual ICollection<LegDocenciaUniv> LegDocenciaUniv { get; set; }
        public virtual ICollection<LegGradoTitulo> LegGradoTitulo { get; set; }
        public virtual ICollection<LegIdiomaOfimatica> LegIdiomaOfimatica { get; set; }
        public virtual ICollection<LegInvestigador> LegInvestigador { get; set; }
        public virtual ICollection<LegParticipacionCongSem> LegParticipacionCongSem { get; set; }
        public virtual ICollection<LegProduccionCiencia> LegProduccionCiencia { get; set; }
        public virtual ICollection<LegProfesNoDocente> LegProfesNoDocente { get; set; }
        public virtual ICollection<LegProyeccionSocial> LegProyeccionSocial { get; set; }
        public virtual ICollection<LegReconocimiento> LegReconocimiento { get; set; }
        public virtual ICollection<LegRegimenDedicacion> LegRegimenDedicacion { get; set; }
        public virtual ICollection<LegTesisAseJur> LegTesisAseJur { get; set; }
        public virtual ICollection<LegResolucion> LegResoluciones { get; set; }
        public virtual ICollection<LegEvaluacionDesemp> LegEvaluacionDesemp { get; set; }
        public virtual ICollection<LegSeleccion> LegSeleccion { get; set; }
        public virtual ICollection<LegOrdinarizacion> LegOrdinarizacion { get; set; }
        public virtual ICollection<LegDeclaracionJurada> LegDeclaracionJurada { get; set; }
        public virtual ICollection<LegDocumentacionInterna> LegDocumentacionInterna { get; set; }

        // EBS - 01/2026 ---------------------------->
        /* Listado Virtual para licencias o registros profesionales (colegios, nº colegiatura, condición, fechas, etc.) */
        public virtual ICollection<LegLicenciaProfesional> LegLicenciaProfesional { get; set; }
        /* Listado Virtual para membresias (colegios, nº colegiatura, fechas, etc.) */
        public virtual ICollection<LegMembresia> LegMembresia { get; set; }
        // EBS - 01/2026 <----------------------------
        #endregion
    }
}