using System;

using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PET.VisaoGeral
{
    [XRegister(typeof(VisaoGeralRule), sCID, typeof(VisaoGeralSVC))]
    public class VisaoGeralRule : VisaoGeralSVC.XRule
    {
        public const String sCID = "0D5793A8-9B92-411C-A736-D17DF16847E0";
        public static Guid gCID = new Guid(sCID);

        public VisaoGeralRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, VisaoGeralSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            VisaoGeralSVC.XTuple tpl = pDataSet.NewTuple();
            foreach (XDataSet child in pDataSet.Children)
                child.Tuples.ForEach(t => t.SetValue(child.PKField.ID, t.PKValue = tpl.PKValue));
        }
    }
}