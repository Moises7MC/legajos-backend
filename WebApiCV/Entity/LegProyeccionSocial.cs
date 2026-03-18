using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegProyeccionSocial
    {
        public int NLegProyCodigo { get; set; }
        public string CLegProyInstitucion { get; set; }
        public string CLegProyOtraInst { get; set; }
        public string CLegProyPais { get; set; }
        public int NLegProyTipo { get; set; }
        public int NValorTipo { get; set; }
        public string CLegProyDescripcion { get; set; }
        public DateTime DLegProyFechaInicio { get; set; }
        public DateTime DLegProyFechaFin { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegProyArchivo { get; set; }
        public int NLegProyDatCodigo { get; set; }
        public bool? CLegProyValida { get; set; }
        public bool? CLegProyEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual Persona CLegProyInstitucionNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Constante vTipo { get; set; }
        public virtual LegDatosGenerales NLegProyDatCodigoNavigation { get; set; }
    }
}
