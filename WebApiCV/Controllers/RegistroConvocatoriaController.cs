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
using WebApiCV.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebApiCV.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistroConvocatoriaController : ControllerBase
    {
        private readonly RegistroConvocatoriaRepository _repository;
        public RegistroConvocatoriaController(RegistroConvocatoriaRepository _repository)
        {
            this._repository = _repository;
        }

        [HttpGet("listarRegistroConvocatoria/{nTipo}",Name = "ListarRegistroConvocatoria")]
        public ActionResult<List<RegistroConvocatoria>> GetRegistroConvocatoria(int nTipo)
        {
            Console.WriteLine();
            try
            {
                var RegistroConvocatoria =
                    this._repository.GetRegistroConvocatoriaByTipo(nTipo);
                return new JsonResult(new Mensajes(HttpContext
                            .Response
                            .StatusCode,
                        true,
                        (
                        RegistroConvocatoria.Result.Count > 0
                            ? "Se ha encontrado " +
                            RegistroConvocatoria.Result.Count.ToString() +
                            " registro(s)."
                            : "No se ha encontrado registros."
                        ),
                        RegistroConvocatoria.Result));
            }
            catch (Exception ex)
            {
                return new JsonResult(new Mensajes(ex.Message));
            }
        }
    }
}
