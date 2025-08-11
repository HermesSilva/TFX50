using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

using Microsoft.AspNetCore.Http;

using Projecao.Core.ERP.GerenciaEmpresa;
using Projecao.Core.IMC.Chat;
using Projecao.Core.IMC.Utils;

using TFX.Core;
using TFX.Core.Exceptions;
using TFX.Core.Model.Cache;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Model.Net;
using TFX.Core.Reflections;
using TFX.Core.Service.Cache;
using TFX.Core.Service.Data;
using TFX.Core.Service.Job;
using TFX.Core.Service.Web;

using Twilio;
using Twilio.Base;
using Twilio.Rest.Api.V2010.Account;

using static Projecao.Core.ERP.DB.ERPx;
using static Projecao.Core.IMC.DB.IMCx;
using static Twilio.Rest.Api.V2010.Account.MessageResource;

namespace Projecao.Core.IMC.Servico
{
    [XRegister(typeof(AtualizaTwilioRule), sCID, typeof(AtualizaTwilioSVC))]
    public class AtualizaTwilioRule : AtualizaTwilioSVC.XRule
    {
        public const String sCID = "B3ECEFD5-3DF9-4835-AD03-27E2021004A0";
        public static Guid gCID = new Guid(sCID);

        static AtualizaTwilioRule()
        {
            XContextManager.ICMReceive += AddMessagem;
        }

        private static void AddMessagem(IFormCollection pForm)
        {
            String id = pForm["SmsSid"].ToString();
            AtualizaTwilioSVC.XCFGTuple cfg = XConfigCache.Get<AtualizaTwilioSVC.XCFGTuple>();
            if (cfg.Token.IsEmpty() || cfg.SID.IsEmpty())
                throw new XError("Há compo sem preenchimento nas configurações do Serviço");
            TwilioClient.Init(cfg.SID, cfg.Token);
            MessageResource inmsg = MessageResource.Fetch(pathSid: id);
            using (XExecContext ctx = XExecContext.Create())
            using (_ERPxContato cnt = XPersistencePool.Get<_ERPxContato>(ctx))
            using (_IMCxMensagem msg = XPersistencePool.Get<_IMCxMensagem>(ctx))
            using (ChatSVC.XService chat = XServicePool.Get<ChatSVC.XService>(ctx))

            {
                try
                {
                    XConsole.Debug(inmsg.Sid + "\r\n" + inmsg.Status + "\r\n" + inmsg.Body);

                    if (inmsg.Direction == DirectionEnum.Inbound)
                    {
                        String from = inmsg.From.ToString().SafeBreak("+")[1];
                        String to = inmsg.To.ToString().SafeBreak("+")[1];
                        if (!msg.Open(IMCxMensagem.Codigo, inmsg.Sid))
                        {
                            ConfigITGFRM.XCFGTuple cfgitg = XConfigCache.Configs.FirstOrDefault<ConfigITGFRM.XCFGTuple>(c => c.TwilioNumero == from || c.TwilioNumero == to);
                            if (cfgitg == null)
                                throw new XError($"Mensagem com origem e destinatário desconhecidos.\r\nOrigem: {from}\r\nDestinatário: {to}\r\nMensagem: {inmsg.Body}");
                            ChatSVC.XTuple tpl = AddMSG(cnt, chat, cfgitg, IMCxClasse.XDefault.Entrada, inmsg);
                            XWSChatMessage ms = new XWSChatMessage();
                            ms.Mensagem = inmsg.Body;
                            ms.ContatoID = tpl.SYSxPessoaID.AsString();
                            ms.CompanyID = tpl.SYSxEmpresaOrigemID.AsString();
                            ms.Data = tpl.Data;
                            ms.Classe = tpl.IMCxClasseID;
                            ms.Estado = tpl.IMCxEstadoID;
                            XWebSocket.Instance.SendChatMessage(cfgitg.CompanyID, tpl.SYSxPessoaID, ms);
                        }
                        return;
                    }
                    if (inmsg.Status == StatusEnum.Read && msg.Open(IMCxMensagem.IMCxEstadoID, XOperator.NotEqualTo, IMCxEstado.XDefault.Lida, IMCxMensagem.Codigo, inmsg.Sid))
                    {
                        msg.Current.IMCxEstadoID = IMCxEstado.XDefault.Lida;
                        msg.Flush();
                    }
                }
                finally
                {
                    ctx.Commit();
                }
            }
        }

        public AtualizaTwilioRule()
        {
            ID = gCID;
        }

        protected override void DoExecute(XExecContext pContext, XJobState pJobState)
        {
            //if (ID != Guid.Empty)
            //    return;

            _IMCxMensagem msg = GetTable<_IMCxMensagem>();
            _IMCxMensagem updmsg = GetTable<_IMCxMensagem>(true);
            AtualizaTwilioSVC.XCFGTuple cfg = GetConfig();
            if (cfg.Token.IsEmpty() || cfg.SID.IsEmpty())
                throw new XError("Há compo sem preenchimento nas configurações do Serviço");

            if (msg.Open(IMCxMensagem.IMCxEstadoID, IMCxEstado.XDefault.Nao_Enviada, IMCxMensagem.IMCxClasseID, XOperator.NotEqualTo, IMCxClasse.XDefault.Entrada))
            {
                foreach (IMCxMensagem.XTuple tpl in msg.Tuples.OrderBy(t => t.Data))
                {
                    try
                    {
                        Thread.Sleep(cfg.Delay);
                        //XTwilioHelper.SendMessage(updmsg, tpl.IMCxMensagemID, cfg, $"+{tpl.Origem}", $"+{tpl.Destino}", XEncoding.Default.GetString(tpl.Mensagem));
                    }
                    catch (Exception pEx)
                    {
                        Log.Error(pEx);
                    }
                }
            }
            _IMCxBuscaMensagem bscmsg = GetTable<_IMCxBuscaMensagem>();
            _ERPxContato cont = GetTable<_ERPxContato>();
            ChatSVC.XService chat = GetService<ChatSVC.XService>();
            bscmsg.Open(Guid.Empty);
            TwilioClient.Init(cfg.SID, cfg.Token);
            foreach (ConfigITGFRM.XCFGTuple cfgitg in XConfigCache.Configs.Where(c => c is ConfigITGFRM.XCFGTuple))
            {
                try
                {
                    if (cfgitg.TwilioNumero.IsEmpty())
                        continue;
                    ReadMessageOptions opt = new ReadMessageOptions();
                    opt.To = $"whatsapp:+{cfgitg.TwilioNumero}";
                    DateTime now = DateTime.Now;
                    if (bscmsg.Current.UltimaChamada.IsEmpty())
                        opt.DateSentBefore = now;
                    else
                        opt.DateSentAfter = bscmsg.Current.UltimaChamada;
                    bscmsg.Current.UltimaChamada = now;

                    ProcessMessages(cont, chat, cfgitg, IMCxClasse.XDefault.Entrada, MessageResource.Read(opt));

                    opt.To = null;
                    opt.From = $"whatsapp:+{cfgitg.TwilioNumero}";
                    ProcessMessages(cont, chat, cfgitg, IMCxClasse.XDefault.Saida_Chat, MessageResource.Read(opt));

                    bscmsg.Flush();
                }
                catch (Exception pEx)
                {
                    Log.Error(pEx);
                }
            }
        }

        private void ProcessMessages(_ERPxContato cont, ChatSVC.XService chat, ConfigITGFRM.XCFGTuple cfgitg, Int16 pIMCxClasseID, ResourceSet<MessageResource> msgs)
        {
            foreach (MessageResource msgr in msgs)
            {
                try
                {
                    AddMSG(cont, chat, cfgitg, pIMCxClasseID, msgr);
                }
                catch (Exception pEx)
                {
                    Log.Error(pEx);
                }
            }
        }

        private static ChatSVC.XTuple AddMSG(_ERPxContato cont, ChatSVC.XService chat, ConfigITGFRM.XCFGTuple cfgitg, short pIMCxClasseID, MessageResource msgr)
        {
            if (!chat.Open(ChatSVC.xIMCxMensagem.Codigo, msgr.Sid))
            {
                ChatSVC.XTuple tpl = chat.NewTuple();
                String from;
                String target;
                if (pIMCxClasseID == IMCxClasse.XDefault.Entrada)
                {
                    from = msgr.From.ToString();
                    target = msgr.To;
                }
                else
                {
                    from = msgr.To;
                    target = msgr.From.ToString();
                }
                List<Object> where = GetWhere(from, ERPxContato.Contato, out String fromnumber);
                where.Add(ERPxContato.ERPxFinalidadeID, ERPxFinalidade.XDefault.Envio_de_Mensagens, ERPxContato.ERPxContatoTipoID,
                        ERPxContatoTipo.XDefault.Mensagem_WhatsApp);
                if (cont.Open(where.ToArray()))
                    tpl.SYSxPessoaID = cont.Current.SYSxPessoaID;
                tpl.SYSxEmpresaID = tpl.SYSxEmpresaOrigemID = cfgitg.CompanyID;
                tpl.Mensagem = XEncoding.Default.GetBytes(msgr.Body);
                tpl.Codigo = msgr.Sid;
                if (msgr.DateSent.HasValue)
                    tpl.Data = msgr.DateSent.Value;
                if (tpl.Data.IsEmpty() && msgr.DateCreated.HasValue)
                    tpl.Data = msgr.DateCreated.Value;

                tpl.IMCxClasseID = pIMCxClasseID;
                tpl.IMCxEstadoID = IMCxEstado.XDefault.Lida;
                tpl.Origem = fromnumber;
                tpl.Retorno = true;
                if (target.SafeContains("+"))
                    target = target.SafeBreak("+")[1];
                tpl.Destino = target;
                chat.Flush();
                XConsole.Debug(msgr.Body);
                return tpl;
            }
            return null;
        }

        private static List<Object> GetWhere(String pNumber, XORMField pContato, out String pFromNumber)
        {
            String[] nbrs = pNumber.SafeBreak("+");
            pFromNumber = nbrs[1];
            Int32[] lens = new[] { 8, 9, 10, 11, 12, 13 };
            List<Object> where = new List<Object>();
            where.Add(XParentheses.Open);
            String nr = nbrs[1];
            Int32 nrl = nr.Length;
            foreach (Int32 len in lens)
            {
                if (nrl - len < 0)
                    break;
                if (where.Count > 1)
                    where.Add(XLogic.OR);
                where.Add(pContato, nr.Substring(nrl - len, len));
            }
            where.Add(XParentheses.Close);
            return where;
        }
    }
}