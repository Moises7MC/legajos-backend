using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegCapacitacionInterna
    {
        public int NLegCicodigo { get; set; }
        public string CLegCicompetenciaMejora { get; set; }
        public int NCapCodigo { get; set; }
        public int NLegDatCodigo { get; set; }

        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        [NotMapped]
        public virtual IFormFile[] cFiles { get; set; }
        public string CLegCiarchivo { get; set; }
        public bool? BLegCiestado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual CapacitacionesUss vCapacitacionUSS { get; set; }
        public virtual LegDatosGenerales vDatosGenerales { get; set; }
    }
}
