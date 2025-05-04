using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using TFX.Core.Authorize;
using TFX.Core.Cache;
using TFX.Core.Exceptions;
using TFX.Core.IDs;
using TFX.Core.IDs.Model;
using TFX.Core.Interfaces;

namespace TFX.Core.Controllers
{
    public class XContextManagerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext pContext)
        {
            var ctlr = pContext.Controller;
            if (ctlr is XBaseController cancl)
            {
                cancl.SetContextData(pContext.HttpContext.RequestAborted, null);

                var fields = ctlr.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields.Where(f => typeof(XIUseContext).IsAssignableFrom(f.FieldType)))
                {
                    var obj = field.GetValue(ctlr);
                    if (obj is XIUseContext cancelable)
                        cancelable.SetContextData(pContext.HttpContext.RequestAborted, cancl.Session);
                }
            }
        }

        public override void OnResultExecuted(ResultExecutedContext pContext)
        {
        }
    }

    public class XStopwatchAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext pContext)
        {
            Stopwatch stp = new Stopwatch();
            pContext.HttpContext.Items["Stopwatch"] = stp;
            stp.Start();
        }

        public override void OnResultExecuted(ResultExecutedContext pContext)
        {
            Stopwatch stp = pContext.HttpContext.Items["Stopwatch"] as Stopwatch;
            stp?.Stop();
            XConsole.Warn($"Ellapsed {stp?.Elapsed.TotalMilliseconds.ToString("#,##0.0")}ms {pContext.HttpContext.Request.Path}");
        }
    }

    [XStopwatch]
    [XContextManager]
    public abstract class XBaseController : ControllerBase, XIUseContext
    {

        protected readonly ILogger<XBaseController> Log;
        private XUserSession _Session;
        public XUserSession Session => _Session;

        protected CancellationToken CancellationToken
        {
            get;
            private set;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void SetContextData(CancellationToken pCancellationToken, XUserSession pSession)
        {
            CancellationToken = pCancellationToken;
            _Session = XSessionCache.GetSession(Guid.Empty);
        }
    }
}
