using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TFX.Core.DB.Migrations
{
    /// <inheritdoc />
    public partial class AddField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CEPxLocalidadePrincipalID",
                table: "CORxPessoa",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "CORxPessoa",
                keyColumn: "CORxPessoaID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000000"),
                column: "CEPxLocalidadePrincipalID",
                value: 0);

            migrationBuilder.UpdateData(
                table: "CORxPessoa",
                keyColumn: "CORxPessoaID",
                keyValue: new Guid("e3d57815-06e9-46e0-96f2-d77a03700ca8"),
                column: "CEPxLocalidadePrincipalID",
                value: 0);

            migrationBuilder.CreateIndex(
                name: "IX_76A39CE315424C0AA399D0DBECB92750",
                table: "CORxPessoa",
                column: "CEPxLocalidadePrincipalID");

            migrationBuilder.AddForeignKey(
                name: "FK_76A39CE315424C0AA399D0DBECB92750",
                table: "CORxPessoa",
                column: "CEPxLocalidadePrincipalID",
                principalTable: "CEPxLocalidade",
                principalColumn: "CEPxLocalidadeID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_76A39CE315424C0AA399D0DBECB92750",
                table: "CORxPessoa");

            migrationBuilder.DropIndex(
                name: "IX_76A39CE315424C0AA399D0DBECB92750",
                table: "CORxPessoa");

            migrationBuilder.DropColumn(
                name: "CEPxLocalidadePrincipalID",
                table: "CORxPessoa");
        }
    }
}
