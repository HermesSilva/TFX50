using System;
using System.Collections.Generic;

using Projecao.Core.PCR.DB;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Integracao
{
    [XRegister(typeof(ImplanteIATFRule), sCID, typeof(ImplanteIATFSVC))]
    public class ImplanteIATFRule : ImplanteIATFSVC.XRule
    {
        public const String sCID = "AE18C88C-94BD-4001-BB16-85480B278EE5";
        public static Guid gCID = new Guid(sCID);

        public ImplanteIATFRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, ImplanteIATFSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            ImplanteIATFSVC.XTuple tpl = pDataSet.NewTuple(Guid.Empty);
            tpl.Nome = " Não Informado";
        }

        protected override void GetWhere(XExecContext pContext, ImplanteIATFSVC.XDataSet pDataSet, List<Object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, Object>, List<Object>>> pRawFilter, Boolean pIsPKGet)
        {
            pWhere.Add(ImplanteIATFSVC.xISExItemCategoria.ISExCategoriaID, PCRx.ISExCategoria.XDefault.Implante_para_IATF);
        }
    }
}