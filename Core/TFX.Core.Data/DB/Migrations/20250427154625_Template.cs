using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TFX.Core.DB.Migrations
{
    /// <inheritdoc />
    public partial class Template : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CORxPessoa",
                columns: new[] { "CORxPessoaID", "Nome" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000000"), "NA" },
                    { new Guid("e3d57815-06e9-46e0-96f2-d77a03700ca8"), "Sistem Admin" }
                });

            migrationBuilder.InsertData(
                table: "CORxRecursoDireito",
                columns: new[] { "CORxRecursoDireitoID", "CORxDireitosID", "CORxRecursoID" },
                values: new object[,]
                {
                    { new Guid("132f09b1-d564-494b-8147-b25b575f0a60"), (short)1, new Guid("13f5ed76-d4cc-46b7-83e9-31c77bd3085c") },
                    { new Guid("2bfd129c-cab6-4bf5-9395-59173ab378bc"), (short)5, new Guid("13f5ed76-d4cc-46b7-83e9-31c77bd3085c") },
                    { new Guid("2c5d7092-96a9-458b-a191-4519c53f194c"), (short)2, new Guid("13f5ed76-d4cc-46b7-83e9-31c77bd3085c") },
                    { new Guid("a5b94287-0dee-4ce2-ae1b-97c604920f3b"), (short)4, new Guid("13f5ed76-d4cc-46b7-83e9-31c77bd3085c") },
                    { new Guid("e3fe8efa-f5cd-4184-a876-ccda2cf53853"), (short)3, new Guid("13f5ed76-d4cc-46b7-83e9-31c77bd3085c") }
                });

            migrationBuilder.InsertData(
                table: "CORxRecursoTemplate",
                columns: new[] { "CORxRecursoTemplateID", "CORxRecursoID", "CORxStatusID", "Template" },
                values: new object[] { new Guid("58cd2032-1081-4582-9f4e-47c60fadaba6"), new Guid("13f5ed76-d4cc-46b7-83e9-31c77bd3085c"), (short)1, "Cloud Admin" });

            migrationBuilder.InsertData(
                table: "CORxRecursoTemplateDireito",
                columns: new[] { "CORxRecursoTemplateDireitoID", "CORxRecursoDireitoID", "CORxRecursoTemplateID" },
                values: new object[,]
                {
                    { new Guid("3f281f5a-81cb-4840-880e-546212b48bb8"), new Guid("e3fe8efa-f5cd-4184-a876-ccda2cf53853"), new Guid("58cd2032-1081-4582-9f4e-47c60fadaba6") },
                    { new Guid("4c4d91b0-4ed8-41ae-90a4-bfd5ee8118a3"), new Guid("2c5d7092-96a9-458b-a191-4519c53f194c"), new Guid("58cd2032-1081-4582-9f4e-47c60fadaba6") },
                    { new Guid("99fb05be-79b0-47c9-9121-d7c76bc67c25"), new Guid("a5b94287-0dee-4ce2-ae1b-97c604920f3b"), new Guid("58cd2032-1081-4582-9f4e-47c60fadaba6") },
                    { new Guid("9f147453-0765-4824-8dd1-9b707bdb0138"), new Guid("132f09b1-d564-494b-8147-b25b575f0a60"), new Guid("58cd2032-1081-4582-9f4e-47c60fadaba6") },
                    { new Guid("e8709409-a964-4092-8b8e-215dabe3514b"), new Guid("2bfd129c-cab6-4bf5-9395-59173ab378bc"), new Guid("58cd2032-1081-4582-9f4e-47c60fadaba6") }
                });

            migrationBuilder.InsertData(
                table: "CORxUsuario",
                columns: new[] { "CORxUsuarioID", "EMail", "_CORxPessoaCORxPessoaID" },
                values: new object[,]
                {
                    { new Guid("00000000-0000-0000-0000-000000000000"), "NA", null },
                    { new Guid("e3d57815-06e9-46e0-96f2-d77a03700ca8"), "admin@tootega.com.br", null }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CORxRecursoTemplateDireito",
                keyColumn: "CORxRecursoTemplateDireitoID",
                keyValue: new Guid("3f281f5a-81cb-4840-880e-546212b48bb8"));

            migrationBuilder.DeleteData(
                table: "CORxRecursoTemplateDireito",
                keyColumn: "CORxRecursoTemplateDireitoID",
                keyValue: new Guid("4c4d91b0-4ed8-41ae-90a4-bfd5ee8118a3"));

            migrationBuilder.DeleteData(
                table: "CORxRecursoTemplateDireito",
                keyColumn: "CORxRecursoTemplateDireitoID",
                keyValue: new Guid("99fb05be-79b0-47c9-9121-d7c76bc67c25"));

            migrationBuilder.DeleteData(
                table: "CORxRecursoTemplateDireito",
                keyColumn: "CORxRecursoTemplateDireitoID",
                keyValue: new Guid("9f147453-0765-4824-8dd1-9b707bdb0138"));

            migrationBuilder.DeleteData(
                table: "CORxRecursoTemplateDireito",
                keyColumn: "CORxRecursoTemplateDireitoID",
                keyValue: new Guid("e8709409-a964-4092-8b8e-215dabe3514b"));

            migrationBuilder.DeleteData(
                table: "CORxUsuario",
                keyColumn: "CORxUsuarioID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "CORxUsuario",
                keyColumn: "CORxUsuarioID",
                keyValue: new Guid("e3d57815-06e9-46e0-96f2-d77a03700ca8"));

            migrationBuilder.DeleteData(
                table: "CORxPessoa",
                keyColumn: "CORxPessoaID",
                keyValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.DeleteData(
                table: "CORxPessoa",
                keyColumn: "CORxPessoaID",
                keyValue: new Guid("e3d57815-06e9-46e0-96f2-d77a03700ca8"));

            migrationBuilder.DeleteData(
                table: "CORxRecursoDireito",
                keyColumn: "CORxRecursoDireitoID",
                keyValue: new Guid("132f09b1-d564-494b-8147-b25b575f0a60"));

            migrationBuilder.DeleteData(
                table: "CORxRecursoDireito",
                keyColumn: "CORxRecursoDireitoID",
                keyValue: new Guid("2bfd129c-cab6-4bf5-9395-59173ab378bc"));

            migrationBuilder.DeleteData(
                table: "CORxRecursoDireito",
                keyColumn: "CORxRecursoDireitoID",
                keyValue: new Guid("2c5d7092-96a9-458b-a191-4519c53f194c"));

            migrationBuilder.DeleteData(
                table: "CORxRecursoDireito",
                keyColumn: "CORxRecursoDireitoID",
                keyValue: new Guid("a5b94287-0dee-4ce2-ae1b-97c604920f3b"));

            migrationBuilder.DeleteData(
                table: "CORxRecursoDireito",
                keyColumn: "CORxRecursoDireitoID",
                keyValue: new Guid("e3fe8efa-f5cd-4184-a876-ccda2cf53853"));

            migrationBuilder.DeleteData(
                table: "CORxRecursoTemplate",
                keyColumn: "CORxRecursoTemplateID",
                keyValue: new Guid("58cd2032-1081-4582-9f4e-47c60fadaba6"));
        }
    }
}
