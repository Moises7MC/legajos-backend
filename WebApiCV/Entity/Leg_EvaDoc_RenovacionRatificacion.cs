using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApiCV.Entity
{
    public partial class Leg_EvaDoc_RenovacionRatificacion
    {
        public int nRenRatCodigo { get; set; }
        public int nLegDatCodigo { get; set; }
        public string cPerCodigo { get; set; }
        public string cPerApellido { get; set; }
        public string cPerNombre { get; set; }
        public string cCargo { get; set; }
        public string cArea { get; set; }
        public string cPerNroDoc { get; set; }

        public double nLegRenRatEDD { get; set; }
        public double nLegRenRatCD { get; set; }
        public double nLegRenRatPC { get; set; }
        public double nLegRenRatPromedio { get; set; }
        public string cLegRenRatCondicion { get; set; }
        public string cLegRenRatRenRat { get; set; }

        public int nLegRenRatEstado { get; set; }

        public string cUsuRegistro { get; set; }
        public DateTime dFechaRegistro { get; set; }
        public string cUsuModifica { get; set; }
        public DateTime? dFechaModifica { get; set; }
    }
}
