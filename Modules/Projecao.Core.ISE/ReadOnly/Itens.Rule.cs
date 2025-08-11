using System;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;

namespace Projecao.Core.ISE.ReadOnly
{
    [XRegister(typeof(ItensRule), sCID, typeof(ItensSVC))]
    public class ItensRule : ItensSVC.XRule
    {
        public const String sCID = "B7CAAD72-AA44-490E-ABCD-936BC1D4CE52";
        public static Guid gCID = new Guid(sCID);

        public ItensRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, ItensSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            _ISExItemPreco preco = GetTable<_ISExItemPreco>();
            pDataSet.Tuples.ForEach(t => t.Valor = PegaPreco.PrecoVenda(pContext, preco, t.ISExItemID));
        }
    }
}