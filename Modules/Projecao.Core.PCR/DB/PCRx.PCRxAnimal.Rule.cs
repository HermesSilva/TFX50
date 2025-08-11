using System;
using System.Linq;

using Projecao.Core.ISE.DB;

using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.SVC;

using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.PCR.DB
{
    [XRegister(typeof(PCRxAnimalRule), sCID, typeof(PCRx.PCRxAnimal))]
    public class PCRxAnimalRule : XPersistenceRule<PCRx._PCRxAnimal, PCRx.PCRxAnimal, PCRx.PCRxAnimal.XTuple>
    {
        public const String sCID = "8A8625FF-B80E-4768-8460-9858BCAA933D";
        public static Guid gCID = new Guid(sCID);

        public PCRxAnimalRule()
        {
            ID = new Guid(sCID);
        }

        public override Int32 ExecuteOrder => 0;

        protected override void AfterFlush(XExecContext pContext, PCRx.PCRxAnimal pModel, PCRx._PCRxAnimal pDataSet)
        {
            ISEx._ISExItemCategoria categ = GetTable<ISEx._ISExItemCategoria>();
            foreach (PCRx.PCRxAnimal.XTuple tpl in pDataSet.Tuples.Where(t => t.State == XTupleState.New))
            {
                ISEx.ISExItemCategoria.XTuple cttpl = categ.NewTuple();
                cttpl.ISExCategoriaID = PCRx.ISExCategoria.XDefault.Gado_em_Pe;
                cttpl.ISExItemID = tpl.PCRxAnimalID;
                cttpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
            }
            categ.Flush();
        }
    }
}