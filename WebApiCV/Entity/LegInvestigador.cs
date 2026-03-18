using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegInvestigador
    {
        public int NLegInvCodigo { get; set; }
        public int NLegInvCentroRegistro { get; set; }
        public int NValorCentroRegistro { get; set; }

        //Variables que almacenan la Clase y el codigo del Nivel de Renacyt de la Tabla Interface EBS - 12/2025
        public int? NLegInvNivelRenacyt { get; set; }
        public int? NValorNivelRenacyt { get; set; }
        // ------------------------------------------------------------------------------------------------------

        public string CLegInvNroRegistro { get; set; }
        public DateTime DLegInvFechaInicio { get; set; }
        public DateTime DLegInvFechaFin { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegInvArchivo { get; set; }
        public int NLegInvDatCodigo { get; set; }
        public bool? CLegInvValida { get; set; }
        public bool? CLegInvEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }

        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }

        [NotMapped]
        public virtual Interface vCentroRegistro { get; set; }
        [NotMapped]
        public virtual Interface vNivelRenacyt { get; set; }  // Lista de los Niveles de Renacyt EBS - 12/2025
        public virtual LegDatosGenerales NLegInvDatCodigoNavigation { get; set; }

    }
}
