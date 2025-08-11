using System;
using System.Linq;
using System.Collections.Generic;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.Job;
using TFX.Core.Service.SVC;
using TFX.Core.Model.Data;

namespace Projecao.Core.PET.Animal
{
    [XRegister(typeof(VacinasRule), sCID, typeof(VacinasSVC))]
    public class VacinasRule : VacinasSVC.XRule
    {

        public const String sCID = "460212FA-4929-4148-AD26-9BCEF7729420";
        public static Guid gCID = new Guid(sCID);

        public VacinasRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, VacinasSVC pModel, VacinasSVC.XDataSet pDataSet)
        {
            foreach (VacinasSVC.XTuple tpl in pDataSet.Tuples.ToArray().Where(t => t.State == XTupleState.New))
            {
                DateTime ldate = tpl.DataRealizar;
                for (int i = 1; i < tpl.Doses; i++)
                {
                    VacinasSVC.XTuple ntpl = pDataSet.NewTuple();
                    ntpl.DataRealizar = ldate = ldate.AddDays(tpl.Intervalo);
                    ntpl.ISExVacinaID = tpl.ISExVacinaID;
                }
            }
        }
    }
}
