using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegArchivos
    {
        public int NLegArcCodigo { get; set; }

        public string CPerCodigo { get; set; }
        public string CLegArcNombre { get; set; }
        public int NLegArcTipo { get; set; }

        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }

    }
}
