using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace WebApiCV.Entity
{
    public class LegGrupInvSem
    {
        
        [NotMapped]
        public int nLegLidGrupInvSem { get; set; }
        public int nTipoLegLidGrupInvSemCodigo { get; set; }
        public int nInvCodigo { get; set; }
        public string cPerCodigo { get; set; }
        public string nPrdCodigo { get; set; }

        public string nLegLidGrupInvSemTitulo { get; set; }
        public string nLegLidGrupInvSemArchivo { get; set; }
        public Boolean nLegLidGrupInvSemEstado { get; set; }
        public string cUsuRegistro { get; set; }
        public DateTime? dFechaRegistro { get; set; }
        public string cUsuModifica { get; set; }
        public DateTime? dFechaModifica { get; set; }

        [NotMapped]
        public virtual Interface vTipo{ get; set; }

        [NotMapped]
        public virtual Interface vLinea{ get; set; }

        [NotMapped]
        public virtual IFormFile cFile { get; set; }

        [NotMapped]
        public string nLegLidGrupInvSemDescription { get; set; }

        [NotMapped]
        public string nTipoLegLidGrupInvSemDescription { get; set; }

        [NotMapped]
        public string cPrdDescripcion { get; set; }
        [NotMapped]
        public string cArchivo { get; set; }
    }
}
