using System;

using Projecao.Core.ERP.GerenciaEmpresa;
using Projecao.Core.LGC.DB;

using TFX.Core;
using TFX.Core.Service.Data;

namespace Projecao.Core.ITG.Sincronizacao
{
    public abstract class XSincronize
    {
        protected static XILog Log = XLog.GetLogFor(typeof(XSincronize));
        public XLegacyTable Table;
        public ConfigITGFRM.XCFGTuple Config;

        public abstract Int32 RunnOrder
        {
            get;
        }

        public abstract void Execute(XExecContext pContext, XHttpBroker pBroker);
    }
}