using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TFX.Core.CEP.Migrations
{
    /// <inheritdoc />
    public partial class Inicial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CEPxLocalidadeTipo",
                columns: table => new
                {
                    CEPxLocalidadeTipoID = table.Column<short>(type: "smallint", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEPxLocalidadeTipo", x => x.CEPxLocalidadeTipoID);
                });

            migrationBuilder.CreateTable(
                name: "CEPxPais",
                columns: table => new
                {
                    CEPxPaisID = table.Column<short>(type: "smallint", nullable: false),
                    BACEN = table.Column<string>(type: "varchar(6)", maxLength: 6, nullable: false),
                    Nome = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false),
                    Sigla = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEPxPais", x => x.CEPxPaisID);
                });

            migrationBuilder.CreateTable(
                name: "CEPxUF",
                columns: table => new
                {
                    CEPxUFID = table.Column<short>(type: "smallint", nullable: false),
                    CEPFinal = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    CEPInicial = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: false),
                    CEPxPaisID = table.Column<short>(type: "smallint", nullable: false),
                    Nome = table.Column<string>(type: "varchar(45)", maxLength: 45, nullable: false),
                    Sigla = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEPxUF", x => x.CEPxUFID);
                    table.ForeignKey(
                        name: "FK_465268",
                        column: x => x.CEPxPaisID,
                        principalTable: "CEPxPais",
                        principalColumn: "CEPxPaisID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CEPxLocalidade",
                columns: table => new
                {
                    CEPxLocalidadeID = table.Column<int>(type: "int", nullable: false),
                    CEPGeral = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true, defaultValue: ""),
                    CEPxLocalidadeTipoID = table.Column<short>(type: "smallint", nullable: false),
                    CEPxMunicipioID = table.Column<int>(type: "int", nullable: false),
                    CEPxUFID = table.Column<short>(type: "smallint", nullable: false),
                    CodigoIBGE = table.Column<string>(type: "varchar(7)", maxLength: 7, nullable: true),
                    Nome = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEPxLocalidade", x => x.CEPxLocalidadeID);
                    table.ForeignKey(
                        name: "FK_465269",
                        column: x => x.CEPxUFID,
                        principalTable: "CEPxUF",
                        principalColumn: "CEPxUFID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_465532",
                        column: x => x.CEPxLocalidadeTipoID,
                        principalTable: "CEPxLocalidadeTipo",
                        principalColumn: "CEPxLocalidadeTipoID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CEPxBairro",
                columns: table => new
                {
                    CEPxBairroID = table.Column<int>(type: "int", nullable: false),
                    Breviatura = table.Column<string>(type: "varchar(25)", maxLength: 25, nullable: true),
                    CEPxLocalidadeID = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEPxBairro", x => x.CEPxBairroID);
                    table.ForeignKey(
                        name: "FK_500579",
                        column: x => x.CEPxLocalidadeID,
                        principalTable: "CEPxLocalidade",
                        principalColumn: "CEPxLocalidadeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "CEPxLogradouro",
                columns: table => new
                {
                    CEPxLogradouroID = table.Column<int>(type: "int", nullable: false),
                    CEP = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true),
                    CEPxBairroID = table.Column<int>(type: "int", nullable: false),
                    CEPxLocalidadeID = table.Column<int>(type: "int", nullable: false),
                    Nome = table.Column<string>(type: "varchar(128)", maxLength: 128, nullable: false),
                    Numero = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CEPxLogradouro", x => x.CEPxLogradouroID);
                    table.ForeignKey(
                        name: "FK_593024",
                        column: x => x.CEPxBairroID,
                        principalTable: "CEPxBairro",
                        principalColumn: "CEPxBairroID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_593032",
                        column: x => x.CEPxLocalidadeID,
                        principalTable: "CEPxLocalidade",
                        principalColumn: "CEPxLocalidadeID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "CEPxLocalidadeTipo",
                columns: new[] { "CEPxLocalidadeTipoID", "Tipo" },
                values: new object[,]
                {
                    { (short)1, "Município" },
                    { (short)2, "Distrito" },
                    { (short)3, "Povoado" }
                });

            migrationBuilder.InsertData(
                table: "CEPxPais",
                columns: new[] { "CEPxPaisID", "BACEN", "Nome", "Sigla" },
                values: new object[,]
                {
                    { (short)0, "0", "NA", "NA" },
                    { (short)1, "1058", "Brasil", "BR" },
                    { (short)2, "7560", "África do Sul", "NI" },
                    { (short)3, "0175", "República da Albânia", "NI" },
                    { (short)4, "0230", "Alemanha", "NI" },
                    { (short)5, "0370", "Andorra", "NI" },
                    { (short)6, "0400", "Angola", "NI" },
                    { (short)7, "0418", "Anguilla", "NI" },
                    { (short)8, "0434", "Antigua e Barbuda", "NI" },
                    { (short)9, "0477", "Antilhas Holandesas", "NI" },
                    { (short)10, "0531", "Arábia Saudita", "NI" },
                    { (short)11, "0590", "Argélia", "NI" },
                    { (short)12, "0639", "Argentina", "NI" },
                    { (short)13, "0647", "República da Armênia", "NI" },
                    { (short)14, "0655", "Aruba", "NI" },
                    { (short)15, "0698", "Austrália", "NI" },
                    { (short)16, "0728", "Áustria", "NI" },
                    { (short)17, "0736", "República do Azerbaijão", "NI" },
                    { (short)18, "0779", "Ilhas Bahamas", "NI" },
                    { (short)19, "0809", "Ilhas Bahrein", "NI" },
                    { (short)20, "0817", "Bangladesh", "NI" },
                    { (short)21, "0833", "Barbados", "NI" },
                    { (short)22, "0850", "Belarus", "NI" },
                    { (short)23, "0876", "Bélgica", "NI" },
                    { (short)24, "0884", "Belize", "NI" },
                    { (short)25, "2291", "Benin", "NI" },
                    { (short)26, "0906", "Bermudas", "NI" },
                    { (short)27, "0973", "Bolívia", "NI" },
                    { (short)28, "0981", "Bósnia-Herzegovina", "NI" },
                    { (short)29, "1015", "Botsuana", "NI" },
                    { (short)31, "1082", "Brunei", "NI" },
                    { (short)32, "1112", "República da Bulgária", "NI" },
                    { (short)33, "0310", "Burkina Faso", "NI" },
                    { (short)34, "1155", "Burundi", "NI" },
                    { (short)35, "1198", "Butão", "NI" },
                    { (short)36, "1279", "República de Cabo Verde", "NI" },
                    { (short)37, "1457", "Camarões", "NI" },
                    { (short)38, "1414", "Camboja", "NI" },
                    { (short)39, "1490", "Canadá", "NI" },
                    { (short)40, "1504", "Ilhas do (Jersey e Guernsey) Canal", "NI" },
                    { (short)41, "1511", "Ilhas Canárias", "NI" },
                    { (short)42, "1546", "Catar", "NI" },
                    { (short)43, "1376", "Ilhas Cayman", "NI" },
                    { (short)44, "1538", "República do Cazaquistão", "NI" },
                    { (short)45, "7889", "Chade", "NI" },
                    { (short)46, "1589", "Chile", "NI" },
                    { (short)47, "1600", "República Popular da China", "NI" },
                    { (short)48, "1635", "Chipre", "NI" },
                    { (short)49, "5118", "Ilha (Navidad) Christmas", "NI" },
                    { (short)50, "7412", "Cingapura", "NI" },
                    { (short)51, "1651", "Ilhas Cocos (Keeling", "NI" },
                    { (short)52, "1694", "Colômbia", "NI" },
                    { (short)53, "1732", "Ilhas Comores", "NI" },
                    { (short)54, "8885", "República Democrática do Congo", "NI" },
                    { (short)55, "1775", "República do Congo", "NI" },
                    { (short)56, "1830", "Ilhas Cook", "NI" },
                    { (short)57, "1872", "Rep. Pop. Democrática da Coréia", "NI" },
                    { (short)58, "1902", "República da Coréia", "NI" },
                    { (short)59, "1937", "Costa do Marfim", "NI" },
                    { (short)60, "1961", "Costa Rica", "NI" },
                    { (short)61, "1988", "Coveite", "NI" },
                    { (short)62, "1953", "República da Croácia", "NI" },
                    { (short)63, "1996", "Cuba", "NI" },
                    { (short)64, "2321", "Dinamarca", "NI" },
                    { (short)65, "7838", "Djibuti", "NI" },
                    { (short)66, "2356", "Ilha Dominica", "NI" },
                    { (short)67, "402 ", "gito", "NI" },
                    { (short)68, "6874", "El Salvador", "NI" },
                    { (short)69, "2445", "Emirados Árabes Unidos", "NI" },
                    { (short)70, "2399", "Equador", "NI" },
                    { (short)71, "2437", "Eritréia", "NI" },
                    { (short)72, "6289", "Escócia", "NI" },
                    { (short)73, "2470", "República Eslovaca", "NI" },
                    { (short)74, "2461", "República da Eslovênia", "NI" },
                    { (short)75, "2453", "Espanha", "NI" },
                    { (short)76, "2496", "Estados Unidos", "NI" },
                    { (short)77, "2518", "República da Estônia", "NI" },
                    { (short)78, "2534", "Etiópia", "NI" },
                    { (short)79, "2550", "Falkland (Ilhas Malvinas)", "NI" },
                    { (short)80, "2593", "Ilhas Feroe", "NI" },
                    { (short)81, "8702", "Fiji", "NI" },
                    { (short)82, "2674", "Filipinas", "NI" },
                    { (short)83, "2712", "Finlândia", "NI" },
                    { (short)84, "1619", "Formosa (Taiwan)", "NI" },
                    { (short)85, "2755", "França", "NI" },
                    { (short)86, "2810", "Gabão", "NI" },
                    { (short)87, "6289", "País de Gales", "NI" },
                    { (short)88, "2852", "Gâmbia", "NI" },
                    { (short)89, "2895", "Gana", "NI" },
                    { (short)90, "2917", "República da Geórgia", "NI" },
                    { (short)91, "2933", "Gibraltar", "NI" },
                    { (short)92, "6289", "Grã-Bretanha", "NI" },
                    { (short)93, "2976", "Granada", "NI" },
                    { (short)94, "3018", "Grécia", "NI" },
                    { (short)95, "3050", "Groenlândia", "NI" },
                    { (short)96, "3093", "Guadalupe", "NI" },
                    { (short)97, "3131", "Guam", "NI" },
                    { (short)98, "3174", "Guatemala", "NI" },
                    { (short)99, "3379", "Guiana", "NI" },
                    { (short)100, "3255", "Guiana Francesa", "NI" },
                    { (short)101, "3298", "Guiné", "NI" },
                    { (short)102, "3344", "Guiné-Bissau", "NI" },
                    { (short)103, "3310", "Guiné-Equatorial", "NI" },
                    { (short)104, "3417", "Haiti", "NI" },
                    { (short)105, "5738", "Holanda (Países Baixos)", "NI" },
                    { (short)106, "3450", "Honduras", "NI" },
                    { (short)107, "3514", "Região Adm. Especial Hong Kong", "NI" },
                    { (short)108, "3557", "República da Hungria", "NI" },
                    { (short)109, "3573", "Iêmen", "NI" },
                    { (short)110, "3611", "Índia", "NI" },
                    { (short)111, "3654", "Indonésia", "NI" },
                    { (short)112, "6289", "Inglaterra", "NI" },
                    { (short)113, "3727", "República Islâmica do Irã", "NI" },
                    { (short)114, "3697", "Iraque", "NI" },
                    { (short)115, "3751", "Irlanda", "NI" },
                    { (short)116, "6289", "Irlanda do Norte", "NI" },
                    { (short)117, "3794", "Islândia", "NI" },
                    { (short)118, "3832", "Israel", "NI" },
                    { (short)119, "3867", "Itália", "NI" },
                    { (short)120, "3883", "República Fed. da Iugoslávia", "NI" },
                    { (short)121, "3913", "Jamaica", "NI" },
                    { (short)122, "3999", "Japão", "NI" },
                    { (short)123, "3964", "Ilhas Johnston", "NI" },
                    { (short)124, "4030", "Jordânia", "NI" },
                    { (short)125, "4111", "Kiribati", "NI" },
                    { (short)126, "4200", "Rep. Pop. Democrática do Laos", "NI" },
                    { (short)127, "4235", "Lebuan", "NI" },
                    { (short)128, "4260", "Lesoto", "NI" },
                    { (short)129, "4278", "República da Letônia", "NI" },
                    { (short)130, "4316", "Líbano", "NI" },
                    { (short)131, "4340", "Libéria", "NI" },
                    { (short)132, "4383", "Líbia", "NI" },
                    { (short)133, "4405", "Liechtenstein", "NI" },
                    { (short)134, "4421", "República da Lituânia", "NI" },
                    { (short)135, "4456", "Luxemburgo", "NI" },
                    { (short)136, "4472", "Macau", "NI" },
                    { (short)137, "4499", "Macedônia", "NI" },
                    { (short)138, "4502", "Madagascar", "NI" },
                    { (short)139, "4525", "Ilha da Madeira", "NI" },
                    { (short)140, "4553", "Malásia", "NI" },
                    { (short)141, "4588", "Malavi", "NI" },
                    { (short)142, "4618", "Maldivas", "NI" },
                    { (short)143, "4642", "Máli", "NI" },
                    { (short)144, "4677", "Malta", "NI" },
                    { (short)145, "3595", "Ilhas Man", "NI" },
                    { (short)146, "4723", "Marianas do Norte", "NI" },
                    { (short)147, "4740", "Marrocos", "NI" },
                    { (short)148, "4766", "Ilhas Marshall", "NI" },
                    { (short)149, "4774", "Martinica", "NI" },
                    { (short)150, "4855", "Maurício", "NI" },
                    { (short)151, "4880", "Mauritânia", "NI" },
                    { (short)152, "4936", "México", "NI" },
                    { (short)153, "0930", "Mianmar (Birmânia)", "NI" },
                    { (short)154, "4995", "Micronésia", "NI" },
                    { (short)155, "4901", "Ilhas Midway", "NI" },
                    { (short)156, "5053", "Moçambique", "NI" },
                    { (short)157, "4944", "República da Moldávia", "NI" },
                    { (short)158, "4952", "Mônaco", "NI" },
                    { (short)159, "4979", "Mongólia", "NI" },
                    { (short)160, "5010", "Ilhas Montserrat", "NI" },
                    { (short)161, "5070", "Namíbia", "NI" },
                    { (short)162, "5088", "Nauru", "NI" },
                    { (short)163, "5177", "Nepal", "NI" },
                    { (short)164, "5215", "Nicarágua", "NI" },
                    { (short)165, "5258", "Niger", "NI" },
                    { (short)166, "5282", "Nigéria", "NI" },
                    { (short)167, "5312", "Ilha Niue", "NI" },
                    { (short)168, "5355", "Ilha Norfolk", "NI" },
                    { (short)169, "5380", "Noruega", "NI" },
                    { (short)170, "5428", "Nova Caledônia", "NI" },
                    { (short)171, "5487", "Nova Zelândia", "NI" },
                    { (short)172, "5568", "Omã", "NI" },
                    { (short)173, "5738", "Países Baixos (Holanda)", "NI" },
                    { (short)174, "5754", "Palau", "NI" },
                    { (short)175, "5800", "Panamá", "NI" },
                    { (short)176, "5452", "Papua Nova Guiné", "NI" },
                    { (short)177, "5762", "Paquistão", "NI" },
                    { (short)178, "5860", "Paraguai", "NI" },
                    { (short)179, "5894", "Peru", "NI" },
                    { (short)180, "5932", "Ilha Pitcairn", "NI" },
                    { (short)181, "5991", "Polinésia Francesa", "NI" },
                    { (short)182, "6033", "República da Polônia", "NI" },
                    { (short)183, "6114", "Porto Rico", "NI" },
                    { (short)184, "6076", "Portugal", "NI" },
                    { (short)185, "6238", "Quênia", "NI" },
                    { (short)186, "6254", "República Quirguiz", "NI" },
                    { (short)187, "6289", "Reino Unido", "NI" },
                    { (short)188, "6408", "República Centro-Africana", "NI" },
                    { (short)189, "6475", "República Dominicana", "NI" },
                    { (short)190, "6602", "Ilha Reunião", "NI" },
                    { (short)191, "6700", "Romênia", "NI" },
                    { (short)192, "6750", "Ruanda", "NI" },
                    { (short)193, "6769", "Rússia", "NI" },
                    { (short)194, "6858", "Saara Ocidental", "NI" },
                    { (short)195, "6777", "Ilhas Salomão", "NI" },
                    { (short)196, "6904", "Samoa", "NI" },
                    { (short)197, "6912", "Samoa Americana", "NI" },
                    { (short)198, "6971", "San Marino", "NI" },
                    { (short)199, "7102", "Santa Helena", "NI" },
                    { (short)200, "7153", "Santa Lúcia", "NI" },
                    { (short)201, "6955", "São Cristóvão e Neves", "NI" },
                    { (short)202, "7005", "São Pedro e Miquelon", "NI" },
                    { (short)203, "7200", "Ilhas São Tomé e Príncipe", "NI" },
                    { (short)204, "7056", "São Vicente e Granadinas", "NI" },
                    { (short)205, "7285", "Senegal", "NI" },
                    { (short)206, "7358", "Serra Leoa", "NI" },
                    { (short)207, "7315", "Seychelles", "NI" },
                    { (short)208, "7447", "República Árabe da Síria", "NI" },
                    { (short)209, "7480", "Somália", "NI" },
                    { (short)210, "7501", "Sri Lanka", "NI" },
                    { (short)211, "7544", "Suazilândia", "NI" },
                    { (short)212, "7595", "Sudão", "NI" },
                    { (short)213, "7641", "Suécia", "NI" },
                    { (short)214, "7676", "Suíça", "NI" },
                    { (short)215, "7706", "Suriname", "NI" },
                    { (short)216, "7722", "Tadjiquistão", "NI" },
                    { (short)217, "7765", "Tailândia", "NI" },
                    { (short)218, "7803", "República Unida da Tanzânia", "NI" },
                    { (short)219, "7919", "República Tcheca", "NI" },
                    { (short)220, "7820", "Território Britânico Oc. Índico", "NI" },
                    { (short)221, "7951", "Timor Leste", "NI" },
                    { (short)222, "8001", "Togo", "NI" },
                    { (short)223, "8109", "Tonga", "NI" },
                    { (short)224, "8052", "Ilhas Toquelau", "NI" },
                    { (short)225, "8150", "Trinidad e Tobago", "NI" },
                    { (short)226, "8206", "Tunísia", "NI" },
                    { (short)227, "8230", "Ilhas Turcas e Caicos", "NI" },
                    { (short)228, "8249", "República do Turcomenistão", "NI" },
                    { (short)229, "8273", "Turquia", "NI" },
                    { (short)230, "8281", "Tuvalu", "NI" },
                    { (short)231, "8311", "Ucrânia", "NI" },
                    { (short)232, "8338", "Uganda", "NI" },
                    { (short)233, "8451", "Uruguai", "NI" },
                    { (short)234, "8478", "República do Uzbequistão", "NI" },
                    { (short)235, "5517", "Vanuatu", "NI" },
                    { (short)236, "8486", "Estado da Cidade do Vaticano", "NI" },
                    { (short)237, "8508", "Venezuela", "NI" },
                    { (short)238, "8583", "Vietnã", "NI" },
                    { (short)239, "8630", "Ilhas (Britânicas) Virgens", "NI" },
                    { (short)240, "8664", "Ilhas (E.U.A.) Virgens", "NI" },
                    { (short)241, "8737", "Ilha Wake", "NI" },
                    { (short)242, "8753", "Ilhas Wallis e Futuna", "NI" },
                    { (short)243, "8907", "Zâmbia", "NI" },
                    { (short)244, "6653", "Zimbábue", "NI" },
                    { (short)245, "8958", "Zona do Canal do Panamá", "NI" },
                    { (short)246, "0132", "Afeganistão", "NI" }
                });

            migrationBuilder.InsertData(
                table: "CEPxUF",
                columns: new[] { "CEPxUFID", "CEPFinal", "CEPInicial", "CEPxPaisID", "Nome", "Sigla" },
                values: new object[,]
                {
                    { (short)0, "0", "0", (short)0, "NA", "NA" },
                    { (short)1, "69999999", "69900000", (short)1, "Acre", "AC" },
                    { (short)2, "0", "0", (short)1, "Alagoas", "Al" },
                    { (short)3, "0", "0", (short)1, "Amazonas Amapá", "AM" },
                    { (short)4, "0", "0", (short)1, "Amapá", "AP" },
                    { (short)5, "0", "0", (short)1, "Bahia", "BA" },
                    { (short)6, "0", "0", (short)1, "Ceará", "CE" },
                    { (short)7, "0", "0", (short)1, "Distrito Federal", "DF" },
                    { (short)8, "0", "0", (short)1, "Espírito Santo", "ES" },
                    { (short)9, "0", "0", (short)1, "Goiás", "GO" },
                    { (short)10, "0", "0", (short)1, "Maranhão", "MA" },
                    { (short)11, "0", "0", (short)1, "Minas Gerais", "MG" },
                    { (short)12, "0", "0", (short)1, "Mato Grosso do Sul", "MS" },
                    { (short)13, "0", "0", (short)1, "Mato Grosso", "MT" },
                    { (short)14, "0", "0", (short)1, "Pará", "PA" },
                    { (short)15, "0", "0", (short)1, "Paraíba", "PB" },
                    { (short)16, "0", "0", (short)1, "Pernambuco", "PE" },
                    { (short)17, "0", "0", (short)1, "Piauí", "PI" },
                    { (short)18, "0", "0", (short)1, "Paraná", "PR" },
                    { (short)19, "0", "0", (short)1, "Rio de Janeiro", "RJ" },
                    { (short)20, "0", "0", (short)1, "Rio Grande do Norte", "RN" },
                    { (short)21, "0", "0", (short)1, "Rondônia", "RO" },
                    { (short)22, "0", "0", (short)1, "Roraima", "RR" },
                    { (short)23, "0", "0", (short)1, "Rio Grande do Sul", "RS" },
                    { (short)24, "0", "0", (short)1, "Santa Catarina", "SC" },
                    { (short)25, "0", "0", (short)1, "Sergipe", "SE" },
                    { (short)26, "0", "0", (short)1, "São Paulo", "SP" },
                    { (short)27, "0", "0", (short)1, "Tocantins", "TO" }
                });

            migrationBuilder.InsertData(
                table: "CEPxLocalidade",
                columns: new[] { "CEPxLocalidadeID", "CEPGeral", "CEPxLocalidadeTipoID", "CEPxMunicipioID", "CEPxUFID", "CodigoIBGE", "Nome", "Numero" },
                values: new object[] { 0, "00000000", (short)3, 0, (short)0, "0", "NA", 0 });

            migrationBuilder.InsertData(
                table: "CEPxBairro",
                columns: new[] { "CEPxBairroID", "Breviatura", "CEPxLocalidadeID", "Nome", "Numero" },
                values: new object[] { 0, null, 0, "NI", 0 });

            migrationBuilder.InsertData(
                table: "CEPxLogradouro",
                columns: new[] { "CEPxLogradouroID", "CEP", "CEPxBairroID", "CEPxLocalidadeID", "Nome", "Numero", "Tipo" },
                values: new object[] { 0, null, 0, 0, "NI", 0, "NA" });

            migrationBuilder.CreateIndex(
                name: "IX_3564A5BB_8819_4E16_A38B_E4F264F0A8DB",
                table: "CEPxBairro",
                columns: new[] { "Nome", "CEPxLocalidadeID" });

            migrationBuilder.CreateIndex(
                name: "IX_732AFBB8F10740A098508B433A239566",
                table: "CEPxBairro",
                column: "CEPxLocalidadeID");

            migrationBuilder.CreateIndex(
                name: "IX_4A9B3534_DFB0_4E42_AA40_F5A8171D216C",
                table: "CEPxLocalidade",
                columns: new[] { "CEPxUFID", "Nome" });

            migrationBuilder.CreateIndex(
                name: "IX_EC80C804EAF744DB9582BB8DA9DC0E95",
                table: "CEPxLocalidade",
                column: "CEPxUFID");

            migrationBuilder.CreateIndex(
                name: "IX_F194D226827240E5B30F67C33A8AEF7F",
                table: "CEPxLocalidade",
                column: "CEPxLocalidadeTipoID");

            migrationBuilder.CreateIndex(
                name: "IX_302AE97447CE45F2A22CB90E0C2DEFF5",
                table: "CEPxLogradouro",
                column: "CEPxBairroID");

            migrationBuilder.CreateIndex(
                name: "IX_A8AD4BE588A64964A2E744F7B40E9142",
                table: "CEPxLogradouro",
                column: "CEPxLocalidadeID");

            migrationBuilder.CreateIndex(
                name: "IX_C83930E8_5C82_4ED4_AD4F_22B7A8392666",
                table: "CEPxLogradouro",
                columns: new[] { "CEPxBairroID", "CEPxLocalidadeID", "CEPxLogradouroID", "Tipo", "Nome", "CEP" });

            migrationBuilder.CreateIndex(
                name: "IX_94F98F7A928E46699CECF2DD030D18C2",
                table: "CEPxUF",
                column: "CEPxPaisID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CEPxLogradouro");

            migrationBuilder.DropTable(
                name: "CEPxBairro");

            migrationBuilder.DropTable(
                name: "CEPxLocalidade");

            migrationBuilder.DropTable(
                name: "CEPxUF");

            migrationBuilder.DropTable(
                name: "CEPxLocalidadeTipo");

            migrationBuilder.DropTable(
                name: "CEPxPais");
        }
    }
}
