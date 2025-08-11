using System;
using System.Collections.Generic;

using Projecao.Core.PET.DB;

using TFX.Core.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.PET.DB.PETx;

namespace Projecao.Core.PET.ReadOnly
{
    [XRegister(typeof(ProfissionaEsteticaRule), sCID, typeof(ProfissionaEsteticaSVC))]
    public class ProfissionaEsteticaRule : ProfissionaEsteticaSVC.XRule
    {
        public const String sCID = "EC0AAAED-AA57-472A-867A-6818F77DD8AC";
        public static Guid gCID = new Guid(sCID);

        public ProfissionaEsteticaRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, ProfissionaEsteticaSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            pDataSet.Tuples.ForEach(t => t.ERPxCategoriaID = ERPxCategoria.XDefault.Estetica_Animal);
        }

        protected override void GetWhere(XExecContext pContext, ProfissionaEsteticaSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(ProfissionaEsteticaSVC.xERPxProfissionalCategoria.ERPxCategoriaID, PETx.ERPxCategoria.XDefault.Estetica_Animal);
        }
    }
}