using System;
using System.IO;
using System.Linq;

using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.LGC.DB;

using TFX.Core;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.Job;

namespace Projecao.Core.ITG.Jobs
{
    [XRegister(typeof(IntegracaoLegadoRule), sCID, typeof(IntegracaoLegadoSVC))]
    public class IntegracaoLegadoRule : IntegracaoLegadoSVC.XRule
    {

        public const String sCID = "9CEF3F1A-9BE0-4EE0-9AA8-BCB7D846CF8A";
        public static Guid gCID = new Guid(sCID);

        public IntegracaoLegadoRule()
        {
            ID = gCID;
        }

        private static Boolean _IsRunning;
        private static void InsereRegistros()
        {
            if (!Directory.Exists(XDefault.DataTemp))
                return;
            String[] folders = Directory.GetDirectories(XDefault.DataTemp);
            foreach (String folder in folders.OrderBy(f => f))
            {
                using (XExecContext ctx = XExecContext.Create())
                {
                    if (!XLegacyManager.IsInitialize && !XGerenciaIntegracao.IsReverse)
                        XLegacyManager.LoadRemoteModel(ctx, XGerenciaIntegracao.KnowTables);

                    foreach (XBaseIntegracaoTabela it in XLegacyManager.ITGTables.OrderBy(i => i.Order))
                    {
                        if (it.MigrationLevel != XMigrationLevel.ServerToServer || it.LegacySide != XLegacySide.Cloud)
                            continue;
                        try
                        {
                            it.DataPath = folder;
                            XConsole.Warn($"Start Execute [{it.RemoteTable}].");
                            it.Prepare(null, ctx);
                            it.Execute(null);
                        }
                        finally
                        {
                            it.Prepare(null, null);
                        }
                    }
                    ctx.Commit();
                    Directory.Delete(folder, true);
                }
            }
        }
        protected override void DoExecute(XExecContext pContext, XJobState pJobState)
        {
            lock (this)
                if (_IsRunning)
                    return;
                else
                    _IsRunning = true;
            try
            {
                if (XGerenciaIntegracao.IsReverse)
                    InsereRegistros();
                else
                    XGerenciaIntegracao.Execute(pContext);
            }
            finally
            {
                _IsRunning = false;
            }
        }
    }
}
