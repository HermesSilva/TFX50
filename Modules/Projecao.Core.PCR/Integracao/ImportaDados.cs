using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;

using Projecao.Core.ERP.DB;
using Projecao.Core.ISE.DB;
using Projecao.Core.NTR.Jobs;
using Projecao.Core.PCR.DB;
using Projecao.Core.PCR.PessoaFisica;

using TFX.Core;
using TFX.Core.LZMA;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Model.DIC.SVC;
using TFX.Core.Model.Interfaces;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.DB;

using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR.Integracao
{
    [XRegister(typeof(ImportaDados), sCID)]
    public class ImportaDados
    {
        public const String sCID = "EACDBEEF-C8F5-4C6D-BFE0-B41B80A5C7A3";
        public static Guid gCID = new Guid(sCID);
        public static Int32? LastCRC = -1;
        public static DateTime LastPrepare = XDefault.NullDateTime;
        public static Int32 PrepareTime;
        private Object _ToLock = new Object();

        public void MigraDados(XExecContext pContext, HPCIntegracaoSVC.XCFGTuple pCondig)
        {
            String sqdb = $"{pCondig.PastaTemoraria}\\Mobile.db";
            if (!Directory.Exists(pCondig.PastaTemoraria))
                Directory.CreateDirectory(pCondig.PastaTemoraria);

            if (File.Exists(sqdb))
                File.Delete(sqdb);
            using (SQLiteConnection cnn = new SQLiteConnection($"Data Source={sqdb}"))
            {
                cnn.Open();
                using (DbTransaction tran = cnn.BeginTransaction())
                {
                    foreach (XORMTable tbl in GetTables())
                        if (tbl.ID != PCRxIATFFaseTipo.gCID)
                            Exporta(cnn, tbl.StaticData);

                    foreach (XORMTable tbl in GetTablesDB())
                    {
                        using (XDBTable dbtbl = XPersistencePool.Get<XDBTable>(pContext, tbl.ID))
                        {
                            dbtbl.FilterActives = true;
                            dbtbl.MaxRows = 0;
                            dbtbl.Open();
                            Exporta(cnn, dbtbl);
                        }
                    }

                    foreach (XSVCModel mdl in XModelCache.Instance.SVC.Where(s => s.CanExport && s.ModuleID == ProjecaoCorePCR.gCID))
                    {
                        XConsole.Warn($"Iniciando Exportação de [{mdl.Name}]");
                        XIDBService svc = XServicePool.Get(mdl.ID);
                        svc.SetContext(pContext);
                        svc.MaxRows = 0;
                        svc.Open();

                        Exporta(cnn, svc.DataSet);
                    }
                    tran.Commit();
                    cnn.Close();
                }
            }
            Byte[] data = File.ReadAllBytes(sqdb);
            LastCRC = XLzma.GetCRC32(data);
            AlmaCorePCRRule.DataChanged = false;
        }

        private IEnumerable<XORMTable> GetTablesDB()
        {
            yield return PCRx.PCRxAnimalLote.Instance;
        }

        private IEnumerable<XORMTable> GetTables()
        {
            foreach (XORMTable tbl in XModelCache.Instance.Tables.Where(t => t.ModuleID == ProjecaoCorePCR.gCID && t.IsCached))
                yield return tbl;
            yield return ISEx.ISExCodigoTipo.Instance;
            yield return ISEx.ISExCodigo.Instance;
            yield return ERPx.ERPxGenero.Instance;
            yield return PCRx.PCRxAnimalFiliacao.Instance;
        }

        private void Exporta(SQLiteConnection pConnection, XDataSet pDataSet)
        {
            StringBuilder ddl = new StringBuilder();
            XConsole.Warn($" Name[{pDataSet.Name}] ID[{pDataSet.ID}]");
            ddl.AppendLine($"Create Table {pDataSet.Name}");
            ddl.AppendLine($"(");
            Int32 cnt = 0;
            foreach (XORMField fld in pDataSet.Fields.OrderBy(f => f.Index))
            {
                if (cnt++ > 0)
                    ddl.AppendLine(",");
                ddl.Append($"{fld.Alias} {XSQLiteUtils.GetSQLiteType(fld.Type.Name)}");
            }
            ddl.AppendLine(",");
            ddl.AppendLine($"Primary Key ({pDataSet.PKField.Alias})");
            ddl.Append($")");
            using (SQLiteCommand cmd = new SQLiteCommand(ddl.ToString(), pConnection))
                cmd.ExecuteNonQuery();

            using (SQLiteCommand cmd = new SQLiteCommand(ddl.ToString(), pConnection))
            {
                XSQLiteUtils.PrepareInsert(cmd, pDataSet);
                foreach (XDataTuple tpl in pDataSet)
                {
                    XSQLiteUtils.AddInsertValues(cmd, pDataSet, tpl);
                    cmd.ExecuteNonQuery();
                }
            }
            XConsole.Warn($"Finalizando Exportação de [{pDataSet.Name}]");
        }
    }
}