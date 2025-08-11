using System;
using System.Linq;

using TFX.Core.Exceptions;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.SVC;

namespace Projecao.Core.PCR.DB
{
    [XRegister(typeof(PCRxEventoRule), sCID, typeof(PCRx.PCRxEvento))]
    public class PCRxEventoRule : XPersistenceRule<PCRx._PCRxEvento, PCRx.PCRxEvento, PCRx.PCRxEvento.XTuple>
    {
        public const String sCID = "F5C8F0B3-1C18-4804-B69C-D4DC86BE10D8";
        public static Guid gCID = new Guid(sCID);

        public PCRxEventoRule()
        {
            ID = new Guid(sCID);
        }

        public override Int32 ExecuteOrder => 0;

        protected override void BeforeFlush(XExecContext pContext, PCRx.PCRxEvento pModel, PCRx._PCRxEvento pDataSet)
        {
            if (pDataSet.Any(t => t.State != XTupleState.New))
                throw new XError("Não é permitido alterar evento.");
        }
    }
}