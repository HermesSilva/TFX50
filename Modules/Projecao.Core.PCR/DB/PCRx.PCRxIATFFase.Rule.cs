using System;
using System.Linq;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.SVC;


namespace Projecao.Core.PCR.DB
{
    [XRegister(typeof(PCRxIATFFaseRule), sCID, typeof(PCRx.PCRxIATFFase))]
    public class PCRxIATFFaseRule : XPersistenceRule<PCRx._PCRxIATFFase, PCRx.PCRxIATFFase, PCRx.PCRxIATFFase.XTuple>
    {

        public const String sCID = "71C537A4-637F-4F2A-93CA-23CFE52980F4";
        public static Guid gCID = new Guid(sCID);

        public PCRxIATFFaseRule()
        {
            ID = new Guid(sCID);
        }

        public override Int32 ExecuteOrder => 0;
    }
}
