using Newtonsoft.Json.Linq;
using TFX.Core.Controllers;
using TFX.Core.Model;
using TFX.Core.Test.Setup;
using Projecao.Core.ERP.Test;

namespace Projecao.Core.ERP.ReadOnly.Test
{
    public class EmpresaTest : TestSetup
    {
        public EmpresaTest(XServerSidePrepare pPrepare)
               :base(pPrepare)
        {
        }
    }
}