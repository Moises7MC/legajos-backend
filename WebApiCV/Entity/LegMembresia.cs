using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    /* Entidad para registrar membresias (colegios, nº colegiatura, fechas, etc.) - EBS 01/2026 */
    public partial class LegMembresia
    {
        public int NLegMemCodigo { get; set; }

        public int NLegMemPais { get; set; }
        public int NClasePais { get; set; }
        public string CLegMemInstitucion { get; set; }
        public string CLegMemOtraInst { get; set; }

        [NotMapped]
        public virtual Persona CLegMemInstitucionNavigation { get; set; }
        [NotMapped]
        public virtual Interface vPais { get; set; }

        public string CLegMemNroRegistro { get; set; }

        public DateTime DLegMemFechaEmision { get; set; }
        public DateTime DLegMemFechaExpiracion { get; set; }

        public int NLegMemDatCodigo { get; set; }
        public bool? CLegMemValida { get; set; }
        public bool? CLegMemEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }

        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }

        public virtual LegDatosGenerales NLegMemDatCodigoNavigation { get; set; }

    }
}
