using System;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Integracao
{
    [XRegister(typeof(FazendaRule), sCID, typeof(FazendaSVC))]
    public class FazendaRule : FazendaSVC.XRule
    {
        public const String sCID = "69A07FED-2EB3-4F6C-910F-E1A113842184";
        public static Guid gCID = new Guid(sCID);

        public FazendaRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, FazendaSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            pDataSet.Tuples.ForEach(t => t.DataBanco = DateTime.Now);
        }
    }
}