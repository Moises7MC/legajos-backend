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
    public class PersonaRepository
    {
        private readonly string _connectionString;
        public PersonaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionDB");
        }
        public async Task<List<Persona>> GetPersonaByTipo(int nTipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Legajos_Get_PersonaCombos", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pnTipo", nTipo));
                    var response = new List<Persona>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToPersona(reader));
                        }
                    }

                    return response;
                }
            }
        }

        public async Task<Persona> GetPersonaDatos(string cPerCodigo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Persona_GetDatos", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pcPerCodigo", cPerCodigo));
                    var response = new Persona();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToPersona(reader);
                        }
                    }

                    return response;
                }
            }
        }

      

        // --------------------------- Edgar 25-02-2022 ----------------------------------------
        public async Task<List<DatosUsuario>> GetLista_Personal(int nTipo, string cPrdNombre,int nUniOrgCodigo = 0)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                Console.WriteLine(nTipo);

                using (SqlCommand cmd = new SqlCommand("usp_Legajos_By_Periodo", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pcCodigo", DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@pnPrdActividad", nTipo == 0 ? DBNull.Value : nTipo));
                    cmd.Parameters.Add(new SqlParameter("@cPrdNombre", cPrdNombre));
                    cmd.Parameters.Add(new SqlParameter("@nUniOrgCodigo", nUniOrgCodigo==0?DBNull.Value : nUniOrgCodigo));
                    var response = new List<DatosUsuario>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToListaPersonal(reader));
                        }
                    }
                    Console.WriteLine(response);
                    return response;
                }
            }
        }

        private DatosUsuario MapToListaPersonal(SqlDataReader reader)
        {
            return new DatosUsuario()
            {
                cPerCodigo = reader["cPerCodigo"].ToString(),
                cPerApellido = reader["cPerApellido"].ToString(),
                cTipoDesc = reader["cTipoDesc"].ToString(),
                cPerNombre = reader["cPerNombre"].ToString(),
                cPerUsuCodigo = reader["cPerUsuCodigo"].ToString(),
                cPerEmail = reader["cPerEmail"].ToString(),
                cPerTipoDoc = reader["cPerTipoDoc"].ToString(),
                cPerNroDoc = reader["cPerNroDoc"].ToString(),
                nTipo = (int)reader["nTipo"],
                cCargo = reader["cCargo"].ToString(),
                nRol = (int)reader["nRol"],
                bLegajo = Boolean.Parse(reader["bLegajo"].ToString()),
                Area = reader["Area"].ToString(),
            };
        }


        public async Task<List<Persona>> GetPersonaDatosXNombres(string cPerNombre)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BDSipan.dbo.sp_BuscaPersona_Get_Persona_by_cPerNombre", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cPerNombre", cPerNombre));
                    var response = new List<Persona>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToListaPersonalXNombres(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private Persona MapToListaPersonalXNombres(SqlDataReader reader)
        {
            return new Persona()
            {
                CPerCodigo = reader["cPerCodigo"].ToString(),
                CPerApellido = reader["NombreCompleto"].ToString(),
                //cTipoDesc = reader["cTipoDesc"].ToString(),
                //cPerNombre = reader["cPerNombre"].ToString(),
                //cPerUsuCodigo = reader["cPerUsuCodigo"].ToString(),
                //cPerEmail = reader["cPerEmail"].ToString(),
                //cPerTipoDoc = reader["cPerTipoDoc"].ToString(),
                //cPerNroDoc = reader["cPerNroDoc"].ToString(),
                //nTipo = (int)reader["nTipo"],
                //cCargo = reader["cCargo"].ToString(),
                //nRol = (int)reader["nRol"],
                //bLegajo = Boolean.Parse(reader["bLegajo"].ToString()),
                //Area = reader["Area"].ToString(),
            };
        }

        private Persona MapToPersona(SqlDataReader reader)
        {
            return new Persona()
            {
                CPerCodigo = reader["CPerCodigo"].ToString(),
                CPerApellido = reader["CPerApellido"].ToString(),
                CPerApellPat = reader["CPerApellPat"].ToString(),
                CPerNombre = reader["CPerNombre"].ToString(),
                CUbigeoCodigo = reader["CUbigeoCodigo"].ToString(),
                Cperestadobiblio = reader["Cperestadobiblio"].ToString(),
            };
        }

    }
}
