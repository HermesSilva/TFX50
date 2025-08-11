using System;
using System.Collections.Generic;
using System.Linq;

using Projecao.Core.PET.Agendamento;
using Projecao.Core.PET.DB;

using TFX.Core;
using TFX.Core.Data;
using TFX.Core.Exceptions;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Model.Net;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.PET.DB.PETx;
using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.PET.FecharAtendimento
{
    [XRegister(typeof(FecharAtendimentoRule), sCID, typeof(FecharAtendimentoSVC))]
    public class FecharAtendimentoRule : FecharAtendimentoSVC.XRule
    {
        public const String sCID = "15BBD0EB-151A-4CB0-B3EE-89620E96B582";
        public static Guid gCID = new Guid(sCID);

        public FecharAtendimentoRule()
        {
            ID = gCID;
        }

        private XILog _Log = XLog.GetLogFor(typeof(FecharAtendimentoRule));
        private AgendamentoSVC.XService _Pacote;
        private _PETxAtendimentoVacina _Vacina;
        private _PETxAtendimentoServicos _Servico;

        protected override void GetWhere(XExecContext pContext, FecharAtendimentoSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(PETxAtendimento.PETxAtendimentoEstadoID, XOperator.In, new[] { PETxAtendimentoEstado.XDefault.Criado, PETxAtendimentoEstado.XDefault.Concluido });
            pWhere.Add(PETxAtendimento.SYSxEstadoID, SYSxEstado.XDefault.Ativo);
        }

        protected override void AfterFlush(XExecContext pContext, FecharAtendimentoSVC pModel, FecharAtendimentoSVC.XDataSet pDataSet)
        {
            try
            {
                AgendamentoSVC.XService agendamento = GetService<AgendamentoSVC.XService>();
                _Pacote = GetService<AgendamentoSVC.XService>();
                _Vacina = GetTable<_PETxAtendimentoVacina>();
                _Servico = GetTable<_PETxAtendimentoServicos>();

                Int32 tot = 0;
                if (pDataSet.Current.PETxAtendimentoEstadoID.In(PETx.PETxAtendimentoEstado.XDefault.Criado, PETx.PETxAtendimentoEstado.XDefault.Concluido))
                {
                    if (!agendamento.OpenFull(AgendamentoSVC.xPETxAtendimento.PETxAtendimentoID, pDataSet.Current.PETxAtendimentoID))
                        throw new XError("Falha no fechamento do Atendimento.");
                    agendamento.Current.GrupoOrdem = ++tot;
                    agendamento.Current.Grupo = pDataSet.Current.PETxAtendimentoID;
                    TrataPacote(_Pacote, _Vacina, _Servico, agendamento.DataSet);
                    agendamento.Flush();
                }
                foreach (FecharAtenAgrupadoSVC.XTuple tpl in pDataSet.FecharAtenAgrupadoDataSet.Tuples.Where(t => t.Agrupar))
                {
                    if (pDataSet.Current.PETxAtendimentoID == tpl.PETxAtendimentoID)
                        continue;
                    if (!agendamento.OpenFull(AgendamentoSVC.xPETxAtendimento.PETxAtendimentoID, tpl.PETxAtendimentoID))
                        throw new XError("Falha no fechamento do Atendimento.");
                    agendamento.Current.GrupoOrdem = ++tot;
                    agendamento.Current.Grupo = pDataSet.Current.PETxAtendimentoID;
                    TrataPacote(_Pacote, _Vacina, _Servico, agendamento.DataSet);
                    agendamento.Flush();
                }
            }
            finally
            {
                XWebSocket.SendWakeup(pContext.Logon.CurrentCompanyID);
            }
        }

        public static void TrataPacote(AgendamentoSVC.XService pPacote, _PETxAtendimentoVacina pVacina, _PETxAtendimentoServicos pServico, AgendamentoSVC.XDataSet pAgendamento, Boolean pSoValidar = false)
        {
            Guid olsource = Guid.Empty;
            try
            {
                olsource = pPacote.Context.SourceID;
                pPacote.Context.SourceID = FecharAtendimentoSAM.gCID;
                foreach (AgendamentoSVC.XTuple tpl in pAgendamento.Tuples.Where(t => t.PETxAtendimentoTipoID == PETxAtendimentoTipo.XDefault.Normal))
                {
                    foreach (AgendamentoVacinaSVC.XTuple vtpl in pAgendamento.AgendamentoVacinaDataSet.Tuples.Where(t => t.PETxAtendimentoID == tpl.PETxAtendimentoID && t.ItemPacote.IsFull()))
                    {
                        if (!pVacina.Open(vtpl.ItemPacote))
                            throw new XError($"Falha ao baixar pacote com Item [{vtpl.ItemPacote.AsString()}].");

                        if (!pPacote.OpenFull(AgendamentoSVC.xPETxAtendimento.PETxAtendimentoID, pVacina.Current.PETxAtendimentoID))
                            throw new XError($"Falha ao baixar pacote com Item [{vtpl.ItemPacote.AsString()}].");
                        if (pPacote.Current.PETxTutorID != tpl.PETxTutorID)
                            throw new XError($"Tentativa de baixar pacote do Tutor [{tpl.NomeTutor}] em atendimento do Tutor [{pPacote.Current.NomeTutor}].");
                        AgendamentoVacinaSVC.XTuple ptpl = pPacote.DataSet.AgendamentoVacinaDataSet.Tuples.FirstOrDefault(t => t.PETxAtendimentoVacinaID == vtpl.ItemPacote);
                        if (ptpl == null)
                            throw new XError($"Não foi encontrado no pacote de Tutor [{tpl.NomeTutor}] o Item [{vtpl.ItemPacote.AsString()}].");
                        if (ptpl.QuantidadePacote + vtpl.QuantidadePacote > ptpl.Quantidade)
                            throw new XUnconformity($"A quantidade [{vtpl.QuantidadePacote}] da Vacina [{ptpl.Nome}], a ser baixada do pacote do Tutor " +
                                $"[{pPacote.Current.NomeTutor}] é maior que o Saldo [{ptpl.Quantidade - ptpl.QuantidadePacote}].");
                        if (!pSoValidar)
                            ptpl.QuantidadePacote += vtpl.QuantidadePacote;
                    }
                    foreach (AgendamentoServicoSVC.XTuple vtpl in pAgendamento.AgendamentoServicoDataSet.Tuples.Where(t => t.PETxAtendimentoID == tpl.PETxAtendimentoID && t.ItemPacote.IsFull()))
                    {
                        if (!pServico.Open(vtpl.ItemPacote))
                            throw new XError($"Falha a baixar pacote com Item [{vtpl.ItemPacote.AsString()}].");
                        if (!pPacote.OpenFull(AgendamentoSVC.xPETxAtendimento.PETxAtendimentoID, pServico.Current.PETxAtendimentoID))
                            throw new XError($"Falha ao baixar pacote com Item [{vtpl.ItemPacote.AsString()}].");
                        if (pPacote.Current.PETxTutorID != tpl.PETxTutorID)
                            throw new XError($"Tentativa de baixar pacote do Tutor [{tpl.NomeTutor}] em atendimento do Tutor [{pPacote.Current.NomeTutor}].");
                        AgendamentoServicoSVC.XTuple ptpl = pPacote.DataSet.AgendamentoServicoDataSet.Tuples.FirstOrDefault(t => t.PETxAtendimentoServicosID == vtpl.ItemPacote);
                        if (ptpl == null)
                            throw new XError($"Não foi encontrado no pacote de Tutor [{tpl.NomeTutor}] o Item [{vtpl.ItemPacote.AsString()}].");
                        if (ptpl.QuantidadePacote + vtpl.QuantidadePacote > ptpl.Quantidade)
                            throw new XUnconformity($"A quantidade [{vtpl.QuantidadePacote}] do Serviço [{ptpl.Nome}], a ser baixada do pacote do Tutor " +
                                $"[{pPacote.Current.NomeTutor}] é maior que o Saldo [{ptpl.Quantidade - ptpl.QuantidadePacote}].");
                        if (!pSoValidar)
                            ptpl.QuantidadePacote += vtpl.QuantidadePacote;
                    }
                    if (!pSoValidar)
                    {
                        pPacote.Flush();
                        pPacote.Clear();
                    }
                }
                if (!pSoValidar)
                {
                    pAgendamento.Tuples.ForEach(t => t.PETxAtendimentoEstadoID = PETx.PETxAtendimentoEstado.XDefault.Fechado);
                    pAgendamento.AgendamentoExamesDataSet.Tuples.ForEach(t => t.PETxAtendimentoEstadoID = PETx.PETxAtendimentoEstado.XDefault.Fechado);
                    pAgendamento.AgendamentoServicoDataSet.Tuples.ForEach(t => t.PETxAtendimentoEstadoID = PETx.PETxAtendimentoEstado.XDefault.Fechado);
                    pAgendamento.AgendamentoVacinaDataSet.Tuples.ForEach(t => t.PETxAtendimentoEstadoID = PETx.PETxAtendimentoEstado.XDefault.Fechado);
                }
            }
            finally
            {
                pPacote.Context.SourceID = olsource;
            }
        }

        protected override void AfterCommit(XExecContext pContext)
        {
            try
            {
                //TODO
                //XHPCPushManager.TryPushTo(pContext);
            }
            catch (Exception pEx)
            {
                _Log.Error(pEx);
            }
        }
    }
}