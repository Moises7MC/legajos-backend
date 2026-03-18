using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using WebApiCV.Entity;

namespace WebApiCV.Data
{
    public class RegistroConvocatoriaRepository
    {
        private readonly string _connectionString;

        public RegistroConvocatoriaRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("CadenaConexionDB");
        }

        public async Task<List<RegistroConvocatoria>>
        GetRegistroConvocatoriaByTipo(int nTipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (
                    SqlCommand cmd =
                        new SqlCommand("usp_ListarRegistroConvocatoria", sql)
                //SqlCommand cmd =new SqlCommand("BDSipan.dbo.usp_Get_SeleccionPersonal_Reporte_Postulantes", sql)
                )
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pConCodigo", nTipo));
                    //cmd.Parameters.Add(new SqlParameter("@nConCodigo ", nTipo));
                    var response = new List<RegistroConvocatoria>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToRegistroConvocatoria(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private RegistroConvocatoria
        MapToRegistroConvocatoria(SqlDataReader reader)
        {
            return new RegistroConvocatoria()
            {
                DNI = reader["DNI"].ToString(),
                APEPAT = reader["APEPAT"].ToString(),
                APEMAT = reader["APEMAT"].ToString(),
                NOMBRES = reader["NOMBRES"].ToString(),
                CORREO = reader["CORREO"].ToString(),
                CELULAR = reader["CELULAR"].ToString(),
                PAIS = reader["PAIS"].ToString(),
                DPTO = reader["DPTO"].ToString(),
                PROV = reader["PROV"].ToString(),
                DSTO = reader["DSTO"].ToString(),
                DIR = reader["DIR"].ToString(),
                MZ = reader["MZ"].ToString(),
                LOTE = reader["LOTE"].ToString(),
                RESIDENCIA = reader["RESIDENCIA"].ToString(),
                PISO = reader["PISO"].ToString(),
                ESCUELA = reader["ESCUELA"].ToString(),
                MODALIDAD = reader["MODALIDAD"].ToString(),
                ASIGNATURA = reader["ASIGNATURA"].ToString(),
                CURRICULO = reader["CURRICULO"].ToString(),
                TITULO = reader["TITULO"].ToString(),
                GRADO = reader["GRADO"].ToString(),
                EXPUNI = reader["EXPUNI"].ToString(),
                EXPPRO = reader["EXPPRO"].ToString(),
                ESTADO = reader["ESTADO"] is DBNull ? 0 : (int)reader["ESTADO"],
                VEZ = reader["VEZ"]  is DBNull ? 0 :(int) reader["VEZ"],
                FECHAREGISTRO = (DateTime) reader["FECHAREGISTRO"],
                SEMESTRE = reader["SEMESTRE"].ToString(),
                cDIR = reader["cDIR"].ToString(),
                cRESIDENCIA = reader["cRESIDENCIA"].ToString(),
                cPISO = reader["cPISO"].ToString(),
                CONDICION = reader["CONDICION"].ToString(),
                nConCodigo =  reader["nConCodigo"]  is DBNull ? 0 :(int) reader["nConCodigo"],
                OBSERVACIONES = reader["OBSERVACIONES"].ToString(),
                FECHAATENCION = (DateTime) reader["FECHAATENCION"],
                nTipo =  reader["nTipo"]  is DBNull ? 0 :(int) reader["nTipo"],
                cPunCurriculo = reader["cPunCurriculo"].ToString(),
                cPunClaModelo = reader["cPunClaModelo"].ToString(),
                cPunEntPersonal = reader["cPunEntPersonal"].ToString()
            };
        }
    }
}
