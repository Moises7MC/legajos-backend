using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class LegProduccionCiencia
    {
        public int NLegProdCodigo { get; set; }
        public int NLegProdTipo { get; set; }
        public int NValorTipo { get; set; }
        public string CLegProdTitulo { get; set; }
        public DateTime DLegProdFecha { get; set; }
        public string CLegProdNroResolucion { get; set; }
        [NotMapped]
        public virtual IFormFile cFile { get; set; }
        public string CLegProdArchivo { get; set; }
        public int NLegProdDatCodigo { get; set; }
        public bool? CLegProdValida { get; set; }
        public bool? CLegProdEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuModificaNavigation { get; set; }
        [NotMapped]
        public virtual PerUsuario CUsuRegistroNavigation { get; set; }
        [NotMapped]
        public virtual Interface vTipo { get; set; }
        public virtual LegDatosGenerales NLegProdDatCodigoNavigation { get; set; }
        public int? NLegProdParticipacion { get; set; }

        public int? NLegProdPais { get; set; } // El ID que se guarda

        [NotMapped]
        public virtual Interface vPais { get; set; } // Para mostrar el nombre en la tabla
    }
}