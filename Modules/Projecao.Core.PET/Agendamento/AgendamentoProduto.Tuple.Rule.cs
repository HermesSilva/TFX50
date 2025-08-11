using TFX.Core.Model.Data;
using TFX.Core.Reflections;

namespace Projecao.Core.PET.Agendamento
{
    [XRegister(typeof(AgendamentoProdutoSVCTupleRule), "7597F64C-8EC8-4A4E-AB43-396FA5D1FEA6", typeof(AgendamentoProdutoSVC))]
    [XToTypeScript(typeof(AgendamentoProdutoSVC))]
    public class AgendamentoProdutoSVCTupleRule : XDataTupleRule<AgendamentoProdutoSVC.XDataSet, AgendamentoProdutoSVC.XTuple>
    {

        public AgendamentoProdutoSVCTupleRule(AgendamentoProdutoSVC.XTuple pTuple)
          : base(pTuple)
        {
            BindField(DataSet.FLDs.Quantidade);
            BindField(DataSet.FLDs.ValorCobrado);
            BindField(DataSet.FLDs.ValorTabela);
            BindField(DataSet.FLDs.DescontoProporcional);
        }

        public override void RefreshValues(AgendamentoProdutoSVC.XDataSet pDataSet)
        {
            Tuple.ValorTotal = (Tuple.ValorCobrado > 0 ? Tuple.ValorCobrado : Tuple.ValorTabela) * Tuple.Quantidade - Tuple.DescontoProporcional;
        }
    }
}
