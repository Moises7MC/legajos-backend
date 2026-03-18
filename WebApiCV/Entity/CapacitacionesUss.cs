using System;
using System.Collections.Generic;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class CapacitacionesUss
    {
        public CapacitacionesUss()
        {
            LegCapacitacionInternas = new HashSet<LegCapacitacionInterna>();
        }

        public int NCapCodigo { get; set; }
        public string CCapTema { get; set; }
        public DateTime DCapFechaInicio { get; set; }
        public DateTime DCapFechaFin { get; set; }
        public int NCapHoras { get; set; }
        public bool? BCapEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }

        public virtual ICollection<LegCapacitacionInterna> LegCapacitacionInternas { get; set; }
    }
}
