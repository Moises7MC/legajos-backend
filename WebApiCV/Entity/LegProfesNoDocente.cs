using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegProfesNoDocente
    {
        public int NLegProCodigo { get; set; }
        public string CLegProInstitucion { get; set; }

        public string CLegProOtraInst { get; set; }
        public string CLegProPais { get; set; }
        public int? NLegProCargo { get; set; }
        public int? NValorCargo { get; set; }

        public string CLegProCargoProf { get; set; }
        public DateTime DLegProFechaInicio { get; set; }
        public DateTime DLegProFechaFin { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegProArchivo { get; set; }
        public int NLegProDatCodigo { get; set; }
        public bool? CLegProValida { get; set; }
        public bool CLegProEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual Persona CLegProInstitucionNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Constante vCargo { get; set; }
        public virtual LegDatosGenerales NLegProDatCodigoNavigation { get; set; }
        public string cLegDescCargo { get; set; }
    }
}
