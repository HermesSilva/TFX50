using System;
using System.Linq;
using System.Text;

using TFX.Core;
using TFX.Core.Exceptions;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Model.DIC.SVC;
using TFX.Core.Reflections;
using TFX.Core.Service;
using TFX.Core.Service.Data;
using TFX.Core.Service.DB;
using TFX.Core.Service.DB.Model;
using TFX.Core.Service.Apps;
using TFX.Core.Service.SVC;

namespace Projecao.Core.JNL.DB
{
    public class XJNLTrigger
    {
        public String NewTrigger(XDBFactory pFactory, XORMTable pTable)
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLineEx($"create or alter trigger JNLx{pTable.Name} On {pTable.Name} After Insert,Delete,Update");
            sb.AppendLineEx($"as ");
            sb.AppendLineEx($"begin");
            sb.AppendLineEx($"	declare @action tinyint, @i tinyint = 0, @d tinyint = 0");
            sb.AppendLineEx($"  if exists(select * from inserted)");
            sb.AppendLineEx($"    set @i = 1");
            sb.AppendLineEx($"  if exists(select * from deleted)");
            sb.AppendLineEx($"    set @d = 1");
            sb.AppendLineEx($"  if @i = 0 and @d = 0");
            sb.AppendLineEx($"    return");
            sb.AppendLineEx($"  if @i = 1 and @d = 1");
            sb.AppendLineEx($"    set @action = {JNLx.JNLxAcao.XDefault.Alteracao}");
            sb.AppendLineEx($"  if @i = 1 and @d = 0");
            sb.AppendLineEx($"    set @action = {JNLx.JNLxAcao.XDefault.Inclusao}");
            sb.AppendLineEx($"  if @i = 0 and @d = 1");
            sb.AppendLineEx($"    set @action = {JNLx.JNLxAcao.XDefault.Delecao}");
            sb.AppendLineEx($"  declare @vlr  varbinary(200)");
            sb.AppendLineEx($"  select @vlr = context_info()");
            sb.AppendLineEx($"  declare @idx int, @len int, @servico uniqueidentifier, @atualizacao int, @usuario uniqueidentifier, @empresa uniqueidentifier, @versao bigint");
            sb.AppendLineEx($"  select @idx = charindex(0x7c, @vlr, 0)");
            sb.AppendLineEx($"  select @atualizacao= convert(int, substring(@vlr, 0, @idx))");
            sb.AppendLineEx($"  select @len = charindex(0x7c, @vlr, @idx + 1) - @idx");
            sb.AppendLineEx($"  select @usuario = convert(uniqueidentifier, convert(varchar(36), substring(@vlr, @idx + 1, @len - 1)))");
            sb.AppendLineEx($"  select @idx = @idx + @len");
            sb.AppendLineEx($"  select @len = charindex(0x7c, @vlr, @idx + 1) - @idx");
            sb.AppendLineEx($"  select @empresa = convert(uniqueidentifier, convert(varchar(36), substring(@vlr, @idx+1, @len - 1)))");
            sb.AppendLineEx($"  select @idx = @idx + @len");
            sb.AppendLineEx($"  select @len= charindex(0x7c, @vlr, @idx + 1) - @idx");
            sb.AppendLineEx($"  select @versao= convert(bigint, substring(@vlr, @idx + 1, @len - 1))");
            sb.AppendLineEx($"  select @idx = @idx + @len");
            sb.AppendLineEx($"  select @len = charindex(0x7c, @vlr, @idx + 1) - @idx");
            sb.AppendLineEx($"  select @servico = convert(uniqueidentifier, convert(varchar(36), substring(@vlr, @idx + 1, @len - 1)))");
            sb.AppendLineEx($"  declare @id uniqueidentifier");
            sb.AppendLineEx($"  declare @hasdata tinyint = 0");
            sb.AppendLineEx($"  select @id = newid()");
            sb.AppendLineEx($"  insert into JNLxRevisao(SYSxServicoID, JNLxAcaoID, JNLxRevisaoID, CTLxUsuarioID, Data,Revisao, SYSxAtualizacaoID, SYSxEmpresaID, SYSxCampoID, SYSxTabelaID) ");
            sb.AppendLineEx($"      (select @servico, @action, @id, @usuario, sysdatetime(), @versao, @atualizacao, @empresa, '{pTable.PKField.ID.AsString()}', '{pTable.ID.AsString()}')");
            sb.AppendLineEx($"  insert into JNLxCampo{pTable.PKField.Type.Name}(JNLxCampo{pTable.PKField.Type.Name}id, JNLxRevisaoID, SYSxCampoID, Valor) ");
            sb.AppendLineEx($"      (select newid(), @id,'{pTable.PKField.ID.AsString()}',{pTable.PKField.Name} from inserted)");
            sb.AppendLineEx($"  if @action = {JNLx.JNLxAcao.XDefault.Inclusao}");
            sb.AppendLineEx($"    return ");
            sb.AppendLineEx($"");
            String decfld = String.Join(", ", pTable.Fields.Select(f => $"@o{f.Name} {pFactory.GetDBType(f)}, @n{f.Name} {pFactory.GetDBType(f)}"));
            sb.AppendLineEx($"  declare {decfld}");
            sb.AppendLineEx($"  if @action = {JNLx.JNLxAcao.XDefault.Alteracao}");
            sb.AppendLineEx($"  begin");
            String varfld = String.Join(", ", pTable.Fields.Select(f => $"@o{f.Name}, @n{f.Name}"));
            String selfld = String.Join(", ", pTable.Fields.Select(f => $"o.{f.Name} o{f.Name}, n.{f.Name} n{f.Name}"));
            sb.AppendLineEx($"    declare CSR{pTable.Name} cursor for select {selfld}");
            sb.AppendLineEx($"      from deleted o join inserted n on o.{pTable.PKField.Name} = n.{pTable.PKField.Name}");
            sb.AppendLineEx($"    open CSR{pTable.Name}");
            sb.AppendLineEx($"    while 1 = 1 ");
            sb.AppendLineEx($"    begin ");
            sb.AppendLineEx($"        fetch next from CSR{pTable.Name} into {varfld}");
            sb.AppendLineEx($"        if (@@fetch_status != 0) ");
            sb.AppendLineEx($"            break");
            foreach (XORMField fld in pTable.Fields)
            {
                if (pTable.PKField.ID == fld.ID)
                    continue;
                sb.AppendLineEx($"        if (@o{fld.Name} != @n{fld.Name}) ");
                sb.AppendLineEx($"        begin ");
                sb.AppendLineEx($"            insert into JNLxCampo{GetTypeName(fld)}(JNLxCampo{GetTypeName(fld)}ID, JNLxRevisaoID, SYSxCampoID, Valor) ");
                sb.AppendLineEx($"                        values (newid(), @id, '{fld.ID.AsString()}', @o{fld.Name})");
                sb.AppendLineEx($"            set @hasdata = 1");
                sb.AppendLineEx($"        end");
            }
            sb.AppendLineEx($"    end");
            sb.AppendLineEx($"    if @hasdata != 1");
            sb.AppendLineEx($"    begin");
            sb.AppendLineEx($"        delete JNLxCampo{pTable.PKField.Type.Name} where JNLxRevisaoID = @id");
            sb.AppendLineEx($"        delete JNLxRevisao where JNLxRevisaoID = @id");
            sb.AppendLineEx($"    end");
            sb.AppendLineEx($"    close CSR{pTable.Name}");
            sb.AppendLineEx($"    deallocate CSR{pTable.Name}");
            sb.AppendLineEx($"    return ");
            sb.AppendLineEx($"  end;");
            sb.AppendLineEx($"    if @action = {JNLx.JNLxAcao.XDefault.Delecao}");
            if (pTable.DeleteOnDeactivate)
            {
                sb.AppendLineEx($"  begin");
                decfld = String.Join(", ", pTable.Fields.Select(f => $"{f.Name}"));
                varfld = String.Join(", ", pTable.Fields.Select(f => $"@o{f.Name}"));
                sb.AppendLineEx($"    declare CSR{pTable.Name} cursor for select {decfld} from deleted");
                sb.AppendLineEx($"    open CSR{pTable.Name}");
                sb.AppendLineEx($"    while 1=1 ");
                sb.AppendLineEx($"    begin ");
                sb.AppendLineEx($"        fetch next from CSR{pTable.Name} into {varfld}");
                sb.AppendLineEx($"        if (@@fetch_status != 0) ");
                sb.AppendLineEx($"            break");
                foreach (XORMField fld in pTable.Fields)
                {
                    if (pTable.PKField.ID == fld.ID)
                        continue;
                    sb.AppendLineEx($"        insert into JNLxCampo{GetTypeName(fld)}(JNLxCampo{GetTypeName(fld)}ID, JNLxRevisaoID, SYSxCampoID, Valor) ");
                    sb.AppendLineEx($"           values (newid(), @id,'{fld.ID.AsString()}',@o{fld.Name})");
                }
                sb.AppendLineEx($"    end");
                sb.AppendLineEx($"    close CSR{pTable.Name}");
                sb.AppendLineEx($"    deallocate CSR{pTable.Name}");
                sb.AppendLineEx($"  end;");
            }
            else
                sb.AppendLineEx($"        raiserror ('Nao é permitido apagar registros da tabela [{pTable.Name}].', 16, 1)");
            sb.AppendLineEx($"end");
            return sb.ToString();
        }

        private String GetTypeName(XORMField pField)
        {
            switch (pField.Type.Name)
            {
                case "Byte[]":
                    return "Binary";

                case "String":
                    if (pField.Length <= 400)
                        return "String";
                    return "Memo";

                default:
                    return pField.Type.Name;
            }
        }
    }

    [XRegister(typeof(XScriptExecutorJNL), "D8A41C4A-0FD1-4163-B456-BA4EFE1148C5")]
    [XModule("D19DA196-F6DA-4702-A07A-D77AF7CA6D93")]
    public class XScriptExecutorJNL : XIScriptExecutorEX
    {
        public void AfterChangeDDL(XExecContext pContext, Boolean pIsMasterDB)
        {
            if (pContext.DataBase.Factory.DBVendor != XDBVendor.MSSQLServer || XServiceDefault.NoJournal)
                return;
            XJNLTrigger tgr = new XJNLTrigger();
            foreach (XORMTable tbl in XModelCache.Instance.Tables.Where(t => t.ModuleID != JNLx.JNLxRevisao.Instance.ModuleID))
            {
                if (tbl.ID.In(SYSx.SYSxContador.gCID))
                    continue;
                String tgg = tgr.NewTrigger(pContext.DataBase.Factory, tbl);
                try
                {
                    XConsole.Debug($"Create Trigger for [{tbl.Name}");
                    pContext.DataBase.ExecSQL(tgg);
                }
                catch (Exception pEx)
                {
                    XConsole.Debug(tgg);
                    throw new XThrowContainer(pEx);
                }
            }
        }

        public void BeforeChangeDDL(XExecContext pContext, Boolean pIsMasterDB)
        {
        }

        public void DoCheck(XExecContext pContext, XMORMScriptBuilder pBuilder, XModelCache pReverseModel, Boolean pIsMasterDB)
        {
        }

        public void DoExecute(XExecContext pContext, Boolean pIsMasterDB)
        {
        }

        public void DoReverse(XExecContext pContext, Boolean pIsMasterDB)
        {
        }

        public void UploadDefaultData(XExecContext pContext, Boolean pIsMasterDB)
        {
        }
    }

    [XRegister(typeof(XServiceLoaderEx), "0EE81D60-BEFE-4C31-8799-640BA167CC03")]
    [XModule("D19DA196-F6DA-4702-A07A-D77AF7CA6D93")]
    public class XServiceLoaderEx : XIServiceLoaderEx
    {
        public void RecordJornalOnFlush(XExecContext pContex, XSVCModel pService, String pFilter)
        {
        }

        public void RecordJornalOnGet(XExecContext pContex, XSVCModel pService, String pFilter)
        {
            //using (JNLx._JNLxFiltro flt = XPersistencePool.Get<JNLx._JNLxFiltro>(pContex, true))
            //{
            //    JNLx.JNLxFiltro.XTuple ftpl = flt.NewTuple();
            //    ftpl.SYSxServicoID = pService.ID;
            //    ftpl.Data = XDefault.Now;
            //    ftpl.Filtro = XEncoding.Default.GetBytes(pFilter);
            //    flt.Flush();
            //}
        }
    }
}