using System;
using System.Linq;

using TFX.Core.Exceptions;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Fazenda
{
    [XRegister(typeof(FazendaRule), sCID, typeof(FazendaSVC))]
    public class FazendaRule : FazendaSVC.XRule
    {
        public const String sCID = "CF760F1C-5B06-4D35-B7E0-ADB0F4FAE14E";
        public static Guid gCID = new Guid(sCID);

        public FazendaRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, FazendaSVC pModel, FazendaSVC.XDataSet pDataSet)
        {
            if (pDataSet.Tuples.Any(t => t.State == XTupleState.New))
                throw new XUnconformity("Não é permitido incluir fazenda.");
        }
    }
}