using System;
using System.Collections.Generic;

using Projecao.Core.PCR.DB;

using TFX.Core.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Vacina
{
    [XRegister(typeof(VacinaRule), sCID, typeof(VacinaSVC))]
    public class VacinaRule : VacinaSVC.XRule
    {
        public const String sCID = "CE05B9F0-A312-4531-9525-EE4B9A4D16EA";
        public static Guid gCID = new Guid(sCID);

        public VacinaRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, VacinaSVC.XDataSet pDataSet, List<Object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, Object>, List<Object>>> pRawFilter, Boolean pIsPKGet)
        {
            pWhere.Add(VacinaSVC.xISExItemCategoria.ISExCategoriaID, XOperator.In, new[] { PCRx.ISExCategoria.XDefault.Medicamento_para_Bovino, PCRx.ISExCategoria.XDefault.Vacina_Bovina });
        }
    }
}