using System;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;

namespace Projecao.Core.ISE.ReadOnly
{
    [XRegister(typeof(ServicosRule), sCID, typeof(ServicosSVC))]
    public class ServicosRule : ServicosSVC.XRule
    {
        public const String sCID = "AB397047-1163-4A49-934E-0869D8193E0F";
        public static Guid gCID = new Guid(sCID);

        public ServicosRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, ServicosSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            pDataSet.Tuples.ForEach(t =>
            {
                t.Tipo = ISExComissaoTipo.XDefault.GetTitle(ISExComissaoTipo.XDefault.Lojista);
                t.TipoID = ISExComissaoTipo.XDefault.Lojista;
            });
        }
    }
}