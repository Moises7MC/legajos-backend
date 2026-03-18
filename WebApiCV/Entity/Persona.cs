using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class Persona
    {
        public Persona()
        {
            //LegAdminitrativaCargaUnivers = new HashSet<LegAdminitrativaCarga>();
            //LegCategoriaDocenteUnivers = new HashSet<LegCategoriaDocente>();
            //LegDatosGeneralesColegProf = new HashSet<LegDatosGenerales>();
            //LegDocenciaUnivUnivers = new HashSet<LegDocenciaUniv>();
            //LegGradoTituloUnivers = new HashSet<LegGradoTitulo>();
            //LegParticipacionCongSemUnivers = new HashSet<LegParticipacionCongSem>();
            //LegProfesNoDocenteUnivers = new HashSet<LegProfesNoDocente>();
            //LegProyeccionSocialUnivers = new HashSet<LegProyeccionSocial>();
            //LegReconocimientoUnivers = new HashSet<LegReconocimiento>();
            //LegRegimenDedicacionUnivers = new HashSet<LegRegimenDedicacion>();
            //LegCapacitacionesUnivers = new HashSet<LegCapacitaciones>();
            //vPermisosModulos = new HashSet<PermisosModulo>();
        }

        public string CPerCodigo { get; set; }
        public string CPerApellido { get; set; }
        public string CPerApellPat { get; set; }
        public string CPerNombre { get; set; }
        public DateTime? DPerNacimiento { get; set; }
        public int? NPerTipo { get; set; }
        public int? NPerEstado { get; set; }
        public string CUbigeoCodigo { get; set; }
        public string Cperestadobiblio { get; set; }
        public int? NUbiGeoCodigo { get; set; }

        //[NotMapped]
        //public virtual PerUsuario PerUsuario { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegAdminitrativaCarga> LegAdminitrativaCargaUnivers { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegCategoriaDocente> LegCategoriaDocenteUnivers { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegDatosGenerales> LegDatosGeneralesColegProf { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegDocenciaUniv> LegDocenciaUnivUnivers { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegGradoTitulo> LegGradoTituloUnivers { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegParticipacionCongSem> LegParticipacionCongSemUnivers { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegProfesNoDocente> LegProfesNoDocenteUnivers { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegProyeccionSocial> LegProyeccionSocialUnivers { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegReconocimiento> LegReconocimientoUnivers { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegRegimenDedicacion> LegRegimenDedicacionUnivers { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegCapacitaciones> LegCapacitacionesUnivers { get; set; }
        //[NotMapped]

        //public virtual ICollection<PermisosModulo> vPermisosModulos { get; set; }

    }
}
