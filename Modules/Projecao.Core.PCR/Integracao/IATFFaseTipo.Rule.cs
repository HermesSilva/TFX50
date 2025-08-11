using System;
using System.Collections.Generic;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Integracao
{
    [XRegister(typeof(PCRxIATFFaseTipoRule), sCID, typeof(IATFFaseTipoSVC))]
    public class PCRxIATFFaseTipoRule : IATFFaseTipoSVC.XRule
    {
        public const String sCID = "F2E7FDA7-1A86-4CC0-A2D5-6E2CB9385F52";
        public static Guid gCID = new Guid(sCID);

        public PCRxIATFFaseTipoRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, IATFFaseTipoSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(IATFFaseTipoSVC.xPCRxIATFFaseTipo.Operacional, true);
        }
    }
}