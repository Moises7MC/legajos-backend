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
    public class TareaModuloRepository
    {
        private readonly string _connectionString;
        public TareaModuloRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionDB");
        }
        public async Task<List<TareaModulo>> GetTareasPermiso(string pcPerCodigo, int pnModCodigo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Legajos_TareasPermiso", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pcPerCodigo", pcPerCodigo));
                    cmd.Parameters.Add(new SqlParameter("@pnModCodigo", pnModCodigo));
                    var response = new List<TareaModulo>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToTareaModulo(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private TareaModulo MapToTareaModulo(SqlDataReader reader)
        {
            return new TareaModulo()
            {
                NModCodigo = (int)reader["nModCodigo"],
                NTarModCodigo = (int)reader["nTarModCodigo"],
                BTarModEstado = Boolean.Parse(reader["bTarModEstado"].ToString()),
                CTarModDescripcion = reader["cTarModDescripcion"].ToString()
            };
        }

        public async Task<List<TareaModulo>> ListarTareasModulo(int pnModCodigo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_leg_listado_de_acciones", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    cmd.Parameters.Add(new SqlParameter("@pnModCodigo", pnModCodigo));
                    var response = new List<TareaModulo>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToTareaModuloAcciones(reader));
                        }
                    }

                    return response;
                }
            }
        }
        private TareaModulo MapToTareaModuloAcciones(SqlDataReader reader)
        {
            return new TareaModulo()
            {
               
                NTarModCodigo = (int)reader["nTarModCodigo"],
                BTarModEstado = Boolean.Parse(reader["bTarModEstado"].ToString()),
                CTarModDescripcion = reader["cTarModDescripcion"].ToString()
            };
        }


        public async Task<List<TareaModulo>>
      GetTareaModuloPermisosAccionesXUsuario(String @cPercodigo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_leg_permiso_xpersona_acciones", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cPercodigo", @cPercodigo));
                    var response = new List<TareaModulo>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToAcciones(reader));
                        }
                    }
                    return response;
                }
            }

        }

        private TareaModulo MapToAcciones(SqlDataReader reader)
        {
            return new TareaModulo()
            {
                NTarModCodigo = (int)reader["nTarModCodigo"],
                BTarModEstado = Boolean.Parse(reader["bTarModEstado"].ToString()),
                CTarModDescripcion = reader["cTarModDescripcion"].ToString()


            };
        }
    }
}
