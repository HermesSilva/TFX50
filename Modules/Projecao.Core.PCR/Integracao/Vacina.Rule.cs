using System;
using System.Collections.Generic;

using Projecao.Core.PCR.DB;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Integracao
{
    [XRegister(typeof(VacinaRule), sCID, typeof(VacinaSVC))]
    public class VacinaRule : VacinaSVC.XRule
    {
        public const String sCID = "46F71250-2F73-449E-92CF-FD989783B607";
        public static Guid gCID = new Guid(sCID);

        public VacinaRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, VacinaSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            VacinaSVC.XTuple tpl = pDataSet.NewTuple(Guid.Empty);
            tpl.Nome = " Não Informado";
        }

        protected override void GetWhere(XExecContext pContext, VacinaSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(VacinaSVC.xISExItemCategoria.ISExCategoriaID, PCRx.ISExCategoria.XDefault.Vacina_Bovina);
        }
    }
}