using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class ReporteCapInvLegajos
    {
        public int nLegDatCodigo { get; set; }
        public String cPerCodigo { get; set; }
        public String cPerApellido { get; set; }
        public String cPerNombre { get; set; }
        public String cCargo { get; set; }
        public String cArea { get; set; }
      
        public String cPerEmail { get; set; }
        public String cPerTelefono { get; set; }
        public String cPerTipoDoc { get; set; }

        public int LegInvestigador { get; set; }
        public DateTime dFechaRegistroInv { get; set; }
        public DateTime dFechaModificaInv { get; set; }
        public int LegTesisAseJur { get; set; }
        public DateTime dFechaRegistroTes { get; set; }
        public DateTime dFechaModificaTes { get; set; }
        public int LegProduccionCiencia { get; set; }
        public DateTime dFechaRegistroProd { get; set; }
        public DateTime dFechaModificaProd { get; set; }
        public int LegParticipacionCongSem { get; set; }
        public DateTime dFechaRegistroCongSem { get; set; }
        public DateTime dFechaModificaCongSem { get; set; }
        public int LegCapacitaciones { get; set; }
        public DateTime dFechaRegistroCap { get; set; }
        public DateTime dFechaModificaCap { get; set; }
        public int LegCapacitacionInterna { get; set; }
        public DateTime dFechaRegistroCapInt { get; set; }
        public DateTime dFechaModificaCapInt { get; set; }
    }
}
