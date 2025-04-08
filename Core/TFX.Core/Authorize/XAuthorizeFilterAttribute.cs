using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.IdentityModel.Tokens;

using TFX.Core.Cache;

namespace TFX.Core.Authorize
{
	public class XAuthorizeFilterAttribute : TypeFilterAttribute
	{
		public XAuthorizeFilterAttribute()
			: base(typeof(XAuthorizationFilter))
		{
		}

		public XAuthorizeFilterAttribute(params object[] claims)
			: base(typeof(XAuthorizationFilter))
		{
		}
	}

	public class XAuthorizationFilter : IAuthorizationFilter
	{
		public void OnAuthorization(AuthorizationFilterContext pContext)
		{
			try
			{
                if (XDefault.IsDebugTime)
                	return;
                var issuer = "https://joydipkanjilal.com/";
				var audience = "https://joydipkanjilal.com/";
				var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("This is a sample secret key - please don't use in production environment.'"));
				var token = pContext.HttpContext.Request.Headers["Authorization"].ToString().Split(' ')[1];
				var validationParameters = new TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidIssuer = issuer,
					ValidateAudience = true,
					ValidAudience = audience,
					ValidateIssuerSigningKey = true,
					IssuerSigningKeys = new[] { key },
					ValidateLifetime = false
				};
				var tokenHandler = new JwtSecurityTokenHandler();

				tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);
				var jwt = (JwtSecurityToken)validatedToken;

				if (jwt != null || XSessionManager.CheckLogin(pContext.HttpContext))
					return;

				pContext.Result = new NotFoundResult();
			}
			catch
			{
				pContext.Result = new NotFoundResult();
			}
		}

	}
}
