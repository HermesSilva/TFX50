using System;
using System.Linq;

using TFX.Core.Exceptions;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.SVC;

using static Projecao.Core.PET.DB.PETx;

namespace Projecao.Core.PET.DB
{
    [XRegister(typeof(PETxAnimalTutorRule), sCID, typeof(PETx.PETxAnimalTutor))]
    public class PETxAnimalTutorRule : XPersistenceRule<PETx._PETxAnimalTutor, PETx.PETxAnimalTutor, PETx.PETxAnimalTutor.XTuple>
    {
        public const String sCID = "57572194-103A-43F5-B5A0-3E8E82ACB9F1";
        public static Guid gCID = new Guid(sCID);

        public PETxAnimalTutorRule()
        {
            ID = new Guid(sCID);
        }

        public override Int32 ExecuteOrder => 0;

        protected override void BeforeFlush(XExecContext pContext, PETx.PETxAnimalTutor pModel, PETx._PETxAnimalTutor pDataSet)
        {
            _PETxAnimalTutor pet = GetTable<_PETxAnimalTutor>();
            foreach (PETx.PETxAnimalTutor.XTuple tpl in pDataSet.Where(t => t.State == XTupleState.Revoked))
                if (!pet.Open(PETxAnimalTutor.PETxAnimalID, tpl.PETxAnimalID) || pet.Count < 2)
                    throw new XUnconformity("Não pode ser deletado o último Tutor de um PET.");
        }
    }
}