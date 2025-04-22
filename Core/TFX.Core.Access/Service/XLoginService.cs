using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

using TFX.Core.Cache;
using TFX.Core.Exceptions;
using TFX.Core.IDs.Model;
using TFX.Core.Interfaces;
using TFX.Core.Services;
using Microsoft.AspNetCore.Http;
using System;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System.Threading;
using System.Threading.Tasks;
using TFX.Core.Access.UsuariosAtivos;

namespace TFX.Core.Access.Service
{
    public class XLoginService : XService, XILoginService
    {
        private XCacheUser _Users = new XCacheUser();

        public async Task<XUserSession> DoLogin(XUser pUser)
        {
            var cnt = 1;
            while (!CancellationToken.IsCancellationRequested)
            {
                await Task.Delay(1000);
                XConsole.Debug(cnt++);
            }
            return null;
            var session = XSessionCache.GetSession(pUser.SessionID);
            if (session != null)
                return session;
            var usr = _Users.GetUser(pUser.Login);
            if (usr == null)
                throw new XUnconformity("Usuário con a credencial informada não existe.");
            var ret = new XUserSession();
            ret.SessionID = Guid.NewGuid();
            ret.UserID = usr.ID.Value;
            ret.Login = usr.Login;

            var issuer = "https://joydipkanjilal.com/";
            var audience = "https://joydipkanjilal.com/";
            var key = Encoding.UTF8.GetBytes("This is a sample secret key - please don't use in production environment.'");

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", ret.SessionID.ToString()),
                    new Claim(JwtRegisteredClaimNames.Sub, pUser.Login),
                    new Claim(JwtRegisteredClaimNames.Email, pUser.Login),
                    new Claim(JwtRegisteredClaimNames.Jti,
                    Guid.NewGuid().ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(5),
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature),
                Claims = GetData()
            };
            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);
            var stringToken = tokenHandler.WriteToken(token);
            ret.Token = stringToken;
            XSessionCache.AddSession(ret);
            return ret;
        }

        private IDictionary<string, object> GetData()
        {
            var dic = new Dictionary<string, object>();
            dic.Add("sjdhjkshd", "dfjkhfdjkhd jkfhjkd f");
            return dic;
        }

        public (XUser User, XUserSession Session) GetUser(string pLogin)
        {
            return (_Users.GetUser(pLogin), null);
        }

        public void RefreshCache(Dictionary<string, XUser> pUsers = null)
        {
            if (pUsers == null)
            {
                using (var scope = XEnvironment.Services.CreateScope())
                {
                    var svc = scope.ServiceProvider.GetRequiredService<IUsuariosAtivosService>();

                    pUsers = new Dictionary<string, XUser>();
                    var dst = svc.Execute(null);
                    foreach (var item in dst.Tuples)
                    {
                        pUsers.Add(item.Login.Value, new XUser { ID = item.TAFxUsuarioID.Value, Login = item.Login.Value });
                    }
                }
            }
            lock (_Users)
                _Users.Swap(pUsers);
        }

        public void GracefullyClose()
        {

        }
    }
}
