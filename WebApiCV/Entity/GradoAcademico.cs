using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class GradoAcademico
    {
        public int Id { get; set; }
        [Required]
        public string cGacNombre { get; set; }
        //public List<Curriculo> Curriculos { get; set; }
    }
}
