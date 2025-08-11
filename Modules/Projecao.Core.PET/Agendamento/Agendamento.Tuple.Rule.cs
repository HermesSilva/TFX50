using System;
using System.Linq;

using TFX.Core.Exceptions;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Utils;

namespace Projecao.Core.PET.Agendamento
{
    [XRegister(typeof(AgendamentoSVCTupleRule), "40611DAF-2816-40A7-A8E4-50F9FE745606", typeof(AgendamentoSVC))]
    [XToTypeScript(typeof(AgendamentoSVC))]
    public class AgendamentoSVCTupleRule : XDataTupleRule<AgendamentoSVC.XDataSet, AgendamentoSVC.XTuple>
    {
        public AgendamentoSVCTupleRule(AgendamentoSVC.XTuple pTuple)
          : base(pTuple)
        {
            BindField(DataSet.FLDs.Desconto);
        }

        public override void RefreshValues(AgendamentoSVC.XDataSet pDataSet)
        {
            if (pDataSet.AgendamentoVacinaDataSet.Count == 0 && pDataSet.AgendamentoProdutoDataSet.Count == 0 && Tuple.Desconto > 0)
            {
                ZeraDesconto();
                ThrowError(new XUnconformity("Não é permitido desconto para Agendamento sem vacinas ou acessórios."));
            }

            if (Tuple.Desconto == 0)
                ZeraDesconto();

            Decimal resto = 0;
            Decimal porcentodesconto = 0;
            Decimal totalbruto = pDataSet.AgendamentoExamesDataSet.Sum(t => t.ValorCobrado > 0 ? t.ValorCobrado : t.ValorTabela);
            totalbruto += pDataSet.AgendamentoDetalheDataSet.Sum(t => t.Acrescimo);
            totalbruto += pDataSet.AgendamentoProdutoDataSet.Sum(t => (t.ValorCobrado > 0 ? t.ValorCobrado : t.ValorTabela) * t.Quantidade);
            totalbruto += pDataSet.AgendamentoServicoDataSet.Sum(t => (t.ValorCobrado > 0 ? t.ValorCobrado : t.ValorTabela) * (t.Quantidade - t.QuantidadePacote));
            totalbruto += pDataSet.AgendamentoVacinaDataSet.Sum(t => (t.ValorCobrado > 0 ? t.ValorCobrado : t.ValorTabela) * (t.Quantidade - t.QuantidadePacote));
            //this.Tuple.ValorTotal = totalbruto;
            porcentodesconto = ChecaTotal(totalbruto);
            pDataSet.AgendamentoVacinaDataSet.ForEach(t => t.DescontoProporcional = XUtils.FixDecimal((t.ValorCobrado > 0 ? t.ValorCobrado : t.ValorTabela) * (t.Quantidade - t.QuantidadePacote) * porcentodesconto, 2));
            pDataSet.AgendamentoProdutoDataSet.ForEach(t => t.DescontoProporcional = XUtils.FixDecimal((t.ValorCobrado > 0 ? t.ValorCobrado : t.ValorTabela) * t.Quantidade * porcentodesconto, 2));
            if (Tuple.Desconto >= 0)
                resto = XUtils.Down2Dec(XUtils.Up2Dec(Tuple.Desconto) - XUtils.Up2Dec(pDataSet.AgendamentoVacinaDataSet.Sum(t => t.DescontoProporcional) + pDataSet.AgendamentoProdutoDataSet.Sum(t => t.DescontoProporcional)));
            AgendamentoVacinaSVC.XTuple m1 = pDataSet.AgendamentoVacinaDataSet.TupleByMax(t => t.DescontoProporcional);
            AgendamentoProdutoSVC.XTuple m2 = pDataSet.AgendamentoProdutoDataSet.TupleByMax(t => t.DescontoProporcional);
            if (resto != 0)
            {
                if (m1 != null && m2 != null)
                {
                    if (m1.DescontoProporcional > m2.DescontoProporcional)
                        m1.DescontoProporcional = XUtils.FixDecimal(XUtils.Down2Dec(XUtils.Up2Dec(m1.DescontoProporcional) + XUtils.Up2Dec(resto)), 2);
                    else
                        m2.DescontoProporcional = XUtils.FixDecimal(XUtils.Down2Dec(XUtils.Up2Dec(m2.DescontoProporcional) + XUtils.Up2Dec(resto)), 2);
                }
                else
                {
                    if (m2 != null)
                        m2.DescontoProporcional = XUtils.FixDecimal(XUtils.Down2Dec(XUtils.Up2Dec(m2.DescontoProporcional) + XUtils.Up2Dec(resto)), 2);
                    if (m1 != null)
                        m1.DescontoProporcional = XUtils.FixDecimal(XUtils.Down2Dec(XUtils.Up2Dec(m1.DescontoProporcional) + XUtils.Up2Dec(resto)), 2);
                }
            }

            this.Tuple.ValorTotal = XUtils.FixDecimal(DataSet.AgendamentoDetalheDataSet.Sum(t => t.Acrescimo) +
                                DataSet.AgendamentoExamesDataSet.Sum(t => t.ValorTabela) +
                                DataSet.AgendamentoVacinaDataSet.Sum(t => t.ValorTabela * (t.Quantidade - t.QuantidadePacote)) +
                                DataSet.AgendamentoServicoDataSet.Sum(t => t.ValorTabela * (t.Quantidade - t.QuantidadePacote)) +
                                DataSet.AgendamentoProdutoDataSet.Sum(t => t.ValorTabela * t.Quantidade), 2);

            this.Tuple.ValorCobrado = XUtils.FixDecimal(DataSet.AgendamentoDetalheDataSet.Sum(t => t.Acrescimo) +
                                 DataSet.AgendamentoExamesDataSet.Sum(t => t.ValorTotal) +
                                 DataSet.AgendamentoVacinaDataSet.Sum(t => t.ValorTotal) +
                                 DataSet.AgendamentoServicoDataSet.Sum(t => t.ValorTotal) +
                                 DataSet.AgendamentoProdutoDataSet.Sum(t => t.ValorTotal), 2);
        }

        private Decimal ChecaTotal(Decimal pTotalBruto)
        {
            if (Tuple.Desconto > 0 && Tuple.Desconto > pTotalBruto)
            {
                ZeraDesconto();
                ThrowError(new XUnconformity("O desconto não pode ser maior que o valor das vacinas e produtos ou acessórios somados."));
            }
            if (pTotalBruto == 0)
                return 0;
            return Tuple.Desconto / pTotalBruto;
        }

        private void ZeraDesconto()
        {
            Tuple.Desconto = 0;
            DataSet.AgendamentoServicoDataSet.ForEach(t => t.DescontoProporcional = 0);
            DataSet.AgendamentoVacinaDataSet.ForEach(t => t.DescontoProporcional = 0);
            DataSet.AgendamentoProdutoDataSet.ForEach(t => t.DescontoProporcional = 0);
        }
    }
}