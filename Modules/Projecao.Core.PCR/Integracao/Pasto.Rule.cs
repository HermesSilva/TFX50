using System;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Integracao
{
    [XRegister(typeof(PastoRule), sCID, typeof(PastoSVC))]
    public class PastoRule : PastoSVC.XRule
    {
        public const String sCID = "8FA9D31A-0B98-4BD7-B63A-702AE0A7D589";
        public static Guid gCID = new Guid(sCID);

        public PastoRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, PastoSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            PastoSVC.XTuple tpl = pDataSet.NewTuple(0);
            tpl.Nome = "NI";
            tpl.NomeRetiro = "NI";
        }
    }
}