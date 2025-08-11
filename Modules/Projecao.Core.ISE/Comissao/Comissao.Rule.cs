using System;
using System.Collections.Generic;
using System.Linq;

using Projecao.Core.ISE.ReadOnly;

using TFX.Core.Data;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;

namespace Projecao.Core.ISE.Comissao
{
    [XRegister(typeof(ComissaoRule), sCID, typeof(ComissaoSVC))]
    public class ComissaoRule : ComissaoSVC.XRule
    {
        public const String sCID = "8D169504-F8F6-4B8D-B2FF-A68D822E8BC8";
        public static Guid gCID = new Guid(sCID);

        public ComissaoRule()
        {
            ID = gCID;
        }

        protected override void BeforeGet(XExecContext pContext, ComissaoSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            if (pContext.SourceID == ComissaoServicoSAM.gCID)
            {
                _ISExServico svs = GetTable<_ISExServico>();
                _ISExComissao cmi = GetTable<_ISExComissao>();
                if (cmi.RecordCount(ISExComissao.SYSxEmpresaID, pContext.Logon.CurrentCompanyID) != svs.RecordCount())
                {
                    ComissaoEmpresaSVC.XService cmis2 = GetService<ComissaoEmpresaSVC.XService>();
                    ComissaoSVC.XService cmis = GetService<ComissaoSVC.XService>();
                    svs.MaxRows = 0;
                    svs.FilterActives = true;
                    cmis2.FilterInactive = false;
                    cmis2.MaxRows = 0;
                    cmis2.Open(ISExComissao.SYSxEmpresaID, pContext.Logon.CurrentCompanyID);
                    svs.Open();

                    foreach (ISExServico.XTuple tpl in svs)
                    {
                        if (cmis2.Tuples.Any(t => t.ISExItemID == tpl.ISExServicoID))
                            continue;
                        cmis.NewTuple();
                        cmis.Current.ISExItemID = tpl.ISExServicoID;
                        cmis.Current.ISExComissaoTipoID = ISExComissaoTipo.XDefault.Lojista;
                    }
                    cmis.Flush();
                }
            }
        }

        protected override void GetWhere(XExecContext pContext, ComissaoSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(ComissaoSVC.xISExComissao.SYSxEmpresaID, pContext.Logon.CurrentCompanyID);
        }

        protected override void BeforeFlush(XExecContext pContext, ComissaoSVC pModel, ComissaoSVC.XDataSet pDataSet)
        {
            pDataSet.Tuples.Where(t => t.State == XTupleState.New).ForEach(x => x.ISExComissaoTipoID = ISExComissaoTipo.XDefault.Lojista);
        }
    }
}