using Newtonsoft.Json.Linq;
using TFX.Core.Controllers;
using TFX.Core.Model;
using TFX.Core.Test.Setup;
using Projecao.Core.ERP.Test;

namespace Projecao.Core.ERP.AutomatedService.Test
{
    public class EMailEnvioTest : TestSetup
    {
        public EMailEnvioTest(XServerSidePrepare pPrepare)
               :base(pPrepare)
        {
        }
    }
}