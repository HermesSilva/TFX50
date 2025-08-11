using System;
using System.Collections.Generic;
using System.Linq;

using Projecao.Core.PET.Agendamento;
using Projecao.Core.PET.Consultas;
using Projecao.Core.PET.FecharAtendimento;

using TFX.Core.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.PET.DB.PETx;
using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.PET.ReadOnly
{
    [XRegister(typeof(BuscaAgendaRule), sCID, typeof(BuscaAgendaSVC))]
    public class BuscaAgendaRule : BuscaAgendaSVC.XRule
    {
        public const String sCID = "13B1B13E-BEC1-49D6-B5F6-27C64644CAFA";
        public static Guid gCID = new Guid(sCID);

        public BuscaAgendaRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, BuscaAgendaSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            if (pContext.Broker.AuxData.SafeLength() == 2)
            {
                pWhere.Add(BuscaAgendaSVC.xPETxAtendimento.PETxAtendimentoID, XOperator.In,
                    $"(select {PETxAtendimentoProfissional.PETxAtendimentoID.Name} from {PETxAtendimentoProfissional.Instance.Name} where " +
                    $"{PETxAtendimentoProfissional.Inicio.Name} between convert(datetime, '{((DateTime)pContext.Broker.AuxData[1]).ToString("yyyy-MM-dd")}T00:00:00', 126) and " +
                    $"convert(datetime, '{((DateTime)pContext.Broker.AuxData[1]).ToString("yyyy-MM-dd")}T23:59:59', 126))");
            }
            pWhere.Add(BuscaAgendaSVC.xPETxAtendimento.SYSxEstadoID, SYSxEstado.XDefault.Ativo);

            pWhere.Add(BuscaAgendaSVC.xPETxAtendimento.PETxAtendimentoEstadoID, PETxAtendimentoEstado.XDefault.Criado);
            if (pContext.SourceID == AgendamentoEsteticaRedSAM.gCID)
                pWhere.Add(BuscaAgendaSVC.xPETxAtendimento.PETxAtendimentoClasseID, PETxAtendimentoClasse.XDefault.Estetica);

            if (pContext.SourceID.In(AgendamentoSaudeRedSAM.gCID, ConsultasSAM.gCID))
                pWhere.Add(BuscaAgendaSVC.xPETxAtendimento.PETxAtendimentoClasseID, PETxAtendimentoClasse.XDefault.Saude);

            pWhere.Add(XParentheses.Open, BuscaAgendaSVC.xSYSxImagem.Principal, true, XLogic.OR, BuscaAgendaSVC.xSYSxImagem.Principal, XOperator.IsNull, XParentheses.Close);
        }

        protected override void AfterGet(XExecContext pContext, BuscaAgendaSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            Guid[] atendsid = pDataSet.Tuples.Select(t => t.PETxAtendimentoID).ToArray();

            if (pContext.SourceID == AgendamentoEsteticaRedSAM.gCID || pContext.SourceID == FecharAtendimentoSAM.gCID)
            {
                AgendamentoServicoSVC.XService ser = GetService<AgendamentoServicoSVC.XService>();
                ser.Open(AgendamentoServicoSVC.xPETxAtendimentoServicos.PETxAtendimentoID, XOperator.In, atendsid);
                foreach (BuscaAgendaSVC.XTuple tpl in pDataSet.Tuples)
                {
                    AgendamentoServicoSVC.XTuple svctpl = ser.Tuples.FirstOrDefault(t => t.PETxAtendimentoID == tpl.PETxAtendimentoID);
                    if (svctpl != null)
                        tpl.Servico = svctpl.Nome.AsTitleCase();
                }
            }

            if (pContext.SourceID == AgendamentoSaudeRedSAM.gCID || pContext.SourceID == FecharAtendimentoSAM.gCID)
            {
                AgendamentoVacinaSVC.XService ser = GetService<AgendamentoVacinaSVC.XService>();
                ser.Open(AgendamentoVacinaSVC.xPETxAtendimentoVacina.PETxAtendimentoID, XOperator.In, atendsid);
                foreach (BuscaAgendaSVC.XTuple tpl in pDataSet.Tuples)
                {
                    AgendamentoVacinaSVC.XTuple svctpl = ser.Tuples.FirstOrDefault(t => t.PETxAtendimentoID == tpl.PETxAtendimentoID);
                    if (svctpl != null && tpl.Servico.IsEmpty())
                        tpl.Servico = svctpl.Nome.AsTitleCase();
                }
            }
            pDataSet.SetOrderedTuples(pDataSet.Tuples.OrderBy(t => pDataSet.BuscaAgendaProfissionalDataSet.FilteredBy(t).FirstOrDefault()?.Inicio).ToArray());
        }
    }
}