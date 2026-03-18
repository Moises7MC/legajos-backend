using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    /* Entidad para registrar licencias o registros profesionales (colegios, nº colegiatura, condición, fechas, archivo, etc.) - EBS 01/2026 */
    public partial class LegLicenciaProfesional
    {
        public int NLegLicCodigo { get; set; }

        public int NLegLicPais { get; set; }
        public int NClasePais { get; set; }
        public string CLegLicInstitucion { get; set; }
        public string CLegLicOtraInst { get; set; }

        [NotMapped]
        public virtual Persona CLegLicInstitucionNavigation { get; set; }
        [NotMapped]
        public virtual Interface vPais { get; set; }

        public string CLegLicNroRegistro { get; set; }

        public int NLegLicCondicion { get; set; }
        public int NClaseCondicion { get; set; }

        [NotMapped]
        public virtual Interface vCondicion { get; set; }

        public DateTime DLegLicFechaEmision { get; set; }
        public DateTime DLegLicFechaExpiracion { get; set; }
        
        public int NLegLicDatCodigo { get; set; }
        public bool? CLegLicValida { get; set; }
        public bool? CLegLicEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }

        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }


        public virtual LegDatosGenerales NLegLicDatCodigoNavigation { get; set; }

    }
}
