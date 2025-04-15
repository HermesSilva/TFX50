using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using TFX.Core.Exceptions;
using TFX.Core.IDs.Model;
using TFX.Core.Interfaces;

namespace TFX.Core.Cache
{
    public delegate Dictionary<string, XUser> XRefreshCache();
    public class XSessionManager
    {
        public static XILoginService _LoginService;

        public static void Initialize(IServiceProvider pServices)
        {
            _LoginService = pServices.GetService<XILoginService>();
            _LoginService.RefreshCache();
        }

        public static XUserSession DoLogin(XUserLogin pLogin)
        {
            var usrsse = _LoginService.GetUser(pLogin.Login);
            if (usrsse.Session != null)
                return usrsse.Session;
            if (usrsse.User == null)
                throw new XUnconformity("Uauário ou senha inválido.");
            var ret = _LoginService.DoLogin(usrsse.User).Result;
            return ret;
        }

        public static bool CheckLogin(HttpContext pHttpContext)
        {
            ClaimsIdentity idt = pHttpContext.User.Identities.FirstOrDefault(i => i.IsAuthenticated);
            if (idt != null)
            {
                Claim clm = idt.Claims.FirstOrDefault(c => c.Type == XDefault.JWTKey);
                if (clm != null)
                {
                    String key = clm.Value;
                    Guid execid;
                    Guid.TryParse(key, out execid);
                    return true;
                }
            }
            return false;
        }
    }
}
