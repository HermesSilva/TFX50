using System;
using System.Collections.Generic;

using Projecao.Core.PCR.DB;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Integracao
{
    [XRegister(typeof(HormonioRule), sCID, typeof(HormonioSVC))]
    public class HormonioRule : HormonioSVC.XRule
    {
        public const String sCID = "3703E624-D733-4BD3-86E1-B34B998CF8EC";
        public static Guid gCID = new Guid(sCID);

        public HormonioRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, HormonioSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            HormonioSVC.XTuple tpl = pDataSet.NewTuple(0);
            tpl.ISExItemID = Guid.Empty;
            tpl.Nome = " Não Informado";
        }

        protected override void GetWhere(XExecContext pContext, HormonioSVC.XDataSet pDataSet, List<Object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, Object>, List<Object>>> pRawFilter, Boolean pIsPKGet)
        {
            pWhere.Add(HormonioSVC.xISExItemCategoria.ISExCategoriaID, PCRx.ISExCategoria.XDefault.Hormonio_para_AITF);
        }
    }
}