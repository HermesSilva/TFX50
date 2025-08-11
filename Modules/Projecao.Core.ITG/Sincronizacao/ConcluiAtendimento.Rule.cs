using System;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.PET.DB.PETx;

namespace Projecao.Core.ITG.Sincronizacao
{
    [XRegister(typeof(ConcluiAtendimentoRule), sCID, typeof(ConcluiAtendimentoSVC))]
    public class ConcluiAtendimentoRule : ConcluiAtendimentoSVC.XRule
    {
        public const String sCID = "F5D2D888-98AD-44C2-A056-91598D4373BD";
        public static Guid gCID = new Guid(sCID);

        public ConcluiAtendimentoRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, ConcluiAtendimentoSVC pModel, ConcluiAtendimentoSVC.XDataSet pDataSet)
        {
            _PETxAtendimento agedamento = GetTable<_PETxAtendimento>();
            foreach (ConcluiAtendimentoSVC.XTuple tpl in pDataSet)
            {
                agedamento.Open(tpl.AtendimentoID);
                agedamento.Current.PETxAtendimentoEstadoID = tpl.EstadoID;
                agedamento.Flush();
            }
        }
    }
}