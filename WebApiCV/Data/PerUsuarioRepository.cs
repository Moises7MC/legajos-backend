using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using WebApiCV.Entity;
namespace WebApiCV.Data
{
    public class PerUsuarioRepository
    {
        private readonly string _connectionString;
        public PerUsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionDB");
        }
        public async Task<List<PerUsuario>> GetPerUsuarioValidate(string cPerUsuCodigo, string cPerUsuClave, Boolean bPerUsuClaCryp, int nSisCodigo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_User_Validate", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cPerUsuCodigo", cPerUsuCodigo));
                    cmd.Parameters.Add(new SqlParameter("@cPerUsuClave", cPerUsuClave));
                    cmd.Parameters.Add(new SqlParameter("@bPerUsuClaCryp", bPerUsuClaCryp));
                    cmd.Parameters.Add(new SqlParameter("@nSisCodigo", nSisCodigo));
                    var response = new List<PerUsuario>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToPerUsuarioValidate(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private PerUsuario MapToPerUsuarioValidate(SqlDataReader reader)
        {
            return new PerUsuario()
            {
                CPerCodigo = reader["CPerCodigo"].ToString(),
                CPerUsuCodigo = reader["CPerUsuCodigo"].ToString(),
                CPerUsuClave = reader["CPerUsuClave"].ToString(),
                CPerUsuEstado = (int)reader["CPerUsuEstado"],
                CPerJuridica = "",
                CPudFecha = null
            };
        }

       
    }
}
    