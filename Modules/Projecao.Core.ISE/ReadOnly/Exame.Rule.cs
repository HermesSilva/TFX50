using System;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;

namespace Projecao.Core.ISE.ReadOnly
{
    [XRegister(typeof(ExameRule), sCID, typeof(ExameSVC))]
    public class ExameRule : ExameSVC.XRule
    {

        public const String sCID = "8C01AC65-5AC7-4EFB-B544-F86E7D89A36B";
        public static Guid gCID = new Guid(sCID);

        public ExameRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, ExameSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            _ISExItemPreco preco = GetTable<_ISExItemPreco>();
            pDataSet.Tuples.ForEach(t => t.Valor = PegaPreco.PrecoVenda(pContext, preco, t.ISExExameID));
        }
    }
}

