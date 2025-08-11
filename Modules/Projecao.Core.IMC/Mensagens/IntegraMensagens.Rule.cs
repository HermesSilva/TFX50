using System;
using System.Linq;
using System.Collections.Generic;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.Job;
using TFX.Core.Service.SVC;
using TFX.Core.Data;

namespace Projecao.Core.IMC.Mensagens
{
    [XRegister(typeof(IntegraMensagensRule), sCID, typeof(IntegraMensagensSVC))]
    public class IntegraMensagensRule : IntegraMensagensSVC.XRule
    {

        public const String sCID = "D0C07A3A-857A-420D-9E50-08A653E2B0BE";
        public static Guid gCID = new Guid(sCID);

        public IntegraMensagensRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, IntegraMensagensSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            pWhere.Add(IntegraMensagensSVC.xIMCxMensagem.SYSxEmpresaID, pContext.Logon.CurrentCompanyID, IntegraMensagensSVC.xIMCxMensagem.Integrado, false);
        }
    }
}
