using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace WebApiCV.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarreraProfesional",
                columns: table => new
                {
                    nCarId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cCarNombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarreraProfesional", x => x.nCarId);
                });

            migrationBuilder.CreateTable(
                name: "GradoAcademico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cGacNombre = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GradoAcademico", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Institucion",
                columns: table => new
                {
                    nInsId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cInsNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cInsAcronimo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cInsFundacion = table.Column<int>(type: "int", nullable: false),
                    cInsSede = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Institucion", x => x.nInsId);
                });

            migrationBuilder.CreateTable(
                name: "RecordPostulantes",
                columns: table => new
                {
                    nCurId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cCurDni = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cCurApellidPaterno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cCurApellidMaterno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cCurNombres = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    dCurFechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RecordPostulantes", x => x.nCurId);
                });

            migrationBuilder.CreateTable(
                name: "Ubigeo",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    cUbiDepartamento = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cUbiProvincia = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cUbiDistrito = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ubigeo", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Curriculo",
                columns: table => new
                {
                    nCurId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    cCurDni = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cCurApellidPaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cCurApellidMaterno = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cCurNombres = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    dCurFechaNacimiento = table.Column<DateTime>(type: "datetime2", nullable: false),
                    cCurEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cCurMovil = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    cCurTelefono = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cCurFoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    cCurAcerca = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nCarId = table.Column<int>(type: "int", nullable: true),
                    nGacId = table.Column<int>(type: "int", nullable: false),
                    cUbiId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    nEstado = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Curriculo", x => x.nCurId);
                    table.ForeignKey(
                        name: "FK_Curriculo_CarreraProfesional_nCarId",
                        column: x => x.nCarId,
                        principalTable: "CarreraProfesional",
                        principalColumn: "nCarId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Curriculo_GradoAcademico_nGacId",
                        column: x => x.nGacId,
                        principalTable: "GradoAcademico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Curriculo_Ubigeo_cUbiId",
                        column: x => x.cUbiId,
                        principalTable: "Ubigeo",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Formacion",
                columns: table => new
                {
                    nForId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    nForAnioInicio = table.Column<int>(type: "int", nullable: false),
                    nForAnioFin = table.Column<int>(type: "int", nullable: false),
                    cForDiplomaUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    nForEstado = table.Column<bool>(type: "bit", nullable: false),
                    nForInstitucionId = table.Column<long>(type: "bigint", nullable: false),
                    nForCarreraProfesionalId = table.Column<int>(type: "int", nullable: false),
                    nForGradoAcademicoId = table.Column<int>(type: "int", nullable: false),
                    nForCurriculoId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Formacion", x => x.nForId);
                    table.ForeignKey(
                        name: "FK_Formacion_CarreraProfesional_nForCarreraProfesionalId",
                        column: x => x.nForCarreraProfesionalId,
                        principalTable: "CarreraProfesional",
                        principalColumn: "nCarId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Formacion_Curriculo_nForCurriculoId",
                        column: x => x.nForCurriculoId,
                        principalTable: "Curriculo",
                        principalColumn: "nCurId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Formacion_GradoAcademico_nForGradoAcademicoId",
                        column: x => x.nForGradoAcademicoId,
                        principalTable: "GradoAcademico",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Formacion_Institucion_nForInstitucionId",
                        column: x => x.nForInstitucionId,
                        principalTable: "Institucion",
                        principalColumn: "nInsId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Curriculo_cUbiId",
                table: "Curriculo",
                column: "cUbiId");

            migrationBuilder.CreateIndex(
                name: "IX_Curriculo_nCarId",
                table: "Curriculo",
                column: "nCarId");

            migrationBuilder.CreateIndex(
                name: "IX_Curriculo_nGacId",
                table: "Curriculo",
                column: "nGacId");

            migrationBuilder.CreateIndex(
                name: "IX_Formacion_nForCarreraProfesionalId",
                table: "Formacion",
                column: "nForCarreraProfesionalId");

            migrationBuilder.CreateIndex(
                name: "IX_Formacion_nForCurriculoId",
                table: "Formacion",
                column: "nForCurriculoId");

            migrationBuilder.CreateIndex(
                name: "IX_Formacion_nForGradoAcademicoId",
                table: "Formacion",
                column: "nForGradoAcademicoId");

            migrationBuilder.CreateIndex(
                name: "IX_Formacion_nForInstitucionId",
                table: "Formacion",
                column: "nForInstitucionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Formacion");

            migrationBuilder.DropTable(
                name: "RecordPostulantes");

            migrationBuilder.DropTable(
                name: "Curriculo");

            migrationBuilder.DropTable(
                name: "Institucion");

            migrationBuilder.DropTable(
                name: "CarreraProfesional");

            migrationBuilder.DropTable(
                name: "GradoAcademico");

            migrationBuilder.DropTable(
                name: "Ubigeo");
        }
    }
}
