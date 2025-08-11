using System;

using Projecao.Core.ITG.Envio;

using TFX.Core;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.ITG.Sincronizacao
{
    [XRegister(typeof(EnviaCliente), sCID)]
    public class EnviaCliente : XEnvios
    {
        public const String sCID = "39A76E98-54F7-473D-87EB-D7A9436CF897";
        public static Guid gCID = new Guid(sCID);

        public override Int32 RunnOrder => 30;
        public override Boolean CanExecute => Config.MigrarClientes;

        public override void Execute(XExecContext pContext, XHttpBroker pBroker)
        {
            Table = new ClientesTable();
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