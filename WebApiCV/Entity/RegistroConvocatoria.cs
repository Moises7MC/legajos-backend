using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApiCV.Entity
{
    [Keyless]
    public class RegistroConvocatoria
    {

        public string? DNI { get; set; }
        public string? APEPAT { get; set; }
        public string? APEMAT { get; set; }
        public string? NOMBRES { get; set; }
        public string? CORREO { get; set; }
        public string? CELULAR { get; set; }
        public string? PAIS { get; set; }
        public string? DPTO { get; set; }
        public string? PROV { get; set; }
        public string? DSTO { get; set; }
        public string? DIR { get; set; }
        public string? MZ { get; set; }
        public string? LOTE { get; set; }
        public string? RESIDENCIA { get; set; }
        public string? PISO { get; set; }
        public string? ESCUELA { get; set; }
        public string? MODALIDAD { get; set; }
        public string? ASIGNATURA { get; set; }
        public string? CURRICULO { get; set; }
        public string? TITULO { get; set; }
        public string? GRADO { get; set; }
        public string? EXPUNI { get; set; }
        public string? EXPPRO { get; set; }
        public int? ESTADO { get; set; }
        public int? VEZ { get; set; }
        public DateTime? FECHAREGISTRO { get; set; }
        public string? SEMESTRE { get; set; }
        public string? cDIR { get; set; }
        public string? cRESIDENCIA { get; set; }
        public string? cPISO { get; set; }
        public string? CONDICION { get; set; }
        public int? nConCodigo { get; set; }
        public string? OBSERVACIONES { get; set; }
        public DateTime? FECHAATENCION { get; set; }
        public int? nTipo { get; set; }
        public string? cPunCurriculo { get; set; }
        public string? cPunClaModelo { get; set; }
        public string? cPunEntPersonal { get; set; }
    }
}
