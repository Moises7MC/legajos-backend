using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegTesisAseJur
    {
        public int NLegTesCodigo { get; set; }
        public int NLegTesTipo { get; set; }
        public int NValorTipo { get; set; }
        public int NLegTesNivel { get; set; }
        public int NValorNivel { get; set; }
        public DateTime DLegTesFecha { get; set; }
        public string CLegTesNroResolucion { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegTesArchivo { get; set; }

        // Nuevos campos para obtener el Pais, Insitucion (lugar) de la actividad - EBS 01/2026
        public int? NLegTesPais { get; set; }
        public int? NClasePais { get; set; }
        public string CLegTesInstitucion { get; set; } = string.Empty; // Valor por defecto
        public string CLegTesOtraInst { get; set; } = string.Empty; // Valor por defecto
        [NotMapped]
        public virtual Persona CLegTesInstitucionNavigation { get; set; }
        [NotMapped]
        public virtual Interface vPais { get; set; }
        // Nuevos campos para obtener el Pais, Insitucion (lugar) de la actividad - EBS 01/2026

        public int NLegTesDatCodigo { get; set; }
        public bool? CLegTesValida { get; set; }
        public bool? CLegTesEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Constante vNivel { get; set; }
        public virtual LegDatosGenerales NLegTesDatCodigoNavigation { get; set; }
        [NotMapped]
        public virtual Interface vTipo { get; set; }
    }
}
