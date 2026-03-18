using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class TareaModulo
    {
        public TareaModulo()
        {
            vPermisosTareas = new HashSet<PermisosTarea>();
        }

        public int NTarModCodigo { get; set; }
        public int? NModCodigo { get; set; }
        public string CTarModDescripcion { get; set; }
        public bool BTarModEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }

        public virtual ModulosTD vModulo { get; set; }
        public virtual ICollection<PermisosTarea> vPermisosTareas { get; set; }

       
    }
}
