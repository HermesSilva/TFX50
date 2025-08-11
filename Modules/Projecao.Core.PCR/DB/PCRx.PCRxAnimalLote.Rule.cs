using System;
using System.Linq;

using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.SVC;

namespace Projecao.Core.PCR.DB
{
    [XRegister(typeof(PCRxAnimalLoteRule), sCID, typeof(PCRx.PCRxAnimalLote))]
    public class PCRxAnimalLoteRule : XPersistenceRule<PCRx._PCRxAnimalLote, PCRx.PCRxAnimalLote, PCRx.PCRxAnimalLote.XTuple>
    {
        public const String sCID = "25107C30-69F6-4822-AA51-F0637044037D";
        public static Guid gCID = new Guid(sCID);

        public PCRxAnimalLoteRule()
        {
            ID = new Guid(sCID);
        }

        public override Int32 ExecuteOrder => 0;

        protected override void BeforeFlush(XExecContext pContext, PCRx.PCRxAnimalLote pModel, PCRx._PCRxAnimalLote pDataSet)
        {
            foreach (PCRx.PCRxAnimalLote.XTuple tpl in pDataSet.Where(t => t.State == XTupleState.New))
            {
                tpl.Ano = tpl.DataCriacao.Year;
                if (!tpl.Lote.SafeContains("-"))
                    tpl.Lote = $"{tpl.Lote}-{tpl.DataCriacao.ToString("yyyy")}";
            }
        }
    }
}