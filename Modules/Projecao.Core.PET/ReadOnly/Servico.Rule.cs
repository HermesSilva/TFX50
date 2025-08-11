using System;
using System.Linq;

using Projecao.Core.ISE;
using Projecao.Core.PET.Pacote;

using TFX.Core.Exceptions;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;
using static Projecao.Core.PET.DB.PETx;

namespace Projecao.Core.PET.ReadOnly
{
    [XRegister(typeof(ServicoRule), sCID, typeof(ServicoSVC))]
    public class ServicoRule : ServicoSVC.XRule
    {
        public const String sCID = "992A7317-0334-4E28-A91E-7EFE59EC5986";
        public static Guid gCID = new Guid(sCID);

        public ServicoRule()
        {
            ID = gCID;
        }

        private PacoteServicoSVC.XService _Pacote;

        protected override void AfterGet(XExecContext pContext, ServicoSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            _ISExItemPreco preco = GetTable<_ISExItemPreco>();
            _PETxAnimalTutor antu = GetTable<_PETxAnimalTutor>();
            if (antu.Open(pContext.Broker.AuxData[1]))
            {
                _Pacote = GetService<PacoteServicoSVC.XService>();
                _Pacote.Open(PacoteVacinaSVC.xPETxAtendimento.PETxAtendimentoTipoID, PETxAtendimentoTipo.XDefault.Pacote,
                     PacoteVacinaSVC.xPETxAtendimento.PETxAtendimentoEstadoID, XOperator.In, new Int16[] { PETxAtendimentoEstado.XDefault.Recebido, PETxAtendimentoEstado.XDefault.Fechado },
                     XParentheses.Open, PacoteVacinaSVC.xPETxAtendimento.PETxTutorID, antu.Current.PETxTutorID, XLogic.OR,
                     PacoteVacinaSVC.xPETxAtendimento.PETxAnimalTutorID, antu.Current.PETxAnimalTutorID, XParentheses.Close);
                foreach (ServicoSVC.XTuple tpl in pDataSet)
                {
                    tpl.Valor = PegaPreco.PrecoVenda(pContext, preco, tpl.ISExServicoID);
                    if (pContext.SourceID != CriacaoPacoteSAM.gCID && _Pacote?.DataSet.Count > 0)
                    {
                        PacoteServicoSVC.XTuple[] pcttpls = _Pacote.DataSet.Tuples.Where(t => t.ISExServicoID == tpl.ISExServicoID && t.QuantidadePacote < t.Quantidade).ToArray();
                        PacoteServicoSVC.XTuple pcttpl = null;
                        if (pcttpls.Length > 0)
                            pcttpl = pcttpls.FirstOrDefault(t => t.PETxAnimalTutorID == antu.Current.PETxAnimalTutorID) ?? pcttpls.FirstOrDefault();
                        if (pcttpl != null)
                        {
                            tpl.QuantidadePacote = pcttpl.Quantidade - pcttpl.QuantidadePacote;
                            tpl.ItemPacote = pcttpl.PETxAtendimentoServicosID;
                        }
                    }
                }
                return;
            }
            throw new XUnconformity("Escolha um PET antes de selecionar Serviço.");
        }
    }
}