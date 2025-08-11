using System;
using System.Collections.Generic;

using TFX.Core.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PET.Tutor
{
    [XRegister(typeof(AnimalRule), sCID, typeof(AnimalSVC))]
    public class AnimalRule : AnimalSVC.XRule
    {
        public const String sCID = "2292A16A-9C0B-418B-9602-7111AF472404";
        public static Guid gCID = new Guid(sCID);

        public AnimalRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, AnimalSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(AnimalSVC.xPETxAnimal.PETxAnimalID, XOperator.NotEqualTo, Guid.Empty);
        }
    }
}