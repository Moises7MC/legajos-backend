using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegOrdinarizacion
    {
        public int NLegOrdCodigo { get; set; }
        public int NLegOrdCargo { get; set; }
        public int NLegValCargo { get; set; }
        public int NLegOrdArea { get; set; }
        public int NLegOrdValArea { get; set; }
        public DateTime DLegOrdFecha { get; set; }
        public string CLegOrdFichaInscripcion { get; set; }
        [NotMapped]
        public virtual IFormFile cFileFichaInscripcion { get; set; }
        public string CLegOrdEvaluacionCv { get; set; }
        [NotMapped]
        public virtual IFormFile cFileEvaluacionCv { get; set; }
        public string CLegOrdClaseModelo { get; set; }
        [NotMapped]
        public virtual IFormFile cFileClaseModelo { get; set; }
        public string CLegOrdEvaluacionPsico { get; set; }
        [NotMapped]
        public virtual IFormFile cFileEvaluacionPsico { get; set; }
        public string CLegOrdEntrevistaPers { get; set; }
        [NotMapped]
        public virtual IFormFile cFileEntrevistaPers { get; set; }
        public bool? BLegOrdEstado { get; set; }
        public int NLegOrdDatCodigo { get; set; }
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
