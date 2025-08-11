using System;
using System.Collections.Generic;
using System.Linq;

using Projecao.Core.ISE.DB;
using Projecao.Core.PCR.DB;

using TFX.Core.Data;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.PCR.ImplanteIATF
{
    [XRegister(typeof(ImplanteIATFRule), sCID, typeof(ImplanteIATFSVC))]
    public class ImplanteIATFRule : ImplanteIATFSVC.XRule
    {
        public const String sCID = "DA83DE6A-27C8-40C4-85AA-2DE3BBC1E9D7";
        public static Guid gCID = new Guid(sCID);

        public ImplanteIATFRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, ImplanteIATFSVC.XDataSet pDataSet, List<Object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, Object>, List<Object>>> pRawFilter, Boolean pIsPKGet)
        {
            pWhere.Add(ImplanteIATFSVC.xISExItemCategoria.ISExCategoriaID, PCRx.ISExCategoria.XDefault.Implante_para_IATF);
        }

        protected override void AfterFlush(XExecContext pContext, ImplanteIATFSVC pModel, ImplanteIATFSVC.XDataSet pDataSet)
        {
            ISEx._ISExItemCategoria categ = GetTable<ISEx._ISExItemCategoria>();
            foreach (ImplanteIATFSVC.XTuple tpl in pDataSet.Tuples.Where(t => t.State == XTupleState.New))
            {
                ISEx.ISExItemCategoria.XTuple cttpl = categ.NewTuple();
                cttpl.ISExCategoriaID = PCRx.ISExCategoria.XDefault.Implante_para_IATF;
                cttpl.ISExItemID = tpl.ISExItemID;
                cttpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
            }
            categ.Flush();
        }
    }
}