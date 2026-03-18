using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegRegimenDedicacion
    {
        public int NLegRegCodigo { get; set; }
        public string CLegCatInstitucion { get; set; }

        public string CLegRegOtraInst { get; set; }
        public string CLegRegPais { get; set; }
        public int NLegRegDedicacion { get; set; }
        public int NValorDedicacion { get; set; }
        public DateTime DLegRegFechaInicio { get; set; }
        public DateTime DLegRegFechaFin { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegRegArchivo { get; set; }
        public int NLegRegDatCodigo { get; set; }
        public bool? CLegRegValida { get; set; }
        public bool CLegRegEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual Persona CLegCatInstitucionNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Constante vDedicacion { get; set; }
        public virtual LegDatosGenerales NLegRegDatCodigoNavigation { get; set; }
    }
}
