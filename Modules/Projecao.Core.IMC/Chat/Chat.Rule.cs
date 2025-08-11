using System;
using System.Linq;

using Projecao.Core.ERP.GerenciaEmpresa;
using Projecao.Core.IMC.Servico;
using Projecao.Core.IMC.Utils;

using TFX.Core;
using TFX.Core.Exceptions;
using TFX.Core.Model.Net;
using TFX.Core.Reflections;
using TFX.Core.Service.Cache;
using TFX.Core.Service.Data;

using static Projecao.Core.ERP.DB.ERPx;
using static Projecao.Core.IMC.DB.IMCx;

namespace Projecao.Core.IMC.Chat
{
    [XRegister(typeof(ChatRule), sCID, typeof(ChatSVC))]
    public class ChatRule : ChatSVC.XRule
    {
        public const String sCID = "E168776E-BDE5-42FF-8D83-06DC6DB213F5";
        public static Guid gCID = new Guid(sCID);

        public ChatRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, ChatSVC pModel, ChatSVC.XDataSet pDataSet)
        {
            AtualizaTwilioSVC.XCFGTuple cfg = XConfigCache.Get<AtualizaTwilioSVC.XCFGTuple>();
            if (cfg == null)
                throw new XUnconformity("Service de Mesagem não configurado.");
            _ERPxContato contato = GetTable<_ERPxContato>();
            foreach (ChatSVC.XTuple tpl in pDataSet.ToArray())
            {
                if (tpl.Retorno)
                    continue;
                if (!contato.Open(ERPxContato.SYSxPessoaID, tpl.SYSxPessoaID, ERPxContato.ERPxFinalidadeID, ERPxFinalidade.XDefault.Envio_de_Mensagens, ERPxContato.ERPxContatoTipoID,
                       ERPxContatoTipo.XDefault.Mensagem_WhatsApp))
                    throw new XUnconformity("Pessoa não tem Contato válido para Mensagem de WhatsApp.");
                tpl.SYSxEmpresaOrigemID = pContext.Logon.CurrentCompanyID;
                ConfigITGFRM.XCFGTuple cfgitg = XConfigCache.GetByCompany<ConfigITGFRM.XCFGTuple>(tpl.SYSxEmpresaOrigemID);
                String dest = PreparePhone(contato.Current.Contato, cfgitg);
                tpl.Destino = dest;
                tpl.Origem = cfgitg.TwilioNumero;
                tpl.Data = XDefault.Now;
                //tpl.Codigo = XTwilioHelper.SendMessage(cfg, $"+{cfgitg.TwilioNumero}", dest, XEncoding.Default.GetString(tpl.Mensagem));
                tpl.IMCxEstadoID = IMCxEstado.XDefault.Enviada;
            }
        }

        private String PreparePhone(String pDestino, ConfigITGFRM.XCFGTuple pCFG)
        {
            if (pDestino.Length > 8)
                pDestino = pDestino.SafePadLeft(8, '0');
            switch (pDestino.Length)
            {
                case 8:
                    return $"+{pCFG.CodigoPais}{pCFG.DDD}{pDestino}";

                case 9:
                    return $"+{pCFG.CodigoPais}{pCFG.DDD}{pDestino}";

                case 10:
                    return $"+{pCFG.CodigoPais}{pDestino}";

                case 11:
                    return $"+{pCFG.CodigoPais}{pDestino}";

                case 12:
                    return $"+{pDestino}";

                case 13:
                    return $"+{pDestino}";

                default:
                    throw new XError("Número do destinatário está invalido, deve conter de 10 à 13 dígitos númericos");
            }
        }

        protected override void AfterFlush(XExecContext pContext, ChatSVC pModel, ChatSVC.XDataSet pDataSet)
        {
            foreach (ChatSVC.XTuple tpl in pDataSet.Tuples.Where(t => !t.Retorno))
            {
                XWSChatMessage ms = new XWSChatMessage();
                ms.Mensagem = XEncoding.Default.GetString(tpl.Mensagem);
                ms.ContatoID = tpl.SYSxPessoaID.AsString();
                ms.Data = tpl.Data;
                ms.Classe = tpl.IMCxClasseID;
                ms.Estado = tpl.IMCxEstadoID;
                XWebSocket.Instance.SendChatMessageByContext(pContext.ID, ms);
            }
        }
    }
}