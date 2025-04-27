using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFX.Core.DB.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ESCxContabilista",
                columns: table => new
                {
                    ESCxContabilistaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CRC = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESCxContabilista", x => x.ESCxContabilistaID);
                    table.ForeignKey(
                        name: "FK_016960BD2E3F483B996E31532D8B0BE2",
                        column: x => x.ESCxContabilistaID,
                        principalTable: "CORxPessoa",
                        principalColumn: "CORxPessoaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ESCxEscritorio",
                columns: table => new
                {
                    ESCxEscritorioID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESCxEscritorio", x => x.ESCxEscritorioID);
                    table.ForeignKey(
                        name: "FK_4A3824E876B5459E8901E1C2D47FB104",
                        column: x => x.ESCxEscritorioID,
                        principalTable: "CORxAgregado",
                        principalColumn: "CORxAgregadoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ESCxEmpresa",
                columns: table => new
                {
                    ESCxEmpresaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ESCxContabilistaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESCxEmpresa", x => x.ESCxEmpresaID);
                    table.ForeignKey(
                        name: "FK_B41CDCE03FE9462798A4F2E3AEFF7442",
                        column: x => x.ESCxEmpresaID,
                        principalTable: "CORxEmpresa",
                        principalColumn: "CORxEmpresaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FCAEEE2632074900A104B57043F6C318",
                        column: x => x.ESCxContabilistaID,
                        principalTable: "ESCxContabilista",
                        principalColumn: "ESCxContabilistaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ESCxContabilistaEscritorio",
                columns: table => new
                {
                    ESCxContabilistaEscritorioID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ESCxContabilistaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ESCxEscritorioID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ESCxContabilistaEscritorio", x => x.ESCxContabilistaEscritorioID);
                    table.ForeignKey(
                        name: "FK_0BD4C3A6E5B349649102390CF008FE7D",
                        column: x => x.ESCxEscritorioID,
                        principalTable: "ESCxEscritorio",
                        principalColumn: "ESCxEscritorioID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EFCC8459492A4DA1A909F4C0AAA6AC6A",
                        column: x => x.ESCxContabilistaID,
                        principalTable: "ESCxContabilista",
                        principalColumn: "ESCxContabilistaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_016960BD2E3F483B996E31532D8B0BE2",
                table: "ESCxContabilista",
                column: "ESCxContabilistaID");

            migrationBuilder.CreateIndex(
                name: "IX_0BD4C3A6E5B349649102390CF008FE7D",
                table: "ESCxContabilistaEscritorio",
                column: "ESCxEscritorioID");

            migrationBuilder.CreateIndex(
                name: "IX_EFCC8459492A4DA1A909F4C0AAA6AC6A",
                table: "ESCxContabilistaEscritorio",
                column: "ESCxContabilistaID");

            migrationBuilder.CreateIndex(
                name: "IX_B41CDCE03FE9462798A4F2E3AEFF7442",
                table: "ESCxEmpresa",
                column: "ESCxEmpresaID");

            migrationBuilder.CreateIndex(
                name: "IX_FCAEEE2632074900A104B57043F6C318",
                table: "ESCxEmpresa",
                column: "ESCxContabilistaID");

            migrationBuilder.CreateIndex(
                name: "IX_4A3824E876B5459E8901E1C2D47FB104",
                table: "ESCxEscritorio",
                column: "ESCxEscritorioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ESCxContabilistaEscritorio");

            migrationBuilder.DropTable(
                name: "ESCxEmpresa");

            migrationBuilder.DropTable(
                name: "ESCxEscritorio");

            migrationBuilder.DropTable(
                name: "ESCxContabilista");
        }
    }
}
