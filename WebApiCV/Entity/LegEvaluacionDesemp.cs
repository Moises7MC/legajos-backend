using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApiCV.Entity
{
    public partial class LegEvaluacionDesemp
    {
        public int NLegEvalCodigo { get; set; }
        public int NLegEvalCargo { get; set; }
        public int NLegValCargo { get; set; }
        public int NLegEvalArea { get; set; }
        public int NLegValArea { get; set; }
        public string CLegEvalSemestre { get; set; }
        public string CLegEvalAnio { get; set; }
        public decimal NLegEvalPuntaje { get; set; }
        public int NLegEvalNivel { get; set; }
        public int NLegValNivel { get; set; }
        public string CLegEvalArchivo { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public bool? BLegEvalEstado { get; set; }
        public int NLegEvalDatCodigo { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }

        [NotMapped]
        public virtual Interface vArea { get; set; }

        [NotMapped]
        public virtual Interface vCargo { get; set; }

        [NotMapped]
        public virtual Constante vNivel { get; set; }
        public virtual LegDatosGenerales vDatosGenerales { get; set; }
    }
}
