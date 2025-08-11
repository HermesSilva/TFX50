using System;
using System.Collections.Generic;

using Projecao.Core.ISE.ReadOnly;
using Projecao.Core.PET.Agendamento;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.PET.DB.PETx;

namespace Projecao.Core.PET.RPTs.Servico
{
    [XRegister(typeof(AtendimentoRule), sCID, typeof(AtendimentoSVC))]
    public class AtendimentoRule : AtendimentoSVC.XRule
    {
        public const String sCID = "872697F6-0D06-439D-A43F-0DE74B2223AF";
        public static Guid gCID = new Guid(sCID);

        public AtendimentoRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, AtendimentoSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(AtendimentoSVC.xPETxAtendimento.PETxAtendimentoEstadoID, PETxAtendimentoEstado.XDefault.Recebido);
            pWhere.Add(AtendimentoSVC.xPETxAtendimento.PETxAtendimentoTipoID, PETxAtendimentoTipo.XDefault.Normal);
        }

        protected override void AfterGet(XExecContext pContext, AtendimentoSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            AgendamentoServicoSVC.XService svc = GetService<AgendamentoServicoSVC.XService>();
            ComissaoEmpresaSVC.XService cms = GetService<ComissaoEmpresaSVC.XService>();

            foreach (AtendimentoSVC.XTuple tpl in pDataSet)
            {
                tpl.ValorServico = 0;
                tpl.Comissao = 0;
                if (svc.Open(AgendamentoServicoSVC.xPETxAtendimentoServicos.PETxAtendimentoID, tpl.PETxAtendimentoID))
                {
                    Decimal comissao = 0;
                    foreach (AgendamentoServicoSVC.XTuple stpl in svc.DataSet)
                    {
                        tpl.ValorServico += stpl.ValorTabela;
                        if (cms.Open(ComissaoEmpresaSVC.xISExComissao.ISExItemID, stpl.ISExServicoID))
                        {
                            foreach (ComissaoEmpresaSVC.XTuple ctpl in cms.DataSet)
                                comissao += ctpl.Comissao;
                        }
                    }
                    tpl.Comissao = tpl.ValorServico * comissao / 100.0M;
                }
            }
        }
    }
}