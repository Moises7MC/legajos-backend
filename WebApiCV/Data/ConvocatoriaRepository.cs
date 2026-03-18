using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using WebApiCV.Entity;

namespace WebApiCV.Data
{
    public class ConvocatoriaRepository
    {
        private readonly string _connectionString;

        public ConvocatoriaRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("CadenaConexionDB");
        }

        public async Task<List<Convocatoria>>
        ObtenerListaConvocatoria(int pPrdCodigo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (
                    SqlCommand cmd = new SqlCommand("cargarConvocatoria", sql)
                //SqlCommand cmd = new SqlCommand("BDSipan.dbo.usp_GestPlazas_Get_ConvocatoriaVigente_By_Periodo_General", sql)
                )
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@pPrdCodigo", pPrdCodigo));
                    // cmd
                    //  .Parameters
                    //.Add(new SqlParameter("@nPrdCodigo", pPrdCodigo));

                    var response = new List<Convocatoria>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapConvocatorias(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private Convocatoria MapConvocatorias(SqlDataReader reader)
        {
            return new Convocatoria()
            {
                nConCodigo = (int) reader["nConCodigo"],
                cConDescripcion = reader["cConDescripcion"].ToString(),
                nPrdCodigo = (int) reader["nPrdCodigo"],
                nConEstado = (int) reader["nConEstado"] == 1 ? true : false,
                dConFecRegistro = (DateTime) reader["dConFecRegistro"]
            };
        }
    }
}
