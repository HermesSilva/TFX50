using System;
using System.Collections.Generic;

using Projecao.Core.PET.DB;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.PET.DB.PETx;

namespace Projecao.Core.PET.ReadOnly
{
    [XRegister(typeof(ProfissionalSaudoRule), sCID, typeof(ProfissionalSaudeSVC))]
    public class ProfissionalSaudoRule : ProfissionalSaudeSVC.XRule
    {
        public const String sCID = "766D0A68-67D5-4AAA-ABB7-47D381282D62";
        public static Guid gCID = new Guid(sCID);

        public ProfissionalSaudoRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, ProfissionalSaudeSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            pDataSet.Tuples.ForEach(t => t.ERPxCategoriaID = ERPxCategoria.XDefault.Saude_Animal);
        }

        protected override void GetWhere(XExecContext pContext, ProfissionalSaudeSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(ProfissionalSaudeSVC.xERPxProfissionalCategoria.ERPxCategoriaID, PETx.ERPxCategoria.XDefault.Saude_Animal);
        }
    }
}