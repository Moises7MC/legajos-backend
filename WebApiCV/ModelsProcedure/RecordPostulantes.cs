using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.ModelsProcedure
{
    public class RecordPostulantes
    {
        [Key]
        public Int64 nCurId { get; set; }
        public string cCurDni { get; set; }
        public string cCurApellidPaterno { get; set; }
        public string cCurApellidMaterno { get; set; }
        public string cCurNombres { get; set; }
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dCurFechaNacimiento { get; set; }
    }
}
