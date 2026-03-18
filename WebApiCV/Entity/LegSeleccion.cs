using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegSeleccion
    {
        public int NLegSelCodigo { get; set; }
        public int NLegSelCargo { get; set; }
        public int NLegValCargo { get; set; }
        public int NLegSelArea { get; set; }
        public int NLegValArea { get; set; }
        public DateTime DLegSelFecha { get; set; }
        public string CLegSelEvaluacionCv { get; set; }
        [NotMapped]
        public virtual IFormFile cFileEvaluacionCv { get; set; }
        public string CLegSelClaseModelo { get; set; }
        [NotMapped]
        public virtual IFormFile cFileClaseModelo { get; set; }
        public string CLegSelEvaluacionPsico { get; set; }
        [NotMapped]
        public virtual IFormFile cFileEvaluacionPsico { get; set; }
        public string CLegSelEntrevistaPers { get; set; }
        [NotMapped]
        public virtual IFormFile cFileEntrevistaPers { get; set; }
        public bool? BLegSelEstado { get; set; }
        public int NLegSelDatCodigo { get; set; }
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
