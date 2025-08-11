using System;

using TFX.Core.Model.DIC;
using TFX.Core.Model.Interfaces;
using TFX.Core.Reflections;

namespace Projecao.Core.PET
{
    [XRegister(typeof(AlmaCorePETRule), sCID, typeof(ProjecaoCorePET))]
    public class AlmaCorePETRule : XModuleRule
    {
        public const String sCID = "142FEC7C-D663-4446-842C-1A288EE02C3C";
        public static Guid gCID = new Guid(sCID);

        public AlmaCorePETRule()
        {
            ID = gCID;
        }

        protected override void OnAfterUpdate(XIExecContext pContext)
        {
        }
    }
}