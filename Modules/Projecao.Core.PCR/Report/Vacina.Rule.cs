using Projecao.Core.PCR.DB;

using System;
using System.Collections.Generic;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Report
{
    [XRegister(typeof(VacinaRule), sCID, typeof(VacinaSVC))]
    public class VacinaRule : VacinaSVC.XRule
    {
        public const String sCID = "B2C8A8A9-C0DB-4FFD-873C-42F49BC30644";
        public static Guid gCID = new Guid(sCID);

        public VacinaRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, VacinaSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(VacinaSVC.xISExItemCategoria.ISExCategoriaID, PCRx.ISExCategoria.XDefault.Vacina_Bovina);
        }
    }
}