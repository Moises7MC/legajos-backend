using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegDocenciaUniv
    {
        public int NLegDocCodigo { get; set; }
        public string CLegDocUniversidad { get; set; }
        public string CLegDocOtraInst { get; set; }
        public string CLegDocPais { get; set; }
        public int NLegDocRegimen { get; set; }
        public int NValorRegimen { get; set; }
        public int NLegDocCategoria { get; set; }
        public int NValorCategoria { get; set; }
        public DateTime DLegDocFechaInicio { get; set; }
        public DateTime DLegDocFechaFin { get; set; }

        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegDocArchivo { get; set; }
        public int NLegDocDatCodigo { get; set; }
        public bool? CLegDocValida { get; set; }
        public bool? CLegDocEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual Persona CLegDocUniversidadNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Constante vCategoria { get; set; }
        public virtual LegDatosGenerales NLegDocDatCodigoNavigation { get; set; }
        [NotMapped]
        public virtual Constante vRegimen { get; set; }
        public string cLegDocCargo { get; set; }
    }
}
