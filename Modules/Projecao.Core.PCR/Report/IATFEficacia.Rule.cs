using System;
using System.Collections.Generic;

using TFX.Core.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR.Report
{
    [XRegister(typeof(IATFEficaciaRule), sCID, typeof(IATFEficaciaSVC))]
    public class IATFEficaciaRule : IATFEficaciaSVC.XRule
    {
        public const String sCID = "85DA9F8B-A37E-48C6-925B-2CDA3967A4D4";
        public static Guid gCID = new Guid(sCID);

        public IATFEficaciaRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, IATFEficaciaSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(IATFEficaciaSVC.xPCRxAnimalEvento.PCRxIATFFaseTipoID, XOperator.In, new[] { PCRxIATFFaseTipo.XDefault.DG1, PCRxIATFFaseTipo.XDefault.DG2, PCRxIATFFaseTipo.XDefault.DG3, PCRxIATFFaseTipo.XDefault.DG_Final });
            pWhere.Add(IATFEficaciaSVC.xPCRxAnimalLote.PCRxAnimalLoteID, XOperator.NotEqualTo, PCRxAnimalLote.XDefault._00000000000000000000000000000000);
        }
    }
}