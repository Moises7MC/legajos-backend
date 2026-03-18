using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCV.Entity
{
    public class DocentesCargaLectiva
    {
        public int nLegDatCodigo { get; set; }
        public string cPerCodigo { get; set; }
        public string cPerNombre { get; set; }
        public string cPerApellido { get; set; }
        public int nLegGraDatCodigo { get; set; }
        public string cIntDescripcion { get; set; }
        public string cLegGraInstitucion { get; set; }
        public string cInstitucion { get; set; }
        public string cLegGraOtraInst { get; set; }
        
    }
}
