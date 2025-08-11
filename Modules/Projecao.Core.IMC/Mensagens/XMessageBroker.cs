using System;

using TFX.Core;
using TFX.Core.Model.Cache;
using TFX.Core.Service.Data;

using static Projecao.Core.IMC.DB.IMCx;

namespace Projecao.Core.IMC.Mensagens
{
    public static class XMessageBroker
    {
        public static void EnviarMensagem(XExecContext pContext, Guid pEmpresaID, Guid pPessoID, String pOrigem, String pDestino, Byte[] pMensagem)
        {
            using (_IMCxMensagem msg = XPersistencePool.Get<_IMCxMensagem>(pContext))
            {
                msg.NewTuple();
                msg.Current.Data = XDefault.Now;
                msg.Current.IMCxClasseID = IMCxClasse.XDefault.Saida_Automatica;
                msg.Current.Mensagem = pMensagem;
                msg.Current.IMCxEstadoID = IMCxEstado.XDefault.Enviada;
                msg.Current.Destino = pDestino;
                msg.Current.Origem = pOrigem;
                msg.Current.SYSxEmpresaID = msg.Current.SYSxEmpresaOrigemID = pEmpresaID;
                msg.Current.SYSxPessoaID = pPessoID;
                msg.Current.Integrado = false;
                msg.Flush();
            }
        }
    }
}