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
    public class LegGrupInvSemRepository
    {
        private readonly string _connectionString;

        public LegGrupInvSemRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("CadenaConexionDB");
        }

        public async Task<List<LegGrupInvSem>> GetLegGrupInvSem(string cPerCodigo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_List_LegGrupInvSem", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pPerCodigo", cPerCodigo));
                    var response = new List<LegGrupInvSem>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapLegGrupInvSem(reader));
                        }
                    }
                    return response;

                }
            }
        }


        private LegGrupInvSem MapLegGrupInvSem(SqlDataReader reader)
        {
            return new LegGrupInvSem()
            {
                nLegLidGrupInvSemDescription = reader["nLegLidGrupInvSemDescription"].ToString(),
                nTipoLegLidGrupInvSemDescription = reader["nTipoLegLidGrupInvSemDescription"].ToString(),
                nLegLidGrupInvSem = (int)reader["nLegLidGrupInvSem"],
                nLegLidGrupInvSemTitulo = reader["nLegLidGrupInvSemTitulo"].ToString(),
                nLegLidGrupInvSemArchivo = reader["nLegLidGrupInvSemArchivo"].ToString(),
                cArchivo = reader["nLegLidGrupInvSemArchivo"].ToString(),                
                nLegLidGrupInvSemEstado = (Boolean)reader["nLegLidGrupInvSemEstado"],
                cPrdDescripcion = reader["cPrdDescripcion"].ToString(),
                nTipoLegLidGrupInvSemCodigo = (int) reader["nTipoLegLidGrupInvSemCodigo"],
                nInvCodigo= (int)reader["nInvCodigo"],
                nPrdCodigo = reader["nPrdCodigo"].ToString(),

            };
        }
    }
}