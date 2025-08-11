using System;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;

namespace Projecao.Core.ISE.Comissao
{
    [XRegister(typeof(ComissaoGrupoRule), sCID, typeof(ComissaoGrupoSVC))]
    public class ComissaoGrupoRule : ComissaoGrupoSVC.XRule
    {
        public const String sCID = "A0922B68-7485-42FF-98E5-B2EEEDE3E4F6";
        public static Guid gCID = new Guid(sCID);

        public ComissaoGrupoRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, ComissaoGrupoSVC pModel, ComissaoGrupoSVC.XDataSet pDataSet)
        {
            _ISExItem item = GetTable<_ISExItem>();
            ComissaoSVC.XService cms = GetService<ComissaoSVC.XService>();
            item.FilterActives = false;
            item.MaxRows = 0;
            foreach (ComissaoGrupoSVC.XTuple tpl in pDataSet.Tuples)
            {
                item.Open(ISExItem.ISExGrupoID, tpl.ISExGrupoID);
                foreach (ISExItem.XTuple ctpl in item.Tuples)
                {
                    if (!cms.OpenAppend(ComissaoSVC.xISExComissao.ISExItemID, ctpl.ISExItemID))
                        cms.NewTuple();
                    cms.Current.ISExItemID = ctpl.ISExItemID;
                    cms.Current.Comissao = tpl.Comissao;
                    cms.Current.ISExComissaoTipoID = ISExComissaoTipo.XDefault.Lojista;
                }
            }
            cms.Flush();
        }
    }
}