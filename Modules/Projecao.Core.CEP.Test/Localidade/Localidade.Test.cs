using Newtonsoft.Json.Linq;
using TFX.Core.Controllers;
using TFX.Core.Model;
using TFX.Core.Test.Setup;
using Projecao.Core.CEP.Test;

namespace Projecao.Core.CEP.Localidade.Test
{
    public class LocalidadeTest : TestSetup
    {
        public LocalidadeTest(XServerSidePrepare pPrepare)
               :base(pPrepare)
        {
        }

        [Fact]
        [XTestPriority(100)]
        public async Task DataSetNullo()
        {
            var obj = PrepareClient("http://localhost:7000/Localidade/Flush", null);
            var ret = await DoCall<XResponse>(obj);
            Assert.Equal(400, ret.Status);
            Assert.Equal(XResponse.BadJSon, ret.Data.ToString());
        }

        [Fact]
        [XTestPriority(101)]
        public async Task DataSetSemTuplas()
        {
            var obj = PrepareClient("http://localhost:7000/Localidade/Flush", new LocalidadeDataSet());
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
            var dst = new LocalidadeDataSet();
            dst.Tuples.Add(new LocalidadeTuple());
            var obj = PrepareClient("http://localhost:7000/Localidade/Flush", dst);
            var ret = await DoCall<XResponse>(obj);
            Assert.Equal(404, ret.Status);
            if (ret.Data is JObject jo)
            {
                var epm = jo.ToObject<XEndPointMessage>();
                if (epm != null)
                {
                    Assert.Contains("Erro: O campo \"CEP Geral\" não pode ser nulo.", epm.Message);
                    Assert.Contains("Erro: O campo \"Código no IBGE\" não pode ser nulo.", epm.Message);
                    Assert.Contains("Erro: O campo \"Nome da Localidade\" não pode ser nulo.", epm.Message);
                    Assert.Contains("Erro: O campo \"Número\" não pode ser nulo.", epm.Message);
                }
            }
        }
    }
}