using System;

using Microsoft.AspNetCore.Http;

using Projecao.Core.ITG;
using Projecao.Core.ITG.RI;

using TFX.Core.Model.Data;
using TFX.Core.Model.DIC;
using TFX.Core.Model.Interfaces;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.Web;

namespace Projecao.Core.PET
{
    [XRegister(typeof(ProjecaoCoreITGRule), sCID, typeof(ProjecaoCoreITG))]
    public class ProjecaoCoreITGRule : XModuleRule
    {
        public const String sCID = "CA2585A7-7D9A-4C63-9AFD-1FDFBAB70783";
        public static Guid gCID = new Guid(sCID);

        public ProjecaoCoreITGRule()
        {
            ID = gCID;
        }

        protected override void OnLoad()
        {
            XContextManager.Paths.Add(XReverseIntegration.ReverseIntegration, "/reverseintegration");
        }
    }
}