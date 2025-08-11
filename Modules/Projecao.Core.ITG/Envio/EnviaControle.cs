using System;

using Projecao.Core.ITG.Envio;

using TFX.Core;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.ITG.Sincronizacao
{
    [XRegister(typeof(EnviaControle), sCID)]
    public class EnviaControle : XEnvios
    {
        public const String sCID = "94869DFE-1DD0-4F3A-8632-444BBA256D6B";
        public static Guid gCID = new Guid(sCID);

        public override Int32 RunnOrder => 10;

        public override void Execute(XExecContext pContext, XHttpBroker pBroker)
        {
            Table = new ControleTable();
            Table.Configure(pContext);
            Table.Prepare(pContext, null);
            Table.Execute(pContext);
            Table.Prepare(null, null);
            if (Table.DataResult.IsEmpty())
                return;
            base.Execute(pContext, pBroker);
        }
    }
}