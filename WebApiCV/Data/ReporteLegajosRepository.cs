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
    public class ReporteLegajosRepository
    {
        private readonly string _connectionString;

        public ReporteLegajosRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("CadenaConexionDB");
        }
        public async Task<List<ReporteLegajos>> GetReporteLegajosCount(int nPrdActividad)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_GetSeccionesLegajosUSS", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pnPrdActividad", nPrdActividad == 0 ? DBNull.Value : nPrdActividad));
                    var response = new List<ReporteLegajos>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToLegajos(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private ReporteLegajos MapToLegajos(SqlDataReader reader)
        {
            return new ReporteLegajos()
            {
                nLegDatCodigo = (int)reader["nLegDatCodigo"],
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
                EsDocente = (int)reader["EsDocente"],
                Secc01 = (int)reader["Secc01"],
                Secc02 = (int)reader["Secc02"],
                Secc03 = (int)reader["Secc03"],
                Secc04 = (int)reader["Secc04"],
                Secc05 = (int)reader["Secc05"],
                Secc06 = (int)reader["Secc06"],
                Secc07 = (int)reader["Secc07"],
                Secc08 = (int)reader["Secc08"],
                Secc09 = (int)reader["Secc09"],
                Secc10 = (int)reader["Secc10"],
                Secc11 = (int)reader["Secc11"],
                Secc12 = (int)reader["Secc12"],
                Secc13 = (int)reader["Secc13"],
                Secc14 = (int)reader["Secc14"],
                Secc15 = (int)reader["Secc15"],
                Secc16 = (int)reader["Secc16"]
            };
        }


        //
        // ------------------- EDGAR BARRETO SANDOVAL --------------------------------------------------------------------------------
        //

        public async Task<List<ReporteCapInvLegajos>> GetReporteInvCapLegajos(int nPrdActividad)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Legajos_InfoCapInvLegajos", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@pnPrdActividad", nPrdActividad == 0 ? DBNull.Value : nPrdActividad));
                    var response = new List<ReporteCapInvLegajos>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToCapInvLegajos(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private ReporteCapInvLegajos MapToCapInvLegajos(SqlDataReader reader)
        {
            return new ReporteCapInvLegajos()
            {
                nLegDatCodigo = (int)reader["nLegDatCodigo"],
                cPerCodigo = reader["cPerCodigo"].ToString(),
                cPerApellido = reader["cPerApellido"].ToString(),
                cPerNombre = reader["cPerNombre"].ToString(),
                cCargo = reader["cCargo"].ToString(),
                cArea = reader["cArea"].ToString(),
                cPerEmail = reader["cPerEmail"].ToString(),
                cPerTelefono = reader["cPerTelefono"] is DBNull ? "" : reader["cPerTelefono"].ToString(),
                cPerTipoDoc = reader["cPerTipoDoc"].ToString(),

                LegInvestigador = (int)reader["LegInvestigador"],
                dFechaRegistroInv = (DateTime)reader["dFechaRegistroInv"],
                dFechaModificaInv = (DateTime)reader["dFechaModificaInv"],
                LegTesisAseJur = (int)reader["LegTesisAseJur"],
                dFechaRegistroTes = (DateTime)reader["dFechaRegistroTes"],
                dFechaModificaTes = (DateTime)reader["dFechaModificaTes"],
                LegProduccionCiencia = (int)reader["LegProduccionCiencia"],
                dFechaRegistroProd = (DateTime)reader["dFechaRegistroProd"],
                dFechaModificaProd = (DateTime)reader["dFechaModificaProd"],
                LegParticipacionCongSem = (int)reader["LegParticipacionCongSem"],
                dFechaRegistroCongSem = (DateTime)reader["dFechaRegistroCongSem"],
                dFechaModificaCongSem = (DateTime)reader["dFechaModificaCongSem"],
                LegCapacitaciones = (int)reader["LegCapacitaciones"],
                dFechaRegistroCap = (DateTime)reader["dFechaRegistroCap"],
                dFechaModificaCap = (DateTime)reader["dFechaModificaCap"],
                LegCapacitacionInterna = (int)reader["LegCapacitacionInterna"],
                dFechaRegistroCapInt = (DateTime)reader["dFechaRegistroCapInt"],
                dFechaModificaCapInt = (DateTime)reader["dFechaModificaCapInt"]
            };
        }





        public async Task<List<EvaluacionDocentesCargaLectiva>> GetEvaluacionDocentesCargaLectiva(string cPrdNombre)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Legajos_EvaluacionDocentesCargaLectiva", sql))

                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add(new SqlParameter("@cPrdNombre", cPrdNombre));
                    var response = new List<EvaluacionDocentesCargaLectiva>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToLeg_EvaluacionDocentesCargaLectiva(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private EvaluacionDocentesCargaLectiva MapToLeg_EvaluacionDocentesCargaLectiva(SqlDataReader reader)
        {

            return new EvaluacionDocentesCargaLectiva()
            {
                cPerCodigo = reader["cPerCodigo"] is DBNull ? "" : reader["cPerCodigo"].ToString(),
                nLegDatCodigo = reader["nLegDatCodigo"] is DBNull ? 0 : (int)reader["nLegDatCodigo"],
                cPerApellido = reader["cPerApellido"] is DBNull ? "" : reader["cPerApellido"].ToString(),
                cPerNombre = reader["cPerNombre"] is DBNull ? "" : reader["cPerNombre"].ToString(),
                cCargo = reader["cCargo"] is DBNull ? "" : reader["cCargo"].ToString(),
                cArea = reader["cArea"] is DBNull ? "" : reader["cArea"].ToString(),
                cPerNroDoc = reader["cPerNroDoc"] is DBNull ? "" : reader["cPerNroDoc"].ToString(),
                PromedioEDD = reader["PromedioEDD"] is DBNull ? 0.00 : Convert.ToDouble(reader["PromedioEDD"]),
            };
        }


        public async Task<List<DocentesCargaLectiva>> GetDocentesCargaLectiva()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Legajos_DocentesCargaLectiva", sql))

                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<DocentesCargaLectiva>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToLeg_DocentesCargaLectiva(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private DocentesCargaLectiva MapToLeg_DocentesCargaLectiva(SqlDataReader reader)
        {

            return new DocentesCargaLectiva()
            {
                nLegDatCodigo = reader["nLegDatCodigo"] is DBNull ? 0 : (int)reader["nLegDatCodigo"],
                cPerCodigo = reader["cPerCodigo"] is DBNull ? "" : reader["cPerCodigo"].ToString(),
                cPerNombre = reader["cPerNombre"] is DBNull ? "" : reader["cPerNombre"].ToString(),
                cPerApellido = reader["cPerApellido"] is DBNull ? "" : reader["cPerApellido"].ToString(),
                nLegGraDatCodigo = reader["nLegGraDatCodigo"] is DBNull ? 0 : (int)reader["nLegGraDatCodigo"],
                cIntDescripcion = reader["cIntDescripcion"] is DBNull ? "" : reader["cIntDescripcion"].ToString(),
                cLegGraInstitucion = reader["cLegGraInstitucion"] is DBNull ? "" : reader["cLegGraInstitucion"].ToString(),
                cInstitucion = reader["cInstitucion"] is DBNull ? "" : reader["cInstitucion"].ToString(),
                cLegGraOtraInst = reader["cLegGraOtraInst"] is DBNull ? "" : reader["cLegGraOtraInst"].ToString()
            };
        }

        public async Task<List<Leg_EvaDoc_RenovacionRatificacion>> GetLeg_EvaDoc_Renovacion_Ratificacion()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("usp_Leg_EvaDoc_Renovacion_Ratificacion", sql))
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    var response = new List<Leg_EvaDoc_RenovacionRatificacion>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToLeg_EvaDoc_RenovacionRatificacion(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private Leg_EvaDoc_RenovacionRatificacion MapToLeg_EvaDoc_RenovacionRatificacion(SqlDataReader reader)
        {

            return new Leg_EvaDoc_RenovacionRatificacion()
            {
                nRenRatCodigo = reader["nRenRatCodigo"] == null ? 0 : (int)reader["nRenRatCodigo"],
                nLegDatCodigo = (int)reader["nLegDatCodigo"],
                cPerCodigo = reader["cPerCodigo"].ToString(),
                cPerApellido = reader["cPerApellido"].ToString(),
                cPerNombre = reader["cPerNombre"].ToString(),
                cCargo = reader["cCargo"].ToString(),
                cArea = reader["cArea"].ToString(),
                cPerNroDoc = reader["cPerNroDoc"].ToString(),
                nLegRenRatEDD = reader["nLegRenRatEDD"] == null ? 0 : Double.Parse(reader["nLegRenRatEDD"].ToString()),
                nLegRenRatCD = reader["dLegRenRatCD"] == null ? 0 : Double.Parse(reader["dLegRenRatCD"].ToString()),
                nLegRenRatPC = reader["dLegRenRatPC"] == null ? 0 : Double.Parse(reader["dLegRenRatPC"].ToString()),
                nLegRenRatPromedio = reader["dLegRenRatPromedio"] == null ? 0 : Double.Parse(reader["dLegRenRatPromedio"].ToString()),
                cLegRenRatCondicion = reader["cLegRenRatCondicion"] == null ? "" : reader["cLegRenRatCondicion"].ToString(),
                cLegRenRatRenRat = reader["cLegRenRatRenRat"] == null ? "" : reader["cLegRenRatRenRat"].ToString(),
                nLegRenRatEstado = reader["nLegRenRatEstado"] == null ? 0 : (int)reader["nLegRenRatEstado"],
            };
        }

        //
        // ------------------- EDGAR BARRETO SANDOVAL --------------------------------------------------------------------------------
        //
    }
}
