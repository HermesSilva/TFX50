using System;
using System.Collections.Generic;

using Projecao.Core.PCR.DB;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.Integracao
{
    [XRegister(typeof(VeterinarioRule), sCID, typeof(VeterinarioSVC))]
    public class VeterinarioRule : VeterinarioSVC.XRule
    {
        public const String sCID = "2FF985A5-CF7E-4F02-AD0B-FB6A85A33923";
        public static Guid gCID = new Guid(sCID);

        public VeterinarioRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, VeterinarioSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(VeterinarioSVC.xERPxPessoaFisicaTipos.ERPxPessoaFisicaTipoID, PCRx.ERPxPessoaFisicaTipo.XDefault.Veterinario);
        }
    }
}