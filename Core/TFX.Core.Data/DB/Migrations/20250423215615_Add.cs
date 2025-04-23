using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TFX.Core.DB.Migrations
{
    /// <inheritdoc />
    public partial class Add : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<short>(
                name: "CORxStatusID",
                table: "CORxRecursoTemplate",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.AddColumn<short>(
                name: "CORxStatusID",
                table: "CORxRecurso",
                type: "smallint",
                nullable: false,
                defaultValue: (short)0);

            migrationBuilder.InsertData(
                table: "CORxRecurso",
                columns: new[] { "CORxRecursoID", "CORxRecursoTipoID", "CORxStatusID", "Nome", "Titulo" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000000"), (short)0, (short)0, "NA", "NA" },
                    { new Guid("13f5ed76-d4cc-46b7-83e9-31c77bd3085c"), (short)1, (short)1, "Usuario", "Cadastro de Usuários" }
                });

            migrationBuilder.InsertData(
                table: "CORxMenuItem",
                columns: new[] { "CORxMenuItemID", "CORxMenuID", "CORxRecursoID", "Item" },
                values: new object[] { new Guid("23acd023-5ce5-412c-becf-fd4d58dc680b"), new Guid("a0194fb8-9893-48c5-bd2b-7626a82b3da3"), new Guid("13f5ed76-d4cc-46b7-83e9-31c77bd3085c"), "Cadastro de Usuários" });

            migrationBuilder.CreateIndex(
                name: "IX_DF3AB7500B414D228CE6266520EA9D8A",
                table: "CORxRecursoTemplate",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_91D14E103000453FA60EC98EA012F459",
                table: "CORxRecurso",
                column: "CORxStatusID");

            migrationBuilder.AddForeignKey(
                name: "FK_91D14E103000453FA60EC98EA012F459",
                table: "CORxRecurso",
                column: "CORxStatusID",
                principalTable: "CORxStatus",
                principalColumn: "CORxStatusID",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_DF3AB7500B414D228CE6266520EA9D8A",
                table: "CORxRecursoTemplate",
                column: "CORxStatusID",
                principalTable: "CORxStatus",
                principalColumn: "CORxStatusID",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_91D14E103000453FA60EC98EA012F459",
                table: "CORxRecurso");

            migrationBuilder.DropForeignKey(
                name: "FK_DF3AB7500B414D228CE6266520EA9D8A",
                table: "CORxRecursoTemplate");

            migrationBuilder.DropIndex(
                name: "IX_DF3AB7500B414D228CE6266520EA9D8A",
                table: "CORxRecursoTemplate");

            migrationBuilder.DropIndex(
                name: "IX_91D14E103000453FA60EC98EA012F459",
                table: "CORxRecurso");

            migrationBuilder.DeleteData(
                table: "CORxMenuItem",
                keyColumn: "CORxMenuItemID",
                keyValue: new Guid("23acd023-5ce5-412c-becf-fd4d58dc680b"));

            migrationBuilder.DeleteData(
                table: "CORxRecurso",
                keyColumn: "CORxRecursoID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "CORxRecurso",
                keyColumn: "CORxRecursoID",
                keyValue: new Guid("13f5ed76-d4cc-46b7-83e9-31c77bd3085c"));

            migrationBuilder.DropColumn(
                name: "CORxStatusID",
                table: "CORxRecursoTemplate");

            migrationBuilder.DropColumn(
                name: "CORxStatusID",
                table: "CORxRecurso");
        }
    }
}
