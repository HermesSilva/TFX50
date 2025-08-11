using TFX.Core.Model.Data;
using TFX.Core.Reflections;

namespace Projecao.Core.PET.Agendamento
{
    [XRegister(typeof(AgendamentoExamesSVCTupleRule), "0DF38CF8-CB5A-40F0-BEE6-ED58127EB42D", typeof(AgendamentoExamesSVC))]
    [XToTypeScript(typeof(AgendamentoExamesSVC))]
    public class AgendamentoExamesSVCTupleRule : XDataTupleRule<AgendamentoExamesSVC.XDataSet, AgendamentoExamesSVC.XTuple>
    {

        public AgendamentoExamesSVCTupleRule(AgendamentoExamesSVC.XTuple pTuple)
          : base(pTuple)
        {
            BindField(DataSet.FLDs.ValorTabela);
            BindField(DataSet.FLDs.ValorCobrado);
        }

        public override void RefreshValues(AgendamentoExamesSVC.XDataSet pDataSet)
        {
            Tuple.ValorTotal = Tuple.ValorCobrado > 0 ? Tuple.ValorCobrado : Tuple.ValorTabela;
        }
    }
}
