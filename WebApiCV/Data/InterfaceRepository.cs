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
    public class InterfaceRepository
    {
        private readonly string _connectionString;

        public InterfaceRepository(IConfiguration configuration)
        {
            _connectionString =
                configuration.GetConnectionString("CadenaConexionDB");
        }

        public async Task<List<Interface>> GetInterfaceByTipo(int nTipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (
                    SqlCommand cmd =
                        new SqlCommand("usp_Legajos_Get_InterfaceCombos", sql)
                )
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandTimeout = 0;
                    cmd.Parameters.Add(new SqlParameter("@pnTipo", nTipo));
                    var response = new List<Interface>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToInterface(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<Interface>GetInterfaceDatos(int pnIntCodigo, int pnIntClase)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (
                    SqlCommand cmd =
                        new SqlCommand("usp_Interface_GetDatos", sql)
                )
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@pnIntCodigo", pnIntCodigo));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@pnIntClase", pnIntClase));
                    var response = new Interface();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToInterface(reader);
                        }
                    }

                    return response;
                }
            }
        }
        public async Task<List<Interface>>
        GetInterfaceDatosPorTipoClase(int nTipo, int pnIntClase)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (
                    SqlCommand cmd =
                        new SqlCommand("BDSipan.dbo.sp_Interface_Get", sql)
                )
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nTipo", nTipo));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nIntClase", pnIntClase));
                    var response = new List<Interface>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToInterface(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private Interface MapToInterface(SqlDataReader reader)
        {
            return new Interface()
            {
                NIntCodigo = (int) reader["nIntCodigo"],
                NIntClase = (int) reader["nIntClase"],
                CIntJerarquia = reader["CIntJerarquia"].ToString(),
                CIntNombre = reader["CIntNombre"].ToString(),
                CIntDescripcion = reader["CIntDescripcion"].ToString(),
                NIntTipo = (int) reader["nIntTipo"]
            };
        }

        public async Task<List<Interface>>
        GetInterfaceMenu(string cPerUsuCodigo,int pTipo)
        {
            using(SqlConnection sql = new SqlConnection(_connectionString))
            {
                using(SqlCommand cmd = new SqlCommand("BDSipan.[dbo].[sp_PerUsuario_Permisos]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pTipo", 1));
                    cmd.Parameters.Add(new SqlParameter("@cPerCodigo", cPerUsuCodigo));
                    var response = new List<Interface>();
                    await sql.OpenAsync();
                    using(var reader = await cmd.ExecuteReaderAsync())
                    {
                        while(await reader.ReadAsync())
                        {
                            response.Add(MapToMenu(reader));
                        }
                    }
                    return response;
                }
            }

        }

        private Interface MapToMenu(SqlDataReader reader)
        {
            return new Interface()
            {
                NIntCodigo = (int)reader["nIntCodigo"],
                NIntClase = (int)reader["nIntClase"],
                CIntJerarquia = reader["CIntJerarquia"].ToString(),
                CIntNombre = reader["CIntNombre"].ToString(),
                CIntDescripcion = reader["CIntDescripcion"].ToString(),
                NIntTipo = (int)reader["nIntTipo"],
                padre = (int)reader["padre"],
                nivel = (int)reader["nivel"],
                cValor = reader["cValor"].ToString()

            };
        }

      


        public async Task<List<Interface>>
        GetInterfacePermisos()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BDSipan.[dbo].[sp_Leg_Permisos]", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;

                    var response = new List<Interface>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToMenu(reader));
                        }
                    }
                    return response;
                }
            }

        }
        
    }
}
