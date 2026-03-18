using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegReconocimiento
    {
        public int NLegRecCodigo { get; set; }
        public int NLegRecDocumento { get; set; }
        public int NValorDocumento { get; set; }
        public int NLegRecTipo { get; set; }
        public int NValorTipo { get; set; }
        public string CLegRecInstitucion { get; set; }
        public string CLegRecOtraInst { get; set; }
        public string CLegRecPais { get; set; }
        public DateTime DLegRecFecha { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegRecArchivo { get; set; }
        public int NLegRecDatCodigo { get; set; }
        public bool CLegRecValida { get; set; }
        public bool? CLegRecEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual Persona CLegRecInstitucionNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Constante vDocumento { get; set; }
        [NotMapped]
        public virtual Constante vTipo { get; set; }
        public virtual LegDatosGenerales NLegRecDatCodigoNavigation { get; set; }
    }
}
