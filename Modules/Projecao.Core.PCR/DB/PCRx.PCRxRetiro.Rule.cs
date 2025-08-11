using System;
using System.Linq;

using TFX.Core.Exceptions;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.SVC;

namespace Projecao.Core.PCR.DB
{
    [XRegister(typeof(PCRxRetiroRule), sCID, typeof(PCRx.PCRxRetiro))]
    public class PCRxRetiroRule : XPersistenceRule<PCRx._PCRxRetiro, PCRx.PCRxRetiro, PCRx.PCRxRetiro.XTuple>
    {
        public const String sCID = "9D8374CB-F9F5-4E74-9E8D-C18E73348646";
        public static Guid gCID = new Guid(sCID);

        public PCRxRetiroRule()
        {
            ID = new Guid(sCID);
        }

        public override Int32 ExecuteOrder => 0;

        protected override void BeforeFlush(XExecContext pContext, PCRx.PCRxRetiro pModel, PCRx._PCRxRetiro pDataSet)
        {
            if (pDataSet.Tuples.Any(t => t.SYSxEmpresaID == Guid.Empty))
                throw new XUnconformity("Não é permitido cadastrar Retiro sem especificar a fazenda.");
        }
    }
}