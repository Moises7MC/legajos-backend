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
    public class UnidadOrganizacionalRepository
    {
        private readonly string _connectionString;

        public UnidadOrganizacionalRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionDB");

        }

       /* public async Task<List<UnidadOrganizacional>> GetCargarUnidadOrganizacional()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_ArchivoCentral_Get_areas", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    var response = new List<UnidadOrganizacional>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToUnidadOrganizacional(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private UnidadOrganizacional MapToUnidadOrganizacional(SqlDataReader reader)
        {
            return new UnidadOrganizacional()
            {
                NUniOrgCodigo = (int)reader["CPerApellido"],
                CUniOrgNombre = reader["CUniOrgNombre"].ToString(),
                CPerJuridad =reader["CPerJuridad"].ToString(),
                CPerNombre = reader["CPerNombre"].ToString()
            };
        }*/

    }
}
