using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DocManager.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class droppedTablesAndCreatedAgain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "Medico",
                columns: table => new
                {
                    medico_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    medico_nombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    medico_telefono = table.Column<int>(type: "int", maxLength: 10, nullable: false),
                    medico_correo = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Medico", x => x.medico_id);
                });

            

            migrationBuilder.CreateTable(
                name: "Paciente",
                columns: table => new
                {
                    paciente_id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    paciente_nombreCompleto = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paciente_fechaNac = table.Column<DateOnly>(type: "date", nullable: false),
                    paciente_estadoCivil = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paciente_direccion = table.Column<string>(type: "nvarchar(80)", maxLength: 80, nullable: false),
                    paciente_telefono = table.Column<int>(type: "int", nullable: false),
                    paciente_correoElectronico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    paciente_DNI = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    medico_id = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Paciente", x => x.paciente_id);
                    table.ForeignKey(
                        name: "FK_Paciente_Medico_medico_id",
                        column: x => x.medico_id,
                        principalTable: "Medico",
                        principalColumn: "medico_id",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateIndex(
                name: "IX_Paciente_medico_id",
                table: "Paciente",
                column: "medico_id");
        }

        /// <inheritdoc />

    }
}
