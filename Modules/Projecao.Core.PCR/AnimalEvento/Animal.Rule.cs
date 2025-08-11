using System;
using System.Linq;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.AnimalEvento
{
    [XRegister(typeof(AnimalRule), sCID, typeof(AnimalSVC))]
    public class AnimalRule : AnimalSVC.XRule
    {
        public const String sCID = "0972180F-E4B8-443D-8540-EF3D3433AABE";
        public static Guid gCID = new Guid(sCID);

        public AnimalRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, AnimalSVC pModel, AnimalSVC.XDataSet pDataSet)
        {
            foreach (AnimalSVC.XTuple tpl in pDataSet.Tuples.Where(t => t.Nome.IsEmpty()))
                tpl.Nome = tpl.Fase + " " + tpl.Raca;
        }
    }
}