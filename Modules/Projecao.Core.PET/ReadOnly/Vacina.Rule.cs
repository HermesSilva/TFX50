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
    [XRegister(typeof(VacinaRule), sCID, typeof(VacinaSVC))]
    public class VacinaRule : VacinaSVC.XRule
    {
        public const String sCID = "11005108-912B-4840-90FB-8CC51BDD4143";
        public static Guid gCID = new Guid(sCID);

        public VacinaRule()
        {
            ID = gCID;
        }

        private PacoteVacinaSVC.XService _Pacote;

        protected override void AfterGet(XExecContext pContext, VacinaSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            _ISExItemPreco preco = GetTable<_ISExItemPreco>();
            _PETxAnimalTutor antu = GetTable<_PETxAnimalTutor>();
            Boolean hsfilter = pContext.Broker.AuxData.SafeLength() > 0;
            if (hsfilter)
            {
                if (antu.Open(pContext.Broker.AuxData[1]))
                {
                    _Pacote = GetService<PacoteVacinaSVC.XService>();
                    _Pacote.Open(PacoteVacinaSVC.xPETxAtendimento.PETxAtendimentoTipoID, PETxAtendimentoTipo.XDefault.Pacote,
                         PacoteVacinaSVC.xPETxAtendimento.PETxAtendimentoEstadoID, XOperator.In, new Int16[] { PETxAtendimentoEstado.XDefault.Recebido, PETxAtendimentoEstado.XDefault.Fechado },
                         XParentheses.Open, PacoteVacinaSVC.xPETxAtendimento.PETxTutorID, antu.Current.PETxTutorID, XLogic.OR,
                         PacoteVacinaSVC.xPETxAtendimento.PETxAnimalTutorID, antu.Current.PETxAnimalTutorID, XParentheses.Close);
                }
                else
                    throw new XUnconformity("Escolha um PET antes de selecionar Vacinas.");
            }
            foreach (VacinaSVC.XTuple tpl in pDataSet)
            {
                tpl.Valor = PegaPreco.PrecoVenda(pContext, preco, tpl.ISExVacinaID);
                if (pContext.SourceID != CriacaoPacoteSAM.gCID && _Pacote?.DataSet.Count > 0 && antu.Current != null)
                {
                    PacoteVacinaSVC.XTuple[] pcttpls = _Pacote.DataSet.Tuples.Where(t => t.ISExVacinaID == tpl.ISExVacinaID && t.QuantidadePacote < t.Quantidade).ToArray();
                    PacoteVacinaSVC.XTuple pcttpl = null;
                    if (pcttpls.Length > 0)
                        pcttpl = pcttpls.FirstOrDefault(t => t.PETxAnimalTutorID == antu.Current.PETxAnimalTutorID) ?? pcttpls.FirstOrDefault();
                    if (pcttpl != null)
                    {
                        tpl.QuantidadePacote = pcttpl.Quantidade - pcttpl.QuantidadePacote;
                        tpl.ItemPacote = pcttpl.PETxAtendimentoVacinaID;
                    }
                }
            }
        }
    }
}