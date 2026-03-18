using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class Institucion
    {
        [Key]
        public Int64 nInsId { get; set; }
        [Required]
        public string cInsNombre { get; set; }
        public string cInsAcronimo { get; set; }
        public int cInsFundacion{ get; set; }
        public string cInsSede { get; set; }
    }
}
