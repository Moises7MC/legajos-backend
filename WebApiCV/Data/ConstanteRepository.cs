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
    public class ConstanteRepository
    {
        private readonly string _connectionString;
        public ConstanteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionDB");
        }
        public async Task<List<Constante>> GetConstanteByTipo(int nTipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Legajos_Get_ConstanteCombos", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pnTipo", nTipo));
                    var response = new List<Constante>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToConstante(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<Constante> GetConstanteDatos(int pnConCodigo, int pnConValor)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Constante_GetDatos", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pnConCodigo", pnConCodigo));
                    cmd.Parameters.Add(new SqlParameter("@pnConValor", pnConValor));
                    var response = new Constante();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToConstante(reader);
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<List<Constante>> GetConstanteDatosGetByNConCodigoNConValor(int nConCodigo, int nConValor)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BDSipan.dbo.sp_Constante_Get_By_nConCodigo_nConValor", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@nConCodigo ", nConCodigo));
                    cmd.Parameters.Add(new SqlParameter("@nConValor ", nConValor));
                    var response = new List<Constante>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToConstante(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private Constante MapToConstante(SqlDataReader reader)
        {
            return new Constante()
            {
                NConCodigo = (int)reader["nConCodigo"],
                NConValor = (int)reader["nConValor"],
                CConDescripcion = reader["cConDescripcion"].ToString()
            };
        }


      
    }
}
