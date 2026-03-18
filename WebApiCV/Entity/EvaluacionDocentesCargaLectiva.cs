using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCV.Entity
{
    public partial class EvaluacionDocentesCargaLectiva
    {
        public string cPerCodigo { get; set; }
        public int nLegDatCodigo { get; set; }
        public string cPerApellido { get; set; }
        public string cPerNombre { get; set; }
        public string cCargo { get; set; }
        public string cArea { get; set; }
        public string cPerNroDoc { get; set; }
        public double PromedioEDD { get; set; }
    }
}
