using System;

using TFX.Core.Model.DIC;
using TFX.Core.Reflections;

namespace Projecao.Core.NTR.PessoaFisica
{
    [XRegister(typeof(AlmaCoreNTRRule), sCID, typeof(ProjecaoCoreNTR))]
    public class AlmaCoreNTRRule : XModuleRule
    {
        public const String sCID = "60AD1341-0AB4-4E74-926E-43BE086449EC";
        public static Guid gCID = new Guid(sCID);

        public AlmaCoreNTRRule()
        {
            ID = gCID;
        }

        protected override void OnLoad()
        {
        }
    }
}