using System;
using System.Collections.Generic;

using Projecao.Core.PCR.DB;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Integracao
{
    [XRegister(typeof(ReprodutorRule), sCID, typeof(ReprodutorSVC))]
    public class ReprodutorRule : ReprodutorSVC.XRule
    {
        public const String sCID = "694598EF-A7CF-4CB3-82D3-41CF28EE235C";
        public static Guid gCID = new Guid(sCID);

        public ReprodutorRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, ReprodutorSVC.XDataSet pDataSet, List<Object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, Object>, List<Object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(ReprodutorSVC.xISExItemCategoria.ISExCategoriaID, PCRx.ISExCategoria.XDefault.Reprodutor_Bovino);
        }
    }
}