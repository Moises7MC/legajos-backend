using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegAdminitrativaCarga
    {
        public int NLegAdmCodigo { get; set; }
        public int NLegAdmCargo { get; set; }
        public int NClaseCargo { get; set; }
        public string CLegAdmInstitucion { get; set; }
        public string CLegAdmOtraInst { get; set; }
        public string CLegAdmPais { get; set; }
        public string CLegAdmDocumento { get; set; }
        public DateTime DLegAdmFechaInicio { get; set; }
        public DateTime DLegAdmFechaFin { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegAdmArchivo { get; set; }
        public int NLegAdmDatCodigo { get; set; }
        public bool? CLegAdmValida { get; set; }
        public bool? CLegAdmEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual Persona CLegAdmInstitucionNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Constante vCargo { get; set; }
        public virtual LegDatosGenerales NLegAdmDatCodigoNavigation { get; set; }
    }
}
