using Projecao.Core.ERP.DB;
using Projecao.Core.PCR.DB;

using System;
using System.Collections.Generic;

using TFX.Core.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.ReadOnly
{
    [XRegister(typeof(VacasRule), sCID, typeof(VacasSVC))]
    public class VacasRule : VacasSVC.XRule
    {
        public const String sCID = "F71B8D9F-D23B-46A7-ADC2-5EAC5583F84D";
        public static Guid gCID = new Guid(sCID);

        public VacasRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, VacasSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(VacasSVC.xISExItemCategoria.ISExCategoriaID, XOperator.NotEqualTo, PCRx.ISExCategoria.XDefault.Reprodutor_Bovino);
            pWhere.Add(VacasSVC.xPCRxAnimal.ERPxGeneroID, ERPx.ERPxGenero.XDefault.Feminino);
            pWhere.Add(VacasSVC.xPCRxAnimal.PCRxAnimalFaseID, XOperator.In, new Int16[] { PCRx.PCRxAnimalFase.XDefault.Vaca, PCRx.PCRxAnimalFase.XDefault.Novilha });
        }
    }
}