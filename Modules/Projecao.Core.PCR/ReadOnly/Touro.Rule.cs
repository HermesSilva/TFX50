using Projecao.Core.PCR.DB;

using System;
using System.Collections.Generic;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.ReadOnly
{
    [XRegister(typeof(TouroRule), sCID, typeof(TouroSVC))]
    public class TouroRule : TouroSVC.XRule
    {
        public const String sCID = "D9FEE325-8651-4735-9577-0CE20156354B";
        public static Guid gCID = new Guid(sCID);

        public TouroRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, TouroSVC.XDataSet pDataSet, List<Object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, Object>, List<Object>>> pRawFilter, Boolean pIsPKGet)
        {
            pWhere.Add(TouroSVC.xISExItemCategoria.ISExCategoriaID, PCRx.ISExCategoria.XDefault.Reprodutor_Bovino);
        }
    }
}