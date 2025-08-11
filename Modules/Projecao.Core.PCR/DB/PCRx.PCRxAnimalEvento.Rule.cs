using System;
using System.Linq;

using TFX.Core.Exceptions;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.SVC;

namespace Projecao.Core.PCR.DB
{
    [XRegister(typeof(PCRxAnimalEventoRule), sCID, typeof(PCRx.PCRxAnimalEvento))]
    public class PCRxAnimalEventoRule : XPersistenceRule<PCRx._PCRxAnimalEvento, PCRx.PCRxAnimalEvento, PCRx.PCRxAnimalEvento.XTuple>
    {
        public const String sCID = "6D99D45C-22E6-4728-AA74-F178E084604C";
        public static Guid gCID = new Guid(sCID);

        public PCRxAnimalEventoRule()
        {
            ID = new Guid(sCID);
        }

        public override Int32 ExecuteOrder => 0;

        protected override void BeforeFlush(XExecContext pContext, PCRx.PCRxAnimalEvento pModel, PCRx._PCRxAnimalEvento pDataSet)
        {
            if (pDataSet.Any(t => t.State != XTupleState.New))
                throw new XError("Não é permitido alterar evento.");
        }
    }
}