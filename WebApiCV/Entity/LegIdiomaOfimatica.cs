using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegIdiomaOfimatica
    {
        public int NLegIdOfCodigo { get; set; }
        public int NLegIdOfCodigoDesc { get; set; }
        public int NValorDesc { get; set; }
        public bool? CLegIdOfTipo { get; set; }
        public int NLegIdOfNivel { get; set; }
        public int NValorNivel { get; set; }
        public DateTime DLegIdOfFecha { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegIdOfArchivo { get; set; }
        public int NLegIdOfDatCodigo { get; set; }
        public bool? CLegIdOfValida { get; set; }
        public bool? CLegIdOfEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Constante vCodigoDesc { get; set; }
        public virtual LegDatosGenerales NLegIdOfDatCodigoNavigation { get; set; }
        [NotMapped]
        public virtual Constante vNivel { get; set; }
    }
}
