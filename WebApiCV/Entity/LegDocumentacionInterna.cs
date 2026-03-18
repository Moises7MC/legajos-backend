using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegDocumentacionInterna
    {
        public int NLegDicodigo { get; set; }
        public string CLegDiarchivo { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public int NLegDitipoDoc { get; set; }
        public int NLegValTipoDoc { get; set; }
        public string CLegDicodigo { get; set; }
        public string CLegDidescripcion { get; set; }
        public bool? BLegDiestado { get; set; }
        public int NLegDidatCodigo { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }

        [NotMapped]
        public virtual Constante vTipo { get; set; }
        public virtual LegDatosGenerales vDatosGenerales { get; set; }
    }
}
