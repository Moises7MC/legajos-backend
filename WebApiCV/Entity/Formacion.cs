using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    public class Formacion
    {
        [Key]
        public Int64 nForId { get; set; }
       
        public int nForAnioInicio { get; set; }
        public int nForAnioFin { get; set; }
        [NotMapped]
        public virtual IFormFile fForDiploma { get; set; }
        public String cForDiplomaUrl { get; set; }
        public Boolean nForEstado { get; set; }
        [ForeignKey("Institucion")]
        public Int64 nForInstitucionId { get; set; }
        [ForeignKey("CarreraProfesional")]
        public int nForCarreraProfesionalId { get; set; }
        [ForeignKey("GradoAcademico")]
        public int nForGradoAcademicoId { get; set; }

        [ForeignKey("Curriculo")]
        public Int64 nForCurriculoId { get; set; }

        public Curriculo Curriculo { get; set; }
        public GradoAcademico GradoAcademico { get; set; }
        public CarreraProfesional CarreraProfesional { get; set; }
        public Institucion Institucion { get; set; }
    }
}
