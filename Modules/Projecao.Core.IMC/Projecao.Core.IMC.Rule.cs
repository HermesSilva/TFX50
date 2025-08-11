using System;

using TFX.Core.Data;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC;
using TFX.Core.Model.Interfaces;
using TFX.Core.Reflections;
using TFX.Core.Utils;

using static Projecao.Core.IMC.DB.IMCx;

namespace Projecao.Core.IMC
{
    [XRegister(typeof(AlmaCoreIMCRule), sCID, typeof(ProjecaoCoreIMC))]
    public class AlmaCoreIMCRule : XModuleRule
    {
        public const String sCID = "31B3D0E8-8CCF-485C-9981-FED8590132A9";
        public static Guid gCID = new Guid(sCID);

        public AlmaCoreIMCRule()
        {
            ID = gCID;
        }

        protected override void OnAfterUpdate(XIExecContext pContext)
        {
            if (XVariableManager.RenameList.Count == 0)
                return;
            using (_IMCxAgendaMensagem msgs = XPersistencePool.Get<_IMCxAgendaMensagem>(pContext))
            {
                msgs.MaxRows = 0;
                msgs.Open();
                foreach (XTuple<String, String> vrl in XVariableManager.RenameList)
                {
                    foreach (IMCxAgendaMensagem.XTuple tpl in msgs)
                    {
                        if (XUtils.Replace(tpl.Texto, vrl.Item0, vrl.Item1, out String message))
                            tpl.Texto = message;
                    }
                }
                msgs.Flush();
            }
        }
    }
}