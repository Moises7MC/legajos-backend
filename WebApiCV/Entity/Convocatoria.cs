using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class Convocatoria
    {
        public int nConCodigo { get; set; }

        public string cConDescripcion { get; set; }

        public int nPrdCodigo { get; set; }

        public bool nConEstado { get; set; }

        public DateTime dConFecRegistro { get; set; }
    }
}
