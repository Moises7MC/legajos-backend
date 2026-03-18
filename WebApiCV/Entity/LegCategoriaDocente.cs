using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegCategoriaDocente
    {
        public int NLegCatCodigo { get; set; }
        public string CLegCatInstitucion { get; set; }
        public string CLegCatOtraInst { get; set; }
        public string CLegCatPais { get; set; }
        public int NLegCatCategoria { get; set; }
        public int NValorCategoria { get; set; }
        public DateTime DLegCatFechaInicio { get; set; }
        public DateTime DLegCatFechaFin { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegCatArchivo { get; set; }
        public int NLegCatDatCodigo { get; set; }
        public bool? CLegCatValida { get; set; }
        public bool? CLegCatEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual Persona CLegCatInstitucionNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Constante vCategoria { get; set; }
        public virtual LegDatosGenerales NLegCatDatCodigoNavigation { get; set; }
    }
}
