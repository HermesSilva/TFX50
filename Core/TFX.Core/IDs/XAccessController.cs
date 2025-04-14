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
    public class XContextManagerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext pContext)
        {
            var ctlr = pContext.Controller;
            var fields = ctlr.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields.Where(f => typeof(XICancelable).IsAssignableFrom(f.FieldType)))
                if (field.GetValue(ctlr) is XICancelable cancelable)
                    cancelable.SetCancellationToken(pContext.HttpContext.RequestAborted);
        }

        public override void OnResultExecuted(ResultExecutedContext pContext)
        {
        }
    }


    [ApiController]
    [XStopwatch]
    [XContextManager()]
    [Route("Access")]
    public class XAccessController : ControllerBase
    {
        private readonly ILogger<XAccessController> _Logger;
        private readonly XILoginService _LoginService;
        private static Process _Process;
        static DateTime _Alive = DateTime.Now;

        public XAccessController(ILogger<XAccessController> pLogger, XILoginService pLoginService)
        {
            _Logger = pLogger;
            _LoginService = pLoginService;
            if (_Process == null)
                _Process = Process.GetCurrentProcess();
        }

        [HttpPost, Route("Login")]
        public async Task<IActionResult> Login([FromBody] XUserLogin pLogin)
        {
            //var x = HttpContext.RequestAborted;
            //var cnt = 1;
            //while (!x.IsCancellationRequested)
            //{
            //    await Task.Delay(1000);
            //    XConsole.Debug(cnt++);
            //}
            _LoginService.DoLogin(null);

            await Task.CompletedTask;
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
