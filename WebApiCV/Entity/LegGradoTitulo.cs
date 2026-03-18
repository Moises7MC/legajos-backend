using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegGradoTitulo
    {
        public int NLegGraCodigo { get; set; }
        public int NLegGraGradoAcad { get; set; }
        public int NClaseGradoAcad { get; set; }
        public string CLegGraInstitucion { get; set; }
        public string CLegGraOtraInst { get; set; }
        public string CLegGraCarreraProf { get; set; }
        public int NLegGraPais { get; set; }
        public int NClasePais { get; set; }
        public int NLegGraUbigeo { get; set; }
        public int NClaseUbigeo { get; set; }
        public DateTime DLegGraFecha { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegGraArchivo { get; set; }
        public int NLegGraDatCodigo { get; set; }
        public bool? CLegGraValida { get; set; }
        public bool? CLegGraEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual Persona CLegGraInstitucionNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Interface vGradoAcad { get; set; }
        //public virtual Interface vUbigeo { get; set; }
        public virtual LegDatosGenerales NLegGraDatCodigoNavigation { get; set; }
        [NotMapped]
        public virtual Interface vPais { get; set; }
    }
}
