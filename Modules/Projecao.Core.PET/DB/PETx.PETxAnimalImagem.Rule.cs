using System;
using System.Linq;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.SVC;

using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.PET.DB
{
    [XRegister(typeof(PETxAnimalImagemRule), sCID, typeof(PETx.PETxAnimalImagem))]
    public class PETxAnimalImagemRule : XPersistenceRule<PETx._PETxAnimalImagem, PETx.PETxAnimalImagem, PETx.PETxAnimalImagem.XTuple>
    {
        public const String sCID = "B5041A3C-26C5-4D0B-AAA3-74D5EA7D49E6";
        public static Guid gCID = new Guid(sCID);

        public PETxAnimalImagemRule()
        {
            ID = new Guid(sCID);
        }

        public override Int32 ExecuteOrder => 0;

        protected override void AfterFlush(XExecContext pContext, PETx.PETxAnimalImagem pModel, PETx._PETxAnimalImagem pDataSet)
        {
            _SYSxImagem img = GetTable<_SYSxImagem>();
            foreach (PETx.PETxAnimalImagem.XTuple tpl in pDataSet.Tuples.Where(t => t.SYSxEstadoID == SYSxEstado.XDefault.Inativo))
            {
                if (img.Open(tpl.PKValue))
                    img.Delete(img.Current);
            }
        }
    }
}