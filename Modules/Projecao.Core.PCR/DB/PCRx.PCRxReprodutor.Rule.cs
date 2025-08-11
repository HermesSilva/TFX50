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
    [XRegister(typeof(PCRxReprodutorRule), sCID, typeof(PCRx.PCRxReprodutor))]
    public class PCRxReprodutorRule : XPersistenceRule<PCRx._PCRxReprodutor, PCRx.PCRxReprodutor, PCRx.PCRxReprodutor.XTuple>
    {
        public const String sCID = "2DCCBE35-0642-4F79-8AEF-EFC7049776CA";
        public static Guid gCID = new Guid(sCID);

        public PCRxReprodutorRule()
        {
            ID = new Guid(sCID);
        }

        public override Int32 ExecuteOrder => 0;

        protected override void AfterFlush(XExecContext pContext, PCRx.PCRxReprodutor pModel, PCRx._PCRxReprodutor pDataSet)
        {
            ISEx._ISExItemCategoria categ = GetTable<ISEx._ISExItemCategoria>();
            foreach (PCRx.PCRxReprodutor.XTuple tpl in pDataSet.Tuples.Where(t => t.State == XTupleState.New))
            {
                ISEx.ISExItemCategoria.XTuple cttpl = categ.NewTuple();
                cttpl.ISExCategoriaID = PCRx.ISExCategoria.XDefault.Reprodutor_Bovino;
                cttpl.ISExItemID = tpl.PCRxReprodutorID;
                cttpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
            }
            categ.Flush();
        }
    }
}