using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class Curriculo
    {
        [Key]
        public Int64 nCurId { get; set; }
        [Required]
        public string cCurDni { get; set; }
        [Required]
        public string cCurApellidPaterno { get; set; }
        [Required]
        public string cCurApellidMaterno { get; set; }
        [Required]
        public string cCurNombres { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime dCurFechaNacimiento { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string cCurEmail { get; set; }
        [Required]
        public string cCurMovil { get; set; }
        public string cCurTelefono { get; set; }
        public string cCurFoto { get; set; }
        [NotMapped]
        public virtual IFormFile cCurFile { get; set; }
        public string cCurAcerca { get; set; }

        [ForeignKey("CarreraProfesional")]
        public int? nCarId { get; set; }
        public GradoAcademico GradoAcademico { get; set; }

        [Required]
        [ForeignKey("GradoAcademico")]
        public int nGacId { get; set; }
        public CarreraProfesional CarreraProfesional { get; set; }

        [Required]
        [ForeignKey("Ubigeo")]
        public string cUbiId { get; set; }
        public  virtual Ubigeo Ubigeo { get; set; }
        public Boolean nEstado { get; set; }
        public IEnumerable<Formacion> lFormacion { get; set; }
    }
}
