using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

using TFX.Core.Test.Setup;
using TFX.ESC.Core.Escritorio;

namespace TFX.ESC.Core.Test
{
    public class TesteTest : ESCTestSetup
    {

        public TesteTest(XServerSidePrepare pPrepare)
            : base(pPrepare)
        {
        }

        [Fact]
        [XTestPriority(100)]
        public async Task TestX()
        {
            var obj = PrepareClient("http://localhost:5100/Escritorio/ListarEscritoriosPaginacao", null);
            var dst = await DoCall<EscritorioDataSet>(obj);
            Assert.Equal(15, dst.Tuples.Count);
        }

        [Fact]
        [XTestPriority(100)]
        public void Test1()
        {
            var svc = ServiceProvider.GetRequiredService<IEscritorioService>();
            var _DS = svc.Execute(null);
            Assert.Equal(2, _DS.Tuples.Count);
        }
    }
}
