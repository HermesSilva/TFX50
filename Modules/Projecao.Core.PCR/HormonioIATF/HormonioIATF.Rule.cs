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

namespace Projecao.Core.PCR.HormonioIATF
{
    [XRegister(typeof(HormonioIATFRule), sCID, typeof(HormonioIATFSVC))]
    public class HormonioIATFRule : HormonioIATFSVC.XRule
    {
        public const String sCID = "1A5C9008-3B85-47F7-8F10-A005003C6392";
        public static Guid gCID = new Guid(sCID);

        public HormonioIATFRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, HormonioIATFSVC.XDataSet pDataSet, List<Object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, Object>, List<Object>>> pRawFilter, Boolean pIsPKGet)
        {
            pWhere.Add(HormonioIATFSVC.xISExItemCategoria.ISExCategoriaID, PCRx.ISExCategoria.XDefault.Hormonio_para_AITF);
        }

        protected override void AfterFlush(XExecContext pContext, HormonioIATFSVC pModel, HormonioIATFSVC.XDataSet pDataSet)
        {
            ISEx._ISExItemCategoria categ = GetTable<ISEx._ISExItemCategoria>();
            foreach (HormonioIATFSVC.XTuple tpl in pDataSet.Tuples.Where(t => t.State == XTupleState.New))
            {
                ISEx.ISExItemCategoria.XTuple cttpl = categ.NewTuple();
                cttpl.ISExCategoriaID = PCRx.ISExCategoria.XDefault.Hormonio_para_AITF;
                cttpl.ISExItemID = tpl.ISExItemID;
                cttpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                categ.Flush();
            }
        }
    }
}