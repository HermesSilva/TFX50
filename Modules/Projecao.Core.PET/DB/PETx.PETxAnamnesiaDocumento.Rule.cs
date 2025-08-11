using System;
using System.Linq;

using TFX.Core;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.SVC;

namespace Projecao.Core.PET.DB
{
    [XRegister(typeof(PETxAnamnesiaDocumentoRule), sCID, typeof(PETx.PETxAnamnesiaDocumento))]
    public class PETxAnamnesiaDocumentoRule : XPersistenceRule<PETx._PETxAnamnesiaDocumento, PETx.PETxAnamnesiaDocumento, PETx.PETxAnamnesiaDocumento.XTuple>
    {
        public const String sCID = "16DA0E33-C666-4ECC-85C5-B07A506B50D7";
        public static Guid gCID = new Guid(sCID);

        public PETxAnamnesiaDocumentoRule()
        {
            ID = new Guid(sCID);
        }

        public override Int32 ExecuteOrder => 0;

        protected override void BeforeFlush(XExecContext pContext, PETx.PETxAnamnesiaDocumento pModel, PETx._PETxAnamnesiaDocumento pDataSet)
        {
            foreach (PETx.PETxAnamnesiaDocumento.XTuple tpl in pDataSet.Where(t => t.State == XTupleState.New))
            {
                if (tpl.Descricao.IsEmpty())
                    tpl.Descricao = $"Adicionado em {XDefault.Now.ToString("dd/MM/yyyy HH:mm:ss")}";
            }
        }
    }
}