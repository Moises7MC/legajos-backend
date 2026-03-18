using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using WebApiCV.Contexts;
using WebApiCV.Entity;
using NETCore.Encrypt;
using Microsoft.AspNetCore.Authorization;

namespace WebApiCV.Controllers
{
    [Route("api/[controller]")]
    [ApiController][Authorize]
    public class CurriculoController : ControllerBase
    {
        
        private readonly ApplicationDbContext context;
        private readonly IConfiguration config;
        public CurriculoController(ApplicationDbContext context, IConfiguration configuration)
        {
            this.context = context;
            this.config = configuration;
        }

        [HttpGet("{pnCurId}", Name = "ObtenerCurriculo")]
        //[HttpGet("renombreruta")]
        //[HttpGet("/renombreruta")] si le agrego slash se borra atributo principal de a ruta
        //[HttpGet("{param1}/{param2?}")] el signo ? significa que es opcional
        //[HttpGet("{param1}/{param2=default}")]  =default si no se envia el valor toma el valor pord efecto
        public ActionResult<Curriculo> Get(Int64 pnCurId)
        {
            var curriculo = context.Curriculo.Include(i => i.Ubigeo).Include(i => i.GradoAcademico).Include(i => i.CarreraProfesional).Include(i => i.lFormacion).ThenInclude(cs => cs.CarreraProfesional).Include(i => i.lFormacion).ThenInclude(cs => cs.Institucion).Include(i => i.lFormacion).ThenInclude(cs => cs.GradoAcademico).FirstOrDefault(x => x.nCurId == pnCurId);
            if(curriculo == null)
            {
                return NotFound();
            }
            curriculo.cCurFoto = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(curriculo.cCurFoto, true)));
            foreach (Formacion f in curriculo.lFormacion)
            {
                f.cForDiplomaUrl = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.cForDiplomaUrl, true)));
            }

            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true,"", curriculo));
        }

        [HttpGet]
        public ActionResult<IEnumerable<Curriculo>> Get()
        {
            
            var obj = context.Curriculo.Include(i => i.Ubigeo).Include(i => i.GradoAcademico).Include(i => i.CarreraProfesional).Include(i => i.lFormacion).ThenInclude(cs => cs.CarreraProfesional).Include(i => i.lFormacion).ThenInclude(cs => cs.Institucion).Include(i => i.lFormacion).ThenInclude(cs => cs.GradoAcademico).ToList();

            if (obj.Count > 0)
            {
                foreach (Curriculo x in obj)
                {
                    x.cCurFoto =  Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(x.cCurFoto, true)));
                    foreach (Formacion f in x.lFormacion)
                    {
                        f.cForDiplomaUrl = Convert.ToBase64String(Encoding.ASCII.GetBytes(EncryptHelper.Encrypt(f.cForDiplomaUrl, true))); 
                    }
                }
            }

           
            return new JsonResult(new Mensajes(HttpContext.Response.StatusCode, true, 
                (obj.Count > 0 ? "Se ha encontrado " + obj.Count.ToString() + " registro(s)." : "No se ha encontrado registros."), obj));
        }

        [HttpPost]
        public ActionResult Post([FromBody] Curriculo curriculo)
        {
            using var transaction = context.Database.BeginTransaction();            
            try
            {
                context.Curriculo.Add(curriculo);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerCurriculo", new { id = curriculo.nCurId }, curriculo);
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Currículo ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(ex.Message));
            }  
        }

        [HttpPost]
        [Route("/api/formacion")]
        public ActionResult PostFormacion([FromForm] Curriculo curriculo)
        {
            using var transaction = context.Database.BeginTransaction();
            try
            {
                String directorio  = this.config.GetValue<string>("ServerLegajos"); //D:/Proyectos/USS"

                string path = Path.Combine(directorio, this.config.GetValue<string>("FolderLegajos") + curriculo.cCurDni); //"LegajosUSS/" 
                
                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }
                //return new JsonResult(new Mensajes(200, true, "Currículo ha sido registrado.", curriculo.cCurFile));
                if (curriculo.cCurFile != null)
                {
                    string fileName = Path.GetFileName(curriculo.cCurFile.FileName);
                    string extension = Path.GetExtension(curriculo.cCurFile.FileName);
                    string photocv = this.config.GetValue<string>("PhotoCV") + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                    using (FileStream stream = new FileStream(Path.Combine(path, photocv), FileMode.Create))
                    {
                        curriculo.cCurFile.CopyTo(stream);
                    }

                    curriculo.cCurFoto = this.config.GetValue<string>("FolderLegajos") + curriculo.cCurDni + "/" + photocv;
                }else{
                    curriculo.cCurFoto = "";
                }

                var forcont = 0;
                
                foreach (Formacion objFor in curriculo.lFormacion.ToList())
                {
                    if (objFor.fForDiploma != null)
                    {
                        string fileName = Path.GetFileName(objFor.fForDiploma.FileName);
                        string extension = Path.GetExtension(objFor.fForDiploma.FileName);
                        string formacioncv = this.config.GetValue<string>("FormacionCV") + forcont.ToString() + DateTime.Now.ToString("yyyyMMddHHmmss") + extension;
                        using (FileStream stream = new FileStream(Path.Combine(path, formacioncv), FileMode.Create))
                        {
                            objFor.fForDiploma.CopyTo(stream);
                        }

                        objFor.cForDiplomaUrl = this.config.GetValue<string>("FolderLegajos") + curriculo.cCurDni + "/" + formacioncv;
                    }
                    else
                    {
                        objFor.cForDiplomaUrl = "";
                    }
                    forcont++;
                    //return new JsonResult(new Mensajes(200, true, "Currículo ha sido registrado.", objFor.fForDiploma));
                }

                curriculo.nEstado = true;
                context.Curriculo.Add(curriculo);
                context.SaveChanges();
                transaction.Commit();
                var obj = new CreatedAtRouteResult("ObtenerCurriculo", new { id = curriculo.nCurId }, curriculo);
                //return new JsonResult(new Mensajes(200, true, "Currículo ha sido registrado.", curriculo));
                return new JsonResult(new Mensajes(obj.StatusCode.Value, true, "Currículo ha sido registrado.", obj.Value));
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return new JsonResult(new Mensajes(400,false,ex.Message,ex));
            }
            
        }
        
    }
}
