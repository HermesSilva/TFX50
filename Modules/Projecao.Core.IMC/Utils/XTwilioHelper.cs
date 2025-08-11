using System;

using Projecao.Core.IMC.Servico;

using Twilio;
using Twilio.Rest.Chat.V2.Service.Channel;
using Twilio.Types;

using static Projecao.Core.IMC.DB.IMCx;

namespace Projecao.Core.IMC.Utils
{
    public class XTwilioHelper
    {
        //public static String SendMessage(_IMCxMensagem pStorage, Guid pID, AtualizaTwilioSVC.XCFGTuple pConfig, String pSource, String pTarget, String pMensagem)
        //{
        //    TwilioClient.Init(pConfig.SID, pConfig.Token);
        //    pStorage.Open(pID);
        //    MessageResource message = MessageResource.Create(body: pMensagem, from: new PhoneNumber($"whatsapp:{pSource}"), to: new PhoneNumber($"whatsapp:{pTarget}"));
        //    pStorage.Current.IMCxEstadoID = IMCxEstado.XDefault.Enviada;
        //    pStorage.Current.Codigo = message.Sid;
        //    pStorage.Flush();
        //    return message.Sid;
        //}

        //public static String SendMessage(AtualizaTwilioSVC.XCFGTuple pConfig, String pSource, String pTarget, String pMensagem)
        //{
        //    TwilioClient.Init(pConfig.SID, pConfig.Token);
        //    MessageResource message = MessageResource.Create( body: pMensagem, from: new PhoneNumber($"whatsapp:{pSource}"), to: new PhoneNumber($"whatsapp:{pTarget}"));
        //    return message.Sid;
        //}

        public static void StoreMessage(_IMCxMensagem pStorage, MessageResource pMessage)
        {
            pStorage.NewTuple();
            pStorage.Current.IMCxEstadoID = IMCxEstado.XDefault.Enviada;
            pStorage.Current.Codigo = pMessage.Sid;
            pStorage.Flush();
        }
    }
}