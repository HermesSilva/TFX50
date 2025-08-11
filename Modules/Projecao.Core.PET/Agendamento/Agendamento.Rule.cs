using System;
using System.Collections.Generic;
using System.Linq;

using Projecao.Core.ERP.GerenciaEmpresa;
using Projecao.Core.ERP.Pessoa;
using Projecao.Core.IMC.Chat;
using Projecao.Core.IMC.Mensagens;
using Projecao.Core.IMC.Utils;
using Projecao.Core.PET.DB;
using Projecao.Core.PET.FecharAtendimento;
using Projecao.Core.PET.Pacote;
using Projecao.Core.PET.ReadOnly;
using Projecao.Core.PET.Tutor;

using TFX.Core;
using TFX.Core.Data;
using TFX.Core.Exceptions;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Model.DIC.SVC;
using TFX.Core.Model.Net;
using TFX.Core.Reflections;
using TFX.Core.Service.Apps.Tools;
using TFX.Core.Service.Cache;
using TFX.Core.Service.Data;

using static System.Net.WebRequestMethods;
using static Projecao.Core.ERP.DB.ERPx;
using static Projecao.Core.IMC.DB.IMCx;
using static Projecao.Core.IMC.Mensagens.MensagensSVC;
using static Projecao.Core.PET.DB.PETx;
using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.PET.Agendamento
{
    [XRegister(typeof(AgendamentoRule), sCID, typeof(AgendamentoSVC))]
    public class AgendamentoRule : AgendamentoSVC.XRule
    {
        public const String sCID = "A8AB8A1D-F8FE-4E9C-974C-BAB57C02ED64";
        public static Guid gCID = new Guid(sCID);

        public AgendamentoRule()
        {
            ID = gCID;
        }

        private List<Int32> _ParaFechar = new List<Int32>();
        private XILog _Log = XLog.GetLogFor(typeof(AgendamentoRule));
        private AgendamentoSVC.XService _Pacote;
        private _PETxAtendimentoVacina _Vacina;
        private _PETxAtendimentoServicos _Servico;

        protected override void BeforeChangeState(XExecContext pContext, AgendamentoSVC pModel, AgendamentoSVC.XDataSet pDataSet)
        {
            if (pDataSet.Tuples.Any(t => t.PETxAtendimentoEstadoID != PETxAtendimentoEstado.XDefault.Criado))
                throw new XUnconformity("Apenas Atendimento com Estado 'Criado' permite ser excluido");
        }

        protected override void GetWhere(XExecContext pContext, AgendamentoSVC.XDataSet pDataSet, List<Object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, Object>, List<Object>>> pRawFilter, Boolean pIsPKGet)
        {
            if (pContext.SourceID == BIVRunnerSVC.gCID)
                pWhere.Add(AgendamentoSVC.xPETxAtendimento.PETxAtendimentoClasseID, PETxAtendimentoClasse.XDefault.Estetica, AgendamentoSVC.xPETxAtendimento.PETxAtendimentoEstadoID, PETxAtendimentoEstado.XDefault.Criado);

            if (pContext.SourceID == CriacaoPacoteSAM.gCID)
                pWhere.Add(AgendamentoSVC.xPETxAtendimento.PETxAtendimentoTipoID, PETxAtendimentoTipo.XDefault.Pacote);
            if (pContext.SourceID == AgendamentoEsteticaRedSAM.gCID)
                pWhere.Add(AgendamentoSVC.xPETxAtendimento.PETxAtendimentoClasseID, PETxAtendimentoClasse.XDefault.Estetica);
            if (pContext.SourceID == AgendamentoSaudeRedSAM.gCID)
                pWhere.Add(AgendamentoSVC.xPETxAtendimento.PETxAtendimentoClasseID, PETxAtendimentoClasse.XDefault.Saude);
        }

        protected override void AfterCommit(XExecContext pContext)
        {
            if (_ParaFechar.Count > 0)
            {
                try
                {
                    using (XExecContext ctx = pContext.Clone())
                    {
                        FecharAtendimentoSVC.XService fechaatendimento = GetService<FecharAtendimentoSVC.XService>();
                        foreach (Int32 atdid in _ParaFechar)
                        {
                            if (fechaatendimento.Open(atdid))
                            {
                                fechaatendimento.Current.Observacao += " - Fechado Automaticamente.";
                                fechaatendimento.Flush();
                            }
                        }
                        XWebSocket.SendWakeup(pContext.Logon.CurrentCompanyID);
                    }
                }
                catch (Exception pEx)
                {
                    _Log.Error(pEx);
                }
                finally
                {
                    _ParaFechar.Clear();
                }
            }
        }

        protected override void AfterGet(XExecContext pContext, AgendamentoSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            EnderecoSVC.XService end = GetService<EnderecoSVC.XService>();
            _ERPxContato cnt = GetTable<_ERPxContato>();
            pDataSet.Tuples.ForEach(t =>
            {
                EnderecoSVC.XTuple ed;
                if (cnt.Open(ERPxContato.SYSxPessoaID, t.PETxTutorID, ERPxContato.ERPxContatoTipoID, XOperator.In, new[] { ERPxContatoTipo.XDefault.Mensagem_WhatsApp, ERPxContatoTipo.XDefault.Chat_, ERPxContatoTipo.XDefault.Telefone_Fixo, ERPxContatoTipo.XDefault.Telefone_Celular }))
                {
                    ERPxContato.XTuple ctpl;
                    if (cnt.Count == 1)
                        ctpl = cnt.Current;
                    else
                        ctpl = cnt.Tuples.FirstOrDefault();
                    t.TutorTelefone = ctpl.Contato;
                }
                if (end.Open(t.ERPxEnderecoBuscaID) && t.ERPxEnderecoBuscaID.IsFull())
                {
                    ed = end.Current;
                    t.EnderecoBusca = $"{(!t.DataBusca.IsEmpty() ? t.DataBusca.ToString("dd/MM HH:mm") + " - " : "")}{t.TipoBusca} {t.LogradouroBusca} {t.NumeroBusca}, Q-{t.QuadraBusca}, L-{t.LoteBusca}, {t.ComplementoBusca}, {ed.Bairro}, {t.CidadeBusca}";
                    if (t.ObservacaoBusca.IsFull())
                        t.EnderecoBusca += $"\r\n{t.ObservacaoBusca}";
                }
                else
                    t.EnderecoBusca = "Não agendado";
                if (end.Open(t.ERPxEnderecoEntregaID) && t.ERPxEnderecoEntregaID.IsFull())
                {
                    ed = end.Current;
                    t.EnderecoEntrega = $"{(!t.DataEntrega.IsEmpty() ? t.DataEntrega.ToString("dd/MM HH:mm") + " - " : "")}{t.TipoEntrega} {t.LogradouroEntrega} {t.NumeroEntrega}, Q-{t.QuadraEntrega}, L-{t.LoteEntrega}, {t.ComplementoEntrega}, {ed.Bairro}, {t.CidadeEntrega}";
                    if (t.ObservacaoEntrega.IsFull())
                        t.EnderecoEntrega += $"\r\n{t.ObservacaoEntrega}";
                }
                else
                    t.EnderecoEntrega = "Não agendado";
                t.SetReadOnly(t.PETxAtendimentoEstadoID.In(PETxAtendimentoEstado.XDefault.Cancelado, PETxAtendimentoEstado.XDefault.Recebido, PETxAtendimentoEstado.XDefault.Fechado));
                t.ErrorMessage = XErrorCache.GetError(AgendamentoSVC.gCID, t.PETxAtendimentoID)?.Mensagem;
            });
        }

        protected override void BeforeFlush(XExecContext pContext, AgendamentoSVC pModel, AgendamentoSVC.XDataSet pDataSet)
        {
            //string msg = "Avisamos que o PET <@Agendamento.NomeAnimal@> foi entendido às <@Agendamento.Data|HH:mm@> do dia <@Agendamento.Data|dd/MM/yyyy@> o valor dos serviços ficou <@Agendamento.ValorCobrado|#,##0.00@>\r\nOs serviços executados foram <@Agendamento.AgendamentoServico.Nome|LISTA@>";
            //msg = XSVCVariableManager.Parse(msg, pDataSet);

            if (pContext.SourceID == FecharAtendimentoSAM.gCID)
                return;
            BuscaAgendaSVC.XService ba = GetService<BuscaAgendaSVC.XService>();
            _ERPxEndereco end = GetTable<_ERPxEndereco>();
            _PETxAnimalTutor atut = GetTable<_PETxAnimalTutor>();
            _Pacote = null;
            if (pDataSet.AgendamentoVacinaDataSet.Tuples.Any(t => t.Quantidade <= 0))
                throw new XUnconformity($"Não é permitido Atendimento com Vacinas onde o campo Quantidade seja menor ou igual a zero.");
            if (pDataSet.AgendamentoServicoDataSet.Tuples.Any(t => t.Quantidade <= 0))
                throw new XUnconformity($"Não é permitido Atendimento com Serviços onde o campo Quantidade seja menor ou igual a zero.");
            if (pDataSet.AgendamentoProdutoDataSet.Tuples.Any(t => t.Quantidade <= 0))
                throw new XUnconformity($"Não é permitido Atendimento com Produtos onde o campo Quantidade seja menor ou igual a zero.");

            _Pacote = GetService<AgendamentoSVC.XService>();
            _Vacina = GetTable<_PETxAtendimentoVacina>();
            _Servico = GetTable<_PETxAtendimentoServicos>();
            FecharAtendimentoRule.TrataPacote(_Pacote, _Vacina, _Servico, pDataSet, true);

            foreach (AgendamentoSVC.XTuple tpl in pDataSet.Where(t => !t.State.In(XTupleState.Revoked, XTupleState.Recycled)))
            {
                if (pContext.SourceID == CriacaoPacoteSAM.gCID)
                    tpl.PETxAtendimentoTipoID = PETxAtendimentoTipo.XDefault.Pacote;

                if (tpl.PETxAtendimentoTipoID == PETxAtendimentoTipo.XDefault.Pacote)
                    if (pDataSet.AgendamentoVacinaDataSet.Count == 0 && pDataSet.AgendamentoServicoDataSet.Count == 0)
                        throw new XUnconformity($"Não é permitido pacote sem Serviços e Vacinas.");

                if (tpl.PETxAnimalTutorID.IsEmpty())
                    throw new XUnconformity($"Não é permitido Atendimento sem PET.");
                if (!end.Open(ERPxEndereco.SYSxPessoaID, tpl.PETxTutorID))
                    throw new XUnconformity($"Tutor [{tpl.NomeTutor}] não possui endereço.");

                if (!atut.Open(PETxAnimalTutor.PETxAnimalTutorID, tpl.PETxAnimalTutorID, PETxAnimalTutor.PETxTutorID, tpl.PETxTutorID))
                    throw new XUnconformity($"O PET informado não pertence ao tutor \"{tpl.NomeTutor}\".");
                tpl.PETxAnamnesiaAnimalID = atut.Current.PETxAnimalID;

                if (tpl.ERPxEnderecoBuscaID.IsFull() && !end.Open(ERPxEndereco.ERPxEnderecoID, tpl.ERPxEnderecoBuscaID, ERPxEndereco.SYSxPessoaID, tpl.PETxTutorID))
                    throw new XUnconformity($"O Endereço de Busca informado não pertence ao tutor \"{tpl.NomeTutor}\".");

                if (tpl.ERPxEnderecoEntregaID.IsFull() && !end.Open(ERPxEndereco.ERPxEnderecoID, tpl.ERPxEnderecoEntregaID, ERPxEndereco.SYSxPessoaID, tpl.PETxTutorID))
                    throw new XUnconformity($"O Endereço de Entreda informado não pertence ao tutor \"{tpl.NomeTutor}\".");

                if (tpl.PETxAtendimentoClasseID == PETxAtendimentoClasse.XDefault.NI && tpl.PETxAtendimentoTipoID == PETxAtendimentoTipo.XDefault.Normal)
                {
                    tpl.PETxAtendimentoClasseID = PETxAtendimentoClasse.XDefault.Todas;
                    if (!(pDataSet.AgendamentoProfiEsteDataSet.Tuples.Any(t => t.SYSxEstadoID == SYSxEstado.XDefault.Ativo && t.PETxAtendimentoID == tpl.PETxAtendimentoID) &&
                        pDataSet.AgendamentoProfiSaudeDataSet.Tuples.Any(t => t.SYSxEstadoID == SYSxEstado.XDefault.Ativo && t.PETxAtendimentoID == tpl.PETxAtendimentoID)))
                    {
                        if (pDataSet.AgendamentoProfiEsteDataSet.Tuples.Any(t => t.PETxAtendimentoID == tpl.PETxAtendimentoID))
                            tpl.PETxAtendimentoClasseID = PETxAtendimentoClasse.XDefault.Estetica;
                        if (pDataSet.AgendamentoProfiSaudeDataSet.Tuples.Any(t => t.PETxAtendimentoID == tpl.PETxAtendimentoID))
                            tpl.PETxAtendimentoClasseID = PETxAtendimentoClasse.XDefault.Saude;
                    }
                    if (pContext.SourceID == AgendamentoEsteticaRedSAM.gCID)
                        tpl.PETxAtendimentoClasseID = PETxAtendimentoClasse.XDefault.Estetica;
                    if (pContext.SourceID == AgendamentoSaudeRedSAM.gCID)
                        tpl.PETxAtendimentoClasseID = PETxAtendimentoClasse.XDefault.Saude;
                }
                if (tpl.State == XTupleState.New)
                {
                    tpl.Numero = NextSequence<Int32>(PETxAtendimento.Numero);
                    tpl.CTLxUsuarioID = pContext.Logon.UserID;
                    tpl.Data = XDefault.Now;
                    tpl.AnamnesiaData = tpl.Data.Date;
                    tpl.AnamnesiaNumero = tpl.Numero;
                    tpl.PETxAnamnesiaClasseID = tpl.PETxAtendimentoClasseID;
                }
                else
                {
                    Object vlr = null;
                    Boolean naopodealterar = tpl.PETxAtendimentoEstadoID.In(PETxAtendimentoEstado.XDefault.Cancelado, PETxAtendimentoEstado.XDefault.Recebido, PETxAtendimentoEstado.XDefault.Fechado);
                    naopodealterar = naopodealterar && tpl.oPETxAtendimentoEstadoID != tpl.PETxAtendimentoEstadoID;
                    naopodealterar = naopodealterar && (new[] { PETxAtendimentoEstado.XDefault.Cancelado, PETxAtendimentoEstado.XDefault.Recebido, PETxAtendimentoEstado.XDefault.Fechado }).Any(v => Object.Equals(v, vlr));
                    if (naopodealterar)
                        throw new XUnconformity($"Não é permitido Alterar um Atendimento que esteja {PETxAtendimentoEstado.Instance.GetValue<String>(PETxAtendimentoEstado.Estado, tpl.PETxAtendimentoEstadoID)}.");
                }
                if (tpl.PETxAtendimentoTipoID == PETxAtendimentoTipo.XDefault.Pacote && (pDataSet.AgendamentoProdutoDataSet.Count > 0 || pDataSet.AgendamentoExamesDataSet.Count > 0))
                    throw new XUnconformity($"Não é permitido Atendimento do tipo Pacote, conter Produtos, Vacinas ou Exames.");

                //if (pContext.SourceID == AtendimentoEsteticaPAM.CID)
                //    _ParaFechar.Add(tpl.PETxAtendimentoID);
            }
        }

        protected override void AfterFlush(XExecContext pContext, AgendamentoSVC pModel, AgendamentoSVC.XDataSet pDataSet)
        {
            EnviarMensagem(pContext, pDataSet, IMCxEvento.XDefault.Entrada_para_Atendimento, IMCxEvento.XDefault.sEntrada_para_Atendimento, _Log);
        }

        protected override string ExecuteCustom(XExecContext pContext, AgendamentoSVC.XDataSet pDataSet)
        {
            AgendamentoSVC.XService atend = GetService<AgendamentoSVC.XService>();
            atend.OpenFull(AgendamentoSVC.xPETxAtendimento.PETxAtendimentoID, pContext.Broker.AuxData[0]);
            if (pContext.Broker.AuxData[1].AsString() == AgendamentoEsteticaRedSAM.Concluido.ID.AsString())
                EnviarMensagem(pContext, atend.DataSet, IMCxEvento.XDefault.Finalizacao_do_Atendimento, IMCxEvento.XDefault.sFinalizacao_do_Atendimento, _Log);
            else
            {
                if (atend.DataSet.Tuples.All(t => t.DataEntrega.IsEmpty()))
                    throw new XUnconformity("Não existe endereço de entrega, do PET, para este atendimento.");
                EnviarMensagem(pContext, atend.DataSet, IMCxEvento.XDefault.Saida_para_Entrega, IMCxEvento.XDefault.sSaida_para_Entrega, _Log);
            }
            return "Mensagem enviada";
        }

        public static void EnviarMensagem(XExecContext pContext, AgendamentoSVC.XDataSet pDataSet, Int16 pEvento, String pEventNome, XILog pLog)
        {
            TutorSVC.XService tutor = XServicePool.Get<TutorSVC.XService>(TutorSVC.gCID, pContext);
            AgendamentoSVC.XService agenda = XServicePool.Get<AgendamentoSVC.XService>(AgendamentoSVC.gCID, pContext);
            MensagensSVC.XService msg = XServicePool.Get<MensagensSVC.XService>(MensagensSVC.gCID, pContext);
            ChatSVC.XService chat = XServicePool.Get<ChatSVC.XService>(ChatSVC.gCID, pContext);
            ConfigITGFRM.XCFGTuple cfg = XConfigCache.Get<ConfigITGFRM.XCFGTuple>(ConfigITGFRM.gCID, pContext.Logon.CurrentCompanyID);
            if (cfg == null || cfg.TwilioNumero.IsEmpty())
            {
                String err = $"Número para envio de mensagem da Empresa [{pContext.Logon.CompanyName}] não foi configurado.";
                if (pEvento == IMCxEvento.XDefault.Entrada_para_Atendimento)
                    pLog.Warn(err);
                else
                    throw new XWarning(err);
                return;
            }
            if (!msg.Open(MensagensSVC.xIMCxAgendaMensagem.IMCxEventoID, pEvento))
            {
                String err = $"Não existe mensagem de {pEventNome} para a empresa [{pContext.Logon.CompanyName}].";
                if (pEvento == IMCxEvento.XDefault.Entrada_para_Atendimento)
                    pLog.Warn(err);
                else
                    throw new XWarning(err);
            }
            foreach (AgendamentoSVC.XTuple tpl in pDataSet.Where(t => (pEvento == IMCxEvento.XDefault.Entrada_para_Atendimento && t.State == XTupleState.New) || t.State != XTupleState.New))
            {
                ContatoSVC.XTuple contato = null;
                agenda.OpenFull(AgendamentoSVC.xPETxAtendimento.PETxAtendimentoID, tpl.PKValue);
                if (tutor.OpenFull(TutorSVC.xPETxTutor.PETxTutorID, tpl.PETxTutorID))
                {
                    contato = tutor.DataSet.ContatoDataSet.Tuples.FirstOrDefault(t => t.ERPxFinalidadeID == ERPxFinalidade.XDefault.Envio_de_Mensagens);
                    if (contato == null)
                    {
                        String err = $"Tutor não tem contato com Finalidade {ERPxFinalidade.XDefault.sEnvio_de_Mensagens}, mensagem não será enviada.";
                        if (pEvento == IMCxEvento.XDefault.Entrada_para_Atendimento)
                        {
                            pLog.Warn(err);
                            continue;
                        }
                        else
                            throw new XWarning(err);
                    }
                }
                else
                    continue;
                foreach (MensagensSVC.XTuple msgtpl in msg.DataSet)
                    XMessageBroker.EnviarMensagem(pContext, pContext.Logon.CurrentCompanyID, tpl.PETxTutorID, cfg.TwilioNumero, contato.Contato,
                        XSVCVariableManager.Parse(msgtpl.Texto, tpl, pDataSet));
            }
        }
    }
}