using System;
using System.Linq;

using TFX.Core.Exceptions;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.SVC;

namespace Projecao.Core.PCR.DB
{
    [XRegister(typeof(PCRxPastoRule), sCID, typeof(PCRx.PCRxPasto))]
    public class PCRxPastoRule : XPersistenceRule<PCRx._PCRxPasto, PCRx.PCRxPasto, PCRx.PCRxPasto.XTuple>
    {
        public const String sCID = "27A9F73E-E3DF-44C9-871E-D536AE76C21E";
        public static Guid gCID = new Guid(sCID);

        public PCRxPastoRule()
        {
            ID = new Guid(sCID);
        }

        public override Int32 ExecuteOrder => 0;

        protected override void BeforeFlush(XExecContext pContext, PCRx.PCRxPasto pModel, PCRx._PCRxPasto pDataSet)
        {
            if (pDataSet.Tuples.Any(t => t.PCRxRetiroID == 0))
                throw new XUnconformity("Não é permitido cadastrar Pasto sem especificar o Retiro.");
        }
    }
}