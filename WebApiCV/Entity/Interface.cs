using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class Interface
    {
        public Interface()
        {
        }

        public  Interface(int pNIntCodigo, int pNIntClase, string pCIntJerarquia, string pCIntNombre, string pCIntDescripcion,  int pNIntTipo)
        {
            NIntCodigo = pNIntCodigo;
            NIntClase = pNIntClase;
            CIntJerarquia = pCIntJerarquia;
            CIntNombre = pCIntNombre;
            CIntDescripcion = pCIntDescripcion;
            NIntTipo = pNIntTipo;
        }

        public int NIntCodigo { get; set; }
        public int NIntClase { get; set; }
        public string CIntJerarquia { get; set; }
        public string CIntNombre { get; set; }
        public string CIntDescripcion { get; set; }
        public int NIntTipo { get; set; }

        //[NotMapped]
        //public virtual ICollection<LegDatosGenerales> LegDatosGeneralesPais { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegDatosGenerales> LegDatosGeneralesTipoDoc { get; set; }
        ////public virtual ICollection<LegDatosGenerales> LegDatosGeneralesUbigeo { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegDatosGenerales> LegDatosGeneralesNacimiento { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegDatosGenerales> LegDatosGeneralesGradoAcad { get; set; }
        ////public virtual ICollection<LegGradoTitulo> LegGradoTituloUbigeo { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegGradoTitulo> LegGradoTituloPais { get; set; }
        //[NotMapped]
        //public virtual ICollection<LegGradoTitulo> LegGradoTituloGradoAcad { get; set; }

        [NotMapped]
        public int  padre { get; set; }

        [NotMapped]
        public int nivel { get; set; }

        [NotMapped]
        public string cValor{ get; set; }
    }
}
