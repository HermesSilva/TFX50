using System;
using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

using TFX.Core.Authorize;
using TFX.Core.Cache;
using TFX.Core.Controllers;
using TFX.Core.IDs.Model;

namespace TFX.Core.IDs
{
    [ApiController]
    [XStopwatch]
    [Route("Access")]
    public class XAccessController : ControllerBase
    {
        private readonly ILogger<XAccessController> _Logger;
        private static Process _Process;
        static DateTime _Alive = DateTime.Now;

        public XAccessController(ILogger<XAccessController> pLogger)
        {
            _Logger = pLogger;
            if (_Process == null)
                _Process = Process.GetCurrentProcess();
        }

        [HttpPost, Route("Login")]
        public IActionResult Login([FromBody] XUserLogin pLogin)
        {
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
