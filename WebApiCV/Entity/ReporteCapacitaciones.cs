using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class ReporteCapacitaciones
    {
        public string cPerCodigo { get; set; }
        public string cPerApellido { get; set; }
        public string cPerNombre { get; set; }
        public string cCargo { get; set; }
        public string cArea { get; set; }
        public int nTipo { get; set; }
        public string cTipoDesc { get; set; }
        public int nRol { get; set; }
        public string cPerUsuCodigo { get; set; }
        public string cPerEmail { get; set; }
        public string cPerNroDoc { get; set; }
        public string cPerTipoDoc { get; set; }
        public string cCapTema { get; set; }
        public DateTime dCapFechaInicio { get; set; }
        public DateTime dCapFechaFin { get; set; }
        public int nCapHoras { get; set; }
        public string cLegCIArchivo { get; set; }
        public string cCompetencias { get; set; }

    }
}
