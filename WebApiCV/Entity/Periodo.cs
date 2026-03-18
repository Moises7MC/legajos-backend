using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCV.Entity
{
    public partial class Periodo
    {
        public int nPrdCodigo { get; set; }
        public string cPrdDescripcion { get; set; }

        public int nPrdActividad { get; set; }
        public DateTime dPrdIni { get; set; }
        public DateTime dPrdFin { get; set; }
        public int nPrdTipo { get; set; }
        public int nPrdEstado { get; set; }
    }
}
