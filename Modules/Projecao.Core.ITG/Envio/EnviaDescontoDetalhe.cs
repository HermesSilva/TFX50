using System;
using System.Linq;

using Projecao.Core.ITG.Envio;
using Projecao.Core.LGC.DB;

using TFX.Core;
using TFX.Core.Cache;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.ITG.Sincronizacao
{
    [XRegister(typeof(EnviaDescontoDetalhe), sCID)]
    public class EnviaDescontoDetalhe : XEnvios
    {
        public const String sCID = "9BDD158D-6E39-480B-BDEC-E2D983B94CC7";
        public static Guid gCID = new Guid(sCID);

        public override Int32 RunnOrder => 20;

        public override void Execute(XExecContext pContext, XHttpBroker pBroker)
        {
            Type tp = XTypeCache.GetTypes<XLegacyTable>().FirstOrDefault(t => t.Name == "FAT_PROMOCAOITEM");
            Table = tp.CreateInstance<XLegacyTable>();
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