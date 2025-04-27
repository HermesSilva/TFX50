using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TFX.Core.DB.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CORxDireitos",
                columns: table => new
                {
                    CORxDireitosID = table.Column<short>(type: "smallint", nullable: false),
                    Direito = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Titulo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxDireitos", x => x.CORxDireitosID);
                });

            migrationBuilder.CreateTable(
                name: "CORxMenu",
                columns: table => new
                {
                    CORxMenuID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Icone = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    Menu = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxMenu", x => x.CORxMenuID);
                });

            migrationBuilder.CreateTable(
                name: "CORxPessoa",
                columns: table => new
                {
                    CORxPessoaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(180)", maxLength: 180, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxPessoa", x => x.CORxPessoaID);
                });

            migrationBuilder.CreateTable(
                name: "CORxRecursoTipo",
                columns: table => new
                {
                    CORxRecursoTipoID = table.Column<short>(type: "smallint", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxRecursoTipo", x => x.CORxRecursoTipoID);
                });

            migrationBuilder.CreateTable(
                name: "CORxStatus",
                columns: table => new
                {
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false),
                    Status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxStatus", x => x.CORxStatusID);
                });

            migrationBuilder.CreateTable(
                name: "CORxUsuario",
                columns: table => new
                {
                    CORxUsuarioID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EMail = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    _CORxPessoaCORxPessoaID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxUsuario", x => x.CORxUsuarioID);
                    table.ForeignKey(
                        name: "FK_CORxUsuario_CORxPessoa__CORxPessoaCORxPessoaID",
                        column: x => x._CORxPessoaCORxPessoaID,
                        principalTable: "CORxPessoa",
                        principalColumn: "CORxPessoaID");
                    table.ForeignKey(
                        name: "FK_F3E4CFF511294AD5884ACAC97FDE96C4",
                        column: x => x.CORxUsuarioID,
                        principalTable: "CORxPessoa",
                        principalColumn: "CORxPessoaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CORxAgregado",
                columns: table => new
                {
                    CORxAgregadoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false),
                    CPFCNPJ = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
                    _CORxPessoaCORxPessoaID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxAgregado", x => x.CORxAgregadoID);
                    table.ForeignKey(
                        name: "FK_04BE7F3F33664F8187708FB8EEE44BB9",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CORxAgregado_CORxPessoa__CORxPessoaCORxPessoaID",
                        column: x => x._CORxPessoaCORxPessoaID,
                        principalTable: "CORxPessoa",
                        principalColumn: "CORxPessoaID");
                    table.ForeignKey(
                        name: "FK_D129F24223084A40BDB3C6BF6BC16ABD",
                        column: x => x.CORxAgregadoID,
                        principalTable: "CORxPessoa",
                        principalColumn: "CORxPessoaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CORxRecurso",
                columns: table => new
                {
                    CORxRecursoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxRecursoTipoID = table.Column<short>(type: "smallint", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false),
                    Nome = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Titulo = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxRecurso", x => x.CORxRecursoID);
                    table.ForeignKey(
                        name: "FK_3D172BE06C0E46E88190AB42F3FF81AC",
                        column: x => x.CORxRecursoTipoID,
                        principalTable: "CORxRecursoTipo",
                        principalColumn: "CORxRecursoTipoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_91D14E103000453FA60EC98EA012F459",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CORxEmpresa",
                columns: table => new
                {
                    CORxEmpresaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CNPJ = table.Column<string>(type: "varchar(14)", maxLength: 14, nullable: false),
                    CORxAgregadoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false),
                    _CORxPessoaCORxPessoaID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxEmpresa", x => x.CORxEmpresaID);
                    table.ForeignKey(
                        name: "FK_3D11CC2230A34B7293C66F8733789E1F",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_4F443AC8953246C19887C496C4CB7DCC",
                        column: x => x.CORxAgregadoID,
                        principalTable: "CORxAgregado",
                        principalColumn: "CORxAgregadoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_751AAA01BDC648D8A2B8DA03E5758339",
                        column: x => x.CORxEmpresaID,
                        principalTable: "CORxPessoa",
                        principalColumn: "CORxPessoaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CORxEmpresa_CORxPessoa__CORxPessoaCORxPessoaID",
                        column: x => x._CORxPessoaCORxPessoaID,
                        principalTable: "CORxPessoa",
                        principalColumn: "CORxPessoaID");
                });

            migrationBuilder.CreateTable(
                name: "CORxMenuItem",
                columns: table => new
                {
                    CORxMenuItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxMenuID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxRecursoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Item = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxMenuItem", x => x.CORxMenuItemID);
                    table.ForeignKey(
                        name: "FK_2540E01112654DC4B4CE3983FA0714B0",
                        column: x => x.CORxRecursoID,
                        principalTable: "CORxRecurso",
                        principalColumn: "CORxRecursoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DB53F2347A1F41978B6CC027E0529DE1",
                        column: x => x.CORxMenuID,
                        principalTable: "CORxMenu",
                        principalColumn: "CORxMenuID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CORxRecursoDireito",
                columns: table => new
                {
                    CORxRecursoDireitoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxDireitosID = table.Column<short>(type: "smallint", nullable: false),
                    CORxRecursoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxRecursoDireito", x => x.CORxRecursoDireitoID);
                    table.ForeignKey(
                        name: "FK_2BBF2707C62C4B4A80B9A3FE91C2D5B3",
                        column: x => x.CORxDireitosID,
                        principalTable: "CORxDireitos",
                        principalColumn: "CORxDireitosID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_8642F0512C404418A0C5B1C586A11496",
                        column: x => x.CORxRecursoID,
                        principalTable: "CORxRecurso",
                        principalColumn: "CORxRecursoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CORxRecursoTemplate",
                columns: table => new
                {
                    CORxRecursoTemplateID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxRecursoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false),
                    Template = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxRecursoTemplate", x => x.CORxRecursoTemplateID);
                    table.ForeignKey(
                        name: "FK_CA7AE4A659F94B55AE8D02FC25FBE69F",
                        column: x => x.CORxRecursoID,
                        principalTable: "CORxRecurso",
                        principalColumn: "CORxRecursoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DF3AB7500B414D228CE6266520EA9D8A",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CORxEmpresaGrupo",
                columns: table => new
                {
                    CORxEmpresaGrupoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxAgregadoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxEmpresaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false),
                    Grupo = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxEmpresaGrupo", x => x.CORxEmpresaGrupoID);
                    table.ForeignKey(
                        name: "FK_0392DD25ED2D4C209696C72A0858FF3F",
                        column: x => x.CORxEmpresaID,
                        principalTable: "CORxEmpresa",
                        principalColumn: "CORxEmpresaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_0F3626E1A65A4611BDB2E747CA5A9893",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_6F15186E1CAA445292860D8634F9D6F3",
                        column: x => x.CORxAgregadoID,
                        principalTable: "CORxAgregado",
                        principalColumn: "CORxAgregadoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CORxFavorito",
                columns: table => new
                {
                    CORxFavoritoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxMenuItemID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxUsuarioID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Frequencia = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxFavorito", x => x.CORxFavoritoID);
                    table.ForeignKey(
                        name: "FK_45F65CC922A3454A8F2297DC234FE876",
                        column: x => x.CORxUsuarioID,
                        principalTable: "CORxUsuario",
                        principalColumn: "CORxUsuarioID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_B9FA63EA76C64A528D3FB64F43A29930",
                        column: x => x.CORxMenuItemID,
                        principalTable: "CORxMenuItem",
                        principalColumn: "CORxMenuItemID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CORxRecursoTemplateDireito",
                columns: table => new
                {
                    CORxRecursoTemplateDireitoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxRecursoDireitoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxRecursoTemplateID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxRecursoTemplateDireito", x => x.CORxRecursoTemplateDireitoID);
                    table.ForeignKey(
                        name: "FK_413AF556566D4C86868658231BC10E05",
                        column: x => x.CORxRecursoDireitoID,
                        principalTable: "CORxRecursoDireito",
                        principalColumn: "CORxRecursoDireitoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_998C84A533E14063A4E30498D27B1762",
                        column: x => x.CORxRecursoTemplateID,
                        principalTable: "CORxRecursoTemplate",
                        principalColumn: "CORxRecursoTemplateID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CORxUsuarioRecursoTemplate",
                columns: table => new
                {
                    CORxUsuarioRecursoTemplateID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxRecursoTemplateID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxUsuarioID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CORxUsuarioRecursoTemplate", x => x.CORxUsuarioRecursoTemplateID);
                    table.ForeignKey(
                        name: "FK_4B924243B5C649B99E8BABAA4C17A5E2",
                        column: x => x.CORxRecursoTemplateID,
                        principalTable: "CORxRecursoTemplate",
                        principalColumn: "CORxRecursoTemplateID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_D3EBA1E15D7E4EF4A84A2EC9053EB6C6",
                        column: x => x.CORxUsuarioID,
                        principalTable: "CORxUsuario",
                        principalColumn: "CORxUsuarioID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CORxDireitos",
                columns: new[] { "CORxDireitosID", "Direito", "Titulo" },
                values: new object[,]
                {
                    { (short)0, "NA", "NA" },
                    { (short)1, "Visualizar", "Visualizar" },
                    { (short)2, "Inserir", "Inserir" },
                    { (short)3, "Alterar", "Alterar" },
                    { (short)4, "Inativar", "Inativar" },
                    { (short)5, "Apagar", "Apagar" }
                });

            migrationBuilder.InsertData(
                table: "CORxMenu",
                columns: new[] { "CORxMenuID", "Icone", "Menu" },
                values: new object[] { new Guid("a0194fb8-9893-48c5-bd2b-7626a82b3da3"), null, "Sistema" });

            migrationBuilder.InsertData(
                table: "CORxRecursoTipo",
                columns: new[] { "CORxRecursoTipoID", "Tipo" },
                values: new object[,]
                {
                    { (short)0, "NA" },
                    { (short)1, "Aplicação de UI" },
                    { (short)2, "Web API" }
                });

            migrationBuilder.InsertData(
                table: "CORxStatus",
                columns: new[] { "CORxStatusID", "Status" },
                values: new object[,]
                {
                    { (short)0, "Inatoivo" },
                    { (short)1, "Ativo" }
                });

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
                name: "IX_04BE7F3F33664F8187708FB8EEE44BB9",
                table: "CORxAgregado",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_CORxAgregado__CORxPessoaCORxPessoaID",
                table: "CORxAgregado",
                column: "_CORxPessoaCORxPessoaID");

            migrationBuilder.CreateIndex(
                name: "IX_D129F24223084A40BDB3C6BF6BC16ABD",
                table: "CORxAgregado",
                column: "CORxAgregadoID");

            migrationBuilder.CreateIndex(
                name: "IX_3D11CC2230A34B7293C66F8733789E1F",
                table: "CORxEmpresa",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_4F443AC8953246C19887C496C4CB7DCC",
                table: "CORxEmpresa",
                column: "CORxAgregadoID");

            migrationBuilder.CreateIndex(
                name: "IX_751AAA01BDC648D8A2B8DA03E5758339",
                table: "CORxEmpresa",
                column: "CORxEmpresaID");

            migrationBuilder.CreateIndex(
                name: "IX_CORxEmpresa__CORxPessoaCORxPessoaID",
                table: "CORxEmpresa",
                column: "_CORxPessoaCORxPessoaID");

            migrationBuilder.CreateIndex(
                name: "IX_0392DD25ED2D4C209696C72A0858FF3F",
                table: "CORxEmpresaGrupo",
                column: "CORxEmpresaID");

            migrationBuilder.CreateIndex(
                name: "IX_0F3626E1A65A4611BDB2E747CA5A9893",
                table: "CORxEmpresaGrupo",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_6F15186E1CAA445292860D8634F9D6F3",
                table: "CORxEmpresaGrupo",
                column: "CORxAgregadoID");

            migrationBuilder.CreateIndex(
                name: "IX_45F65CC922A3454A8F2297DC234FE876",
                table: "CORxFavorito",
                column: "CORxUsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_B9FA63EA76C64A528D3FB64F43A29930",
                table: "CORxFavorito",
                column: "CORxMenuItemID");

            migrationBuilder.CreateIndex(
                name: "IX_2540E01112654DC4B4CE3983FA0714B0",
                table: "CORxMenuItem",
                column: "CORxRecursoID");

            migrationBuilder.CreateIndex(
                name: "IX_DB53F2347A1F41978B6CC027E0529DE1",
                table: "CORxMenuItem",
                column: "CORxMenuID");

            migrationBuilder.CreateIndex(
                name: "IX_3D172BE06C0E46E88190AB42F3FF81AC",
                table: "CORxRecurso",
                column: "CORxRecursoTipoID");

            migrationBuilder.CreateIndex(
                name: "IX_91D14E103000453FA60EC98EA012F459",
                table: "CORxRecurso",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_2BBF2707C62C4B4A80B9A3FE91C2D5B3",
                table: "CORxRecursoDireito",
                column: "CORxDireitosID");

            migrationBuilder.CreateIndex(
                name: "IX_8642F0512C404418A0C5B1C586A11496",
                table: "CORxRecursoDireito",
                column: "CORxRecursoID");

            migrationBuilder.CreateIndex(
                name: "IX_CA7AE4A659F94B55AE8D02FC25FBE69F",
                table: "CORxRecursoTemplate",
                column: "CORxRecursoID");

            migrationBuilder.CreateIndex(
                name: "IX_DF3AB7500B414D228CE6266520EA9D8A",
                table: "CORxRecursoTemplate",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_413AF556566D4C86868658231BC10E05",
                table: "CORxRecursoTemplateDireito",
                column: "CORxRecursoDireitoID");

            migrationBuilder.CreateIndex(
                name: "IX_998C84A533E14063A4E30498D27B1762",
                table: "CORxRecursoTemplateDireito",
                column: "CORxRecursoTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_CORxUsuario__CORxPessoaCORxPessoaID",
                table: "CORxUsuario",
                column: "_CORxPessoaCORxPessoaID");

            migrationBuilder.CreateIndex(
                name: "IX_F3E4CFF511294AD5884ACAC97FDE96C4",
                table: "CORxUsuario",
                column: "CORxUsuarioID");

            migrationBuilder.CreateIndex(
                name: "IX_4B924243B5C649B99E8BABAA4C17A5E2",
                table: "CORxUsuarioRecursoTemplate",
                column: "CORxRecursoTemplateID");

            migrationBuilder.CreateIndex(
                name: "IX_D3EBA1E15D7E4EF4A84A2EC9053EB6C6",
                table: "CORxUsuarioRecursoTemplate",
                column: "CORxUsuarioID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CORxEmpresaGrupo");

            migrationBuilder.DropTable(
                name: "CORxFavorito");

            migrationBuilder.DropTable(
                name: "CORxRecursoTemplateDireito");

            migrationBuilder.DropTable(
                name: "CORxUsuarioRecursoTemplate");

            migrationBuilder.DropTable(
                name: "CORxEmpresa");

            migrationBuilder.DropTable(
                name: "CORxMenuItem");

            migrationBuilder.DropTable(
                name: "CORxRecursoDireito");

            migrationBuilder.DropTable(
                name: "CORxRecursoTemplate");

            migrationBuilder.DropTable(
                name: "CORxUsuario");

            migrationBuilder.DropTable(
                name: "CORxAgregado");

            migrationBuilder.DropTable(
                name: "CORxMenu");

            migrationBuilder.DropTable(
                name: "CORxDireitos");

            migrationBuilder.DropTable(
                name: "CORxRecurso");

            migrationBuilder.DropTable(
                name: "CORxPessoa");

            migrationBuilder.DropTable(
                name: "CORxRecursoTipo");

            migrationBuilder.DropTable(
                name: "CORxStatus");
        }
    }
}
