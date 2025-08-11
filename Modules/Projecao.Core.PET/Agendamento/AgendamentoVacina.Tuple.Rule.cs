using TFX.Core.Model.Data;
using TFX.Core.Reflections;

namespace Projecao.Core.PET.Agendamento
{
    [XRegister(typeof(AgendamentoVacinaSVCTupleRule), "E950395F-0187-4CEC-8932-61450C34D985", typeof(AgendamentoVacinaSVC))]
    [XToTypeScript(typeof(AgendamentoVacinaSVC))]
    public class AgendamentoVacinaSVCTupleRule : XDataTupleRule<AgendamentoVacinaSVC.XDataSet, AgendamentoVacinaSVC.XTuple>
    {
        public AgendamentoVacinaSVCTupleRule(AgendamentoVacinaSVC.XTuple pTuple)
          : base(pTuple)
        {
            BindField(DataSet.FLDs.Quantidade);
            BindField(DataSet.FLDs.ValorCobrado);
            BindField(DataSet.FLDs.ValorTabela);
            BindField(DataSet.FLDs.DescontoProporcional);
        }

        public override void RefreshValues(AgendamentoVacinaSVC.XDataSet pDataSet)
        {
            Tuple.ValorTotal = (Tuple.ValorCobrado > 0 ? Tuple.ValorCobrado : Tuple.ValorTabela) * (Tuple.Quantidade - Tuple.QuantidadePacote) - Tuple.DescontoProporcional;
        }
    }
}