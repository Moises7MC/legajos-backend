using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class PermisosModulo
    {
        public PermisosModulo()
        {
            vPermisosTareas = new HashSet<PermisosTarea>();
        }

        public int NPrmModCodigo { get; set; }
        public int? NModCodigo { get; set; }
        public string CPerCodigo { get; set; }
        public bool? BPrmModAdministrador { get; set; }
        public bool? BPrmModAlcance { get; set; }
        public bool BPrmModEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }
        
        public virtual Persona vPersona { get; set; }
        public virtual ModulosTD vModulos { get; set; }
        public virtual ICollection<PermisosTarea> vPermisosTareas { get; set; }
    }
}
