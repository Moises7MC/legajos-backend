using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegParticipacionCongSem
    {
        public int NLegParCodigo { get; set; }
        public string CLegParInstitucion { get; set; }
        public string CLegParOtraInst { get; set; }
        public string CLegParPais { get; set; }
        public int NLegParRol { get; set; }
        public int NValorRol { get; set; }
        public int NLegParAmbito { get; set; }
        public int NValorAmbito { get; set; }
        public string CLegParNombre { get; set; }
        public DateTime DLegParFecha { get; set; }

        public DateTime DLegParFechaFin { get; set; } //Nuevos
        public int NLegParHoras { get; set; } //Nuevos

        public string CLegParArchivoOrig { get; set; } //Nuevos

        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegParArchivo { get; set; }
        public int NLegParDatCodigo { get; set; }
        public bool? CLegParValida { get; set; }
        public bool? CLegParEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual Persona CLegParInstitucionNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Interface vAmbito { get; set; }

        public virtual LegDatosGenerales NLegParDatCodigoNavigation { get; set; }
        [NotMapped]
        public virtual Interface vRol { get; set; }
        [NotMapped]
        public virtual IFormFile[] cFiles { get; set; }
        [NotMapped]
        public string cPerCodigo { get; set; }

    }
}
