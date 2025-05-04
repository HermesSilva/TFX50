using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using TFX.Core.Authorize;
using TFX.Core.Cache;
using TFX.Core.Controllers;
using TFX.Core.IDs.Model;
using TFX.Core.Interfaces;

namespace TFX.Core.IDs
{
 

    [ApiController]
    [XStopwatch]
    [Route("Access")]
    public class XAccessController : XBaseController
    {
        private readonly XILoginService _LoginService;
        private static Process _Process;
        static DateTime _Alive = DateTime.Now;

        public XAccessController(XILoginService pLoginService)
        {
            _LoginService = pLoginService;
            if (_Process == null)
                _Process = Process.GetCurrentProcess();
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromBody] XUserLogin pLogin)
        {

            var tabId = HttpContext.Request.Headers["SessionID"].FirstOrDefault();
            XConsole.WriteLine(tabId);
            //await _LoginService.DoLogin(null);
            //var session = XSessionManager.DoLogin(HttpContext, pLogin);
            //if (session == null)
            //    return Unauthorized(XDefault.Unauthorized());
            return Ok(new XUserSession());
        }

        [HttpPost, Route("HealthCheck")]
        public ActionResult HealthCheck()
        {
            return Ok($"We alive since {_Alive} ({(DateTime.Now - _Alive)})");
        }

        [HttpPost, Route("X21")]
        [XAuthorizeFilter]
        public ActionResult X21()
        {
            return Ok($"We alive since {_Alive} ({(DateTime.Now - _Alive)})");
        }


    }
}
