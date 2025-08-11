using System;

using Projecao.Core.PET.Agendamento;

using TFX.Core;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.PET.DB.PETx;

namespace Projecao.Core.PET.FecharAtendimento
{
    [XRegister(typeof(FecharAtenAgrupadoRule), sCID, typeof(FecharAtenAgrupadoSVC))]
    public class FecharAtenAgrupadoRule : FecharAtenAgrupadoSVC.XRule
    {
        public const String sCID = "B2151726-7251-426C-8033-4A7E5AA8CFDC";
        public static Guid gCID = new Guid(sCID);

        public FecharAtenAgrupadoRule()
        {
            ID = gCID;
        }

        private XILog _Log = XLog.GetLogFor(typeof(FecharAtenAgrupadoRule));

        protected override void AfterGet(XExecContext pContext, FecharAtenAgrupadoSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            if (pIsPKGet)
            {
                pDataSet.Clear();
                FecharAtendimentoSVC.XTuple owtpl = ((FecharAtendimentoSVC.XDataSet)pDataSet.Owner).Current;
                AgendamentoSVC.XService agendamento = GetService<AgendamentoSVC.XService>();
                agendamento.FilterInactive = true;
                agendamento.Open(PETxAtendimento.PETxTutorID, owtpl.PETxTutorID, PETxAtendimento.PETxAtendimentoEstadoID, XOperator.In, new[] { PETxAtendimentoEstado.XDefault.Criado, PETxAtendimentoEstado.XDefault.Concluido });

                foreach (AgendamentoSVC.XTuple tpl in agendamento.Tuples)
                {
                    FecharAtenAgrupadoSVC.XTuple ntpl = pDataSet.NewTuple();
                    ntpl.PETxAtendimentoID = tpl.PETxAtendimentoID;
                    ntpl.PETxPrincipalID = owtpl.PETxAtendimentoID;
                    ntpl.Nome = tpl.NomeAnimal;
                    ntpl.Data = tpl.Data;
                    ntpl.Numero = tpl.Numero;
                    ntpl.ValorCobrado = tpl.ValorCobrado;
                    if (tpl.PETxAtendimentoID != owtpl.PETxAtendimentoID)
                        ntpl.Grupo = owtpl.PETxAtendimentoID;
                    else
                        ntpl.IsReadOnly = true;
                }
            }
        }
    }
}