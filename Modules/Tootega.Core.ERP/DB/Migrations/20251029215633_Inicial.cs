using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Tootega.Core.ERP.DB.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EMLxEstado",
                columns: table => new
                {
                    EMLxEstadoID = table.Column<short>(type: "smallint", nullable: false),
                    Estado = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMLxEstado", x => x.EMLxEstadoID);
                });

            migrationBuilder.CreateTable(
                name: "EMLxServidorFinalizade",
                columns: table => new
                {
                    EMLxServidorFinalizadeID = table.Column<short>(type: "smallint", nullable: false),
                    Finalidade = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMLxServidorFinalizade", x => x.EMLxServidorFinalizadeID);
                });

            migrationBuilder.CreateTable(
                name: "ERPxCategoria",
                columns: table => new
                {
                    ERPxCategoriaID = table.Column<short>(type: "smallint", nullable: false),
                    Categoria = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxCategoria", x => x.ERPxCategoriaID);
                });

            migrationBuilder.CreateTable(
                name: "ERPxContatoTipo",
                columns: table => new
                {
                    ERPxContatoTipoID = table.Column<short>(type: "smallint", nullable: false),
                    Mascara = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: true),
                    Tipo = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxContatoTipo", x => x.ERPxContatoTipoID);
                });

            migrationBuilder.CreateTable(
                name: "ERPxDocumentoTipo",
                columns: table => new
                {
                    ERPxDocumentoTipoID = table.Column<short>(type: "smallint", nullable: false),
                    Filtro = table.Column<int>(type: "int", nullable: false),
                    Mascara = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    Tipo = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxDocumentoTipo", x => x.ERPxDocumentoTipoID);
                });

            migrationBuilder.CreateTable(
                name: "ERPxFinalidade",
                columns: table => new
                {
                    ERPxFinalidadeID = table.Column<short>(type: "smallint", nullable: false),
                    Finalidade = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxFinalidade", x => x.ERPxFinalidadeID);
                });

            migrationBuilder.CreateTable(
                name: "ERPxGenero",
                columns: table => new
                {
                    ERPxGeneroID = table.Column<short>(type: "smallint", nullable: false),
                    Designacao = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    Genero = table.Column<string>(type: "varchar(15)", maxLength: 15, nullable: false),
                    Invisivel = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxGenero", x => x.ERPxGeneroID);
                });

            migrationBuilder.CreateTable(
                name: "ERPxPessoaFisicaTipo",
                columns: table => new
                {
                    ERPxPessoaFisicaTipoID = table.Column<short>(type: "smallint", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxPessoaFisicaTipo", x => x.ERPxPessoaFisicaTipoID);
                });

            migrationBuilder.CreateTable(
                name: "ERPxPessoaJuridica",
                columns: table => new
                {
                    ERPxPessoaJuridicaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    RazaoSocial = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false, defaultValue: "NI")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxPessoaJuridica", x => x.ERPxPessoaJuridicaID);
                    table.ForeignKey(
                        name: "FK_1183A3DBAC464B44986F8D45935BB8E4",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ERPxProfissionalHorarioTipo",
                columns: table => new
                {
                    ERPxProfissionalHorarioTipoID = table.Column<short>(type: "smallint", nullable: false),
                    Horario = table.Column<string>(type: "varchar(35)", maxLength: 35, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxProfissionalHorarioTipo", x => x.ERPxProfissionalHorarioTipoID);
                });

            migrationBuilder.CreateTable(
                name: "EMLxServidor",
                columns: table => new
                {
                    EMLxServidorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EMLxServidorFinalizadeID = table.Column<short>(type: "smallint", nullable: false),
                    Nome = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false),
                    Porta = table.Column<int>(type: "int", nullable: false),
                    Senha = table.Column<string>(type: "varchar(256)", maxLength: 256, nullable: false),
                    SMTPServidor = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    SSL = table.Column<bool>(type: "bit", nullable: false),
                    Usuario = table.Column<string>(type: "varchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMLxServidor", x => x.EMLxServidorID);
                    table.ForeignKey(
                        name: "FK_593603",
                        column: x => x.EMLxServidorFinalizadeID,
                        principalTable: "EMLxServidorFinalizade",
                        principalColumn: "EMLxServidorFinalizadeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ERPxDocumento",
                columns: table => new
                {
                    ERPxDocumentoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxPessoaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    ERPxDocumentoTipoID = table.Column<short>(type: "smallint", nullable: false),
                    Numero = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxDocumento", x => x.ERPxDocumentoID);
                    table.ForeignKey(
                        name: "FK_529872",
                        column: x => x.ERPxDocumentoTipoID,
                        principalTable: "ERPxDocumentoTipo",
                        principalColumn: "ERPxDocumentoTipoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_5F7E452D79C944328446AAD57587917C",
                        column: x => x.CORxPessoaID,
                        principalTable: "CORxPessoa",
                        principalColumn: "CORxPessoaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_7B2BB2D7B5DD4C2C9FBCFAFE802FB8B3",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ERPxContato",
                columns: table => new
                {
                    ERPxContatoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Contato = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    CORxPessoaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    ERPxContatoTipoID = table.Column<short>(type: "smallint", nullable: false),
                    ERPxFinalidadeID = table.Column<short>(type: "smallint", nullable: false),
                    Observacao = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    Validado = table.Column<bool>(type: "bit", nullable: false, defaultValue: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxContato", x => x.ERPxContatoID);
                    table.ForeignKey(
                        name: "FK_0E654D4AED51402E9FF6E98504AEB5CA",
                        column: x => x.CORxPessoaID,
                        principalTable: "CORxPessoa",
                        principalColumn: "CORxPessoaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_51C6F265549348069F2821C19BFCE13E",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_529662",
                        column: x => x.ERPxFinalidadeID,
                        principalTable: "ERPxFinalidade",
                        principalColumn: "ERPxFinalidadeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_529664",
                        column: x => x.ERPxContatoTipoID,
                        principalTable: "ERPxContatoTipo",
                        principalColumn: "ERPxContatoTipoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ERPxEndereco",
                columns: table => new
                {
                    ERPxEnderecoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CEPxLogradouroID = table.Column<int>(type: "int", nullable: false),
                    Complemento = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: true),
                    CORxPessoaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    ERPxFinalidadeID = table.Column<short>(type: "smallint", nullable: false),
                    Latitude = table.Column<decimal>(type: "numeric(20,10)", nullable: false),
                    Longitude = table.Column<decimal>(type: "numeric(20,10)", nullable: false),
                    Lote = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, defaultValue: ""),
                    Numero = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: true, defaultValue: ""),
                    Observacao = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true),
                    Quadra = table.Column<string>(type: "varchar(4)", maxLength: 4, nullable: true, defaultValue: "")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxEndereco", x => x.ERPxEnderecoID);
                    table.ForeignKey(
                        name: "FK_24448961F77B4CAC9D842F76E306CF5B",
                        column: x => x.CEPxLogradouroID,
                        principalTable: "CEPxLogradouro",
                        principalColumn: "CEPxLogradouroID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_529658",
                        column: x => x.ERPxFinalidadeID,
                        principalTable: "ERPxFinalidade",
                        principalColumn: "ERPxFinalidadeID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_B9782A035C6C4ACC9AA722AB30D5DC89",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_F92D21F245294E54AE7006F94BFE60C7",
                        column: x => x.CORxPessoaID,
                        principalTable: "CORxPessoa",
                        principalColumn: "CORxPessoaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ERPxPessoaFisica",
                columns: table => new
                {
                    ERPxPessoaFisicaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false, defaultValue: (short)0),
                    ERPxGeneroID = table.Column<short>(type: "smallint", nullable: false),
                    Nascimento = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxPessoaFisica", x => x.ERPxPessoaFisicaID);
                    table.ForeignKey(
                        name: "FK_29E58B640111432D9BDB6C2801597F81",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_532463",
                        column: x => x.ERPxGeneroID,
                        principalTable: "ERPxGenero",
                        principalColumn: "ERPxGeneroID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ERPxFornecedor",
                columns: table => new
                {
                    ERPxFornecedorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    _ERPxPessoaJuridicaERPxPessoaJuridicaID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxFornecedor", x => x.ERPxFornecedorID);
                    table.ForeignKey(
                        name: "FK_A4E089E6F08C47DEA7A2093E7207245A",
                        column: x => x.ERPxFornecedorID,
                        principalTable: "ERPxPessoaJuridica",
                        principalColumn: "ERPxPessoaJuridicaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ERPxFornecedor_ERPxPessoaJuridica__ERPxPessoaJuridicaERPxPessoaJuridicaID",
                        column: x => x._ERPxPessoaJuridicaERPxPessoaJuridicaID,
                        principalTable: "ERPxPessoaJuridica",
                        principalColumn: "ERPxPessoaJuridicaID");
                });

            migrationBuilder.CreateTable(
                name: "EMLxCaixa",
                columns: table => new
                {
                    EMLxCaixaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Asunto = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Criacao = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EMLxEstadoID = table.Column<short>(type: "smallint", nullable: false),
                    EMLxServidorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Envio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Mensagem = table.Column<string>(type: "varchar(max)", nullable: false),
                    SYSxEmitenteID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMLxCaixa", x => x.EMLxCaixaID);
                    table.ForeignKey(
                        name: "FK_593073",
                        column: x => x.EMLxEstadoID,
                        principalTable: "EMLxEstado",
                        principalColumn: "EMLxEstadoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_593162",
                        column: x => x.EMLxServidorID,
                        principalTable: "EMLxServidor",
                        principalColumn: "EMLxServidorID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_740A2C9FCA9E45B7A8474CF152CC7787",
                        column: x => x.SYSxEmitenteID,
                        principalTable: "CORxPessoa",
                        principalColumn: "CORxPessoaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EMLxEmpresaServidor",
                columns: table => new
                {
                    EMLxEmpresaServidorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxPessoaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EMLxServidorID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMLxEmpresaServidor", x => x.EMLxEmpresaServidorID);
                    table.ForeignKey(
                        name: "FK_1B5E64FF1C794BC4A7BB0A2180621E4D",
                        column: x => x.CORxPessoaID,
                        principalTable: "CORxPessoa",
                        principalColumn: "CORxPessoaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_593262",
                        column: x => x.EMLxServidorID,
                        principalTable: "EMLxServidor",
                        principalColumn: "EMLxServidorID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ERPxPessoaFisicaTipos",
                columns: table => new
                {
                    ERPxPessoaFisicaTiposID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false),
                    ERPxPessoaFisicaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ERPxPessoaFisicaTipoID = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxPessoaFisicaTipos", x => x.ERPxPessoaFisicaTiposID);
                    table.ForeignKey(
                        name: "FK_C3C07B29B31C4BE1B90079B3256A0572",
                        column: x => x.ERPxPessoaFisicaID,
                        principalTable: "ERPxPessoaFisica",
                        principalColumn: "ERPxPessoaFisicaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_D1BEE9AA310A476EAA08D7D9A46961A1",
                        column: x => x.ERPxPessoaFisicaTipoID,
                        principalTable: "ERPxPessoaFisicaTipo",
                        principalColumn: "ERPxPessoaFisicaTipoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_D5C25A2F357B4DC58334CE40FF90A20B",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ERPxProfissional",
                columns: table => new
                {
                    ERPxProfissionalID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false),
                    _ERPxPessoaFisicaERPxPessoaFisicaID = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxProfissional", x => x.ERPxProfissionalID);
                    table.ForeignKey(
                        name: "FK_596632",
                        column: x => x.ERPxProfissionalID,
                        principalTable: "ERPxPessoaFisica",
                        principalColumn: "ERPxPessoaFisicaID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_9B7B5569232745D4B3C4181A55269727",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ERPxProfissional_ERPxPessoaFisica__ERPxPessoaFisicaERPxPessoaFisicaID",
                        column: x => x._ERPxPessoaFisicaERPxPessoaFisicaID,
                        principalTable: "ERPxPessoaFisica",
                        principalColumn: "ERPxPessoaFisicaID");
                });

            migrationBuilder.CreateTable(
                name: "EMLxAnexo",
                columns: table => new
                {
                    EMLxAnexoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Dado = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    EMLxCaixaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Nome = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMLxAnexo", x => x.EMLxAnexoID);
                    table.ForeignKey(
                        name: "FK_593111",
                        column: x => x.EMLxCaixaID,
                        principalTable: "EMLxCaixa",
                        principalColumn: "EMLxCaixaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EMLxDestinatario",
                columns: table => new
                {
                    EMLxDestinatarioID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EMLxCaixaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ERPxContatoID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMLxDestinatario", x => x.EMLxDestinatarioID);
                    table.ForeignKey(
                        name: "FK_593078",
                        column: x => x.ERPxContatoID,
                        principalTable: "ERPxContato",
                        principalColumn: "ERPxContatoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_593178",
                        column: x => x.EMLxCaixaID,
                        principalTable: "EMLxCaixa",
                        principalColumn: "EMLxCaixaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EMLxLog",
                columns: table => new
                {
                    EMLxLogID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Data = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EMLxCaixaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Mensagem = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: false),
                    Pilha = table.Column<string>(type: "varchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EMLxLog", x => x.EMLxLogID);
                    table.ForeignKey(
                        name: "FK_593093",
                        column: x => x.EMLxCaixaID,
                        principalTable: "EMLxCaixa",
                        principalColumn: "EMLxCaixaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ERPxProfissionalCategoria",
                columns: table => new
                {
                    ERPxProfissionalCategoriaID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ERPxCategoriaID = table.Column<short>(type: "smallint", nullable: false),
                    ERPxProfissionalID = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxProfissionalCategoria", x => x.ERPxProfissionalCategoriaID);
                    table.ForeignKey(
                        name: "FK_596636",
                        column: x => x.ERPxProfissionalID,
                        principalTable: "ERPxProfissional",
                        principalColumn: "ERPxProfissionalID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_596638",
                        column: x => x.ERPxCategoriaID,
                        principalTable: "ERPxCategoria",
                        principalColumn: "ERPxCategoriaID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ERPxProfissionalHorario",
                columns: table => new
                {
                    ERPxProfissionalHorarioID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CORxStatusID = table.Column<short>(type: "smallint", nullable: false),
                    ERPxProfissionalHorarioTipoID = table.Column<short>(type: "smallint", nullable: false),
                    ERPxProfissionalID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Fim = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Inicio = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ERPxProfissionalHorario", x => x.ERPxProfissionalHorarioID);
                    table.ForeignKey(
                        name: "FK_599320",
                        column: x => x.ERPxProfissionalID,
                        principalTable: "ERPxProfissional",
                        principalColumn: "ERPxProfissionalID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_599322",
                        column: x => x.ERPxProfissionalHorarioTipoID,
                        principalTable: "ERPxProfissionalHorarioTipo",
                        principalColumn: "ERPxProfissionalHorarioTipoID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CDC71151B0274EDC84B4475F4717DAA6",
                        column: x => x.CORxStatusID,
                        principalTable: "CORxStatus",
                        principalColumn: "CORxStatusID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "EMLxEstado",
                columns: new[] { "EMLxEstadoID", "Estado" },
                values: new object[,]
                {
                    { (short)1, "A Enviar" },
                    { (short)2, "Enviado" },
                    { (short)3, "Com Erro" }
                });

            migrationBuilder.InsertData(
                table: "EMLxServidorFinalizade",
                columns: new[] { "EMLxServidorFinalizadeID", "Finalidade" },
                values: new object[] { (short)1, "Uso Geral" });

            migrationBuilder.InsertData(
                table: "ERPxContatoTipo",
                columns: new[] { "ERPxContatoTipoID", "Mascara", "Tipo" },
                values: new object[,]
                {
                    { (short)1, "0000-0000|(00) 0000-0000|+00 (00) 0000-0000|00000-0000|(00) 00000-0000|+00 (00) 00000-0000", "Telefone Celular" },
                    { (short)2, null, "E-Mail" },
                    { (short)3, "0000-0000|(00) 0000-0000|+00 (00) 0000-0000|00000-0000|(00) 00000-0000|+00 (00) 00000-0000", "Telefone Fixo" },
                    { (short)4, null, "Outros" },
                    { (short)5, null, "Chat " },
                    { (short)6, "0000-0000|(00) 0000-0000|+00 (00) 0000-0000|00000-0000|(00) 00000-0000|+00 (00) 00000-0000", "Mensagem WhatsApp" }
                });

            migrationBuilder.InsertData(
                table: "ERPxDocumentoTipo",
                columns: new[] { "ERPxDocumentoTipoID", "Filtro", "Mascara", "Tipo" },
                values: new object[,]
                {
                    { (short)1, -1, "000.000.000-00", "CPF" },
                    { (short)2, 2, "00.000.000/0000-00", "CNPJ" },
                    { (short)3, 0, null, "Outros" },
                    { (short)4, 0, null, "IE" },
                    { (short)5, -1, null, "RG" },
                    { (short)6, -1, null, "Passaporte" },
                    { (short)7, 2, null, "Inscrição Municipal" },
                    { (short)9, 2, null, "Alvará Municipal" }
                });

            migrationBuilder.InsertData(
                table: "ERPxFinalidade",
                columns: new[] { "ERPxFinalidadeID", "Finalidade" },
                values: new object[,]
                {
                    { (short)1, "Outros" },
                    { (short)2, "Cobrança" },
                    { (short)3, "Documentos Fiscais" },
                    { (short)4, "Envio de Mensagens" }
                });

            migrationBuilder.InsertData(
                table: "ERPxGenero",
                columns: new[] { "ERPxGeneroID", "Designacao", "Genero", "Invisivel" },
                values: new object[,]
                {
                    { (short)0, "NA", "NI", false },
                    { (short)1, "Macho", "Masculino", false },
                    { (short)2, "Fêmea", "Feminino", false }
                });

            migrationBuilder.InsertData(
                table: "ERPxPessoaFisicaTipo",
                columns: new[] { "ERPxPessoaFisicaTipoID", "Tipo" },
                values: new object[] { (short)1, "Usuário do Sistema" });

            migrationBuilder.InsertData(
                table: "ERPxPessoaJuridica",
                columns: new[] { "ERPxPessoaJuridicaID", "CORxStatusID", "RazaoSocial" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000000"), (short)1, "NA" });

            migrationBuilder.InsertData(
                table: "ERPxProfissionalHorarioTipo",
                columns: new[] { "ERPxProfissionalHorarioTipoID", "Horario" },
                values: new object[,]
                {
                    { (short)0, "Às Domingos" },
                    { (short)1, "Às Segundas Feiras" },
                    { (short)2, "Às Terças Feiras" },
                    { (short)3, "Às Quartas Feiras" },
                    { (short)4, "Às Quintas Feiras" },
                    { (short)5, "Às Sextas Feiras" },
                    { (short)6, "Aos Sábados" },
                    { (short)7, "De Segunda à Sexta" }
                });

            migrationBuilder.InsertData(
                table: "ERPxDocumento",
                columns: new[] { "ERPxDocumentoID", "CORxPessoaID", "ERPxDocumentoTipoID", "Numero" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00000000-0000-0000-0000-000000000000"), (short)2, "NI" });

            migrationBuilder.InsertData(
                table: "ERPxEndereco",
                columns: new[] { "ERPxEnderecoID", "CEPxLogradouroID", "CORxPessoaID", "CORxStatusID", "Complemento", "ERPxFinalidadeID", "Latitude", "Longitude", "Lote", "Numero", "Observacao", "Quadra" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000000"), 0, new Guid("00000000-0000-0000-0000-000000000000"), (short)1, null, (short)1, 0m, 0m, "NI", "NI", null, "NI" });

            migrationBuilder.InsertData(
                table: "ERPxPessoaFisica",
                columns: new[] { "ERPxPessoaFisicaID", "ERPxGeneroID", "Nascimento" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000000"), (short)0, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "ERPxPessoaFisica",
                columns: new[] { "ERPxPessoaFisicaID", "CORxStatusID", "ERPxGeneroID", "Nascimento" },
                values: new object[] { new Guid("f4b32152-8189-4525-bcee-a6a62a290b38"), (short)1, (short)0, new DateTime(1975, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.InsertData(
                table: "ERPxPessoaFisicaTipos",
                columns: new[] { "ERPxPessoaFisicaTiposID", "CORxStatusID", "ERPxPessoaFisicaID", "ERPxPessoaFisicaTipoID" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000000"), (short)1, new Guid("f4b32152-8189-4525-bcee-a6a62a290b38"), (short)1 });

            migrationBuilder.InsertData(
                table: "ERPxProfissional",
                columns: new[] { "ERPxProfissionalID", "CORxStatusID", "_ERPxPessoaFisicaERPxPessoaFisicaID" },
                values: new object[] { new Guid("00000000-0000-0000-0000-000000000000"), (short)0, null });

            migrationBuilder.CreateIndex(
                name: "IX_8DD4566AC9AB45DC8B0B2EB79544A8C7",
                table: "EMLxAnexo",
                column: "EMLxCaixaID");

            migrationBuilder.CreateIndex(
                name: "IX_66241D59536E47B49E34165C7A8239F1",
                table: "EMLxCaixa",
                column: "EMLxEstadoID");

            migrationBuilder.CreateIndex(
                name: "IX_740A2C9FCA9E45B7A8474CF152CC7787",
                table: "EMLxCaixa",
                column: "SYSxEmitenteID");

            migrationBuilder.CreateIndex(
                name: "IX_BF992DDE37E7499BA349BF3A1D51E7FC",
                table: "EMLxCaixa",
                column: "EMLxServidorID");

            migrationBuilder.CreateIndex(
                name: "IX_6016D7428D524850B145B80FE6A42488",
                table: "EMLxDestinatario",
                column: "EMLxCaixaID");

            migrationBuilder.CreateIndex(
                name: "IX_F0EFA1AC90904286A4F197BDD9B374DE",
                table: "EMLxDestinatario",
                column: "ERPxContatoID");

            migrationBuilder.CreateIndex(
                name: "IX_1B5E64FF1C794BC4A7BB0A2180621E4D",
                table: "EMLxEmpresaServidor",
                column: "CORxPessoaID");

            migrationBuilder.CreateIndex(
                name: "IX_B7B582743EC343B69CEFAE26C3488E0F",
                table: "EMLxEmpresaServidor",
                column: "EMLxServidorID");

            migrationBuilder.CreateIndex(
                name: "IX_AF99BCD81EE54C5CAF4C709A847F830D",
                table: "EMLxLog",
                column: "EMLxCaixaID");

            migrationBuilder.CreateIndex(
                name: "IX_DC14ECA948514440A1C13AEBCE33BD32",
                table: "EMLxServidor",
                column: "EMLxServidorFinalizadeID");

            migrationBuilder.CreateIndex(
                name: "IX_0E654D4AED51402E9FF6E98504AEB5CA",
                table: "ERPxContato",
                column: "CORxPessoaID");

            migrationBuilder.CreateIndex(
                name: "IX_497F7B77DB22487891CEF147BB460A8B",
                table: "ERPxContato",
                column: "ERPxFinalidadeID");

            migrationBuilder.CreateIndex(
                name: "IX_51C6F265549348069F2821C19BFCE13E",
                table: "ERPxContato",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_95C55966E7914D66A601A3EBAA8AB674",
                table: "ERPxContato",
                column: "ERPxContatoTipoID");

            migrationBuilder.CreateIndex(
                name: "IX_F37B5226_0067_4E6D_912E_7E42D3C4BADA",
                table: "ERPxContato",
                columns: new[] { "CORxPessoaID", "Contato" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_530206",
                table: "ERPxDocumento",
                columns: new[] { "ERPxDocumentoTipoID", "CORxPessoaID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_5F7E452D79C944328446AAD57587917C",
                table: "ERPxDocumento",
                column: "CORxPessoaID");

            migrationBuilder.CreateIndex(
                name: "IX_601892",
                table: "ERPxDocumento",
                columns: new[] { "Numero", "ERPxDocumentoTipoID", "CORxStatusID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_7B2BB2D7B5DD4C2C9FBCFAFE802FB8B3",
                table: "ERPxDocumento",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_A31E2B713C84465F8D7C5CC4F735F8F7",
                table: "ERPxDocumento",
                column: "ERPxDocumentoTipoID");

            migrationBuilder.CreateIndex(
                name: "IX_12E333D9_332C_41E8_B1B4_8284251435D1",
                table: "ERPxEndereco",
                columns: new[] { "CORxPessoaID", "CORxStatusID", "ERPxEnderecoID", "CEPxLogradouroID", "ERPxFinalidadeID", "Quadra", "Lote", "Latitude", "Longitude", "Complemento", "Numero" });

            migrationBuilder.CreateIndex(
                name: "IX_24448961F77B4CAC9D842F76E306CF5B",
                table: "ERPxEndereco",
                column: "CEPxLogradouroID");

            migrationBuilder.CreateIndex(
                name: "IX_6E425692A0734A0785AF5E5650EAE7F8",
                table: "ERPxEndereco",
                column: "ERPxFinalidadeID");

            migrationBuilder.CreateIndex(
                name: "IX_B9782A035C6C4ACC9AA722AB30D5DC89",
                table: "ERPxEndereco",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_F92D21F245294E54AE7006F94BFE60C7",
                table: "ERPxEndereco",
                column: "CORxPessoaID");

            migrationBuilder.CreateIndex(
                name: "IX_A4E089E6F08C47DEA7A2093E7207245A",
                table: "ERPxFornecedor",
                column: "ERPxFornecedorID");

            migrationBuilder.CreateIndex(
                name: "IX_ERPxFornecedor__ERPxPessoaJuridicaERPxPessoaJuridicaID",
                table: "ERPxFornecedor",
                column: "_ERPxPessoaJuridicaERPxPessoaJuridicaID");

            migrationBuilder.CreateIndex(
                name: "IX_29E58B640111432D9BDB6C2801597F81",
                table: "ERPxPessoaFisica",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_E0DF81FD6086469CB2A8E8FB6B3021FA",
                table: "ERPxPessoaFisica",
                column: "ERPxGeneroID");

            migrationBuilder.CreateIndex(
                name: "IX_15335A7B_CC5F_4F8E_9FBA_7B47C7B9FFEA",
                table: "ERPxPessoaFisicaTipos",
                columns: new[] { "ERPxPessoaFisicaID", "ERPxPessoaFisicaTipoID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_C3C07B29B31C4BE1B90079B3256A0572",
                table: "ERPxPessoaFisicaTipos",
                column: "ERPxPessoaFisicaID");

            migrationBuilder.CreateIndex(
                name: "IX_D1BEE9AA310A476EAA08D7D9A46961A1",
                table: "ERPxPessoaFisicaTipos",
                column: "ERPxPessoaFisicaTipoID");

            migrationBuilder.CreateIndex(
                name: "IX_D5C25A2F357B4DC58334CE40FF90A20B",
                table: "ERPxPessoaFisicaTipos",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_1183A3DBAC464B44986F8D45935BB8E4",
                table: "ERPxPessoaJuridica",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_9B7B5569232745D4B3C4181A55269727",
                table: "ERPxProfissional",
                column: "CORxStatusID");

            migrationBuilder.CreateIndex(
                name: "IX_CCAFAC3553BA4F51A0F61624663A299D",
                table: "ERPxProfissional",
                column: "ERPxProfissionalID");

            migrationBuilder.CreateIndex(
                name: "IX_ERPxProfissional__ERPxPessoaFisicaERPxPessoaFisicaID",
                table: "ERPxProfissional",
                column: "_ERPxPessoaFisicaERPxPessoaFisicaID");

            migrationBuilder.CreateIndex(
                name: "IX_597572",
                table: "ERPxProfissionalCategoria",
                columns: new[] { "ERPxProfissionalID", "ERPxCategoriaID" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_C64D39DCDE244E23843A0B80799F2BE2",
                table: "ERPxProfissionalCategoria",
                column: "ERPxCategoriaID");

            migrationBuilder.CreateIndex(
                name: "IX_FA9890819D9B4F36B642E078DBA111C6",
                table: "ERPxProfissionalCategoria",
                column: "ERPxProfissionalID");

            migrationBuilder.CreateIndex(
                name: "IX_6789BA23775349D993D8B002E038C3B8",
                table: "ERPxProfissionalHorario",
                column: "ERPxProfissionalHorarioTipoID");

            migrationBuilder.CreateIndex(
                name: "IX_AFEC2A2EE5804F908FD8FE09629C9A5F",
                table: "ERPxProfissionalHorario",
                column: "ERPxProfissionalID");

            migrationBuilder.CreateIndex(
                name: "IX_CDC71151B0274EDC84B4475F4717DAA6",
                table: "ERPxProfissionalHorario",
                column: "CORxStatusID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EMLxAnexo");

            migrationBuilder.DropTable(
                name: "EMLxDestinatario");

            migrationBuilder.DropTable(
                name: "EMLxEmpresaServidor");

            migrationBuilder.DropTable(
                name: "EMLxLog");

            migrationBuilder.DropTable(
                name: "ERPxDocumento");

            migrationBuilder.DropTable(
                name: "ERPxEndereco");

            migrationBuilder.DropTable(
                name: "ERPxFornecedor");

            migrationBuilder.DropTable(
                name: "ERPxPessoaFisicaTipos");

            migrationBuilder.DropTable(
                name: "ERPxProfissionalCategoria");

            migrationBuilder.DropTable(
                name: "ERPxProfissionalHorario");

            migrationBuilder.DropTable(
                name: "ERPxContato");

            migrationBuilder.DropTable(
                name: "EMLxCaixa");

            migrationBuilder.DropTable(
                name: "ERPxDocumentoTipo");

            migrationBuilder.DropTable(
                name: "ERPxPessoaJuridica");

            migrationBuilder.DropTable(
                name: "ERPxPessoaFisicaTipo");

            migrationBuilder.DropTable(
                name: "ERPxCategoria");

            migrationBuilder.DropTable(
                name: "ERPxProfissional");

            migrationBuilder.DropTable(
                name: "ERPxProfissionalHorarioTipo");

            migrationBuilder.DropTable(
                name: "ERPxFinalidade");

            migrationBuilder.DropTable(
                name: "ERPxContatoTipo");

            migrationBuilder.DropTable(
                name: "EMLxEstado");

            migrationBuilder.DropTable(
                name: "EMLxServidor");

            migrationBuilder.DropTable(
                name: "ERPxPessoaFisica");

            migrationBuilder.DropTable(
                name: "EMLxServidorFinalizade");

            migrationBuilder.DropTable(
                name: "ERPxGenero");
        }
    }
}
