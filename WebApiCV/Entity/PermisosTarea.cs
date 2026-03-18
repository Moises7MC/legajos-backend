using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class PermisosTarea
    {
        public int NPrmTarCodigo { get; set; }
        public int? NPrmModCodigo { get; set; }
        public int? NTarModCodigo { get; set; }
        public bool? BPrmEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }

        public virtual TareaModulo vTareaModulo { get; set; }
        public virtual PermisosModulo vPermisosModulo { get; set; }
    }
}
