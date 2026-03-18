using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiCV.Entity;

namespace WebApiCV.Data
{
    public class DatosUsuarioRepository
    {
        private readonly string _connectionString;
        public DatosUsuarioRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionDB");
        }
        public async Task<DatosUsuario> GetLogin(string cPerUsuCodigo, string cPerUsuClave)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BDSipan.dbo.sp_User_Validate", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cPerUsuCodigo", cPerUsuCodigo));
                    cmd.Parameters.Add(new SqlParameter("@cPerUsuClave", cPerUsuClave));
                    cmd.Parameters.Add(new SqlParameter("@bPerUsuClaCryp", 1));
                    var response = new DatosUsuario();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToDatosLogin(reader);

                        }
                    }

                    return response;
                }
            }
        }

        public async Task<DatosUsuario> GetDatosUsuario(string cCodigo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Legajos_DatosUsuario", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pcCodigo", cCodigo));
                    var response = new DatosUsuario();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToDatosUsuario(reader);

                        }
                    }

                    return response;
                }
            }
        }

        public async Task<List<DatosUsuario>> GetListaUsuario(int nTipo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                Console.WriteLine(nTipo);

                using (SqlCommand cmd = new SqlCommand("usp_Legajos_DatosUsuario", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pcCodigo", DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@pnPrdActividad", nTipo == 0 ? DBNull.Value : nTipo));
                    var response = new List<DatosUsuario>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToDatosUsuario(reader));

                        }
                    }
                    Console.WriteLine(response);
                    return response;
                }
            }
        }


        public async Task<List<DatosUsuario>> GetLista_Legajos_by_Periodo(int nTipo, string cPrdNombre)
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
                    var response = new List<DatosUsuario>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToDatosUsuario(reader));

                        }
                    }
                    Console.WriteLine(response);
                    return response;
                }
            }
        }


        private DatosUsuario MapToDatosUsuario(SqlDataReader reader)
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
                bLegajo = Boolean.Parse(reader["bLegajo"].ToString())
            };
        }

        private DatosUsuario MapToDatosLogin(SqlDataReader reader)
        {
            return new DatosUsuario()
            {
                cPerCodigo = reader["cPerCodigo"].ToString(),
                cPerUsuClave = reader["cPerUsuClave"].ToString(),
                cPerUsuCodigo = reader["cPerUsuCodigo"].ToString(),
                cPerUsuEstado = (int)reader["cPerUsuEstado"],
                /*declaracionjuradaflag = (bool)reader["declaracionjuradaflag"]*/
            };
        }

        public async Task<List<Periodo>> Get_Lst_PrdAcademico_By_Jur(string cPerCodigo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {

                using (SqlCommand cmd = new SqlCommand("BDSipan.dbo.usp_Periodo_Get_Lst_PrdAcademico_By_Jur", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    
                    var response = new List<Periodo>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToPeriodo(reader));
                        }
                    }
                    Console.WriteLine(response);
                    return response;
                }
            }
        }

        private Periodo MapToPeriodo(SqlDataReader reader)
        {
            return new Periodo()
            {
                nPrdCodigo = (int)reader["nPrdCodigo"],
                cPrdDescripcion = reader["cPrdDescripcion"].ToString(),
                nPrdActividad = (int)reader["nPrdActividad"],
                dPrdIni = (DateTime)reader["dPrdIni"],
                dPrdFin = (DateTime)reader["dPrdFin"],
                nPrdTipo = (int)reader["nPrdTipo"],
                nPrdEstado = (int)reader["nPrdEstado"]
            };
        }
        public async Task<List<PerUsuario>> GetComboTipoUsuario(string CPerCodigo)
        {

            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BDSipan.dbo.sp_Persona_Get_Perfil_By_Codigo_By_System", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cPerCodigo", CPerCodigo));
                    cmd.Parameters.Add(new SqlParameter("@cPerJuridica", "1000003833"));
                    cmd.Parameters.Add(new SqlParameter("@nUniOrgCodigo", 1045));
                    cmd.Parameters.Add(new SqlParameter("@nTipo", 1));
                    var response = new List<PerUsuario>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToPerUsuarioTipoUsuario(reader));
                        }
                    }

                    return response;
                }
            }

        }
        private PerUsuario MapToPerUsuarioTipoUsuario(SqlDataReader reader)
        {
            return new PerUsuario()
            {
                nPerRelacion = reader["nPerRelacion"].ToString()
            };
        }

        public async Task<List<DatosUsuario>> GetLista_Legajos_by_Periodo_Excel(int nTipo, string cPrdNombre)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                Console.WriteLine(nTipo);

                using (SqlCommand cmd = new SqlCommand("usp_Legajos_By_Periodo_ReportExcel", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pcCodigo", DBNull.Value));
                    cmd.Parameters.Add(new SqlParameter("@pnPrdActividad", nTipo == 0 ? DBNull.Value : nTipo));
                    cmd.Parameters.Add(new SqlParameter("@cPrdNombre", cPrdNombre));
                    var response = new List<DatosUsuario>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToDatosUsuarioExcel(reader));

                        }
                    }
                    Console.WriteLine(response);
                    return response;
                }
            }
        }


        private DatosUsuario MapToDatosUsuarioExcel(SqlDataReader reader)
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
                LegAdminitrativaCarga = reader["LegAdminitrativaCarga"].ToString(),
                LegAdminitrativaCargaNoValida = reader["LegAdminitrativaCargaNoValida"].ToString(),
                LegCapacitaciones = reader["LegCapacitaciones"].ToString(),
                LegCapacitacionesNoValida = reader["LegCapacitacionesNoValida"].ToString(),
                LegCategoriaDocente = reader["LegCategoriaDocente"].ToString(),
                LegCategoriaDocenteNoValida = reader["LegCategoriaDocenteNoValida"].ToString(),
                LegDocenciaUniv = reader["LegDocenciaUniv"].ToString(),
                LegDocenciaUnivNoValida = reader["LegDocenciaUnivNoValida"].ToString(),
                LegGradoTitulo = reader["LegGradoTitulo"].ToString(),
                LegGradoTituloNoValida = reader["LegGradoTituloNoValida"].ToString(),
                LegIdiomaOfimatica = reader["LegIdiomaOfimatica"].ToString(),
                LegIdiomaOfimaticaNoValida = reader["LegIdiomaOfimaticaNoValida"].ToString(),
                LegInvestigador = reader["LegInvestigador"].ToString(),
                LegInvestigadorNoValida = reader["LegInvestigadorNoValida"].ToString(),
                LegParticipacionCongSem = reader["LegParticipacionCongSem"].ToString(),
                LegParticipacionCongSemNoValida = reader["LegParticipacionCongSemNoValida"].ToString(),
                LegProduccionCiencia = reader["LegProduccionCiencia"].ToString(),
                LegProduccionCienciaNoValida = reader["LegProduccionCienciaNoValida"].ToString(),
                LegProfesNoDocente = reader["LegProfesNoDocente"].ToString(),
                LegProfesNoDocenteNoValida = reader["LegProfesNoDocenteNoValida"].ToString(),
                LegProyeccionSocial = reader["LegProyeccionSocial"].ToString(),
                LegProyeccionSocialNoValida = reader["LegProyeccionSocialNoValida"].ToString(),
                LegReconocimiento = reader["LegReconocimiento"].ToString(),
                LegReconocimientoNoValida = reader["LegReconocimientoNoValida"].ToString(),
                LegRegimenDedicacion = reader["LegRegimenDedicacion"].ToString(),
                LegRegimenDedicacionNoValida = reader["LegRegimenDedicacionNoValida"].ToString(),
                LegTesisAseJur = reader["LegTesisAseJur"].ToString(),
                LegTesisAseJurNoValida = reader["LegTesisAseJurNoValida"].ToString(),

            };
        }

        public async Task<List<PerUsuario>> GetPerUsuariosConPermisos()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("BDSipan.dbo.sp_leg_usuario_con_permisos", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<PerUsuario>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapUsuarioConPermisos(reader));
                        }
                    }

                    return response;
                }

            }
        }

        public async Task<List<PerUsuario>> GetPerUsuariosConAcciones()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("sp_leg_permiso_personas_acciones", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<PerUsuario>();
                    await sql.OpenAsync();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapUsuarioConPermisos(reader));
                        }
                    }

                    return response;
                }

            }
        }

        private PerUsuario MapUsuarioConPermisos(SqlDataReader reader)
        {
            return new PerUsuario()
            {
                CPerCodigo = reader["cPerCodigo"].ToString(),
                cPerApellido = reader["cPerApellido"].ToString(),
                cPerNombre = reader["cPerNombre"].ToString(),
                CPerUsuCodigo = reader["cPerUsuCodigo"].ToString(),
                CPerUsuEstado = (int)reader["cPerUsuEstado"],
                CPerJuridica = "",
                CPudFecha = null
            };
        }
    }
}
