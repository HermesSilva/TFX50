using System;
using System.Collections.Generic;
using System.Linq;

using TFX.Core;
using TFX.Core.Cache;
using TFX.Core.Exceptions;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Service.Data;
using TFX.Core.Service.DB;

namespace Projecao.Core.LGC.DB
{
    public class XLegacyManager
    {
        public static Boolean IsInitialize = false;

        public static XDBLegacyTable Create(String pTable, XExecContext pContex, String[] pTables, Boolean pUseSession = false)
        {
            if (RemoteModel != null && !RemoteModel.Tables.Any(t => t.Name.SafeLower() == pTable.SafeLower()))
            {
                RemoteModel.SetContext(pContex);
                RemoteModel.Execute(pTables);
            }
            else
                LoadRemoteModel(pContex, pTables);
            XORMLegacyTable tbl = RemoteModel.Tables.FirstOrDefault(t => t.Name.SafeLower() == pTable.SafeLower());
            if (tbl == null)
                throw new XError($"Tabela com nome [{pTable}] não existe no modelo legado.");
            return XDBLegacyTable.Create(tbl, pContex, pUseSession);
        }

        static XLegacyManager()
        {
            foreach (Type tp in XTypeCache.GetTypes<XLegacyTable>())
            {
                XLegacyTable bmt = tp.CreateInstance<XLegacyTable>();
                ITGTables.Add(bmt);
            }
        }

        public static List<XLegacyTable> ITGTables = new List<XLegacyTable>();
        private static XOracleLegacyDBReverse _RemoteModel;

        public static XOracleLegacyDBReverse RemoteModel
        {
            get
            {
                return _RemoteModel;
            }
        }

        public static void LoadRemoteModel(XExecContext pContext, String[] pTables)
        {
            if (_RemoteModel != null)
                return;
            try
            {
                XOracleLegacyDBReverse rev = new XOracleLegacyDBReverse(pContext);
                rev.Execute(pTables);
                _RemoteModel = rev;
                if (!XDefault.IsClient && XDefault.UseBCOFunction)
                    Configure(pContext);
                XLegacyManager.IsInitialize = true;
            }
            catch (Exception pEx)
            {
                _RemoteModel = null;
                throw new XThrowContainer(pEx);
            }
        }

        public static void Configure(XExecContext pSource)
        {
            pSource.DataBase.ExecSQL("call BCO_FUNCAO('GESTCOM')");
            foreach (XLegacyTable it in ITGTables)
                it.Configure(pSource);
        }
    }
}