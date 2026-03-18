using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegDeclaracionJurada
    {
        public int NLegDjcodigo { get; set; }
        public DateTime DLegDjfecha { get; set; }
        public string CLegDjanexo2 { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjanexo2 { get; set; }
        public string CLegDjanexo6 { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjanexo6 { get; set; }
        public string CLegDjanexo7 { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjanexo7 { get; set; }
        public bool? BLegDjestado { get; set; }
        public int NLegDjdatCodigo { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }


        // ------------------ EDGAR_BS-2025---------------------------------------->
        public string? CLegDjanexo1 { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjanexo1 { get; set; }
        public string? CLegDjanexo2_2 { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjanexo2_2 { get; set; }
        public string? CLegDjanexo3 { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjanexo3 { get; set; }
        public string? CLegDjanexo4 { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjanexo4 { get; set; }
        public string? CLegDjanexo5 { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjanexo5 { get; set; }
        public string? CLegDjanexo6_2 { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjanexo6_2 { get; set; }
        public string? CLegDjDNI { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjDNI { get; set; }
        public string? CLegDjDNI_DH { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjDNI_DH { get; set; }
        
        public string? CLegDjFotoCarnet { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjFotoCarnet { get; set; }

        public string? CLegDjNumCta { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjNumCta { get; set; }

        public string? CLegDjConsJubilacion { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjConsJubilacion { get; set; }

        public string? CLegDjConsAfiliacionOnpAfp { get; set; }
        [NotMapped]
        public virtual IFormFile cFileDjConsAfiliacionOnpAfp { get; set; }


        // ------------------ EDGAR_BS-2025---------------------------------------->

        public virtual LegDatosGenerales vDatosGenerales { get; set; }
    }
}
