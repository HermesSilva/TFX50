using TFX.Core.Cache;
using TFX.Core.IDs.Model;

namespace TAF.Access.Test
{
    public class SessionCache
    {
        public SessionCache()
        {

            XSessionCache.Wash();
        }

        [Theory]
        [InlineData("8552FECD-18A0-48F5-A7AC-9D6F8D6D70DC", "CAF3C24B-2935-4D31-92DC-C02F15A664AA")]
        [InlineData("C252626E-882F-47CD-8B2D-305DFF4ACFC3", "6CD31676-6037-4693-876F-4DABB5CAFB93")]
        [InlineData("183A1F40-0289-425E-ABC9-E937771108B3", "92D1D9B2-F6E4-4244-809C-05640FB8ED9A")]
        public void AddUser(string pSessionID, string pUserID)
        {
            XSessionCache.AddSession(new XUserSession { SessionID = new Guid(pSessionID), UserID = new Guid(pUserID) });
            Assert.True(XSessionCache.Count == 1);
            Assert.True(XSessionCache.GetSession(new Guid(pSessionID)) != null);
            XSessionCache.Wash();
            Assert.True(XSessionCache.Count == 0);
        }

        [Theory]
        [InlineData("8552FECD-18A0-48F5-A7AC-9D6F8D6D70DC")]
        public void WrongUser(string pSessionID)
        {
            Assert.True(XSessionCache.Count == 0);
            Assert.Null(XSessionCache.GetSession(new Guid(pSessionID)));
        }
    }
}
