using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace WebApiCV.Entity
{
    public partial class LegResolucion
    {
        public int NLegResCodigo { get; set; }
        public int NLegResTipo { get; set; }
        public int NLegValTipo { get; set; }
        public DateTime DLegResFecha { get; set; }
        public string CLegResResuelve { get; set; }
        public string CLegResNroResolucion { get; set; }
        public string CLegResArchivo { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public bool BLegResEstado { get; set; }
        public int NLegResDatCodigo { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }

        [NotMapped]
        public virtual Constante vResolucion { get; set; }
        public virtual LegDatosGenerales vDatosGenerales { get; set; }
    }
}
