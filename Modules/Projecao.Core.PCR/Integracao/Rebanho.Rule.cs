using Projecao.Core.PCR.DB;

using System;
using System.Collections.Generic;

using TFX.Core;
using TFX.Core.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Integracao
{
    [XRegister(typeof(RebanhoRule), sCID, typeof(RebanhoSVC))]
    public class RebanhoRule : RebanhoSVC.XRule
    {
        public const String sCID = "CC774EEF-D0F8-440B-BE20-C4F4AE29E617";
        public static Guid gCID = new Guid(sCID);

        public RebanhoRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, RebanhoSVC.XDataSet pDataSet, List<Object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, Object>, List<Object>>> pRawFilter, Boolean pIsPKGet)
        {
            pWhere.Add(RebanhoSVC.xISExItemCategoria.ISExCategoriaID, PCRx.ISExCategoria.XDefault.Gado_em_Pe);
            pWhere.Add(RebanhoSVC.xPCRxAnimal.PCRxAnimalEstadoID, XOperator.NotEqualTo, PCRx.PCRxAnimalEstado.XDefault.Morto);
        }

        protected override void AfterGet(XExecContext pContext, RebanhoSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            foreach (RebanhoSVC.XTuple tpl in pDataSet)
                if (tpl.Nascimento > XDefault.NullDateTime)
                    tpl.IdadeMeses = (DateTime.Now.Year - tpl.Nascimento.Year) * 12 + (DateTime.Now.Month - tpl.Nascimento.Month);
        }
    }
}