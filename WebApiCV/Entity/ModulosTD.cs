using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class ModulosTD
    {
        public ModulosTD()
        {
            vPermisosModulos = new HashSet<PermisosModulo>();
            vTareaModulos = new HashSet<TareaModulo>();
        }

        public ModulosTD(int pnModCodigo, string pcModDescripcion, Constante pvMOdulo)
        {
            CModDescripcion = pcModDescripcion;
            NModCodigo = pnModCodigo;
            vModulo = pvMOdulo;
        }

        public int NModCodigo { get; set; }
        public int? NConValor { get; set; }
        public int? NConCodigo { get; set; }
        public string CModDescripcion { get; set; }
        public string CModRuta { get; set; }
        public bool BModEstado { get; set; }
        public string CUsuRegistro { get; set; }
        public DateTime DFechaRegistro { get; set; }
        public string CUsuModifica { get; set; }
        public DateTime? DFechaModifica { get; set; }

        public virtual Constante vModulo { get; set; }
        public virtual ICollection<PermisosModulo> vPermisosModulos { get; set; }
        public virtual ICollection<TareaModulo> vTareaModulos { get; set; }

    }
}
