using System;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;

namespace Projecao.Core.ISE.ReadOnly
{
    [XRegister(typeof(ProdutoRule), sCID, typeof(ProdutoSVC))]
    public class ProdutoRule : ProdutoSVC.XRule
    {

        public const String sCID = "21C9D0F6-610D-4EAD-8CBE-DAE0130B9D25";
        public static Guid gCID = new Guid(sCID);

        public ProdutoRule()
        {
            ID = gCID;
        }


        protected override void AfterGet(XExecContext pContext, ProdutoSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            _ISExItemPreco preco = GetTable<_ISExItemPreco>();
            pDataSet.Tuples.ForEach(t => t.Valor = PegaPreco.PrecoVenda(pContext, preco, t.ISExProdutoID));
        }
    }
}
