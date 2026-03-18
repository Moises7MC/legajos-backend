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
    public class ReporteCapacitacionesRepository
    {
        private readonly string _connectionString;
        public ReporteCapacitacionesRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionDB");
        }
        public async Task<List<ReporteCapacitaciones>> GetCapacitaciones(int nPrdActividad, int nCapCodigo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetCapacitacionesUSS", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pnPrdActividad", nPrdActividad == 0 ? DBNull.Value : nPrdActividad));
                    cmd.Parameters.Add(new SqlParameter("@pnCapCodigo", nCapCodigo == 0 ? DBNull.Value : nCapCodigo));
                    var response = new List<ReporteCapacitaciones>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToCapacitaciones(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private ReporteCapacitaciones MapToCapacitaciones(SqlDataReader reader)
        {
            return new ReporteCapacitaciones()
            {
                cPerCodigo = reader["cPerCodigo"].ToString(),
                cPerApellido = reader["cPerApellido"].ToString(),
                cPerNombre = reader["cPerNombre"].ToString(),
                cCargo = reader["cCargo"].ToString(),
                cArea = reader["cArea"].ToString(),
                nTipo = (int)reader["nTipo"],
                cTipoDesc = reader["cTipoDesc"].ToString(),
                nRol = (int)reader["nRol"],
                cPerUsuCodigo = reader["cPerUsuCodigo"].ToString(),
                cPerEmail = reader["cPerEmail"].ToString(),
                cPerNroDoc = reader["cPerNroDoc"].ToString(),
                cPerTipoDoc = reader["cPerTipoDoc"].ToString(),
                cCapTema = reader["cCapTema"].ToString(),
                dCapFechaInicio = (DateTime)reader["dCapFechaInicio"],
                dCapFechaFin = (DateTime)reader["dCapFechaFin"],
                nCapHoras = (int)reader["nCapHoras"],
                cLegCIArchivo = reader["cLegCIArchivo"].ToString(),
                cCompetencias = reader["cCompetencias"].ToString()
            };
        }
    }
}
