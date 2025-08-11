using System.Linq;

using TFX.Core.Model.Data;
using TFX.Core.Reflections;

namespace Projecao.Core.PET.FecharAtendimento
{
    [XRegister(typeof(FecharAtendimentoSVCTupleRule), "3E876A25-F005-4DA0-81ED-8F0845C75453", typeof(FecharAtendimentoSVC))]
    [XToTypeScript(typeof(FecharAtendimentoSVC))]
    public class FecharAtendimentoSVCTupleRule : XDataTupleRule<FecharAtendimentoSVC.XDataSet, FecharAtendimentoSVC.XTuple>
    {

        public FecharAtendimentoSVCTupleRule(FecharAtendimentoSVC.XTuple pTuple)
          : base(pTuple)
        {
        }

        public override void RefreshValuesByChild(FecharAtendimentoSVC.XDataSet pDataSet)
        {
            pDataSet.Current.Soma = pDataSet.FecharAtenAgrupadoDataSet.Tuples.Where(t => t.Agrupar).Sum(t => t.ValorCobrado);
        }
    }
}
