using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegCapacitaciones
    {
        public int NLegCapCodigo { get; set; }
        public string CLegCapNombre { get; set; }
        public int NLegCapTipo { get; set; }
        public int NLegCapTipoEsp { get; set; }
        public int NLegCapHoras { get; set; }
        public DateTime DLegCapFechaInicio { get; set; }
        public DateTime DLegCapFechaFin { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
    
        [NotMapped]
        public virtual IFormFile[] cFiles { get; set; }

        public string CLegCapArchivo { get; set; }
        public string CLegCapInstitucion { get; set; }
        public string? CLegCapOtraInst { get; set; }
        public string CLegCapPais { get; set; }
        public int NLegCapDatCodigo { get; set; }
        public bool? CLegCapValida { get; set; }
        public bool? CLegCapEstado { get; set; }
        public int NValorTipo { get; set; }
        public int NValorTipoEsp { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Constante vTipo { get; set; }
        public virtual LegDatosGenerales NLegCapDatCodigoNavigation { get; set; }
        [NotMapped]
        public virtual Persona vInstitucion { get; set; }
        [NotMapped]
        public virtual Constante vTipoEsp { get; set; }
    }
}
