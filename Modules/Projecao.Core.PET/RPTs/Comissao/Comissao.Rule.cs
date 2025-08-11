using System;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PET.RPTs.Comissao
{
    [XRegister(typeof(ComissaoRule), sCID, typeof(ComissaoSVC))]
    public class ComissaoRule : ComissaoSVC.XRule
    {
        public const String sCID = "8DA61709-55B7-4F2B-A6E9-8E452D913E9E";
        public static Guid gCID = new Guid(sCID);

        public ComissaoRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, ComissaoSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            pDataSet.Tuples.ForEach(t => CalculaComissao(t));
        }

        private void CalculaComissao(ComissaoSVC.XTuple pTuple)
        {
            Int32 qtdepacote = pTuple.QuantidadePacote;
            Int32 qtdpago = pTuple.Quantidade - pTuple.QuantidadePacote;
            Decimal vlritempacote = pTuple.PacoteValorCobrado > 0 ? pTuple.PacoteValorCobrado : pTuple.PacoteValorTabela;
            Decimal vlritem = pTuple.ValorCobrado > 0 ? pTuple.ValorCobrado : pTuple.ValorTabela;
            Decimal basecomissao = 0;

            if (qtdepacote > 0)
                basecomissao = vlritempacote * qtdepacote + qtdpago * vlritem;
            else
                basecomissao = qtdpago * vlritem;

            pTuple.ComissaoPaga = basecomissao * pTuple.Comissao / 100.0M;
        }
    }
}