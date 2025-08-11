using TFX.Core.Model.Data;
using TFX.Core.Reflections;

namespace Projecao.Core.PET.FecharAtendimento
{
    [XRegister(typeof(FecharAtenAgrupadoSVCTupleRule), "D2E04DB6-43C5-4972-88BB-98B0172171BC", typeof(FecharAtenAgrupadoSVC))]
    [XToTypeScript(typeof(FecharAtenAgrupadoSVC))]
    public class FecharAtenAgrupadoSVCTupleRule : XDataTupleRule<FecharAtenAgrupadoSVC.XDataSet, FecharAtenAgrupadoSVC.XTuple>
    {

        public FecharAtenAgrupadoSVCTupleRule(FecharAtenAgrupadoSVC.XTuple pTuple)
          : base(pTuple)
        {
            BindField(DataSet.FLDs.Agrupar);
        }
    }
}
