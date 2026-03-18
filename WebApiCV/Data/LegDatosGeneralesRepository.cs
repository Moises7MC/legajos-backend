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
    public class LegDatosGeneralesRepository
    {
        private readonly string _connectionString;

        private readonly ConstanteRepository repositorycons;

        private readonly InterfaceRepository repositoryinterf;

        private readonly PersonaRepository repositorypersona;

        public LegDatosGeneralesRepository(
            IConfiguration configuration,
            ConstanteRepository repositorycons,
            InterfaceRepository repositoryinterf,
            PersonaRepository repositorypersona
        )
        {
            _connectionString =
                configuration.GetConnectionString("CadenaConexionDB");
            this.repositorycons = repositorycons;
            this.repositoryinterf = repositoryinterf;
            this.repositorypersona = repositorypersona;
        }

        public async Task<List<MensajeLegajo>>
        GetMensajeLegajos(int pnLegDatCodigo, Boolean pbDocente)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (
                    SqlCommand cmd = new SqlCommand("usp_Legajos_Mensaje", sql)
                )
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@pnLegDatCodigo",
                            pnLegDatCodigo));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@pbDocente", pbDocente));
                    var response = new List<MensajeLegajo>();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response.Add(MapToMensaje(reader));
                        }
                    }

                    return response;
                }
            }
        }

        private MensajeLegajo MapToMensaje(SqlDataReader reader)
        {
            return new MensajeLegajo()
            {
                nTipo = (int) reader["nTipo"],
                nCantidad = (int) reader["nCantidad"],
                cMensaje = reader["cMensaje"].ToString()
            };
        }

        public async Task<Boolean>
        UpdateStateDJ(int pcLegDatCodigo, String pcPerCodigo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (
                    SqlCommand cmd =
                        new SqlCommand("usp_Legajos_UpdateStsateDJ", sql)
                )
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@pnLegDJDatCodigo",
                            pcLegDatCodigo));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@pcPerCodigo", pcPerCodigo));
                    await sql.OpenAsync();
                    cmd.ExecuteNonQuery();

                    return true;
                }
            }
        }

        public async Task<LegDatosGenerales> GetDatosSEUSS(String pcPerCodigo)
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (
                    SqlCommand cmd =
                        new SqlCommand("usp_Legajos_DatosSEUSS", sql)
                )
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@pcPerCodigo", pcPerCodigo));
                    var response = new LegDatosGenerales();
                    await sql.OpenAsync();

                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            response = MapToLegDatosGenerales(reader);
                        }
                    }

                    return response;
                }
            }
        }

        private LegDatosGenerales MapToLegDatosGenerales(SqlDataReader reader)
        {
            return new LegDatosGenerales()
            {
                NLegDatCodigo = (int) reader["nLegDatCodigo"],
                NLegDatTipoDoc =reader["nLegDatTipoDoc"] == null? 0: (int) reader["nLegDatTipoDoc"],
                NClaseTipoDoc =reader["nClaseTipoDoc"] == null? 0: (int) reader["nClaseTipoDoc"],
                CLegDatNroDoc = reader["cLegDatNroDoc"].ToString(),
                CLegDatApellidoPaterno =reader["cLegDatApellidoPaterno"].ToString(),
                CLegDatApellidoMaterno =reader["cLegDatApellidoMaterno"].ToString(),
                CLegDatNombres = reader["cLegDatNombres"].ToString(),
                DLegDatFechaNacimiento =DateTime.Parse(reader["dLegDatFechaNacimiento"].ToString()),
                NLegDatSexo = reader["nLegDatSexo"] == null? 0: (int) reader["nLegDatSexo"],
                NClaseSexo = reader["nClaseSexo"] == null? 0: (int) reader["nClaseSexo"],
                NLegDatEstadoCivil = reader["nLegDatEstadoCivil"] == null? 0: (int) reader["nLegDatEstadoCivil"],
                NClaseEstadoCivil = reader["nClaseEstadoCivil"] == null? 0: (int) reader["nClaseEstadoCivil"],
                CLegDatFoto = reader["cLegDatFoto"].ToString(),
                cLegDatFirma = reader["cLegDatFirma"].ToString(),
                cLegDatSunedu = reader["cLegDatSunedu"].ToString(),
                cLegDatPolicial = reader["cLegDatPolicial"].ToString(),
                cLegDatJudicial = reader["cLegDatJudicial"].ToString(),
                CLegDatEmail = reader["cLegDatEmail"].ToString(),
                CLegDatTelefono = reader["cLegDatTelefono"].ToString(),
                CLegDatMovil = reader["cLegDatMovil"].ToString(),
                NLegDatGradoAcad = reader["nLegDatGradoAcad"] == null ? 0 : (int)reader["nLegDatGradoAcad"],
                NClaseGradoAcad = reader["nClaseGradoAcad"] == null ? 0 : (int)reader["nClaseGradoAcad"],
                NLegDatPais = reader["nLegDatPais"] == null ? 0 : (int)reader["nLegDatPais"],
                NClasePais = reader["nClasePais"] == null ? 0 : (int)reader["nClasePais"],
                CLegDatAcerca = reader["cLegDatAcerca"].ToString(),
                NLegDatTipoDomicilio = reader["nLegDatTipoDomicilio"] == null ? 0 : (int)reader["nLegDatTipoDomicilio"],
                NValorTipoDomicilio = reader["nValorTipoDomicilio"] == null ? 0 : (int)reader["nValorTipoDomicilio"],
                NLegDatZona = reader["nLegDatZona"] == null ? 0 : (int)reader["nLegDatZona"],
                NValorZona = reader["nValorZona"] == null ? 0 : (int)reader["nValorZona"],
                CLegDatCalleDomicilio = reader["cLegDatCalleDomicilio"].ToString(),
                CLegDatNroDomicilio = reader["cLegDatNroDomicilio"].ToString(),
                CLegDatMzaDomicilio = reader["cLegDatMzaDomicilio"].ToString(),
                CLegDatLtDomicilio = reader["cLegDatLtDomicilio"].ToString(),
                CLegDatDptoDomicilio = reader["cLegDatDptoDomicilio"].ToString(),
                CLegDatReferencia = reader["cLegDatReferencia"].ToString(),
                NLetDatUbigeo = reader["nLetDatUbigeo"] == null? 0: (int) reader["nLetDatUbigeo"],
                NClaseUbigeo = reader["nClaseUbigeo"] == null? 0: (int) reader["nClaseUbigeo"],
                NLetDatNacimiento = reader["nLetDatNacimiento"] == null? 0: (int) reader["nLetDatNacimiento"],
                NClaseNacimiento = reader["nClaseNacimiento"] == null? 0: (int) reader["nClaseNacimiento"],
                CLegDatColegioProf = reader["cLegDatColegioProf"].ToString(),
                CLegDatNroColegiatura = reader["cLegDatNroColegiatura"].ToString(),
                NLegDatCondicionColeg = reader["nLegDatCondicionColeg"] == null? 0 : (int) reader["nLegDatCondicionColeg"],
                NValorCondicionColeg = reader["nValorCondicionColeg"] == null ? 0 : (int) reader["nValorCondicionColeg"],
                DLegDatosFechaEmisionColeg = DateTime.Parse(reader["dLegDatosFechaEmisionColeg"].ToString()),
                DLegDatosFechaExpiraColeg = DateTime.Parse(reader["dLegDatosFechaExpiraColeg"].ToString()),
                CLegDatEstado = true,
                cPerCodigo = reader["cPerCodigo"].ToString(),
                CUsuRegistro = reader["cUsuRegistro"].ToString(),
                DFechaRegistro = DateTime.Parse(reader["dFechaRegistro"].ToString()),
                CUsuModifica = reader["cUsuModifica"].ToString(),
                DFechaModifica = DateTime.Parse(reader["dFechaModifica"].ToString()),
				
                // ------------------ EDGAR_BS-2025---------------------------------------->
                // Nuevos campos - Régimen Pensionario
                NLegDatRegPenAfiliado = reader["nLegDatRegPenAfiliado"] == DBNull.Value ? 0 : (int)reader["nLegDatRegPenAfiliado"],
                NValorAfiliado = reader["nValorAfiliado"] == DBNull.Value ? 0 : (int)reader["nValorAfiliado"],
                NLegDatRegPenEntidad = reader["nLegDatRegPenEntidad"] == DBNull.Value ? 0 : (int)reader["nLegDatRegPenEntidad"],
                NValorEntidad = reader["nValorEntidad"] == DBNull.Value ? 0 : (int)reader["nValorEntidad"],
                DLegDatRegPenFechaCese = reader["dLegDatRegPenFechaCese"] == DBNull.Value ? (DateTime?)null : DateTime.Parse(reader["dLegDatRegPenFechaCese"].ToString()),

                // Nuevos campos - Cuenta de haberes
                NLegDatCtaHabHaberes = reader["nLegDatCtaHabHaberes"] == DBNull.Value ? 0 : (int)reader["nLegDatCtaHabHaberes"],
                NValorHaberes = reader["nValorHaberes"] == DBNull.Value ? 0 : (int)reader["nValorHaberes"],
                NLegDatCtaHabBanco = reader["nLegDatCtaHabBanco"] == DBNull.Value ? 0 : (int)reader["nLegDatCtaHabBanco"],
                NValorBanco = reader["nValorBanco"] == DBNull.Value ? 0 : (int)reader["nValorBanco"],
                CLegDatCtaHabNumCta = reader["cLegDatCtaHabNumCta"].ToString(),
                CLegDatCtaHabNumCtaCci = reader["cLegDatCtaHabNumCtaCci"].ToString(),
                NLegDatCtaHabBancoAperturar = reader["nLegDatCtaHabBancoAperturar"] == DBNull.Value ? 0 : (int)reader["nLegDatCtaHabBancoAperturar"],
                NValorBancoAperturar = reader["nValorBancoAperturar"] == DBNull.Value ? 0 : (int)reader["nValorBancoAperturar"],

                CLegDatMencionEnMayGradAcad = reader["CLegDatMencionEnMayGradAcad"].ToString(),
                CLegDatInstitucionMayGradAcad = reader["CLegDatInstitucionMayGradAcad"].ToString(),

                NLegDatAceptaTerminos = reader["nLegDatAceptaTerminos"] == DBNull.Value ? (bool?)null : (bool)(reader["nLegDatAceptaTerminos"]),
                // ------------------ EDGAR_BS-2025---------------------------------------->

                vSexo = reader["nClaseSexo"] == null ? null
                        : repositorycons.GetConstanteDatos((int) reader["nLegDatSexo"],(int) reader["nClaseSexo"]).Result?? null,
                vEstadoCivil = reader["nClaseEstadoCivil"] == null ? null
                        : repositorycons.GetConstanteDatos((int) reader["nLegDatEstadoCivil"],(int) reader["nClaseEstadoCivil"]).Result?? null,
                vTipoDoc = reader["nClaseTipoDoc"] == null ? null
                        : repositoryinterf.GetInterfaceDatos((int) reader["NLegDatTipoDoc"],(int) reader["nClaseTipoDoc"]).Result?? null,
                vTipoDomicilio = reader["nValorTipoDomicilio"] == null ? null
                        : repositorycons.GetConstanteDatos((int) reader["nLegDatTipoDomicilio"],(int) reader["nValorTipoDomicilio"]).Result?? null
            };
        }

        /* ANALISTA: JZ*/
        public async Task<Boolean>
        ValidarCodigoPersonaCargaMasiva(string cPerCodigo)
        {
            System.Diagnostics.Debug.Write (cPerCodigo);

            var val = true;
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (
                    SqlCommand cmd =
                        new SqlCommand("per_validar_codigo_cargamasiva", sql)
                )
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cPerCodigo", cPerCodigo));
                    await sql.OpenAsync();
                    cmd.ExecuteNonQuery();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            if (ObtenerCantidadRegistros(reader) == 0)
                            {
                                val = false;
                            }
                        }
                    }
                }
            }
            return val;
        }

        public int ObtenerCantidadRegistros(SqlDataReader reader)
        {
            return (int) reader["cantExist"];
        }

        public async Task<Boolean>
        cargaMasivaLegParticipacionCongSem(
            string cLegParInstitucion,
            int nLegParRol,
            int nValorRol,
            int nLegParAmbito,
            int nValorAmbito,
            string cLegParNombre,
            DateTime dLegParFecha,
            string cLegParArchivo,
            int nLegParDatCodigo,
            bool? cLegParEstado,
            string cUsuRegistro,
            DateTime dFechaRegistro,
            string cUsuModifica,
            DateTime? dFechaModifica,
            string cLegParOtraInst,
            string cLegParPais,
            DateTime dLegParFechaFin,
            int? nLegParHoras,
            string cPerCodigo
        )
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                try
                {
                    await sql.OpenAsync();
                    var cmd =
                        new SqlCommand("LegParticipacionCongSem_cargamasiva",
                            sql);

                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegParInstitucion",
                            cLegParInstitucion));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nLegParRol", nLegParRol));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nValorRol", nValorRol));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nLegParAmbito", nLegParAmbito));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nValorAmbito", nValorAmbito));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegParNombre", cLegParNombre));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@dLegParFecha", dLegParFecha));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegParArchivo",
                            cLegParArchivo));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nLegParDatCodigo",
                            nLegParDatCodigo));
                    cmd.Parameters.Add(new SqlParameter("@cLegParValida", '0'));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegParEstado", cLegParEstado));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cUsuRegistro", cUsuRegistro));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@dFechaRegistro",
                            dFechaRegistro));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cUsuModifica", cUsuModifica));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@dFechaModifica",
                            dFechaModifica));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegParOtraInst",
                            cLegParOtraInst));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegParPais", cLegParPais));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@dLegParFechaFin",
                            dLegParFechaFin));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nLegParHoras", nLegParHoras));

                    //cmd.Parameters.Add(new SqlParameter("@cLegParValida", 0));
                    //cmd.Parameters.Add(new SqlParameter("@cLegParArchivoOrig", cLegParArchivoOrig
                    Console.WriteLine('d');
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine (e);
                    return false;
                }
            }
        }

        /*  */
        public async Task<List<UnidadOrganizacional>>
        GetCargarUnidadOrganizacional()
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (
                    SqlCommand cmd =
                        new SqlCommand("BDSipan.dbo.usp_ArchivoCentral_Get_areas",
                            sql)
                )
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

        private UnidadOrganizacional
        MapToUnidadOrganizacional(SqlDataReader reader)
        {
            return new UnidadOrganizacional()
            {
                NUniOrgCodigo = (int) reader["nUniOrgCodigo"],
                CUniOrgNombre = reader["CUniOrgNombre"].ToString(),
                CPerJuridad = reader["CPerJuridica"].ToString(),
                CPerNombre = reader["CPerNombre"].ToString()
            };
        }

        public async Task<Boolean>
        cargaMasivaCapacitacion(
            int nLegCapTipo,
            int nLegCapTipoEsp,
            string cLegCapNombre,
            int nLegCapHoras,
            DateTime dLegCapFechaInicio,
            DateTime dLegCapFechaFin,
            string? cLegCapArchivo,
            string? cLegCapInstitucion,
            int nLegCapDatCodigo,
            bool? cLegCapValida,
            bool? cLegCapEstado,
            int nValorTipo,
            int nValorTipoEsp,
            string cUsuRegistro,
            DateTime dFechaRegistro,
            string cUsuModifica,
            DateTime? dFechaModifica,
            string? cLegCapOtraInst,
            string cLegCapPais
        )
        {
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                try
                {
                    await sql.OpenAsync();
                    var cmd =
                        new SqlCommand("LegCapacitaciones_CargaMasiva",
                            sql);

                    //cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nLegCapTipo", nLegCapTipo));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nLegCapTipoEsp",
                            nLegCapTipoEsp));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegCapNombre", cLegCapNombre));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nLegCapHoras", nLegCapHoras));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@dLegCapFechaInicio",
                            dLegCapFechaInicio));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@dLegCapFechaFin",
                            dLegCapFechaFin));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegCapArchivo",
                            cLegCapArchivo));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegCapInstitucion",
                            cLegCapInstitucion));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nLegCapDatCodigo",
                            nLegCapDatCodigo));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegCapValida", cLegCapValida));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegCapEstado", cLegCapEstado));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nValorTipo", nValorTipo));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@nValorTipoEsp", nValorTipoEsp));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cUsuRegistro", cUsuRegistro));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@dFechaRegistro",
                            dFechaRegistro));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cUsuModifica", cUsuModifica));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@dFechaModifica",
                            dFechaModifica));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegCapOtraInst",
                            cLegCapOtraInst));
                    cmd
                        .Parameters
                        .Add(new SqlParameter("@cLegCapPais", cLegCapPais));

                    //cmd.Parameters.Add(new SqlParameter("@cLegParValida", 0));
                    //cmd.Parameters.Add(new SqlParameter("@cLegParArchivoOrig", cLegParArchivoOrig
                    Console.WriteLine('d');
                    await cmd.ExecuteNonQueryAsync();
                    return true;
                }
                catch (Exception e)
                {
                    Console.WriteLine (e);
                    return false;
                }
            }
        }




        //------------------------------------------------------
        public async Task<Boolean>
        RegistrarPermisos(String nIntCodigo, String cPerCodigo)
        {
            var val = true;
            try { 
            
            using (SqlConnection sql = new SqlConnection(_connectionString))
            {
                using (
                    SqlCommand cmd = new SqlCommand("BDSipan.dbo.leg_actualizar_permisos", sql)
                )
                {
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    //cmd.Parameters.Add(new SqlParameter("@cIntJerarquia", cIntJerarquia));
                    //cmd.Parameters.Add(new SqlParameter("@nIntClase", nIntClase));
                    cmd.Parameters.Add(new SqlParameter("@nIntCodigo", nIntCodigo));
                    cmd.Parameters.Add(new SqlParameter("@cPerCodigo", cPerCodigo));
                    await sql.OpenAsync();
                    cmd.ExecuteNonQuery();
                    using (var reader = await cmd.ExecuteReaderAsync())
                    {
                       
                                val = false;
                       
                    }
                }
            }
                return val;
            }
            catch(Exception e)
            {
                Console.Write(e);
                return val;
            }
           
        }

        public async Task<Boolean>
      RegistrarAccionesXUsuario(String nIntCodigo, String cPerCodigo)
        {
            var val = true;
            try
            {

                using (SqlConnection sql = new SqlConnection(_connectionString))
                {
                    using (
                        SqlCommand cmd = new SqlCommand("leg_actualizar_accionesxusuario", sql)
                    )
                    {
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        
                        cmd.Parameters.Add(new SqlParameter("@nIntCodigo", nIntCodigo));
                        cmd.Parameters.Add(new SqlParameter("@cPerCodigo", cPerCodigo));
                        await sql.OpenAsync();
                        cmd.ExecuteNonQuery();
                        using (var reader = await cmd.ExecuteReaderAsync())
                        {

                            val = false;

                        }
                    }
                }
                return val;
            }
            catch (Exception e)
            {
                Console.Write(e);
                return val;
            }

        }
       
    }
}
