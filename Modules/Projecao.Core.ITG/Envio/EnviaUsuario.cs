using System;

using Projecao.Core.ITG.Envio;

using TFX.Core;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.ITG.Sincronizacao
{
    [XRegister(typeof(EnviaUsuario), sCID)]
    public class EnviaUsuario : XEnvios
    {
        public const String sCID = "DF57DE49-AE89-4E1E-9501-89FE2B1DF6C9";
        public static Guid gCID = new Guid(sCID);

        public override Int32 RunnOrder => 30;
        public override Boolean CanExecute => true;

        public override void Execute(XExecContext pContext, XHttpBroker pBroker)
        {
            Table = new UsuariosTable();
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