using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace WebApiCV.Entity
{
    public partial class UnidadOrganizacional
    {

        public int NUniOrgCodigo { get; set; }
        public string CUniOrgCodigo { get; set; }
        public string CUniOrgAbrev { get; set; }
        public string CUniOrgNombre { get; set; }
        public string CPerJuridad { get; set; }
        public int NIntTipo { get; set; }
        [NotMapped]
        public string CPerApellido { get; set; }
        public string CPerNombre { get; set; }
        public string CUniOrgRelacion { get; set; }

    }
}
