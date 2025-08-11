using Newtonsoft.Json.Linq;
using TFX.Core.Controllers;
using TFX.Core.Model;
using TFX.Core.Test.Setup;
using Projecao.Core.ERP.Test;

namespace Projecao.Core.ERP.Profissional.Test
{
    public class HorariosTest : TestSetup
    {
        public HorariosTest(XServerSidePrepare pPrepare)
               :base(pPrepare)
        {
        }

        [Fact]
        [XTestPriority(100)]
        public async Task DataSetNullo()
        {
            var obj = PrepareClient("http://localhost:7000/Horarios/Flush", null);
            var ret = await DoCall<XResponse>(obj);
            Assert.Equal(400, ret.Status);
            Assert.Equal(XResponse.BadJSon, ret.Data.ToString());
        }

        [Fact]
        [XTestPriority(101)]
        public async Task DataSetSemTuplas()
        {
            var obj = PrepareClient("http://localhost:7000/Horarios/Flush", new HorariosDataSet());
            var ret = await DoCall<XResponse>(obj);
            Assert.Equal(404, ret.Status);
            if (ret.Data is JObject jo)
            {
                var epm = jo.ToObject<XEndPointMessage>();
                if (epm != null)
                    Assert.Equal(XResponse.TuplesCount, epm.Message.FirstOrDefault());
            }
        }

        [Fact]
        [XTestPriority(102)]
        public async Task DataSetCamposNulos()
        {
            var dst = new HorariosDataSet();
            dst.Tuples.Add(new HorariosTuple());
            var obj = PrepareClient("http://localhost:7000/Horarios/Flush", dst);
            var ret = await DoCall<XResponse>(obj);
            Assert.Equal(404, ret.Status);
            if (ret.Data is JObject jo)
            {
                var epm = jo.ToObject<XEndPointMessage>();
                if (epm != null)
                {
                    Assert.Contains("Erro: O campo \"Fim\" não pode ser nulo.", epm.Message);
                    Assert.Contains("Erro: O campo \"Início\" não pode ser nulo.", epm.Message);
                }
            }
        }
    }
}