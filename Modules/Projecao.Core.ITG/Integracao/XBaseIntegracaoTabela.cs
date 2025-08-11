using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;



using Projecao.Core.ITG.Jobs;
using Projecao.Core.LGC.DB;

using TFX.Core;
using TFX.Core.Cache;
using TFX.Core.Exceptions;
using TFX.Core.LZMA;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Model.DIC.Types;
using TFX.Core.Reflections;
using TFX.Core.Service.Cache;
using TFX.Core.Service.Data;
using TFX.Core.Service.DB;
using TFX.Core.Service.SVC;
using TFX.Core.Utils;

namespace Projecao.Core.Integracao.Rule.Integracao
{
    public abstract class XBaseIntegracaoTabela : XLegacyTable
    {
        private const String _Prefix = "ITG_";
        private XORMLegacyTable _ORMRemoteTable = null;

        protected XIBasePersistence PLocalDBTable;
        private static XILog _Log = XLog.GetLogFor(typeof(XBaseIntegracaoTabela));
        public String DataPath;
        public Boolean IsReverse => XGerenciaIntegracao.IsReverse;
        protected virtual Boolean UseDistinct => false;
        protected String RemotePKValues(XDataTuple pTuple)
        {
            return String.Join("|", ORMRemoteTable.PKField.Select(f => pTuple[f.Name].AsString()).ToArray());
        }

        public XORMLegacyTable ORMRemoteTable
        {
            get
            {
                if (IsReverse)
                    return null;
                if (_ORMRemoteTable == null)
                {
                    _ORMRemoteTable = XGerenciaIntegracao.RemoteModel.Tables.FirstOrDefault(t => t.Name == RemoteTable);
                    if (_ORMRemoteTable == null)
                        throw new XError($"A tabela de origem [{RemoteTable}] não existe.");
                }
                return _ORMRemoteTable;
            }
        }

        public Byte[] GetData()
        {
            return SourceResult.GetData();
        }

        public virtual String RemoteControlTable
        {
            get
            {
                XRegister reg = XTypeCache.GetRegister(GetType().GetCID());
                return _Prefix + reg.Tag;
            }
        }

        public override void DeleteTemp()
        {
            if (IsReverse)
                return;
            if (CheckDelete)
                Remote.DataBase.ExecSQL($"DELETE ITG_DELECAO where TABELA = '{RemoteTable}'");
        }

        protected XMemoryStream WriteDelete(XExecContext pContext)
        {
            XMemoryStream ms = new XMemoryStream();
            if (!CheckDelete)
                return ms;
            using (XQuery qry = new XQuery(Remote))
            {
                qry.SQL = $"SELECT CHAVE,TABELA FROM ITG_DELECAO where TABELA = '{RemoteTable}'";
                if (!qry.Open())
                    return ms;
                using (StreamWriter sw = new StreamWriter(ms, XEncoding.Default, leaveOpen: true))
                {
                    sw.AutoFlush = true;
                    sw.WriteLine(String.Join(";", qry.Fields.Select(f => f.Name)));
                    foreach (XDataTuple tpl in qry)
                    {
                        foreach (String fld in qry.Fields.Select(f => f.Name))
                        {
                            String data = tpl[fld].AsString().SafeReplace(";", "");
                            if (data.IsFull())
                                sw.Write(data);
                            sw.Write(";");
                        }
                        sw.WriteLine();
                    }
                }
                return ms;
            }
        }

        public virtual Boolean Open(String pWhere = null)
        {
            if (IsReverse)
                return OpenReverse();
            if (XDefault.TestRunning && RemoteControlTable.IsFull())
                Remote.DataBase.ExecSQL("DELETE FROM " + RemoteControlTable);

            if (RemoteControlTable.IsFull() && MigrationDirection.In(XMigrationDirection.Both, XMigrationDirection.RemoteToLocal) && Remote.DataBase.ExecSQL(GetInsert()) == 0)
                return false;
            XQuery qry = new XQuery(Remote);
            qry.SQL = GetSelect() + " " + pWhere;
            qry.IsLegacy = true;
            HasData = qry.Open();
            SourceResult = qry;
            if (XGerenciaIntegracao.IsClientReverse && HasData)
            {
                if (!Directory.Exists(XDefault.DataTemp))
                    Directory.CreateDirectory(XDefault.DataTemp);
                using (XMemoryStream ms = new XMemoryStream())
                using (StreamWriter sw = new StreamWriter(ms))
                {
                    sw.AutoFlush = true;
                    foreach (XORMField fld in qry.Fields)
                        sw.Write(fld.Name + "|" + fld.Type.Name + ";");
                    sw.WriteLine();
                    foreach (XDataTuple tpl in qry)
                    {
                        foreach (XORMField fld in qry.Fields)
                            sw.Write(XUtils.GetCSVValue(tpl[fld.Index]) + ";");
                        sw.WriteLine();
                    }
                    File.WriteAllBytes(Path.Combine(XDefault.DataTemp, RemoteTable + ".bin"), XLzma.Encode(ms.ToArray()));
                }


                return false;
            }
            return HasData;
        }

        protected virtual Boolean OpenReverse()
        {
            if (XGerenciaIntegracao.IsReverse)
            {
                String file = Path.Combine(DataPath, RemoteTable + ".bin");
                if (!File.Exists(file))
                    return false;
                XDataSet dst = new XDataSet();
                SourceResult = dst;
                using (FileStream fs = new FileStream(file, FileMode.Open))
                using (StreamReader sr = new StreamReader(fs))
                {
                    String[] header = sr.ReadLine().SafeBreak(";");
                    for (int i = 0; i < header.Length; i++)
                    {
                        String[] fld = header[i].SafeBreak("|");
                        XORMField ofld = new XORMField();
                        ofld.TypeID = XType.FromCSType(fld[1]);
                        ofld.Name = fld[0];
                        ofld.ID = Guid.NewGuid();
                        SourceResult.Fields.AddOrdered(ofld);
                    }
                    XORMPKField pk = new XORMPKField();
                    pk.TypeID = XInt32.CID;
                    pk.Name = "PrimaryKeyID";
                    pk.ID = Guid.NewGuid();
                    SourceResult.Fields.AddOrdered(pk);
                    Int32 cnt = 1;
                    while (!sr.EndOfStream)
                    {
                        String[] data = sr.ReadLine().SafeBreak(";");
                        XDataTuple tpl = SourceResult.NewTuple(cnt++);
                        for (int i = 0; i < data.Length; i++)
                        {
                            XORMField fld = SourceResult.Fields.Single(f => f.Index == i);
                            tpl[i] = XConvert.FromString(data[i], fld.Type);
                        }
                    }
                }
            }
            return SourceResult?.Count > 0;
        }

        public Boolean OpenToDelete()
        {
            XQuery qry = new XQuery(Remote);
            qry.SQL = GetSelectToDelete();
            Boolean has = qry.Open();
            qry.IsLegacy = true;
            SourceResult = qry;
            return has;
        }

        public override void Prepare(XExecContext pRemote, XExecContext pLocal)
        {
            Remote = pRemote;
            Local = pLocal;
            if (pLocal == null)
                Release();
            else
                Prepare();
        }

        protected virtual void Release()
        {
            PLocalDBTable.SafeDispose();
        }

        protected virtual void Prepare()
        {
            PLocalDBTable = (XIBasePersistence)XPersistencePool.Get(Local, LocalTableID);
        }

        public virtual String GetValidateDelete()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLineEx($"DECLARE");
            sb.AppendLineEx($"  V01 INT := 0;");
            sb.AppendLineEx($"BEGIN");
            sb.AppendLineEx($"  SELECT COUNT(*) INTO V01 FROM USER_TABLES WHERE TABLE_NAME = 'ITG_DELECAO';");
            sb.AppendLineEx($"  IF V01 <= 0 THEN");
            sb.AppendLineEx($"    EXECUTE IMMEDIATE 'CREATE TABLE ITG_DELECAO(CHAVE VARCHAR2(80 BYTE) NOT NULL,TABELA VARCHAR2(30 BYTE) NOT NULL)';");
            sb.AppendLineEx($"  END IF;");
            sb.AppendLineEx($"END;");
            return sb.ToString();
        }

        public virtual String GetCreateTrigger()
        {
            String where = String.Join(" AND ", ORMRemoteTable.PKField.Select(f => f.Name + " = :OLD." + f.Name).ToArray());
            StringBuilder sb = new StringBuilder();
            sb.AppendLineEx($"CREATE OR REPLACE TRIGGER {RemoteControlTable}_TG");
            sb.AppendLineEx($"  AFTER UPDATE OR DELETE ON {ORMRemoteTable.Name} FOR EACH ROW");
            sb.AppendLineEx($"DECLARE");
            sb.AppendLineEx($"  V01 INT;");
            sb.AppendLineEx($"  KEY VARCHAR2(80 BYTE);");
            sb.AppendLineEx($"BEGIN");
            sb.AppendLineEx($"  IF (UPDATING) THEN");
            sb.AppendLineEx($"    DELETE FROM {RemoteControlTable} WHERE {where};");
            sb.AppendLineEx($"  END IF;");
            if (CheckDelete)
            {
                sb.AppendLineEx($"  IF (INSERTING) AND V01 > 0 THEN");
                sb.AppendLineEx($"    SELECT {String.Join("||'@'||", ORMRemoteTable.PKField.Select(f => ":NEW." + f.Name).ToArray())} INTO KEY FROM DUAL;");
                sb.AppendLineEx($"  ELSE");
                sb.AppendLineEx($"    SELECT {String.Join("||'@'||", ORMRemoteTable.PKField.Select(f => ":OLD." + f.Name).ToArray())} INTO KEY FROM DUAL;");
                sb.AppendLineEx($"  END IF;");
                sb.AppendLineEx($"  SELECT COUNT(*) INTO V01 FROM ITG_DELECAO WHERE TABELA = '{ORMRemoteTable.Name}' AND CHAVE = KEY;");
                sb.AppendLineEx($"  IF (INSERTING) AND V01 > 0 THEN");
                sb.AppendLineEx($"    DELETE ITG_DELECAO WHERE TABELA = '{ORMRemoteTable.Name}' AND CHAVE = KEY;");
                sb.AppendLineEx($"  END IF;");
                sb.AppendLineEx($"  IF (DELETING) AND V01 = 0 THEN");
                sb.AppendLineEx($"    INSERT INTO ITG_DELECAO(TABELA, CHAVE) VALUES('{ORMRemoteTable.Name}', KEY);");
                sb.AppendLineEx($"  END IF;");
            }
            sb.AppendLineEx($"END;");
            return sb.ToString();
        }

        public virtual String GetInsert()
        {
            StringBuilder sb = new StringBuilder();
            String tpks = String.Join(", ", ORMRemoteTable.PKField.Select(f => f.Name).ToArray());
            String where = String.Join(" AND ", ORMRemoteTable.PKField.Select(f => "TBL." + f.Name + " = INT." + f.Name).ToArray());
            sb.AppendLineEx($"INSERT INTO  {RemoteControlTable} (");
            sb.AppendLineEx($"{tpks})");

            sb.AppendLineEx($"SELECT distinct {tpks} FROM ");
            sb.AppendLineEx("(SELECT INT.DELETAR, ");
            sb.AppendLineEx(String.Join(", ", TargetFieldNames.Select(F => "TBL." + F).ToArray()));
            sb.AppendLineEx($"FROM (select {(UseDistinct ? " distinct " : "")} {String.Join(", ", TargetFieldSelect)} from {RemoteTable} ");
            AddJoins(sb);
            String addw = RemoteTableWhere;
            if (addw.IsFull())
                sb.Append(" where " + addw);
            sb.AppendLineEx($") TBL LEFT JOIN {RemoteControlTable} INT ON {where} ");
            sb.AppendLineEx(") SRC where SRC.DELETAR IS NULL");
            return sb.ToString();
        }

        protected virtual String GetSelect()
        {
            StringBuilder sb = new StringBuilder();
            String where = String.Join(" AND ", ORMRemoteTable.PKField.Select(f => "TBL." + f.Name + " = INT." + f.Name).ToArray());
            sb.AppendLineEx("SELECT * FROM (");

            sb.AppendLineEx("SELECT ");
            sb.AppendLineEx(String.Join(", ", TargetFieldNames.Select(F => "TBL." + F).ToArray()));
            sb.AppendLineEx(", INT.DELETAR DELETAR ");
            sb.AppendLineEx($"FROM (select  {(UseDistinct ? " distinct " : "")} {String.Join(", ", TargetFieldSelect)} from {RemoteTable} ");
            AddJoins(sb);
            String addw = RemoteTableWhere;
            if (addw.IsFull())
                sb.Append(" where " + addw);
            sb.AppendLineEx($") TBL ");
            sb.AppendLineEx($"LEFT JOIN {RemoteControlTable} INT ON {where} ");
            sb.AppendLineEx(") SRC where SRC.DELETAR IS NULL ");

            return sb.ToString();
        }

        protected virtual void AddJoins(StringBuilder pBuilder)
        {
        }

        public virtual String GetCreateTable(XExecContext pRemote)
        {
            String pkflds = String.Join(", ", ORMRemoteTable.PKField.Select(f => f.Name).ToArray());
            StringBuilder sb = new StringBuilder();
            sb.AppendLineEx($"CREATE TABLE {RemoteControlTable}");
            sb.AppendLineEx($"(");
            foreach (XORMField fld in ORMRemoteTable.PKField)
                sb.AppendLineEx($"  {XOracleFieldLegacyFields.GetCreate(fld, true, false, pRemote.DataBase.Factory, false)},");
            sb.AppendLineEx($"  DELETAR VARCHAR2(1) NULL,");
            sb.AppendLineEx($"  CONSTRAINT \"{RemoteControlTable}_PK\" PRIMARY KEY({pkflds})");
            sb.AppendLineEx($");");
            sb.AppendLineEx($"ALTER TABLE {RemoteControlTable} ADD CONSTRAINT {RemoteControlTable}_FK");
            sb.AppendLineEx($"  FOREIGN KEY({pkflds})");
            sb.AppendLineEx($"  REFERENCES {ORMRemoteTable.Name}({pkflds}) ON DELETE CASCADE ENABLE");
            return sb.ToString();
        }

        private String GetSelectToDelete()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLineEx($"SELECT * FROM {RemoteControlTable} where DELETAR = 'S'");
            return sb.ToString();
        }

        private String GetCloseRemoteData()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLineEx($"UPDATE  {RemoteControlTable} set DELETAR = 'N' where DELETAR is null;");
            sb.AppendLineEx($"DELETE {RemoteControlTable} WHERE DELETAR = 'S'");
            return sb.ToString();
        }

        private String GetCloseLocalData()
        {
            StringBuilder sb = new StringBuilder();
            XORMTable ttbl = XModelCache.Instance.Tables.Get(LocalTableID);
            sb.AppendLineEx($"UPDATE  {Local.DataBase.Factory.DBObjectName(ttbl.Name)} set {Local.DataBase.Factory.DBObjectName("ITGEnviado")} = 1 where " +
                $"{Local.DataBase.Factory.DBObjectName("ITGEnviado")} = 0");
            return sb.ToString();
        }

        private String GetDeleteLocalRef()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLineEx($"DELETE {RemoteControlTable} where DELETAR = 'S'");
            return sb.ToString();
        }

        private String TriggerExistsSQL => $"SELECT COUNT(*) FROM USER_TRIGGERS WHERE STATUS ='VALID' AND TRIGGER_NAME = '{RemoteControlTable}_TG' AND TABLE_NAME = '{RemoteTable}'";

        protected virtual String RemoteTableWhere
        {
            get;
        }

        public override void Configure(XExecContext pRemote)
        {
            if (IsReverse)
                return;

            if (MigrationDirection.In(XMigrationDirection.Both, XMigrationDirection.RemoteToLocal))
            {
                if (RemoteControlTable.IsFull() && !pRemote.DataBase.TableExists(RemoteControlTable, null))// pRemote.DataBase.Factory.Config.User
                {
                    //_Log.Warn($"Configure [{RemoteControlTable}] no found");
                    //_Log.Warn($"Create [{GetCreateTable(pRemote)}] ");
                    pRemote.DataBase.ExecManySQL(GetCreateTable(pRemote));
                }
                if (RemoteControlTable.IsFull() && CheckDelete)
                    pRemote.DataBase.ExecSQL(GetValidateDelete());
                String ttg = GetCreateTrigger();
                if (ttg.IsFull() && Convert.ToInt32(pRemote.DataBase.ExecScalar<Object>(TriggerExistsSQL)) == 0)
                    pRemote.DataBase.ExecSQL(ttg);
            }
        }

        public void SetContext(XExecContext pRemote)
        {
            Remote = pRemote;
        }

        public Boolean IsConfigurated
        {
            get
            {
                return false;
            }
        }

        public virtual Boolean CheckDelete => false;

        public bool HasData
        {
            get;
            private set;
        }

        public override void Execute(XExecContext pContext)
        {
            if (MigrationDirection.In(XMigrationDirection.Both, XMigrationDirection.RemoteToLocal))
            {
                MigrateRemoteToLocal(pContext);
                DeleteLocal(pContext);
            }
            if (MigrationDirection.In(XMigrationDirection.Both, XMigrationDirection.LocalToRemote))
                MigrateLocalToRemote(pContext);
            if (MigrationDirection.In(XMigrationDirection.Both, XMigrationDirection.LocalToRemote))
                Remote.DataBase.ExecSQL(GetDeleteLocalRef());
            if (Remote != null & RemoteControlTable.IsFull() && MigrationDirection.In(XMigrationDirection.Both, XMigrationDirection.RemoteToLocal))
                Remote.DataBase.ExecManySQL(GetCloseRemoteData());
            if (MigrationDirection.In(XMigrationDirection.Both, XMigrationDirection.LocalToRemote))
                Local.DataBase.ExecManySQL(GetCloseLocalData());
            FlushLocal();
        }

        protected virtual void FlushLocal()
        {
            PLocalDBTable?.Flush();
        }

        protected virtual void MigrateRemoteToLocal(XExecContext pContext)
        {
        }

        protected virtual void MigrateLocalToRemote(XExecContext pContext)
        {
        }

        protected virtual void DeleteLocal(XExecContext pContext)
        {
        }
    }

    public abstract class XBaseIntegracaoTabela<TTable, TTuple> : XBaseIntegracaoTabela
        where TTable : XIBasePersistence
        where TTuple : XDataTuple
    {
        public XBaseIntegracaoTabela()
        {
            GuidAttribute gid = typeof(TTable).GetCustomAttribute<GuidAttribute>();
            LocalTableID = new Guid(gid.Value);
        }

        public virtual Guid ServerCID
        {
            get;
        }

        protected TTable LocalDBTable
        {
            get
            {
                return (TTable)PLocalDBTable;
            }
        }

        protected override void MigrateRemoteToLocal(XExecContext pContext)
        {
            Boolean hasdata = Open();
            DataResult = new Byte[0];
            if (!Directory.Exists(XDefault.TempFolder))
                Directory.CreateDirectory(XDefault.TempFolder);
            if (File.Exists(FileName))
                File.Delete(FileName);
            using (XMemoryStream ms = new XMemoryStream())
            using (StreamWriter sw = new StreamWriter(ms, XEncoding.Default))
            {
                if (hasdata)
                {
                    sw.AutoFlush = true;
                    sw.WriteLine(String.Join(";", TargetFieldNames));
                    foreach (XDataTuple tpl in SourceResult)
                    {
                        if (!IsValid(tpl))
                            continue;
                        for (int i = 0; i < TargetFieldNames.Length; i++)
                        {
                            if (i > 0)
                                sw.Write(";");
                            String fld = TargetFieldNames[i];
                            String data = tpl[fld].AsString().SafeReplace(";", "");
                            if (data.IsFull())
                                sw.Write(data);
                        }
                        sw.WriteLine();
                    }
                }
                if (!IsReverse)
                {
                    using (XMemoryStream ms2 = new XMemoryStream())
                    {
                        using (XMemoryStream del = WriteDelete(pContext))
                        {
                            if (ms.Length == 0 && del.Length == 0)
                                return;
                            ms2.Write(ServerCID);
                            ms2.Write((Int64)0);
                            ms2.Write(ms.ToArray());
                            ms2.Write(del.ToArray());
                            ms2.Position = 16;
                            ms2.Write(ms2.Length);
                        }
                        DataResult = ms2.ToArray();
                    }
                }
            }
        }

        protected virtual Boolean IsValid(XDataTuple tpl)
        {
            return true;
        }
    }
}