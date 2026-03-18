using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Configuration;
using WebApiCV.Entity;
using WebApiCV.ModelsProcedure;
#nullable disable

namespace WebApiCV.Contexts
{
    
    public partial class bdLegajosContext : DbContext
    {

        public bdLegajosContext() { }

        public bdLegajosContext(DbContextOptions<bdLegajosContext> options)
            : base(options)
        {}

        public virtual DbSet<Constante> Constantes { get; set; }
        public virtual DbSet<Interface> Interfaces { get; set; }
        public virtual DbSet<LegAdminitrativaCarga> LegAdminitrativaCarga { get; set; }
        public virtual DbSet<LegCapacitaciones> LegCapacitaciones { get; set; }
        public virtual DbSet<LegCategoriaDocente> LegCategoriaDocente { get; set; }
        public virtual DbSet<LegDatosGenerales> LegDatosGenerales { get; set; }
        public virtual DbSet<LegDocenciaUniv> LegDocenciaUniv { get; set; }
        public virtual DbSet<LegGradoTitulo> LegGradoTitulo { get; set; }
        public virtual DbSet<LegIdiomaOfimatica> LegIdiomaOfimatica { get; set; }
        public virtual DbSet<LegInvestigador> LegInvestigador { get; set; }
        public virtual DbSet<LegParticipacionCongSem> LegParticipacionCongSem { get; set; }
        public virtual DbSet<LegProduccionCiencia> LegProduccionCiencia { get; set; }
        public virtual DbSet<LegProfesNoDocente> LegProfesNoDocente { get; set; }
        public virtual DbSet<LegProyeccionSocial> LegProyeccionSocial { get; set; }
        public virtual DbSet<LegReconocimiento> LegReconocimiento { get; set; }
        public virtual DbSet<LegRegimenDedicacion> LegRegimenDedicacion { get; set; }
        public virtual DbSet<LegTesisAseJur> LegTesisAseJur { get; set; }
        public virtual DbSet<PerUsuario> PerUsuario { get; set; }
        public virtual DbSet<Persona> Persona { get; set; }
        public virtual DbSet<UnidadOrganizacional> UnidadOrganizacional { get; set; }
        public virtual DbSet<LegGrupInvSem> LegGrupInvSem { get; set; }
        public virtual DbSet<CapacitacionesUss> CapacitacionesUss { get; set; }
        public virtual DbSet<LegArchivos> LegArchivos { get; set; }
        public virtual DbSet<LegCapacitacionInterna> LegCapacitacionInterna { get; set; }
        public virtual DbSet<LegContrato> LegContrato { get; set; }
        public virtual DbSet<LegResolucion> LegResolucion { get; set; }
        public virtual DbSet<LegEvaluacionDesemp> LegEvaluacionDesemp { get; set; }
        public virtual DbSet<LegSeleccion> LegSeleccion { get; set; }
        public virtual DbSet<LegOrdinarizacion> LegOrdinarizacion { get; set; }
        public virtual DbSet<LegDeclaracionJurada> LegDeclaracionJurada { get; set; }
        public virtual DbSet<LegDocumentacionInterna> LegDocumentacionInterna { get; set; }

        // EBS - 01/2025 ------------>
        //LegLicenciaProfesional:: para registrar licencias o registros profesionales (colegios, nº colegiatura, condición, fechas, archivo, etc.)
        public virtual DbSet<LegLicenciaProfesional> LegLicenciaProfesional { get; set; }
        //LegMembresia:: para registrar membresias (colegios, nº colegiatura, fechas, etc.)
        public virtual DbSet<LegMembresia> LegMembresia { get; set; }
        // EBS - 01/2025 <------------
        public virtual DbSet<PermisosModulo> PermisosModulo { get; set; }
        public virtual DbSet<PermisosTarea> PermisosTarea { get; set; }
        public virtual DbSet<TareaModulo> TareaModulo { get; set; }
        public virtual DbSet<ModulosTD> ModulosTD { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Modern_Spanish_CI_AS");

            modelBuilder.Entity<Constante>(entity =>
            {
                entity.HasKey(e => new { e.NConCodigo, e.NConValor })
                    .HasName("PK__Constant__6522CFA9F6A0950A");

                entity.ToTable("BDSipan.Constante");

                entity.Property(e => e.NConCodigo).HasColumnName("nConCodigo");

                entity.Property(e => e.NConValor).HasColumnName("nConValor");

                entity.Property(e => e.CConDescripcion)
                    .IsRequired()
                    .HasMaxLength(250)
                    .HasColumnName("cConDescripcion");
            });

            modelBuilder.Entity<Interface>(entity =>
            {
                entity.HasKey(e => new { e.NIntCodigo, e.NIntClase })
                    .HasName("PK__Interfac__6DB1CF41C3BF1D81");

                entity.ToTable("BDSipan.Interface");

                entity.Property(e => e.NIntCodigo).HasColumnName("nIntCodigo");

                entity.Property(e => e.NIntClase).HasColumnName("nIntClase");

                entity.Property(e => e.CIntDescripcion)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("cIntDescripcion");

                entity.Property(e => e.CIntJerarquia)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("cIntJerarquia");

                entity.Property(e => e.CIntNombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("cIntNombre");

                entity.Property(e => e.NIntTipo).HasColumnName("nIntTipo");
            });

            modelBuilder.Entity<LegAdminitrativaCarga>(entity =>
            {
                entity.HasKey(e => e.NLegAdmCodigo)
                    .HasName("PK_LEGADMINITRATIVACARGA");

                entity.ToTable("LegAdminitrativaCarga");

                entity.Property(e => e.NLegAdmCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegAdmCodigo");

                entity.Property(e => e.CLegAdmArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegAdmArchivo");

                entity.Property(e => e.CLegAdmDocumento)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("cLegAdmDocumento");

                entity.Property(e => e.CLegAdmEstado)
                    .IsRequired()
                    .HasColumnName("cLegAdmEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegAdmInstitucion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegAdmInstitucion")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegAdmPais).HasColumnName("cLegAdmPais");

                entity.Property(e => e.CLegAdmOtraInst).HasColumnName("cLegAdmOtraInst");

                entity.Property(e => e.CLegAdmValida)
                    .IsRequired()
                    .HasColumnName("cLegAdmValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegAdmFechaFin)
                    .HasColumnType("date")
                    .HasColumnName("dLegAdmFechaFin");

                entity.Property(e => e.DLegAdmFechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("dLegAdmFechaInicio");

                entity.Property(e => e.NClaseCargo).HasColumnName("nClaseCargo");

                entity.Property(e => e.NLegAdmCargo).HasColumnName("nLegAdmCargo");

                entity.Property(e => e.NLegAdmDatCodigo).HasColumnName("nLegAdmDatCodigo");

                entity.HasOne(d => d.NLegAdmDatCodigoNavigation)
                    .WithMany(p => p.LegAdminitrativaCarga)
                    .HasForeignKey(d => d.NLegAdmDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegAdminitrativaCarga_fk3");
            });

            modelBuilder.Entity<LegCapacitaciones>(entity =>
            {
                entity.HasKey(e => e.NLegCapCodigo)
                    .HasName("PK_LegCapacitaciones");

                entity.Property(e => e.NLegCapCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegCapCodigo");

                entity.Property(e => e.CLegCapArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegCapArchivo");

                entity.Property(e => e.CLegCapEstado)
                    .IsRequired()
                    .HasColumnName("cLegCapEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegCapValida)
                    .IsRequired()
                    .HasColumnName("cLegCapValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegCapFechaFin)
                    .HasColumnType("date")
                    .HasColumnName("dLegCapFechaFin");

                entity.Property(e => e.DLegCapFechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("dLegCapFechaInicio");

                entity.Property(e => e.CLegCapNombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegCapNombre");

                entity.Property(e => e.NLegCapDatCodigo).HasColumnName("nLegCapDatCodigo");

                entity.Property(e => e.CLegCapInstitucion).HasColumnName("cLegCapInstitucion");

                entity.Property(e => e.CLegCapPais).HasColumnName("cLegCapPais");

                entity.Property(e => e.CLegCapOtraInst).HasColumnName("cLegCapOtraInst");

                entity.Property(e => e.NLegCapHoras).HasColumnName("nLegCapHoras");

                entity.Property(e => e.NLegCapTipo).HasColumnName("nLegCapTipo");

                entity.Property(e => e.NLegCapTipoEsp).HasColumnName("nLegCapTipoEsp");

                entity.Property(e => e.NValorTipo).HasColumnName("nValorTipo");

                entity.Property(e => e.NValorTipoEsp).HasColumnName("nValorTipoEsp");

                entity.HasOne(d => d.NLegCapDatCodigoNavigation)
                    .WithMany(p => p.LegCapacitaciones)
                    .HasForeignKey(d => d.NLegCapDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegCapacitaciones_fk2");

            });

            modelBuilder.Entity<LegCategoriaDocente>(entity =>
            {
                entity.HasKey(e => e.NLegCatCodigo)
                    .HasName("PK_LEGCATEGORIADOCENTE");

                entity.ToTable("LegCategoriaDocente");

                entity.Property(e => e.NLegCatCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegCatCodigo");

                entity.Property(e => e.CLegCatArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegCatArchivo");

                entity.Property(e => e.CLegCatEstado)
                    .IsRequired()
                    .HasColumnName("cLegCatEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegCatInstitucion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegCatInstitucion")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegCatPais).HasColumnName("cLegCatPais");

                entity.Property(e => e.CLegCatOtraInst).HasColumnName("cLegCatOtraInst");


                entity.Property(e => e.CLegCatValida)
                    .IsRequired()
                    .HasColumnName("cLegCatValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegCatFechaFin)
                    .HasColumnType("date")
                    .HasColumnName("dLegCatFechaFin");

                entity.Property(e => e.DLegCatFechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("dLegCatFechaInicio");

                entity.Property(e => e.NLegCatCategoria).HasColumnName("nLegCatCategoria");

                entity.Property(e => e.NLegCatDatCodigo).HasColumnName("nLegCatDatCodigo");

                entity.Property(e => e.NValorCategoria).HasColumnName("nValorCategoria");

                entity.HasOne(d => d.NLegCatDatCodigoNavigation)
                    .WithMany(p => p.LegCategoriaDocente)
                    .HasForeignKey(d => d.NLegCatDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegCategoriaDocente_fk3");
            });

            modelBuilder.Entity<LegDatosGenerales>(entity =>
            {
                entity.HasKey(e => e.NLegDatCodigo)
                    .HasName("PK_LegDatosGenerales");

                entity.HasIndex(e => e.CLegDatNroDoc, "UQ__LegDatos__FCF993D13D7E7095")
                    .IsUnique();

                entity.Property(e => e.NLegDatCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegDatCodigo");

                entity.Property(e => e.CLegDatAcerca)
                    .HasColumnType("text")
                    .HasColumnName("cLegDatAcerca");

                entity.Property(e => e.CLegDatApellidoMaterno)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatApellidoMaterno");

                entity.Property(e => e.CLegDatApellidoPaterno)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatApellidoPaterno");

                entity.Property(e => e.CLegDatCalleDomicilio)
                    .IsRequired()
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatCalleDomicilio");

                entity.Property(e => e.CLegDatColegioProf)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatColegioProf")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegDatDptoDomicilio)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatDptoDomicilio");

                entity.Property(e => e.CLegDatEmail)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatEmail");

                entity.Property(e => e.CLegDatEstado)
                    .IsRequired()
                    .HasColumnName("cLegDatEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegDatFoto)
                    .HasColumnName("cLegDatFoto");

                entity.Property(e => e.CLegDatFoto)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatFoto");

                entity.Property(e => e.cLegDatSunedu)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatSunedu");

                entity.Property(e => e.cLegDatPolicial)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDatPolicial");

                entity.Property(e => e.cLegDatJudicial)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDatJudicial");

                entity.Property(e => e.cLegDatFirma)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDatFirma");

                entity.Property(e => e.CLegDatLtDomicilio)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatLtDomicilio");

                entity.Property(e => e.CLegDatMovil)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatMovil");

                entity.Property(e => e.CLegDatMzaDomicilio)
                    .HasMaxLength(3)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatMzaDomicilio")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegDatNombres)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatNombres");

                entity.Property(e => e.CLegDatNroColegiatura)
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatNroColegiatura");

                entity.Property(e => e.CLegDatNroDoc)
                    .IsRequired()
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatNroDoc");

                entity.Property(e => e.CLegDatNroDomicilio)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatNroDomicilio");

                entity.Property(e => e.CLegDatReferencia)
                    .HasColumnType("text")
                    .HasColumnName("cLegDatReferencia");

                entity.Property(e => e.CLegDatTelefono)
                    .HasMaxLength(25)
                    .IsUnicode(false)
                    .HasColumnName("cLegDatTelefono");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cPerCodigo")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");


                // Mapeo de los nuevos campos ------------ EdgarBS 2025 --------------------------------
                // Régimen Pensionario 

                entity.Property(e => e.NLegDatRegPenAfiliado)
                    .HasColumnName("nLegDatRegPenAfiliado");

                entity.Property(e => e.NValorAfiliado)
                    .HasColumnName("nValorAfiliado");

                entity.Property(e => e.NLegDatRegPenEntidad)
                    .HasColumnName("nLegDatRegPenEntidad");

                entity.Property(e => e.NValorEntidad)
                    .HasColumnName("nValorEntidad");

                entity.Property(e => e.DLegDatRegPenFechaCese)
                    .HasColumnType("datetime")
                    .HasColumnName("dLegDatRegPenFechaCese");

                // Cuenta de Haberes 

                entity.Property(e => e.NLegDatCtaHabHaberes)
                    .HasColumnName("nLegDatCtaHabHaberes");

                entity.Property(e => e.NValorHaberes)
                    .HasColumnName("nValorHaberes");

                entity.Property(e => e.NLegDatCtaHabBanco)
                    .HasColumnName("nLegDatCtaHabBanco");

                entity.Property(e => e.NValorBanco)
                    .HasColumnName("nValorBanco");

                entity.Property(e => e.CLegDatCtaHabNumCta)
                    .HasMaxLength(20)
                    .HasColumnName("cLegDatCtaHabNumCta");

                entity.Property(e => e.CLegDatCtaHabNumCtaCci)
                    .HasMaxLength(20)
                    .HasColumnName("cLegDatCtaHabNumCtaCci");

                entity.Property(e => e.NLegDatCtaHabBancoAperturar)
                    .HasColumnName("nLegDatCtaHabBancoAperturar");

                entity.Property(e => e.NValorBancoAperturar)
                    .HasColumnName("nValorBancoAperturar");

                entity.Property(e => e.NLegDatAceptaTerminos)
                    .HasColumnName("nLegDatAceptaTerminos");
                // Cuenta de Haberes --------------------------->


                entity.Property(e => e.CLegDatMencionEnMayGradAcad)
                    .HasMaxLength(100)
                    .HasColumnName("cLegDatMencionEnMayGradAcad");
                entity.Property(e => e.CLegDatInstitucionMayGradAcad)
                    .HasMaxLength(10)
                    .HasColumnName("cLegDatInstitucionMayGradAcad");




                entity.Property(e => e.DLegDatFechaNacimiento)
                    .HasColumnType("date")
                    .HasColumnName("dLegDatFechaNacimiento");

                entity.Property(e => e.DLegDatosFechaEmisionColeg)
                    .HasColumnType("date")
                    .HasColumnName("dLegDatosFechaEmisionColeg");

                entity.Property(e => e.DLegDatosFechaExpiraColeg)
                    .HasColumnType("date")
                    .HasColumnName("dLegDatosFechaExpiraColeg");

                entity.Property(e => e.NClaseGradoAcad).HasColumnName("nClaseGradoAcad");

                entity.Property(e => e.NClasePais).HasColumnName("nClasePais");

                entity.Property(e => e.NClaseSexo).HasColumnName("nClaseSexo");

                entity.Property(e => e.NClaseEstadoCivil).HasColumnName("nClaseEstadoCivil");

                entity.Property(e => e.NClaseTipoDoc).HasColumnName("nClaseTipoDoc");

                entity.Property(e => e.NClaseUbigeo).HasColumnName("nClaseUbigeo");

                entity.Property(e => e.NLegDatCondicionColeg).HasColumnName("nLegDatCondicionColeg");

                entity.Property(e => e.NLegDatGradoAcad).HasColumnName("nLegDatGradoAcad");

                entity.Property(e => e.NLegDatPais).HasColumnName("nLegDatPais");

                entity.Property(e => e.NLegDatSexo).HasColumnName("nLegDatSexo");

                entity.Property(e => e.NLegDatEstadoCivil).HasColumnName("nLegDatEstadoCivil");

                entity.Property(e => e.NLegDatTipoDoc).HasColumnName("nLegDatTipoDoc");

                entity.Property(e => e.NLegDatTipoDomicilio).HasColumnName("nLegDatTipoDomicilio");

                entity.Property(e => e.NLegDatZona).HasColumnName("nLegDatZona");

                entity.Property(e => e.NLetDatUbigeo).HasColumnName("nLetDatUbigeo");

                entity.Property(e => e.NValorCondicionColeg).HasColumnName("nValorCondicionColeg");

                entity.Property(e => e.NValorTipoDomicilio).HasColumnName("nValorTipoDomicilio");

                entity.Property(e => e.NValorZona).HasColumnName("nValorZona");

               
            });

            modelBuilder.Entity<LegDocenciaUniv>(entity =>
            {
                entity.HasKey(e => e.NLegDocCodigo)
                    .HasName("PK_LEGDOCENCIAUNIV");

                entity.ToTable("LegDocenciaUniv");

                entity.Property(e => e.NLegDocCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegDocCodigo");

                entity.Property(e => e.CLegDocArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegDocArchivo");

                entity.Property(e => e.CLegDocEstado)
                    .IsRequired()
                    .HasColumnName("cLegDocEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegDocUniversidad)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegDocUniversidad")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegDocPais).HasColumnName("cLegDocPais");

                entity.Property(e => e.CLegDocOtraInst).HasColumnName("cLegDocOtraInst");

                entity.Property(e => e.CLegDocValida)
                    .IsRequired()
                    .HasColumnName("cLegDocValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegDocFechaFin)
                    .HasColumnType("date")
                    .HasColumnName("dLegDocFechaFin");

                entity.Property(e => e.DLegDocFechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("dLegDocFechaInicio");

                entity.Property(e => e.NLegDocCategoria).HasColumnName("nLegDocCategoria");

                entity.Property(e => e.NLegDocDatCodigo).HasColumnName("nLegDocDatCodigo");

                entity.Property(e => e.NLegDocRegimen).HasColumnName("nLegDocRegimen");

                entity.Property(e => e.NValorCategoria).HasColumnName("nValorCategoria");

                entity.Property(e => e.NValorRegimen).HasColumnName("nValorRegimen");

                entity.HasOne(d => d.NLegDocDatCodigoNavigation)
                    .WithMany(p => p.LegDocenciaUniv)
                    .HasForeignKey(d => d.NLegDocDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegDocenciaUniv_fk5");
            });

            modelBuilder.Entity<LegGradoTitulo>(entity =>
            {
                entity.HasKey(e => e.NLegGraCodigo)
                    .HasName("PK_LEGGRADOTITULO");

                entity.ToTable("LegGradoTitulo");

                entity.Property(e => e.NLegGraCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegGraCodigo");

                entity.Property(e => e.CLegGraArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegGraArchivo");

                entity.Property(e => e.CLegGraEstado)
                    .IsRequired()
                    .HasColumnName("cLegGraEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegGraInstitucion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegGraInstitucion")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegGraValida)
                    .IsRequired()
                    .HasColumnName("cLegGraValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegGraFecha)
                    .HasColumnType("date")
                    .HasColumnName("dLegGraFecha");

                entity.Property(e => e.NClaseGradoAcad).HasColumnName("nClaseGradoAcad");

                entity.Property(e => e.NClasePais).HasColumnName("nClasePais");

                entity.Property(e => e.NClaseUbigeo).HasColumnName("nClaseUbigeo");

                entity.Property(e => e.CLegGraCarreraProf).HasColumnName("cLegGraCarreraProf");

                entity.Property(e => e.NLegGraDatCodigo).HasColumnName("nLegGraDatCodigo");

                entity.Property(e => e.NLegGraGradoAcad).HasColumnName("nLegGraGradoAcad");

                entity.Property(e => e.NLegGraPais).HasColumnName("nLegGraPais");

                entity.Property(e => e.CLegGraOtraInst).HasColumnName("cLegGraOtraInst");

                entity.Property(e => e.NLegGraUbigeo).HasColumnName("nLegGraUbigeo");

                entity.HasOne(d => d.NLegGraDatCodigoNavigation)
                    .WithMany(p => p.LegGradoTitulo)
                    .HasForeignKey(d => d.NLegGraDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegGradoTitulo_fk8");
            });

            modelBuilder.Entity<LegIdiomaOfimatica>(entity =>
            {
                entity.HasKey(e => e.NLegIdOfCodigo)
                    .HasName("PK_LEGIDIOMAOFIMATICA");

                entity.ToTable("LegIdiomaOfimatica");

                entity.Property(e => e.NLegIdOfCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegIdOfCodigo");

                entity.Property(e => e.CLegIdOfArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegIdOfArchivo");

                entity.Property(e => e.CLegIdOfEstado)
                    .IsRequired()
                    .HasColumnName("cLegIdOfEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegIdOfTipo)
                    .IsRequired()
                    .HasColumnName("cLegIdOfTipo")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegIdOfValida)
                    .IsRequired()
                    .HasColumnName("cLegIdOfValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegIdOfFecha)
                    .HasColumnType("date")
                    .HasColumnName("dLegIdOfFecha");

                entity.Property(e => e.NLegIdOfCodigoDesc).HasColumnName("nLegIdOfCodigoDesc");

                entity.Property(e => e.NLegIdOfDatCodigo).HasColumnName("nLegIdOfDatCodigo");

                entity.Property(e => e.NLegIdOfNivel).HasColumnName("nLegIdOfNivel");

                entity.Property(e => e.NValorDesc).HasColumnName("nValorDesc");

                entity.Property(e => e.NValorNivel).HasColumnName("nValorNivel");

                //entity.HasOne(d => d.CUsuModificaNavigation)
                //    .WithMany(p => p.LegIdiomaOfimaticaCUsuModificaNavigations)
                //    .HasForeignKey(d => d.CUsuModifica)
                //    .HasConstraintName("LegIdiomaOfimatica_fk6");

                //entity.HasOne(d => d.CUsuRegistroNavigation)
                //    .WithMany(p => p.LegIdiomaOfimaticaCUsuRegistroNavigations)
                //    .HasForeignKey(d => d.CUsuRegistro)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegIdiomaOfimatica_fk5");

                entity.HasOne(d => d.NLegIdOfDatCodigoNavigation)
                    .WithMany(p => p.LegIdiomaOfimatica)
                    .HasForeignKey(d => d.NLegIdOfDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegIdiomaOfimatica_fk4");

                //entity.HasOne(d => d.vCodigoDesc)
                //    .WithMany(p => p.LegIdiomaOfimaticaCodigoDesc)
                //    .HasForeignKey(d => new { d.NLegIdOfCodigoDesc, d.NValorDesc })
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegIdiomaOfimatica_fk0");

                //entity.HasOne(d => d.vNivel)
                //    .WithMany(p => p.LegIdiomaOfimaticaNivel)
                //    .HasForeignKey(d => new { d.NLegIdOfNivel, d.NValorNivel })
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegIdiomaOfimatica_fk2");
            });

            modelBuilder.Entity<LegInvestigador>(entity =>
            {
                entity.HasKey(e => e.NLegInvCodigo)
                    .HasName("PK_LEGINVESTIGADOR");

                entity.ToTable("LegInvestigador");

                entity.Property(e => e.NLegInvCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegInvCodigo");

                entity.Property(e => e.CLegInvArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegInvArchivo");

                entity.Property(e => e.CLegInvEstado)
                    .IsRequired()
                    .HasColumnName("cLegInvEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegInvNroRegistro)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cLegInvNroRegistro");

                entity.Property(e => e.CLegInvValida)
                    .IsRequired()
                    .HasColumnName("cLegInvValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegInvFechaFin)
                    .HasColumnType("date")
                    .HasColumnName("dLegInvFechaFin");

                entity.Property(e => e.DLegInvFechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("dLegInvFechaInicio");

                entity.Property(e => e.NLegInvCentroRegistro).HasColumnName("nLegInvCentroRegistro");

                entity.Property(e => e.NLegInvDatCodigo).HasColumnName("nLegInvDatCodigo");

                entity.Property(e => e.NValorCentroRegistro).HasColumnName("nValorCentroRegistro");


                entity.Property(e => e.NLegInvNivelRenacyt).HasColumnName("nLegInvNivelRenacyt");

                entity.Property(e => e.NValorNivelRenacyt).HasColumnName("nValorNivelRenacyt");

                //entity.HasOne(d => d.CUsuModificaNavigation)
                //    .WithMany(p => p.LegInvestigadorCUsuModificaNavigations)
                //    .HasForeignKey(d => d.CUsuModifica)
                //    .HasConstraintName("LegInvestigador_fk8");

                //entity.HasOne(d => d.CUsuRegistroNavigation)
                //    .WithMany(p => p.LegInvestigadorCUsuRegistroNavigations)
                //    .HasForeignKey(d => d.CUsuRegistro)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegInvestigador_fk7");

                entity.HasOne(d => d.NLegInvDatCodigoNavigation)
                    .WithMany(p => p.LegInvestigador)
                    .HasForeignKey(d => d.NLegInvDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegInvestigador_fk6");

                //entity.HasOne(d => d.vCentroRegistro)
                //    .WithMany(p => p.LegInvestigadorCentroRegistro)
                //    .HasForeignKey(d => new { d.NLegInvCentroRegistro, d.NValorCentroRegistro })
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegInvestigador_fk0");
            });

            modelBuilder.Entity<LegParticipacionCongSem>(entity =>
            {
                entity.HasKey(e => e.NLegParCodigo)
                    .HasName("PK_LEGPARTICIPACIONCONGSEM");

                entity.ToTable("LegParticipacionCongSem");

                entity.Property(e => e.NLegParCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegParCodigo");

                entity.Property(e => e.CLegParArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegParArchivo");

                entity.Property(e => e.CLegParEstado)
                    .IsRequired()
                    .HasColumnName("cLegParEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegParInstitucion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegParInstitucion")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegParPais).HasColumnName("cLegParPais");

                entity.Property(e => e.CLegParOtraInst).HasColumnName("cLegParOtraInst");

                entity.Property(e => e.CLegParNombre)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("cLegParNombre");

                entity.Property(e => e.CLegParValida)
                    .IsRequired()
                    .HasColumnName("cLegParValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegParFecha)
                    .HasColumnType("date")
                    .HasColumnName("dLegParFecha");

                entity.Property(e => e.DLegParFechaFin)
                    .HasColumnType("date")
                    .HasColumnName("dLegParFechaFin");

                entity.Property(e => e.CLegParArchivoOrig)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegParArchivoOrig");


                entity.Property(e => e.NLegParHoras).HasColumnName("nLegParHoras");


                entity.Property(e => e.NLegParAmbito).HasColumnName("nLegParAmbito");

                entity.Property(e => e.NLegParDatCodigo).HasColumnName("nLegParDatCodigo");

                entity.Property(e => e.NLegParRol).HasColumnName("nLegParRol");

                entity.Property(e => e.NValorAmbito).HasColumnName("nValorAmbito");

                entity.Property(e => e.NValorRol).HasColumnName("nValorRol");

                //entity.HasOne(d => d.CLegParInstitucionNavigation)
                //    .WithMany(p => p.LegParticipacionCongSemUnivers)
                //    .HasForeignKey(d => d.CLegParInstitucion)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegParticipacionCongSem_fk0");

                //entity.HasOne(d => d.CUsuModificaNavigation)
                //    .WithMany(p => p.LegParticipacionCongSemCUsuModificaNavigations)
                //    .HasForeignKey(d => d.CUsuModifica)
                //    .HasConstraintName("LegParticipacionCongSem_fk7");

                //entity.HasOne(d => d.CUsuRegistroNavigation)
                //    .WithMany(p => p.LegParticipacionCongSemCUsuRegistroNavigations)
                //    .HasForeignKey(d => d.CUsuRegistro)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegParticipacionCongSem_fk6");

                entity.HasOne(d => d.NLegParDatCodigoNavigation)
                    .WithMany(p => p.LegParticipacionCongSem)
                    .HasForeignKey(d => d.NLegParDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegParticipacionCongSem_fk5");

                //entity.HasOne(d => d.vAmbito)
                //    .WithMany(p => p.LegParticipacionCongSemAmbito)
                //    .HasForeignKey(d => new { d.NLegParAmbito, d.NValorAmbito })
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegParticipacionCongSem_fk3");

                //entity.HasOne(d => d.vRol)
                //    .WithMany(p => p.LegParticipacionCongSemRol)
                //    .HasForeignKey(d => new { d.NLegParRol, d.NValorRol })
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegParticipacionCongSem_fk1");
            });

            modelBuilder.Entity<LegProduccionCiencia>(entity =>
            {
                entity.HasKey(e => e.NLegProdCodigo)
                    .HasName("PK_LEGPRODUCCIONCIENCIA");

                entity.Property(e => e.NLegProdCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegProdCodigo");

                entity.Property(e => e.CLegProdArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegProdArchivo");

                entity.Property(e => e.CLegProdEstado)
                    .IsRequired()
                    .HasColumnName("cLegProdEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegProdNroResolucion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cLegProdNroResolucion");

                entity.Property(e => e.CLegProdTitulo)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("cLegProdTitulo");

                entity.Property(e => e.CLegProdValida)
                    .IsRequired()
                    .HasColumnName("cLegProdValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegProdFecha)
                    .HasColumnType("date")
                    .HasColumnName("dLegProdFecha");

                entity.Property(e => e.NLegProdDatCodigo).HasColumnName("nLegProdDatCodigo");

                entity.Property(e => e.NLegProdTipo).HasColumnName("nLegProdTipo");

                entity.Property(e => e.NValorTipo).HasColumnName("nValorTipo");

                //entity.HasOne(d => d.CUsuModificaNavigation)
                //    .WithMany(p => p.LegProduccionCienciaCUsuModificaNavigations)
                //    .HasForeignKey(d => d.CUsuModifica)
                //    .HasConstraintName("LegProduccionCiencia_fk4");

                //entity.HasOne(d => d.CUsuRegistroNavigation)
                //    .WithMany(p => p.LegProduccionCienciaCUsuRegistroNavigations)
                //    .HasForeignKey(d => d.CUsuRegistro)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegProduccionCiencia_fk3");

                entity.HasOne(d => d.NLegProdDatCodigoNavigation)
                    .WithMany(p => p.LegProduccionCiencia)
                    .HasForeignKey(d => d.NLegProdDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegProduccionCiencia_fk2");

                //entity.HasOne(d => d.vTipo)
                //    .WithMany(p => p.LegProduccionCienciaTipo)
                //    .HasForeignKey(d => new { d.NLegProdTipo, d.NValorTipo })
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegProduccionCiencia_fk0");
            });

            modelBuilder.Entity<LegProfesNoDocente>(entity =>
            {
                entity.HasKey(e => e.NLegProCodigo)
                    .HasName("PK_LEGPROFESNODOCENTE");

                entity.ToTable("LegProfesNoDocente");

                entity.Property(e => e.NLegProCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegProCodigo");

                entity.Property(e => e.CLegProArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegProArchivo");

                entity.Property(e => e.CLegProEstado).HasColumnName("cLegProEstado");

                entity.Property(e => e.CLegProCargoProf).HasColumnName("cLegProCargoProf");

                entity.Property(e => e.CLegProInstitucion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegProInstitucion")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegProPais).HasColumnName("cLegProPais");

                entity.Property(e => e.CLegProOtraInst).HasColumnName("cLegProOtraInst");

                entity.Property(e => e.CLegProValida)
                    .IsRequired()
                    .HasColumnName("cLegProValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegProFechaFin)
                    .HasColumnType("date")
                    .HasColumnName("dLegProFechaFin");

                entity.Property(e => e.DLegProFechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("dLegProFechaInicio");

                entity.Property(e => e.NLegProCargo).HasColumnName("nLegProCargo");

                entity.Property(e => e.NLegProDatCodigo).HasColumnName("nLegProDatCodigo");

                entity.Property(e => e.NValorCargo).HasColumnName("nValorCargo");

                //entity.HasOne(d => d.CLegProInstitucionNavigation)
                //    .WithMany(p => p.LegProfesNoDocenteUnivers)
                //    .HasForeignKey(d => d.CLegProInstitucion)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegProfesNoDocente_fk0");

                //entity.HasOne(d => d.CUsuModificaNavigation)
                //    .WithMany(p => p.LegProfesNoDocenteCUsuModificaNavigations)
                //    .HasForeignKey(d => d.CUsuModifica)
                //    .HasConstraintName("LegProfesNoDocente_fk5");

                //entity.HasOne(d => d.CUsuRegistroNavigation)
                //    .WithMany(p => p.LegProfesNoDocenteCUsuRegistroNavigations)
                //    .HasForeignKey(d => d.CUsuRegistro)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegProfesNoDocente_fk4");

                entity.HasOne(d => d.NLegProDatCodigoNavigation)
                    .WithMany(p => p.LegProfesNoDocente)
                    .HasForeignKey(d => d.NLegProDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegProfesNoDocente_fk3");

                //entity.HasOne(d => d.vCargo)
                //    .WithMany(p => p.LegProfesNoDocenteCargo)
                //    .HasForeignKey(d => new { d.NLegProCargo, d.NValorCargo })
                //    .HasConstraintName("LegProfesNoDocente_fk1");
            });

            modelBuilder.Entity<LegProyeccionSocial>(entity =>
            {
                entity.HasKey(e => e.NLegProyCodigo)
                    .HasName("PK_LEGPROYECCIONSOCIAL");

                entity.ToTable("LegProyeccionSocial");

                entity.Property(e => e.NLegProyCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegProyCodigo");

                entity.Property(e => e.CLegProyArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegProyArchivo");

                entity.Property(e => e.CLegProyDescripcion)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("cLegProyDescripcion");

                entity.Property(e => e.CLegProyEstado)
                    .IsRequired()
                    .HasColumnName("cLegProyEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegProyInstitucion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegProyInstitucion")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegProyPais)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("cLegProyPais")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegProyOtraInst).HasColumnName("cLegProyOtraInst");

                entity.Property(e => e.CLegProyValida)
                    .IsRequired()
                    .HasColumnName("cLegProyValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegProyFechaFin)
                    .HasColumnType("date")
                    .HasColumnName("dLegProyFechaFin");

                entity.Property(e => e.DLegProyFechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("dLegProyFechaInicio");

                entity.Property(e => e.NLegProyDatCodigo).HasColumnName("nLegProyDatCodigo");

                entity.Property(e => e.NLegProyTipo).HasColumnName("nLegProyTipo");

                entity.Property(e => e.NValorTipo).HasColumnName("nValorTipo");

                //entity.HasOne(d => d.CLegProyInstitucionNavigation)
                //    .WithMany(p => p.LegProyeccionSocialUnivers)
                //    .HasForeignKey(d => d.CLegProyInstitucion)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegProyeccionSocial_fk0");

                //entity.HasOne(d => d.CUsuModificaNavigation)
                //    .WithMany(p => p.LegProyeccionSocialCUsuModificaNavigations)
                //    .HasForeignKey(d => d.CUsuModifica)
                //    .HasConstraintName("LegProyeccionSocial_fk5");

                //entity.HasOne(d => d.CUsuRegistroNavigation)
                //    .WithMany(p => p.LegProyeccionSocialCUsuRegistroNavigations)
                //    .HasForeignKey(d => d.CUsuRegistro)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegProyeccionSocial_fk4");

                entity.HasOne(d => d.NLegProyDatCodigoNavigation)
                    .WithMany(p => p.LegProyeccionSocial)
                    .HasForeignKey(d => d.NLegProyDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegProyeccionSocial_fk3");

                //entity.HasOne(d => d.vTipo)
                //    .WithMany(p => p.LegProyeccionSocialTipo)
                //    .HasForeignKey(d => new { d.NLegProyTipo, d.NValorTipo })
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegProyeccionSocial_fk1");
            });

            modelBuilder.Entity<LegReconocimiento>(entity =>
            {
                entity.HasKey(e => e.NLegRecCodigo)
                    .HasName("PK_LEGRECONOCIMIENTO");

                entity.ToTable("LegReconocimiento");

                entity.Property(e => e.NLegRecCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegRecCodigo");

                entity.Property(e => e.CLegRecArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegRecArchivo");

                entity.Property(e => e.CLegRecEstado)
                    .IsRequired()
                    .HasColumnName("cLegRecEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegRecInstitucion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegRecInstitucion")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegRecPais)
                    .HasMaxLength(6)
                    .IsUnicode(false)
                    .HasColumnName("cLegRecPais")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegRecOtraInst).HasColumnName("cLegRecOtraInst");

                entity.Property(e => e.CLegRecValida).HasColumnName("cLegRecValida");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegRecFecha)
                    .HasColumnType("date")
                    .HasColumnName("dLegRecFecha");

                entity.Property(e => e.NLegRecDatCodigo).HasColumnName("nLegRecDatCodigo");

                entity.Property(e => e.NLegRecDocumento).HasColumnName("nLegRecDocumento");

                entity.Property(e => e.NLegRecTipo).HasColumnName("nLegRecTipo");

                entity.Property(e => e.NValorDocumento).HasColumnName("nValorDocumento");

                entity.Property(e => e.NValorTipo).HasColumnName("nValorTipo");

                //entity.HasOne(d => d.CLegRecInstitucionNavigation)
                //    .WithMany(p => p.LegReconocimientoUnivers)
                //    .HasForeignKey(d => d.CLegRecInstitucion)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegReconocimiento_fk2");

                //entity.HasOne(d => d.CUsuModificaNavigation)
                //    .WithMany(p => p.LegReconocimientoCUsuModificaNavigations)
                //    .HasForeignKey(d => d.CUsuModifica)
                //    .HasConstraintName("LegReconocimiento_fk5");

                //entity.HasOne(d => d.CUsuRegistroNavigation)
                //    .WithMany(p => p.LegReconocimientoCUsuRegistroNavigations)
                //    .HasForeignKey(d => d.CUsuRegistro)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegReconocimiento_fk4");

                entity.HasOne(d => d.NLegRecDatCodigoNavigation)
                    .WithMany(p => p.LegReconocimiento)
                    .HasForeignKey(d => d.NLegRecDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegReconocimiento_fk3");

                //entity.HasOne(d => d.vDocumento)
                //    .WithMany(p => p.LegReconocimientoDocumento)
                //    .HasForeignKey(d => new { d.NLegRecDocumento, d.NValorDocumento })
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegReconocimiento_fk0");

                //entity.HasOne(d => d.vTipo)
                //    .WithMany(p => p.LegReconocimientoTipo)
                //    .HasForeignKey(d => new { d.NLegRecTipo, d.NValorTipo })
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("LegReconocimiento_fk1");
            });

            modelBuilder.Entity<LegRegimenDedicacion>(entity =>
            {
                entity.HasKey(e => e.NLegRegCodigo)
                    .HasName("PK_LEGREGIMENDEDICACION");

                entity.ToTable("LegRegimenDedicacion");

                entity.Property(e => e.NLegRegCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegRegCodigo");

                entity.Property(e => e.CLegCatInstitucion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegCatInstitucion")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegRegPais).HasColumnName("cLegRegPais");

                entity.Property(e => e.CLegRegOtraInst).HasColumnName("cLegRegOtraInst");

                entity.Property(e => e.CLegRegArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegRegArchivo");

                entity.Property(e => e.CLegRegEstado).HasColumnName("cLegRegEstado");

                entity.Property(e => e.CLegRegValida)
                    .IsRequired()
                    .HasColumnName("cLegRegValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegRegFechaFin)
                    .HasColumnType("date")
                    .HasColumnName("dLegRegFechaFin");

                entity.Property(e => e.DLegRegFechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("dLegRegFechaInicio");

                entity.Property(e => e.NLegRegDatCodigo).HasColumnName("nLegRegDatCodigo");

                entity.Property(e => e.NLegRegDedicacion).HasColumnName("nLegRegDedicacion");

                entity.Property(e => e.NValorDedicacion).HasColumnName("nValorDedicacion");


                entity.HasOne(d => d.NLegRegDatCodigoNavigation)
                    .WithMany(p => p.LegRegimenDedicacion)
                    .HasForeignKey(d => d.NLegRegDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegRegimenDedicacion_fk3");
            });

            modelBuilder.Entity<LegTesisAseJur>(entity =>
            {
                entity.HasKey(e => e.NLegTesCodigo)
                    .HasName("PK_LEGTESISASEJUR");

                entity.ToTable("LegTesisAseJur");

                entity.Property(e => e.NLegTesCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegTesCodigo");

                entity.Property(e => e.CLegTesArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegTesArchivo");

                // Nuevos campos para obtener el Pais, Insitucion (lugar) de la actividad - EBS 01/2026
                entity.Property(e => e.NLegTesPais).HasColumnName("nLegTesPais").HasDefaultValue(0); // Valor por defecto
                entity.Property(e => e.NClasePais).HasColumnName("nClasePais").HasDefaultValue(0); // Valor por defecto
                entity.Property(e => e.CLegTesInstitucion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegTesInstitucion")
                    .IsFixedLength(true)
                    .HasDefaultValue("");
                entity.Property(e => e.CLegTesOtraInst)
                .HasColumnName("cLegTesOtraInst")
                .HasDefaultValue("");
                // Nuevos campos para obtener el Pais, Insitucion (lugar) de la actividad - EBS 01/2026

                entity.Property(e => e.CLegTesEstado)
                    .IsRequired()
                    .HasColumnName("cLegTesEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegTesNroResolucion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cLegTesNroResolucion");

                entity.Property(e => e.CLegTesValida)
                    .IsRequired()
                    .HasColumnName("cLegTesValida")
                    .HasDefaultValueSql("('false')");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegTesFecha)
                    .HasColumnType("date")
                    .HasColumnName("dLegTesFecha");

                entity.Property(e => e.NLegTesDatCodigo).HasColumnName("nLegTesDatCodigo");

                entity.Property(e => e.NLegTesNivel).HasColumnName("nLegTesNivel");

                entity.Property(e => e.NLegTesTipo).HasColumnName("nLegTesTipo");

                entity.Property(e => e.NValorNivel).HasColumnName("nValorNivel");

                entity.Property(e => e.NValorTipo).HasColumnName("nValorTipo");


                entity.HasOne(d => d.NLegTesDatCodigoNavigation)
                    .WithMany(p => p.LegTesisAseJur)
                    .HasForeignKey(d => d.NLegTesDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegTesisAseJur_fk4");
            });

            modelBuilder.Entity<PerUsuario>(entity =>
            {
                entity.HasKey(e => e.CPerCodigo)
                    .HasName("PK_PERUSUARIO");

                entity.ToTable("BDSipan.PerUsuario");

                entity.Property(e => e.CPerCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cPerCodigo")
                    .IsFixedLength(true);

                entity.Property(e => e.CPerJuridica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cPerJuridica")
                    .IsFixedLength(true);

                entity.Property(e => e.CPerUsuClave)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("cPerUsuClave");

                entity.Property(e => e.CPerUsuCodigo)
                    .IsRequired()
                    .HasMaxLength(15)
                    .IsUnicode(false)
                    .HasColumnName("cPerUsuCodigo");

                entity.Property(e => e.CPerUsuEstado)
                    .HasColumnName("cPerUsuEstado")
                    .HasDefaultValueSql("('1')");

                entity.Property(e => e.CPudFecha)
                    .HasColumnType("datetime")
                    .HasColumnName("cPudFecha");

                //entity.HasOne(d => d.CPerCodigoNavigation)
                //    .WithOne(p => p.PerUsuario)
                //    .HasForeignKey<PerUsuario>(d => d.CPerCodigo)
                //    .OnDelete(DeleteBehavior.ClientSetNull)
                //    .HasConstraintName("PerUsuario_fk0");
            });

            modelBuilder.Entity<Persona>(entity =>
            {
                entity.HasKey(e => e.CPerCodigo)
                    .HasName("PK_PERSONA");

                entity.ToTable("BDSipan.Persona");

                entity.Property(e => e.CPerCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cPerCodigo")
                    .IsFixedLength(true);

                entity.Property(e => e.CPerApellPat)
                    .HasMaxLength(75)
                    .IsUnicode(false)
                    .HasColumnName("cPerApellPat");

                entity.Property(e => e.CPerApellido)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("cPerApellido");

                entity.Property(e => e.CPerNombre)
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("cPerNombre");

                entity.Property(e => e.CUbigeoCodigo)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("cUbigeoCodigo")
                    .IsFixedLength(true);

                entity.Property(e => e.Cperestadobiblio)
                    .HasMaxLength(1)
                    .IsUnicode(false)
                    .HasColumnName("cperestadobiblio")
                    .IsFixedLength(true);

                entity.Property(e => e.DPerNacimiento)
                    .HasColumnType("date")
                    .HasColumnName("dPerNacimiento");

                entity.Property(e => e.NPerEstado).HasColumnName("nPerEstado");

                entity.Property(e => e.NPerTipo).HasColumnName("nPerTipo");

                entity.Property(e => e.NUbiGeoCodigo).HasColumnName("nUbiGeoCodigo");
            });

            modelBuilder.Entity<UnidadOrganizacional>(entity =>
            {
                entity.HasKey(e => e.NUniOrgCodigo)
                    .HasName("PK_UNIDADORGANIZACIONAL");

                entity.ToTable("UnidadOrganizacional");

                entity.Property(e => e.NUniOrgCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nUniOrgCodigo");

                entity.Property(e => e.CPerApellido)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("cPerApellido");

                entity.Property(e => e.CPerJuridad)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cPerJuridad")
                    .IsFixedLength(true);

                entity.Property(e => e.CPerNombre)
                    .IsRequired()
                    .HasMaxLength(120)
                    .IsUnicode(false)
                    .HasColumnName("cPerNombre");

                entity.Property(e => e.CUniOrgAbrev)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("cUniOrgAbrev");

                entity.Property(e => e.CUniOrgCodigo)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("cUniOrgCodigo");

                entity.Property(e => e.CUniOrgNombre)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName("cUniOrgNombre");

                entity.Property(e => e.CUniOrgRelacion)
                    .IsRequired()
                    .HasMaxLength(25)
                    .HasColumnName("cUniOrgRelacion");

                entity.Property(e => e.NIntTipo).HasColumnName("nIntTipo");
            });

            modelBuilder.Entity<CapacitacionesUss>(entity =>
            {
                entity.HasKey(e => e.NCapCodigo)
                    .HasName("PK__Capacita__B08AAE3110E91F47");

                entity.ToTable("CapacitacionesUSS");

                entity.Property(e => e.NCapCodigo).HasColumnName("nCapCodigo");

                entity.Property(e => e.BCapEstado)
                    .HasColumnName("bCapEstado")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CCapTema)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("cCapTema");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DCapFechaFin)
                    .HasColumnType("datetime")
                    .HasColumnName("dCapFechaFin");

                entity.Property(e => e.DCapFechaInicio)
                    .HasColumnType("datetime")
                    .HasColumnName("dCapFechaInicio");

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NCapHoras).HasColumnName("nCapHoras");
            });

            modelBuilder.Entity<LegArchivos>(entity =>
            {
                entity.HasKey(e => e.NLegArcCodigo)
                    .HasName("PK__LegArchi__0C9DF1F926BB6246");

                entity.ToTable("LegArchivos");


                entity.Property(e => e.NLegArcCodigo).HasColumnName("nLegArcCodigo");


                entity.Property(e => e.CPerCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cPerCodigo")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegArcNombre)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegArcNombre");

                entity.Property(e => e.NLegArcTipo).HasColumnName("nLegArcTipo");

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaRegistro)
                   .HasColumnType("datetime")
                   .HasColumnName("dFechaRegistro")
                   .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");
            });

            modelBuilder.Entity<LegCapacitacionInterna>(entity =>
            {
                entity.HasKey(e => e.NLegCicodigo)
                    .HasName("PK__LegCapac__D6DC98BB274D3047");

                entity.ToTable("LegCapacitacionInterna");

                entity.Property(e => e.NLegCicodigo).HasColumnName("nLegCICodigo");

                entity.Property(e => e.BLegCiestado)
                    .HasColumnName("bLegCIEstado")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CLegCiarchivo)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegCIArchivo");

                entity.Property(e => e.CLegCicompetenciaMejora)
                    .HasColumnType("text")
                    .HasColumnName("cLegCICompetenciaMejora");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NCapCodigo).HasColumnName("nCapCodigo");

                entity.Property(e => e.NLegDatCodigo).HasColumnName("nLegDatCodigo");

                entity.HasOne(d => d.vCapacitacionUSS)
                    .WithMany(p => p.LegCapacitacionInternas)
                    .HasForeignKey(d => d.NCapCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegCapacitacionInterna_fk1");

                entity.HasOne(d => d.vDatosGenerales)
                    .WithMany(p => p.LegCapacitacionInternas)
                    .HasForeignKey(d => d.NLegDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegCapacitacionInterna_fk2");
            });

            modelBuilder.Entity<LegContrato>(entity =>
            {
                entity.HasKey(e => e.NLegConCodigo)
                    .HasName("PK_LEGCONTRATO");

                entity.ToTable("LegContrato");

                entity.Property(e => e.NLegConCodigo).HasColumnName("nLegConCodigo");

                entity.Property(e => e.BLegConEstado)
                    .HasColumnName("bLegConEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegConArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegConArchivo");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegConFechaFin)
                    .HasColumnType("date")
                    .HasColumnName("dLegConFechaFin");

                entity.Property(e => e.DLegConFechaInicio)
                    .HasColumnType("date")
                    .HasColumnName("dLegConFechaInicio");

                entity.Property(e => e.NLegConCargo).HasColumnName("nLegConCargo");

                entity.Property(e => e.NLegConArea).HasColumnName("nLegConArea");

                entity.Property(e => e.NLegConDatCodigo).HasColumnName("nLegConDatCodigo");

                entity.Property(e => e.NLegConSueldo)
                    .HasColumnType("decimal(10, 2)")
                    .HasColumnName("nLegConSueldo");

                entity.Property(e => e.NLegValArea).HasColumnName("nLegValArea");

                entity.HasOne(d => d.vDatosGenerales)
                    .WithMany(p => p.LegContratos)
                    .HasForeignKey(d => d.NLegConDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegContrato_fk1");
            });

            modelBuilder.Entity<LegEvaluacionDesemp>(entity =>
            {
                entity.HasKey(e => e.NLegEvalCodigo)
                    .HasName("PK_EVALDESEMPENIO");

                entity.ToTable("LegEvaluacionDesemp");

                entity.Property(e => e.NLegEvalCodigo).HasColumnName("nLegEvalCodigo");

                entity.Property(e => e.BLegEvalEstado)
                    .HasColumnName("bLegEvalEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegEvalArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegEvalArchivo");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.CLegEvalAnio).HasColumnName("cLegEvalAnio");

                entity.Property(e => e.NLegEvalPuntaje).HasColumnName("nLegEvalPuntaje");

                entity.Property(e => e.CLegEvalSemestre).HasColumnName("cLegEvalSemestre");

                entity.Property(e => e.NLegEvalArea).HasColumnName("nLegEvalArea");

                entity.Property(e => e.NLegEvalCargo).HasColumnName("nLegEvalCargo");

                entity.Property(e => e.NLegEvalDatCodigo).HasColumnName("nLegEvalDatCodigo");

                entity.Property(e => e.NLegValArea).HasColumnName("nLegValArea");

                entity.Property(e => e.NLegValCargo).HasColumnName("nLegValCargo");

                entity.HasOne(d => d.vDatosGenerales)
                    .WithMany(p => p.LegEvaluacionDesemp)
                    .HasForeignKey(d => d.NLegEvalDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegEvaluacionDesemp_fk1");
            });

            modelBuilder.Entity<LegResolucion>(entity =>
            {
                entity.HasKey(e => e.NLegResCodigo)
                    .HasName("PK_LEGRESOLUCION");

                entity.ToTable("LegResolucion");

                entity.Property(e => e.NLegResCodigo).HasColumnName("nLegResCodigo");

                entity.Property(e => e.BLegResEstado)
                    .HasColumnName("bLegResEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegResArchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegResArchivo");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegResFecha)
                    .HasColumnType("date")
                    .HasColumnName("dLegResFecha");

                entity.Property(e => e.CLegResNroResolucion)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .HasColumnName("cLegResNroResolucion");

                entity.Property(e => e.CLegResResuelve)
                    .IsRequired()
                    .HasColumnType("text")
                    .HasColumnName("cLegResResuelve");

                entity.Property(e => e.NLegResDatCodigo).HasColumnName("nLegResDatCodigo");

                entity.Property(e => e.NLegResTipo).HasColumnName("nLegResTipo");

                entity.Property(e => e.NLegValTipo).HasColumnName("nLegValTipo");

                entity.HasOne(d => d.vDatosGenerales)
                    .WithMany(p => p.LegResoluciones)
                    .HasForeignKey(d => d.NLegResDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegResolucion_fk1");
            });

            modelBuilder.Entity<LegOrdinarizacion>(entity =>
            {
                entity.HasKey(e => e.NLegOrdCodigo)
                    .HasName("PK_LEGORDINARIZACION");

                entity.ToTable("LegOrdinarizacion");

                entity.Property(e => e.NLegOrdCodigo).HasColumnName("nLegOrdCodigo");

                entity.Property(e => e.BLegOrdEstado)
                    .HasColumnName("bLegOrdEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegOrdClaseModelo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegOrdClaseModelo");

                entity.Property(e => e.CLegOrdEntrevistaPers)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegOrdEntrevistaPers");

                entity.Property(e => e.CLegOrdEvaluacionCv)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegOrdEvaluacionCV");

                entity.Property(e => e.CLegOrdEvaluacionPsico)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegOrdEvaluacionPsico");

                entity.Property(e => e.CLegOrdFichaInscripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegOrdFichaInscripcion");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegOrdFecha)
                    .HasColumnType("date")
                    .HasColumnName("dLegOrdFecha");

                entity.Property(e => e.NLegOrdArea).HasColumnName("nLegOrdArea");

                entity.Property(e => e.NLegOrdCargo).HasColumnName("nLegOrdCargo");

                entity.Property(e => e.NLegOrdDatCodigo).HasColumnName("nLegOrdDatCodigo");

                entity.Property(e => e.NLegOrdValArea).HasColumnName("nLegOrdValArea");

                entity.Property(e => e.NLegValCargo).HasColumnName("nLegValCargo");

                entity.HasOne(d => d.vDatosGenerales)
                    .WithMany(p => p.LegOrdinarizacion)
                    .HasForeignKey(d => d.NLegOrdDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegOrdinnarizacion_fk1");
            });

            modelBuilder.Entity<LegSeleccion>(entity =>
            {
                entity.HasKey(e => e.NLegSelCodigo)
                    .HasName("PK_LEGSELECCION");

                entity.ToTable("LegSeleccion");

                entity.Property(e => e.NLegSelCodigo).HasColumnName("nLegSelCodigo");

                entity.Property(e => e.BLegSelEstado)
                    .HasColumnName("bLegSelEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegSelClaseModelo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegSelClaseModelo");

                entity.Property(e => e.CLegSelEntrevistaPers)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegSelEntrevistaPers");

                entity.Property(e => e.CLegSelEvaluacionCv)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegSelEvaluacionCV");

                entity.Property(e => e.CLegSelEvaluacionPsico)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegSelEvaluacionPsico");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegSelFecha)
                    .HasColumnType("date")
                    .HasColumnName("dLegSelFecha");

                entity.Property(e => e.NLegSelArea).HasColumnName("nLegSelArea");

                entity.Property(e => e.NLegSelCargo).HasColumnName("nLegSelCargo");

                entity.Property(e => e.NLegSelDatCodigo).HasColumnName("nLegSelDatCodigo");

                entity.Property(e => e.NLegValArea).HasColumnName("nLegValArea");

                entity.Property(e => e.NLegValCargo).HasColumnName("nLegValCargo");

                entity.HasOne(d => d.vDatosGenerales)
                    .WithMany(p => p.LegSeleccion)
                    .HasForeignKey(d => d.NLegSelDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegSeleccion_fk1");
            });

            modelBuilder.Entity<LegDeclaracionJurada>(entity =>
            {
                entity.HasKey(e => e.NLegDjcodigo)
                    .HasName("PK_LEGDECLARACIONJURADA");

                entity.Property(e => e.NLegDjcodigo).HasColumnName("nLegDJCodigo");

                entity.Property(e => e.BLegDjestado)
                    .HasColumnName("bLegDJEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegDjanexo2)
                    //.IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegDJAnexo2");

                entity.Property(e => e.CLegDjanexo6)
                    //.IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegDJAnexo6");

                entity.Property(e => e.CLegDjanexo7)
                    //.IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegDJAnexo7");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DLegDjfecha)
                    .HasColumnType("date")
                    .HasColumnName("dLegDJFecha");

                // ------------------ EDGAR_BS-2025---------------------------------------->

                entity.Property(e => e.CLegDjanexo1)
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegDJAnexo1");

                entity.Property(e => e.CLegDjanexo2_2)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDJAnexo2_2");

                entity.Property(e => e.CLegDjanexo3)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDJAnexo3");

                entity.Property(e => e.CLegDjanexo4)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDJAnexo4");

                entity.Property(e => e.CLegDjanexo5)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDJAnexo5");

                entity.Property(e => e.CLegDjanexo6_2)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDJAnexo6_2");

                entity.Property(e => e.CLegDjDNI)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDJDNI");

                entity.Property(e => e.CLegDjDNI_DH)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDJDNI_DH");

                entity.Property(e => e.CLegDjFotoCarnet)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDJFotoCarnet");

                entity.Property(e => e.CLegDjNumCta)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDJNumCta");

                entity.Property(e => e.CLegDjConsJubilacion)
                   .HasMaxLength(100)
                   .IsUnicode(false)
                   .HasColumnName("cLegDJConsJubilacion");
                // ------------------ EDGAR_BS-2025---------------------------------------->



                entity.Property(e => e.NLegDjdatCodigo).HasColumnName("nLegDJDatCodigo");

                entity.HasOne(d => d.vDatosGenerales)
                    .WithMany(p => p.LegDeclaracionJurada)
                    .HasForeignKey(d => d.NLegDjdatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegDeclaracionJurada_fk1");
            });

            modelBuilder.Entity<LegDocumentacionInterna>(entity =>
            {
                entity.HasKey(e => e.NLegDicodigo)
                    .HasName("PK_LEGDOCUMENTACIONINTERNA");

                entity.ToTable("LegDocumentacionInterna");

                entity.Property(e => e.NLegDicodigo).HasColumnName("nLegDICodigo");

                entity.Property(e => e.BLegDiestado)
                    .HasColumnName("bLegDIEstado")
                    .HasDefaultValueSql("('true')");

                entity.Property(e => e.CLegDiarchivo)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cLegDIArchivo");

                entity.Property(e => e.CLegDicodigo)
                    .HasMaxLength(12)
                    .IsUnicode(false)
                    .HasColumnName("cLegDICodigo")
                    .IsFixedLength(true);

                entity.Property(e => e.CLegDidescripcion)
                    .HasMaxLength(150)
                    .IsUnicode(false)
                    .HasColumnName("cLegDIDescripcion");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NLegDidatCodigo).HasColumnName("nLegDIDatCodigo");

                entity.Property(e => e.NLegDitipoDoc).HasColumnName("nLegDITipoDoc");

                entity.Property(e => e.NLegValTipoDoc).HasColumnName("nLegValTipoDoc");

                entity.HasOne(d => d.vDatosGenerales)
                    .WithMany(p => p.LegDocumentacionInterna)
                    .HasForeignKey(d => d.NLegDidatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegDocumentacionInterna_fk1");
            });

            // EBS - 01/2026 ---------------------------->
            /* Entidad para registrar licencias o registros profesionales (colegios, nº colegiatura, condición, fechas, etc.) */
            modelBuilder.Entity<LegLicenciaProfesional>(entity =>
            {
                entity.HasKey(e => e.NLegLicCodigo)
                    .HasName("PK_LEGLICENCIAPROFESIONAL");

                entity.ToTable("LegLicenciaProfesional");

                entity.Property(e => e.NLegLicCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegLicCodigo");

                entity.Property(e => e.NLegLicPais).HasColumnName("nLegLicPais");
                entity.Property(e => e.NClasePais).HasColumnName("nClasePais");
                entity.Property(e => e.CLegLicInstitucion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegLicInstitucion")
                    .IsFixedLength(true)
                    .HasDefaultValue("");

                entity.Property(e => e.CLegLicOtraInst)
                .HasColumnName("cLegLicOtraInst")
                .HasDefaultValue("");

                entity.Property(e => e.CLegLicNroRegistro)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cLegLicNroRegistro");

                entity.Property(e => e.NLegLicCondicion).HasColumnName("nLegLicCondicion");

                entity.Property(e => e.NClaseCondicion).HasColumnName("nValorCondicion");

                entity.Property(e => e.DLegLicFechaEmision)
                    .HasColumnType("date")
                    .HasColumnName("dLegLicFechaEmision");

                entity.Property(e => e.DLegLicFechaExpiracion)
                    .HasColumnType("date")
                    .HasColumnName("dLegLicFechaExpiracion");

                entity.Property(e => e.CLegLicValida)
                    .IsRequired()
                    .HasColumnType("bit")
                    .HasColumnName("cLegLicValida")
                    .HasDefaultValueSql("('false')");
                entity.Property(e => e.CLegLicEstado)
                    .IsRequired()
                    .HasColumnType("bit")
                    .HasColumnName("cLegLicEstado")
                    .HasDefaultValueSql("('true')");
                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);
                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");
                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);
                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.NLegLicDatCodigo).HasColumnName("nLegLicDatCodigo");

                entity.HasOne(d => d.NLegLicDatCodigoNavigation)
                    .WithMany(p => p.LegLicenciaProfesional)
                    .HasForeignKey(d => d.NLegLicDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegLicenciaProfesional_fk1");
            });

            /* Entidad para registrar membresias (colegios, nº colegiatura, fechas, etc.) */
            modelBuilder.Entity<LegMembresia>(entity =>
            {
                entity.HasKey(e => e.NLegMemCodigo)
                    .HasName("PK_MEMBRESIA");

                entity.ToTable("LegMembresia");

                entity.Property(e => e.NLegMemCodigo)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("nLegMemCodigo");

                entity.Property(e => e.NLegMemPais).HasColumnName("nLegMemPais");
                entity.Property(e => e.NClasePais).HasColumnName("nClasePais");
                entity.Property(e => e.CLegMemInstitucion)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cLegMemInstitucion")
                    .IsFixedLength(true)
                    .HasDefaultValue("");

                entity.Property(e => e.CLegMemOtraInst)
                .HasColumnName("cLegMemOtraInst")
                .HasDefaultValue("");

                entity.Property(e => e.CLegMemNroRegistro)
                    .IsRequired()
                    .HasMaxLength(20)
                    .IsUnicode(false)
                    .HasColumnName("cLegMemNroRegistro");

                entity.Property(e => e.DLegMemFechaEmision)
                    .HasColumnType("date")
                    .HasColumnName("dLegMemFechaEmision");

                entity.Property(e => e.DLegMemFechaExpiracion)
                    .HasColumnType("date")
                    .HasColumnName("dLegMemFechaExpiracion");

                entity.Property(e => e.CLegMemValida)
                    .IsRequired()
                    .HasColumnType("bit")
                    .HasColumnName("cLegMemValida")
                    .HasDefaultValueSql("('false')");
                entity.Property(e => e.CLegMemEstado)
                    .IsRequired()
                    .HasColumnType("bit")
                    .HasColumnName("cLegMemEstado")
                    .HasDefaultValueSql("('true')");
                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);
                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");
                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);
                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.NLegMemDatCodigo).HasColumnName("nLegMemDatCodigo");

                entity.HasOne(d => d.NLegMemDatCodigoNavigation)
                    .WithMany(p => p.LegMembresia)
                    .HasForeignKey(d => d.NLegMemDatCodigo)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("LegMembresia_fk1");
            });
            // EBS - 01/2026 <----------------------------

            modelBuilder.Entity<ModulosTD>(entity =>
            {
                entity.HasKey(e => e.NModCodigo)
                    .HasName("PK__ModulosT__7D0701305B259421");

                entity.ToTable("ModulosTD");

                entity.Property(e => e.NModCodigo).HasColumnName("nModCodigo");

                entity.Property(e => e.BModEstado)
                    .HasColumnName("bModEstado")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CModDescripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cModDescripcion");

                entity.Property(e => e.CModRuta)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("cModRuta");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NConCodigo).HasColumnName("nConCodigo");

                entity.Property(e => e.NConValor).HasColumnName("nConValor");

                //entity.HasOne(d => d.vModulo)
                //    .WithMany(p => p.ModulosTD)
                //    .HasForeignKey(d => new { d.NConCodigo, d.NConValor })
                //    .HasConstraintName("ModulosTD_fk1");
            });

            modelBuilder.Entity<TareaModulo>(entity =>
            {
                entity.HasKey(e => e.NTarModCodigo)
                    .HasName("PK__TareaMod__BB57A1C3ADE24AB6");

                entity.ToTable("TareaModulo");

                entity.Property(e => e.NTarModCodigo).HasColumnName("nTarModCodigo");

                entity.Property(e => e.BTarModEstado)
                    .HasColumnName("bTarModEstado")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CTarModDescripcion)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false)
                    .HasColumnName("cTarModDescripcion");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NModCodigo).HasColumnName("nModCodigo");

                entity.HasOne(d => d.vModulo)
                    .WithMany(p => p.vTareaModulos)
                    .HasForeignKey(d => d.NModCodigo)
                    .HasConstraintName("TareaModulo_fk1");
            });

            modelBuilder.Entity<PermisosModulo>(entity =>
            {
                entity.HasKey(e => e.NPrmModCodigo)
                    .HasName("PK__Permisos__5D42A179546EED6F");

                entity.ToTable("PermisosModulo");

                entity.Property(e => e.NPrmModCodigo).HasColumnName("nPrmModCodigo");

                entity.Property(e => e.BPrmModAdministrador)
                    .HasColumnName("bPrmModAdministrador")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.BPrmModAlcance)
                    .HasColumnName("bPrmModAlcance")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.BPrmModEstado)
                    .HasColumnName("bPrmModEstado")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CPerCodigo)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cPerCodigo")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NModCodigo).HasColumnName("nModCodigo");

                //entity.HasOne(d => d.vPersona)
                //    .WithMany(p => p.vPermisosModulos)
                //    .HasForeignKey(d => d.CPerCodigo)
                //    .HasConstraintName("Permisos_fk2");

                entity.HasOne(d => d.vModulos)
                    .WithMany(p => p.vPermisosModulos)
                    .HasForeignKey(d => d.NModCodigo)
                    .HasConstraintName("Permisos_fk1");
            });

            modelBuilder.Entity<PermisosTarea>(entity =>
            {
                entity.HasKey(e => e.NPrmTarCodigo)
                    .HasName("PK__Permisos__63E05C12800201B2");

                entity.ToTable("PermisosTarea");

                entity.Property(e => e.NPrmTarCodigo).HasColumnName("nPrmTarCodigo");

                entity.Property(e => e.BPrmEstado)
                    .HasColumnName("bPrmEstado")
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.CUsuModifica)
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuModifica")
                    .IsFixedLength(true);

                entity.Property(e => e.CUsuRegistro)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsUnicode(false)
                    .HasColumnName("cUsuRegistro")
                    .IsFixedLength(true);

                entity.Property(e => e.DFechaModifica)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaModifica");

                entity.Property(e => e.DFechaRegistro)
                    .HasColumnType("datetime")
                    .HasColumnName("dFechaRegistro")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.NPrmModCodigo).HasColumnName("nPrmModCodigo");

                entity.Property(e => e.NTarModCodigo).HasColumnName("nTarModCodigo");

                entity.HasOne(d => d.vPermisosModulo)
                    .WithMany(p => p.vPermisosTareas)
                    .HasForeignKey(d => d.NPrmModCodigo)
                    .HasConstraintName("PermisosTarea_fk1");

                entity.HasOne(d => d.vTareaModulo)
                    .WithMany(p => p.vPermisosTareas)
                    .HasForeignKey(d => d.NPrmTarCodigo)
                    .HasConstraintName("PermisosTarea_fk2");
            });

            modelBuilder.Entity<LegGrupInvSem>(entity =>
            {
                entity.HasKey(e => e.nLegLidGrupInvSem)
                   .HasName("PK__LegGrupInvSem__63E05C12800201B2");

                entity.ToTable("LegGrupInvSem");
            });


           OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
