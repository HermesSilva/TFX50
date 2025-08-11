using TFX.Core.Model.Data;
using TFX.Core.Reflections;

namespace Projecao.Core.PET.Agendamento
{
    [XRegister(typeof(AgendamentoServicoSVCTupleRule), "ED5368B0-732A-476A-B230-F155AB8BE7A7", typeof(AgendamentoServicoSVC))]
    [XToTypeScript(typeof(AgendamentoServicoSVC))]
    public class AgendamentoServicoSVCTupleRule : XDataTupleRule<AgendamentoServicoSVC.XDataSet, AgendamentoServicoSVC.XTuple>
    {
        public AgendamentoServicoSVCTupleRule(AgendamentoServicoSVC.XTuple pTuple)
          : base(pTuple)
        {
            BindField(DataSet.FLDs.Quantidade);
            BindField(DataSet.FLDs.ValorCobrado);
            BindField(DataSet.FLDs.DescontoProporcional);
        }

        public override void RefreshValues(AgendamentoServicoSVC.XDataSet pDataSet)
        {
            Tuple.ValorTotal = (Tuple.ValorCobrado > 0 ? Tuple.ValorCobrado : Tuple.ValorTabela) * (Tuple.Quantidade - Tuple.QuantidadePacote) - Tuple.DescontoProporcional;
        }
    }
}