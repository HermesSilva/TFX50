using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

using Microsoft.EntityFrameworkCore;

using TFX.Core;
using TFX.Core.Exceptions;
using TFX.Journal.DB;

namespace TFX.Journal
{
    public class XJournalManager
    {
        private static string _DBTOJournal;
        private static string _DBJournal;
        private static bool _IsOk = false;
        private static List<string> _NotJournaledTables = new List<string>();
        private static List<string> _NotDeletableTables = new List<string>();

        static XJournalManager()
        {                      
            _NotJournaledTables.Add("__EFMigrationsHistory");
        }

        public static void Initialize(DbContext pDBContext)
        {
            if (_IsOk || XDefault.IsDebugTime)
                return;
            //_DBTOJournal = AppSettingsBase.DbToJournal;
            //_DBJournal = AppSettingsBase.DbJournal;
            if (_DBJournal.IsEmpty() || _DBTOJournal.IsEmpty())
                throw new XError("Não é permitido usar Journal se definições da variáveis [SITTAX_DB_APP] e [SITTAX_DB_JOURNAL].");
            _IsOk = true;
            var mng = new XJournalManager();
            mng.Execute(pDBContext);
        }

        public void Execute(DbContext pDBContext)
        {
            using (var dbx = new JNLxDBContex())
            {
                dbx.BeginTransaction();
                var sb = Prepare(dbx);
                var lsttbl = GerarModelos(dbx);
                GerarViews(dbx, lsttbl);
                dbx.SaveChanges();
                dbx.Commit();
                GerarTriggers(pDBContext, lsttbl);
            }
        }

        private void GerarTriggers(DbContext dbx, List<Table> lsttbl)
        {
            foreach (var tbl in lsttbl)
            {
                if (string.IsNullOrEmpty(tbl.PKFieldName) || _NotJournaledTables.Contains(tbl.Name))
                    continue;
                PrepareTrigger(dbx, tbl);
            }
        }

        private void GerarViews(JNLxDBContex dbx, List<Table> lsttbl)
        {
            var views = dbx.Database.SqlQuery<string>(FormattableStringFactory.Create("select TABLE_NAME from INFORMATION_SCHEMA.VIEWS"));
            foreach (var view in views)
                dbx.Database.ExecuteSql(FormattableStringFactory.Create($"drop view {view}"));
            foreach (var tbl in lsttbl)
            {
                if (_NotJournaledTables.Contains(tbl.Name))
                    continue;
                PrepareViews(dbx, tbl);
            }
        }

        private List<Table> GerarModelos(JNLxDBContex ctx)
        {
            var tbls = ctx.Tabelas.Where(t => t.Ativo).ToList();
            var flds = ctx.Campos.Where(f => f.Ativo).ToList();
            var lsttbl = new List<Table>();
            foreach (var tbl in tbls)
            {
                var ntbl = new Table(tbl, flds.Where(f => f.idTabelas == tbl.idTabelas));
                if (ntbl.PKField != null)
                    lsttbl.Add(ntbl);
            }
            return lsttbl;
        }

        private string TableName(Field pField)
        {
            var key = pField.Type.Substring(0, 1).ToUpper() + pField.Type.Substring(1);
            var len = pField.Length;
            switch (pField.Type)
            {
                case "real":
                case "float":
                    return $"Campo{key}";

                case "decimal":
                case "numeric":
                    return $"Campo{key}";

                case "char":
                case "varchar":
                case "varbinary":
                case "nvarchar":
                    switch (len)
                    {
                        case int l when l > 1 && l <= 255:
                            return $"Campo{key}";

                        case int l when l > 255 && l <= 4000:
                            return $"Campo{key}4K";

                        default:
                            return $"Campo{key}Max";
                    }
                case "bigint":
                case "tinyint":
                case "smallint":
                case "datetime":
                case "datetime2":
                case "datetimeoffset":
                case "uniqueidentifier":
                case "date":
                case "int":
                case "bit":
                    return $"Campo{key}";

                case "text":
                    return $"Campo{key}";

                default:
                    throw new Exception($"Tipo de dado [{key}] não previso na Jornalização");
            }
        }

        private void PrepareViews(JNLxDBContex ctx, Table tabela)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Select ");
            foreach (Field fld in tabela.Fields)
                if (fld.Name == tabela.PKField.Name)
                    sb.AppendLine($"PK.Valor as {fld.Name},");
                else
                    sb.AppendLine($"(select C.Valor from {TableName(fld)} C where C.idRevisoes = LT.idRevisoes and C.idCampos = {fld.ID} and C.idCampoPK = PK.idValor) as {fld.Name},");
            sb.AppendLine($"  LT.idRevisoes idRevisoesJournal,");
            sb.AppendLine($"  LT.idTabelas idTabelasJournal,");
            sb.AppendLine($"  LT.Data DataJournal,");
            sb.AppendLine($"  LT.idUsuario idUsuarioJournal,");
            sb.AppendLine($"  LT.idEmpresa idEmpresaJournal,");
            sb.AppendLine($"  LT.idEscritorio idEscritorioJournal,");
            sb.AppendLine($"  LT.idPKTabela idPKTabelaJournal,");
            sb.AppendLine($"  LT.idAcao idAcaoJournal,");
            sb.AppendLine($"  LT.idTransacao idTransacaoJournal,");
            sb.AppendLine($"  LT.Transacao TransacaoJournal,");
            sb.AppendLine($"  LT.Maquina MaquinaJournal,");
            sb.AppendLine($"  LT.Programa ProgramaJournal");
            sb.AppendLine($"  from {TableName(tabela.PKField)} PK");
            sb.AppendLine($"     join Revisoes LT on LT.idRevisoes = PK.idRevisoes");
            sb.AppendLine($"where LT.idTabelas = {tabela.ID} and LT.IsValid = 1 and PK.idCampos = {tabela.PKField.ID}");
            ctx.Database.ExecuteSql(FormattableStringFactory.Create($"create view {tabela.Name} as {sb.ToString()}"));
        }
                     
        private void PrepareTrigger(DbContext ctx, Table tabela)
        {
            var usedelete = !_NotDeletableTables.Contains(tabela.Name);
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"create or alter trigger [JNL_{tabela.Name}] On [{tabela.Name}] After Insert, Delete, Update");
            sb.AppendLine($"as ");
            sb.AppendLine($"begin");
            sb.AppendLine($"  set nocount on");
            sb.AppendLine($"  declare @action tinyint, @i tinyint = 0, @d tinyint = 0");
            sb.AppendLine($"  if exists(select * from inserted)");
            sb.AppendLine($"    set @i = 1");
            sb.AppendLine($"  if exists(select * from deleted)");
            sb.AppendLine($"    set @d = 1");
            sb.AppendLine($"  if @i = 0 and @d = 0");
            sb.AppendLine($"    return");
            sb.AppendLine($"  if @i = 1 and @d = 1");
            sb.AppendLine($"    set @action = {Acao.Alteracao}");
            sb.AppendLine($"  if @i = 1 and @d = 0");
            sb.AppendLine($"    set @action = {Acao.Inclusao}");
            sb.AppendLine($"  if @i = 0 and @d = 1");
            sb.AppendLine($"    set @action = {Acao.Delecao}");
            sb.AppendLine();
            sb.AppendLine($"  declare @idCampoPK bigint");
            sb.AppendLine($"  declare @tranid uniqueidentifier");
            sb.AppendLine($"  declare @idSequencia bigint, @idUsuario uniqueidentifier, @idEscritorio varchar(14), @idEmpresa varchar(14), @idRevisoes bigint");
            sb.AppendLine($"  declare @host varchar(128), @program varchar(128)");
            sb.AppendLine($"  declare @hasdata tinyint = 0");
            sb.AppendLine();
            sb.AppendLine($"   select @host = hostname, @program = program_name from sys.sysprocesses where spid = @@SPID");
            sb.AppendLine($"   select @tranid = convert(uniqueidentifier, coalesce(session_context(N'tranid'),'00000000-0000-0000-0000-000000000000')),");
            sb.AppendLine($"          @idSequencia = convert(bigint, coalesce(session_context(N'idsequencia'),'0')),");
            sb.AppendLine($"          @idUsuario = convert(uniqueidentifier, coalesce(session_context(N'idusuario'),'00000000-0000-0000-0000-000000000000')),");
            sb.AppendLine($"          @idEscritorio = convert(varchar(14), coalesce(session_context(N'idescritorio'),'00000000000000')),");
            sb.AppendLine($"          @idEmpresa = convert(varchar(14), coalesce(session_context(N'idempresa'),'00000000000000'))");
            sb.AppendLine();
            sb.AppendLine($"  insert into [{_DBJournal}]..Revisoes(idTabelas,Data,idUsuario,idEmpresa,idEscritorio,idPKTabela,idAcao,Maquina,Programa,Transacao,idTransacao) ");
            sb.AppendLine($"    (select {tabela.ID}, sysdatetime(), @idUsuario, @idEmpresa, @idEscritorio, {tabela.PKField.ID}, @action, @host, @program, @tranid, current_transaction_id())");
            sb.AppendLine();
            sb.AppendLine($"  select @idRevisoes = @@IDENTITY");
            sb.AppendLine();
            sb.AppendLine($"  insert into [{_DBJournal}]..{TableName(tabela.PKField)}(idRevisoes, idCampoPK, idCampos, Valor) ");
            sb.AppendLine($"    (select  @idRevisoes, 0, {tabela.PKField.ID}, {tabela.PKField.Name} from inserted)");
            sb.AppendLine();
            sb.AppendLine($"  if @action = {Acao.Inclusao}");
            sb.AppendLine($"    return ");
            sb.AppendLine();
            String decfld = String.Join(", ", tabela.Fields.Select(f => $"@o{f.Name} {GetType(f)}, @n{f.Name} {GetType(f)}"));
            sb.AppendLine($"  declare {decfld}");
            sb.AppendLine($"  if @action = {Acao.Alteracao}");
            sb.AppendLine($"  begin");
            String varfld = String.Join(", ", tabela.Fields.Select(f => $"@o{f.Name}, @n{f.Name}"));
            String selfld = String.Join(", ", tabela.Fields.Select(f => $"o.{f.Name} o{f.Name}, n.{f.Name} n{f.Name}"));
            sb.AppendLine($"    declare CSR{tabela.Name} cursor for select {selfld}, c.idValor");
            sb.AppendLine($"      from deleted o join inserted n on o.{tabela.PKField.Name} = n.{tabela.PKField.Name}");
            sb.AppendLine($"					           join [{_DBJournal}]..{TableName(tabela.PKField)} c on c.Valor = n.{tabela.PKField.Name} and c.idRevisoes = @idRevisoes and idCampos =  {tabela.PKField.ID}");
            sb.AppendLine($"    open CSR{tabela.Name}");
            sb.AppendLine($"    while 1 = 1 ");
            sb.AppendLine($"    begin ");
            sb.AppendLine($"        fetch next from CSR{tabela.Name} into {varfld}, @idCampoPK");
            sb.AppendLine($"        if (@@fetch_status != 0) ");
            sb.AppendLine($"            break");
            foreach (Field fld in tabela.Fields)
            {
                if (tabela.PKField.Name == fld.Name)
                    continue;
                sb.AppendLine($"        if (@o{fld.Name} != @n{fld.Name}) ");
                sb.AppendLine($"        begin ");
                sb.AppendLine($"            insert into [{_DBJournal}]..{TableName(fld)}(idRevisoes, idCampos, idCampoPK,  Valor) ");
                sb.AppendLine($"                        values (@idRevisoes, {fld.ID}, @idCampoPK, @o{fld.Name})");
                sb.AppendLine($"            set @hasdata = 1");
                sb.AppendLine($"        end");
            }
            sb.AppendLine($"    end");
            sb.AppendLine($"    if @hasdata = 0");
            sb.AppendLine($"        update [{_DBJournal}]..Revisoes set IsValid = 0 where idRevisoes = @idRevisoes");
            sb.AppendLine($"    close CSR{tabela.Name}");
            sb.AppendLine($"    deallocate CSR{tabela.Name}");
            sb.AppendLine($"    return ");
            sb.AppendLine($"  end;");
            if (usedelete)
            {
                sb.AppendLine($"  if @action = {Acao.Delecao}");
                sb.AppendLine($"  begin");
                decfld = String.Join(", ", tabela.Fields.Select(f => $"d.{f.Name}"));
                varfld = String.Join(", ", tabela.Fields.Select(f => $"@o{f.Name}"));
                sb.AppendLine($"    declare CSR{tabela.Name} cursor for select {decfld}, c.idValor from deleted d");
                sb.AppendLine($"					           join [{_DBJournal}]..{TableName(tabela.PKField)} c on c.Valor = d.{tabela.PKField.Name} and c.idRevisoes = @idRevisoes and idCampos =  {tabela.PKField.ID}");
                sb.AppendLine($"    open CSR{tabela.Name}");
                sb.AppendLine($"    while 1=1 ");
                sb.AppendLine($"    begin ");
                sb.AppendLine($"        fetch next from CSR{tabela.Name} into {varfld}, @idCampoPK");
                sb.AppendLine($"        if (@@fetch_status != 0) ");
                sb.AppendLine($"            break");
                foreach (Field fld in tabela.Fields)
                {
                    if (tabela.PKField.ID == fld.ID)
                        continue;
                    sb.AppendLine($"        insert into [{_DBJournal}]..{TableName(fld)}(idRevisoes, idCampos, Valor, idCampoPK) ");
                    sb.AppendLine($"           values (@idRevisoes, {fld.ID}, @o{fld.Name}, @idCampoPK)");
                }
                sb.AppendLine($"    end");
                sb.AppendLine($"    close CSR{tabela.Name}");
                sb.AppendLine($"    deallocate CSR{tabela.Name}");
                sb.AppendLine($"  end;");
            }
            else
                sb.AppendLine($"        raiserror ('Nao é permitido apagar registros da tabela [{tabela.Name}].', 16, 1)");
            sb.AppendLine($"end");

            //File.WriteAllText(@$"D:\Temp\Triggers\JNL_{tabela.Name}.sql", sb.ToString());
            ctx.Database.ExecuteSql(FormattableStringFactory.Create(sb.ToString()));
        }

        private object GetType(Field campo)
        {
            switch (campo.Type)
            {
                case "decimal":
                case "numeric":
                case "char":
                case "varchar":
                case "varbinary":
                case "nvarchar":
                    return $"{campo.Type}({(campo.Length > 0 ? campo.Length.ToString() : "max")})";

                case "bigint":
                case "tinyint":
                case "smallint":
                case "datetime":
                case "datetime2":
                case "datetimeoffset":
                case "uniqueidentifier":
                case "float":
                case "date":
                case "int":
                case "text":
                case "bit":
                case "real":
                    return campo.Type;

                default:
                    throw new Exception($"Tipo de dado [{campo.Type}] não previso na Jornalização");
            }
        }

        private StringBuilder Prepare(JNLxDBContex ctx)
        {
            ctx.Database.ExecuteSql(FormattableStringFactory.Create(@"
if not exists(select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Tabelas')
CREATE TABLE[Tabelas](
	[idTabelas][int] IDENTITY(1, 1) NOT NULL,
	[Nome] [varchar] (128) NOT NULL,
	[Esquema] [varchar] (128) NOT NULL,
	[CampoPK] [varchar] (128) NOT NULL,
	[TipoPK] [varchar] (128) NOT NULL,
    [Ativo] [bit] not null default 1,
	[RowVersion] [timestamp] NOT NULL,
 CONSTRAINT[PK_Journal_Tabelas] PRIMARY KEY CLUSTERED([idTabelas])
)
"));

            ctx.Database.ExecuteSql(FormattableStringFactory.Create(@"
if not exists(select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Campos')
begin
	CREATE TABLE [Campos](
		[idCampos] [int] IDENTITY(1,1) NOT NULL,
		[idTabelas] [int] NOT NULL,
		[Nome] [varchar](128) NOT NULL,
		[Type] [varchar](30) NOT NULL,
		[State] [bit] NOT NULL,
		[Length] [int] NOT NULL,
		[Scale] [int] NOT NULL,
		[IsPK] [bit] NOT NULL,
	    [Ativo] [bit] not null default 1,
	    [RowVersion] [timestamp] NOT NULL,
	 CONSTRAINT [PK_Journal_Campos] PRIMARY KEY CLUSTERED ([idCampos])
	)

	ALTER TABLE [Campos]  WITH CHECK ADD  CONSTRAINT [FK_Journal_Campos_idTabelas] FOREIGN KEY([idTabelas])
	REFERENCES [Tabelas] ([idTabelas])
end
"));

            ctx.Database.ExecuteSql(FormattableStringFactory.Create(@"
if not exists(select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'Revisoes')
begin
	CREATE TABLE [Revisoes](
		[idRevisoes] [bigint] IDENTITY(1,1) NOT NULL,
		[idTabelas] [int] NOT NULL,
		[Data] [datetime2](7) NOT NULL,
		[idUsuario] [uniqueidentifier] NOT NULL,
		[idEmpresa] varchar(14) NOT NULL,
		[idEscritorio] varchar(14) NOT NULL,
		[idPKTabela] [int] NOT NULL,
		[idAcao] [tinyint] NOT NULL,
		[idTransacao] bigint not null,
		[Transacao] [uniqueidentifier] NOT NULL,
		[Maquina] [varchar](128) NULL,
		[Programa] [varchar](128) NULL,
        [IsValid] [bit] not null default 1,
	    [RowVersion] [timestamp] NOT NULL,
	    CONSTRAINT [PK_Journal_Revisoes] PRIMARY KEY CLUSTERED ([idRevisoes])
	)

	ALTER TABLE [Revisoes] ADD  DEFAULT (newid()) FOR [Transacao]

	ALTER TABLE [Revisoes]  WITH CHECK ADD  CONSTRAINT [FK_Journal_Revisoes_idPKTabela] FOREIGN KEY([idPKTabela]) REFERENCES [Campos] ([idCampos])

	ALTER TABLE [Revisoes]  WITH CHECK ADD  CONSTRAINT [FK_Journal_Revisoes_idTabelas] FOREIGN KEY([idTabelas]) REFERENCES [Tabelas] ([idTabelas])
end
"));
            ctx.Database.ExecuteSql(FormattableStringFactory.Create(@"

if not exists(select 1 from Tabelas where idTabelas = 0)
begin
	set identity_insert Tabelas on
	insert into Tabelas(idTabelas,Nome,Esquema,CampoPK,Ativo,TipoPK) values (0,'NA','NA','NA',0,'NA')
	set identity_insert Tabelas off

	set identity_insert Campos on
	insert into Campos(idCampos,idTabelas,Nome,Type,State,Length,Scale,IsPK,Ativo) values (0,0,'NA','NA',0,0,0,0,0)
	set identity_insert Campos off
end
"));

            ctx.Database.ExecuteSql(FormattableStringFactory.Create(@"
if not exists(select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = 'JNLxLog')
begin
	CREATE TABLE JNLxLog(
		JNLxLogID bigint IDENTITY(1,1) NOT NULL,
		idUsuario uniqueidentifier NOT NULL,
		idEmpresa varchar(14) NOT NULL,
		idEscritorio varchar(14) NOT NULL,
		Data datetime2(7) NOT NULL,
		Tipo varchar(25) not null,
		Mensagem varchar(400) not NULL,
		Detalhe varbinary(6000) NULL,
	    [RowVersion] [timestamp] NOT NULL,
	  CONSTRAINT [PK_JNLxLog] PRIMARY KEY CLUSTERED ([JNLxLogID])
	)
end

"));

            ctx.Database.ExecuteSql(FormattableStringFactory.Create(_SQLPrepareTabelas(_DBTOJournal)));
            ctx.Database.ExecuteSql(FormattableStringFactory.Create(_SQLPrepareCampos(_DBTOJournal)));
            var sb = new StringBuilder();
            sb.AppendLine(PrepareTabelas(ctx).ToString());
            return sb;
        }

        private StringBuilder PrepareTabelas(JNLxDBContex ctx)
        {
            var sb = new StringBuilder();
            var flds = ctx.Campos.Where(f => f.State).ToList();
            foreach (var fl in flds.GroupBy(f => f.Type))
            {
                var key = fl.Key.Substring(0, 1).ToUpper() + fl.Key.Substring(1);
                var len = fl.Max(f => f.Length);
                switch (fl.Key)
                {
                    case "real":
                    case "float":
                        sb.AppendLine(CriarTabela(ctx, $"Campo{key}", $"{key}", true).ToString());
                        break;

                    case "decimal":
                    case "numeric":
                        sb.AppendLine(CriarTabela(ctx, $"Campo{key}", $"{key}(38,10)", true).ToString());
                        break;

                    case "char":
                    case "varchar":
                    case "varbinary":
                    case "nvarchar":
                        sb.AppendLine(CriarTabela(ctx, $"Campo{key}", $"{key}(255)", true).ToString());
                        sb.AppendLine(CriarTabela(ctx, $"Campo{key}4K", $"{key}(4000)", true).ToString());
                        sb.AppendLine(CriarTabela(ctx, $"Campo{key}Max", $"{key}(max)", false).ToString());
                        break;

                    case "bigint":
                    case "tinyint":
                    case "smallint":
                    case "datetime":
                    case "datetime2":
                    case "datetimeoffset":
                    case "uniqueidentifier":
                    case "date":
                    case "int":
                    case "bit":
                        sb.AppendLine(CriarTabela(ctx, $"Campo{key}", key).ToString());
                        break;

                    case "text":
                        sb.AppendLine(CriarTabela(ctx, $"Campo{key}", key, false).ToString());
                        break;

                    default:
                        throw new Exception($"Tipo de dado [{key}] não previso na Jornalização");
                }
            }
            return sb;
        }

        private StringBuilder CriarTabela(JNLxDBContex ctx, string nomeTabela, string tipo, bool indexValor = true)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"if not exists(select 1 from INFORMATION_SCHEMA.TABLES where TABLE_NAME = '{nomeTabela}')");
            sb.AppendLine($"begin");
            sb.AppendLine($"    create table {nomeTabela}");
            sb.AppendLine($"    (");
            sb.AppendLine($"        idValor bigint identity not null,");
            sb.AppendLine($"        idCampoPK bigint not null,");
            sb.AppendLine($"        idCampos int not null,");
            sb.AppendLine($"        idRevisoes bigint not null,");
            sb.AppendLine($"        Valor {tipo} null,");
            sb.AppendLine($"        RowVersion timestamp NOT NULL,");
            sb.AppendLine($"        CONSTRAINT [PK_Journal_{nomeTabela}] primary key clustered (idValor)");
            sb.AppendLine($"    )");
            if (indexValor)
                sb.AppendLine($"    create index [IDX_Journal_{nomeTabela}_Valor] on {nomeTabela} (Valor) where Valor is not null");
            sb.AppendLine($"    alter table {nomeTabela}");
            sb.AppendLine($"        add constraint [FK_Journal_{nomeTabela}_idRevisoes] foreign key(idRevisoes) references Revisoes (idRevisoes)");
            sb.AppendLine($"    alter table {nomeTabela}");
            sb.AppendLine($"        add constraint [FK_Journal_{nomeTabela}_idCampos] foreign key(idCampos) references Campos (idCampos)");
            sb.AppendLine($"end");
            ctx.Database.ExecuteSql(FormattableStringFactory.Create(sb.ToString()));
            return sb;
        }

        private static string _SQLPrepareCampos(string pDBName)
        {
            return @$"
declare @PKs table (Esquema varchar(128), Nome varchar(128),Field varchar(128))
  insert into @PKs (Esquema, Nome, Field) (select
    s.name as schema_name,
    t.name as table_name,
    c.name as Field
    from [{pDBName}].sys.indexes i
    join [{pDBName}].sys.index_columns ic on ((i.object_id = ic.object_id) and (i.index_id = ic.index_id))
    join [{pDBName}].sys.columns c on ((ic.object_id = c.object_id) and (ic.column_id = c.column_id))
    join [{pDBName}].sys.objects t on (t.object_id = i.object_id) and ((t.type = 'V') or (t.type = 'U'))
    join [{pDBName}].sys.schemas s on (t.schema_id = s.schema_id)
    where (i.type_desc <> 'XML') and (i.is_unique_constraint = 0) and is_primary_key = 1)

    INSERT INTO [Campos] (idTabelas,Nome,Type,Length,Scale,State,IsPK)
      select idTabelas,COLUMN_NAME,DATA_TYPE,coalesce(coalesce(CHARACTER_MAXIMUM_LENGTH,NUMERIC_PRECISION),0) Length,coalesce(NUMERIC_SCALE,0) Scale,1 State,IsPK from
         (select CHARACTER_MAXIMUM_LENGTH,NUMERIC_PRECISION,NUMERIC_SCALE,TABLE_NAME,TABLE_SCHEMA,COLUMN_NAME,DATA_TYPE,Campos.Nome, Tabelas.idTabelas,
         (select count(*)  from @PKs p where p.Esquema = Tabelas.Esquema and p.Nome = Tabelas.Nome and p.Field = COLUMNS.COLUMN_NAME) IsPK from [{pDBName}].INFORMATION_SCHEMA.COLUMNS
          left join Tabelas on COLUMNS.TABLE_NAME = Tabelas.Nome and COLUMNS.TABLE_SCHEMA = Tabelas.Esquema and COLUMNS.COLUMN_NAME not like 'RowVersion%'
          left join Campos on COLUMNS.COLUMN_NAME = Campos.Nome and Campos.idTabelas = Tabelas.idTabelas) X
          where x.Nome is null and x.idTabelas is not null

update Campos set Ativo = 0

update C set Ativo = 1 from Campos C
join Tabelas T on T.idTabelas=C.idTabelas
 join [{pDBName}].INFORMATION_SCHEMA.COLUMNS TC on tc.TABLE_NAME = t.Nome and tc.COLUMN_NAME = c.Nome and tc.COLUMN_NAME not like 'RowVersion%'
";
        }

        private static string _SQLPrepareTabelas(string pDBName)
        {
            return
        @$"
  declare @PKs table (Esquema varchar(128), Nome varchar(128),Field varchar(128), TypeName varchar(128))

  insert into @PKs (Esquema, Nome, Field, TypeName) (select
      s.name as schema_name,
      t.name as table_name,
      c.name as Field,
      type_name(c.system_type_id) TypeName
      from [{pDBName}].sys.indexes i
      join [{pDBName}].sys.index_columns ic on ((i.object_id = ic.object_id) and (i.index_id = ic.index_id))
      join [{pDBName}].sys.columns c on ((ic.object_id = c.object_id) and (ic.column_id = c.column_id))
      join [{pDBName}].sys.objects t on (t.object_id = i.object_id) and ((t.type = 'V') or (t.type = 'U'))
      join [{pDBName}].sys.schemas s on (t.schema_id = s.schema_id)
      where (i.type_desc <> 'XML') and (i.is_unique_constraint = 0) and is_primary_key = 1)

  insert into Tabelas (Nome,Esquema,CampoPK, TipoPK)
     (select * from (select TABLE_NAME,TABLE_SCHEMA,( select Field from @PKs p where p.Esquema = TABLE_SCHEMA  and p.Nome = TABLE_NAME) PKField,
                                              ( select TypeName from @PKs p where p.Esquema = TABLE_SCHEMA  and p.Nome = TABLE_NAME) TypeName
      from (select TABLE_NAME,TABLE_SCHEMA,Nome from [{pDBName}].INFORMATION_SCHEMA.TABLES
      left join Tabelas on TABLES.TABLE_NAME = Tabelas.Nome and  TABLES.TABLE_SCHEMA = Tabelas.Esquema
         where TABLES.TABLE_NAME not in ('AspNetUserLogins','AspNetUserRoles','AspNetUserTokens', 'DadosPainelPrincipal')) X
      where x.Nome is null ) x where PKField is not null)

update Tabelas set Ativo = 0

update Tabelas set Ativo = 1 where Nome in (select TABLE_NAME from [{pDBName}].INFORMATION_SCHEMA.TABLES)

";
        }
    }
}
