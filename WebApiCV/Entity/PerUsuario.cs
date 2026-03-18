using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class PerUsuario
    {
        public PerUsuario()
        {
            //LegAdminitrativaCargaCUsuModificaNavigations = new HashSet<LegAdminitrativaCarga>();
            //LegAdminitrativaCargaCUsuRegistroNavigations = new HashSet<LegAdminitrativaCarga>();
            //LegCapacitacionesCUsuModificaNavigations = new HashSet<LegCapacitaciones>();
            //LegCapacitacionesCUsuRegistroNavigations = new HashSet<LegCapacitaciones>();
            //LegCategoriaDocenteCUsuModificaNavigations = new HashSet<LegCategoriaDocente>();
            //LegCategoriaDocenteCUsuRegistroNavigations = new HashSet<LegCategoriaDocente>();
            //LegDatosGeneralesCUsuModificaNavigations = new HashSet<LegDatosGenerales>();
            //LegDatosGeneralesCUsuRegistroNavigations = new HashSet<LegDatosGenerales>();
            //LegDocenciaUnivCUsuModificaNavigations = new HashSet<LegDocenciaUniv>();
            //LegDocenciaUnivCUsuRegistroNavigations = new HashSet<LegDocenciaUniv>();
            //LegGradoTituloCUsuModificaNavigations = new HashSet<LegGradoTitulo>();
            //LegGradoTituloCUsuRegistroNavigations = new HashSet<LegGradoTitulo>();
            //LegIdiomaOfimaticaCUsuModificaNavigations = new HashSet<LegIdiomaOfimatica>();
            //LegIdiomaOfimaticaCUsuRegistroNavigations = new HashSet<LegIdiomaOfimatica>();
            //LegInvestigadorCUsuModificaNavigations = new HashSet<LegInvestigador>();
            //LegInvestigadorCUsuRegistroNavigations = new HashSet<LegInvestigador>();
            //LegParticipacionCongSemCUsuModificaNavigations = new HashSet<LegParticipacionCongSem>();
            //LegParticipacionCongSemCUsuRegistroNavigations = new HashSet<LegParticipacionCongSem>();
            //LegProduccionCienciaCUsuModificaNavigations = new HashSet<LegProduccionCiencia>();
            //LegProduccionCienciaCUsuRegistroNavigations = new HashSet<LegProduccionCiencia>();
            //LegProfesNoDocenteCUsuModificaNavigations = new HashSet<LegProfesNoDocente>();
            //LegProfesNoDocenteCUsuRegistroNavigations = new HashSet<LegProfesNoDocente>();
            //LegProyeccionSocialCUsuModificaNavigations = new HashSet<LegProyeccionSocial>();
            //LegProyeccionSocialCUsuRegistroNavigations = new HashSet<LegProyeccionSocial>();
            //LegReconocimientoCUsuModificaNavigations = new HashSet<LegReconocimiento>();
            //LegReconocimientoCUsuRegistroNavigations = new HashSet<LegReconocimiento>();
            //LegRegimenDedicacionCUsuModificaNavigations = new HashSet<LegRegimenDedicacion>();
            //LegRegimenDedicacionCUsuRegistroNavigations = new HashSet<LegRegimenDedicacion>();
            //LegTesisAseJurCUsuModificaNavigations = new HashSet<LegTesisAseJur>();
            //LegTesisAseJurCUsuRegistroNavigations = new HashSet<LegTesisAseJur>();
        }

        public string CPerCodigo { get; set; }
        public string CPerUsuCodigo { get; set; }
        public string CPerUsuClave { get; set; }
        public int CPerUsuEstado { get; set; }
        public string CPerJuridica { get; set; }
        public DateTime? CPudFecha { get; set; }
        [NotMapped]
        public string nPerRelacion { get; set; }

        

        [NotMapped]
        public string cPerApellido  {get;set;}
        [NotMapped] public string cPerNombre { get; set; }

        //[NotMapped]
        //public virtual Persona CPerCodigoNavigation { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegAdminitrativaCarga> LegAdminitrativaCargaCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegAdminitrativaCarga> LegAdminitrativaCargaCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegCapacitaciones> LegCapacitacionesCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegCapacitaciones> LegCapacitacionesCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegCategoriaDocente> LegCategoriaDocenteCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegCategoriaDocente> LegCategoriaDocenteCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegDatosGenerales> LegDatosGeneralesCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegDatosGenerales> LegDatosGeneralesCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegDocenciaUniv> LegDocenciaUnivCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegDocenciaUniv> LegDocenciaUnivCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegGradoTitulo> LegGradoTituloCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegGradoTitulo> LegGradoTituloCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegIdiomaOfimatica> LegIdiomaOfimaticaCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegIdiomaOfimatica> LegIdiomaOfimaticaCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegInvestigador> LegInvestigadorCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegInvestigador> LegInvestigadorCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegParticipacionCongSem> LegParticipacionCongSemCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegParticipacionCongSem> LegParticipacionCongSemCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegProduccionCiencia> LegProduccionCienciaCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegProduccionCiencia> LegProduccionCienciaCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegProfesNoDocente> LegProfesNoDocenteCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegProfesNoDocente> LegProfesNoDocenteCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegProyeccionSocial> LegProyeccionSocialCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegProyeccionSocial> LegProyeccionSocialCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegReconocimiento> LegReconocimientoCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegReconocimiento> LegReconocimientoCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegRegimenDedicacion> LegRegimenDedicacionCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegRegimenDedicacion> LegRegimenDedicacionCUsuRegistroNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegTesisAseJur> LegTesisAseJurCUsuModificaNavigations { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegTesisAseJur> LegTesisAseJurCUsuRegistroNavigations { get; set; }
    }
}
