using System;
using System.Collections.Generic;

using TFX.Core;
using TFX.Core.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PET.VisaoGeral
{
    [XRegister(typeof(AniversarioRule), sCID, typeof(AniversarioSVC))]
    public class AniversarioRule : AniversarioSVC.XRule
    {
        public const String sCID = "731BE478-C09F-48DD-A180-390DEA3E0372";
        public static Guid gCID = new Guid(sCID);

        public AniversarioRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, AniversarioSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(AniversarioSVC.xPETxAnimal.Nascimento, XOperator.GreaterThan, XDefault.NullDateTime);
        }

        protected override void AfterGet(XExecContext pContext, AniversarioSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            pDataSet.Remove<AniversarioSVC.XTuple>(t => (t.Nascimento.Day != DateTime.Now.Day || t.Nascimento.Month != DateTime.Now.Month) && t.Nascimento.Year < DateTime.Now.Year);
        }
    }
}