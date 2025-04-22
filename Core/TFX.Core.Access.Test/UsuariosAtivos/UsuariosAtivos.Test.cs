using Newtonsoft.Json.Linq;
using TFX.Core.Controllers;
using TFX.Core.Model;
using TFX.Core.Test.Setup;
using TFX.Core.Access.Test;

namespace TFX.Core.Access.UsuariosAtivos.Test
{
    public class UsuariosAtivosTest : TestSetup
    {
        public UsuariosAtivosTest(XServerSidePrepare pPrepare)
               :base(pPrepare)
        {
        }

        [Fact]
        [XTestPriority(100)]
        public async Task DataSetNullo()
        {
            var obj = PrepareClient("http://localhost:7000/UsuariosAtivos/Flush", null);
            var ret = await DoCall<XResponse>(obj);
            Assert.Equal(400, ret.Status);
            Assert.Equal(XResponse.BadJSon, ret.Data.ToString());
        }

        [Fact]
        [XTestPriority(101)]
        public async Task DataSetSemTuplas()
        {
            var obj = PrepareClient("http://localhost:7000/UsuariosAtivos/Flush", new UsuariosAtivosDataSet());
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
            var dst = new UsuariosAtivosDataSet();
            dst.Tuples.Add(new UsuariosAtivosTuple());
            var obj = PrepareClient("http://localhost:7000/UsuariosAtivos/Flush", dst);
            var ret = await DoCall<XResponse>(obj);
            Assert.Equal(404, ret.Status);
            if (ret.Data is JObject jo)
            {
                var epm = jo.ToObject<XEndPointMessage>();
                if (epm != null)
                {
                    Assert.Contains("Erro: O campo \"Login\" não pode ser nulo.", epm.Message);
                    Assert.Contains("Erro: O campo \"Nome\" não pode ser nulo.", epm.Message);
                }
            }
        }
    }
}