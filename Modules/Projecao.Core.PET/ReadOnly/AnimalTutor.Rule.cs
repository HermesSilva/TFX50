using System;
using System.Linq;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.PET.ReadOnly
{
    [XRegister(typeof(AnimalTutorRule), sCID, typeof(AnimalTutorSVC))]
    public class AnimalTutorRule : AnimalTutorSVC.XRule
    {
        public const String sCID = "C2181DC1-F09B-45E6-8210-BCC7C79667CB";
        public static Guid gCID = new Guid(sCID);

        public AnimalTutorRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, AnimalTutorSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            foreach (AnimalTutorSVC.XTuple tpl in pDataSet.Tuples.Where(t => t.SYSxEstadoID == SYSxEstado.XDefault.Inativo && t.PETxAnimalID.IsFull()).ToArray())
                pDataSet.Remove(tpl);
        }
    }
}