using System.Collections.Generic;

using Microsoft.AspNetCore.Http;

using TFX.Core.IDs.Model;

namespace TFX.Core.Interfaces
{
    public interface XILoginService : XIService
    {
        XUserSession DoLogin(XUser pUser);
        (XUser User, XUserSession Session) GetUser(string pLogin);
        void RefreshCache(Dictionary<string, XUser> pUsers = null);
    }
}
