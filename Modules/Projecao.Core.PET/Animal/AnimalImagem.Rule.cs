using System;
using System.Linq;

using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PET.Animal
{
    [XRegister(typeof(AnimalImagemRule), sCID, typeof(AnimalImagemSVC))]
    public class AnimalImagemRule : AnimalImagemSVC.XRule
    {
        public const String sCID = "56287C8C-A4A6-405D-AD2D-4136FE19FF72";
        public static Guid gCID = new Guid(sCID);

        public AnimalImagemRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, AnimalImagemSVC pModel, AnimalImagemSVC.XDataSet pDataSet)
        {
            if (pDataSet.Tuples.Where(t => t.Principal).Count() > 1)
            {
                AnimalImagemSVC.XTuple def = pDataSet.Tuples.FirstOrDefault(t => t.Principal && (t.State == XTupleState.New || t.GetOldValue(AnimalImagemSVC.xSYSxImagem.Principal, out Object ov)));
                pDataSet.Tuples.ForEach(t => t.Principal = false);
                def.Principal = true;
            }
        }
    }
}