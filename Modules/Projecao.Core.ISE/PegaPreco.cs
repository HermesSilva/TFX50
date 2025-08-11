using System;
using System.Linq;

using Projecao.Core.ISE.ReadOnly;

using TFX.Core;
using TFX.Core.Model.Cache;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;

namespace Projecao.Core.ISE
{
    public static class PegaPreco
    {
        public static Decimal PrecoVenda(XExecContext pContext, _ISExItemPreco pPreco, Guid pItemID)
        {
            using (PromocaoSVC.XService promo = XServicePool.Get<PromocaoSVC.XService>(PromocaoSVC.gCID, pContext))
            {
                Boolean hspromo = promo.Open(PromocaoSVC.xISExItemPromocao.Inicio, XOperator.LessThanOrEqualTo, XDefault.Now, PromocaoSVC.xISExItemPromocao.Fim,
                                             XOperator.GreaterThanOrEqualTo, XDefault.Now, PromocaoSVC.xISExItemPromocaoDetalhe.ISExItemID, pItemID);
                Boolean hspreco = pPreco.Open(ISExItemPreco.ISExItemID, pItemID, ISExItemPreco.InicioValidade, XOperator.LessThanOrEqualTo, XDefault.Now,
                                              ISExItemPreco.FimValidade, XOperator.GreaterThanOrEqualTo, XDefault.Now, ISExItemPreco.ISExItemPrecoTipoID, ISExItemPrecoTipo.XDefault.Normal_de_Venda);

                ISExItemPreco.XTuple pretpl = pPreco.Tuples.FirstOrDefault();
                PromocaoSVC.XTuple protpl = promo.Tuples.FirstOrDefault();
                if (hspromo)
                    return protpl.Valor;
                if (hspreco)
                    return pretpl.Valor;
                return 0;
            }
        }
    }
}