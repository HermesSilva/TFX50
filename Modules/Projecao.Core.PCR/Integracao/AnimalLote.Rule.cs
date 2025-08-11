using System;
using System.Collections.Generic;

using TFX.Core.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Integracao
{
    [XRegister(typeof(AnimalLoteRule), sCID, typeof(AnimalLoteSVC))]
    public class AnimalLoteRule : AnimalLoteSVC.XRule
    {
        public const String sCID = "12569C80-1ECC-4864-A770-B453F2B231E1";
        public static Guid gCID = new Guid(sCID);

        public AnimalLoteRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, AnimalLoteSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(AnimalLoteSVC.xPCRxAnimalLote.DataCriacao, XOperator.GreaterThanOrEqualTo, DateTime.Now.AddMonths(-11).Date);
        }
    }
}