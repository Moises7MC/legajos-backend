using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class LegContrato
    {
        public int NLegConCodigo { get; set; }
        public int NLegConCargo { get; set; }
        public int NLegValCargo { get; set; }
        public int NLegConArea { get; set; }
        public int NLegValArea { get; set; }
        public DateTime DLegConFechaInicio { get; set; }
        public DateTime DLegConFechaFin { get; set; }
        public decimal NLegConSueldo { get; set; }
        public string CLegConArchivo { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public bool? BLegConEstado { get; set; }
        public int NLegConDatCodigo { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }

        [NotMapped]
        public virtual Interface vArea { get; set; }

        [NotMapped]
        public virtual Interface vCargo { get; set; }
        public virtual LegDatosGenerales vDatosGenerales { get; set; }
    }
}
