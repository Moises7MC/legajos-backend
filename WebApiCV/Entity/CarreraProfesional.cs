using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class CarreraProfesional
    {
        [Key]
        public int nCarId { get; set; }
        [Required]
        public string cCarNombre { get; set; }
    }
}
