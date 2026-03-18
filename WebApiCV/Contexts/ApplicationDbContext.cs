using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCV.Entity;
using WebApiCV.ModelsProcedure;

namespace WebApiCV.Contexts
{
    public class ApplicationDbContext: DbContext
    {

        //Scaffold-DBContext "Server=localhost;Database=dblegajo;Trusted_Connection=True;" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Entity
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            :base(options)
        { }

        public DbSet<Ubigeo> Ubigeo { get; set; }
        public DbSet<GradoAcademico> GradoAcademico { get; set; }
        public DbSet<Curriculo> Curriculo { get; set; }
        public DbSet<Institucion> Institucion { get; set; }
        public DbSet<CarreraProfesional> CarreraProfesional { get; set; }

        public DbSet<Formacion> Formacion { get; set; }
        public virtual DbSet<RecordPostulantes> RecordPostulantes { get; set; }

        public DbSet<UnidadOrganizacional> UnidadOrganizacional { get; set; }
    }
}
