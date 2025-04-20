using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using TFX.Core.Test.Setup;

namespace TFX.ESC.Core.Test
{
    public class ESCTestSetup : XBaseTest
    {
        public ESCTestSetup(XServerSidePrepare pPrepare)
            : base(pPrepare)
        {
        }

        protected override void AppPrepare(WebApplicationBuilder pBuilder)
        {
            
            base.AppPrepare(pBuilder);
        }
    }
}
