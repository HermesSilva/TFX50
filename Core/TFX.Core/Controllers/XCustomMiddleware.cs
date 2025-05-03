using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

using TFX.Core.Data;

namespace TFX.Core.Controllers
{
    public delegate Task XExecute(HttpContext pHttpContext);

    public class XCustomMiddleware : IMiddleware
    {
        static XCustomMiddleware()
        {
            Paths.Add("/", GetIndex);
        }

        public static Dictionary<string, XExecute> Paths = new Dictionary<string, XExecute>();

        public async Task InvokeAsync(HttpContext pContext, RequestDelegate next)
        {
            try
            {
                String path = pContext.Request.Path.Value.SafeLower();
                if (Paths.TryGetValue(path, out XExecute action))
                {
                    await action(pContext);
                    return;
                }
                await next(pContext);
            }
            catch (Exception pEx)
            {
                if (pContext.Response.StatusCode == 0)
                {
                    pContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                    pContext.Response.ContentType = "application/json";
                }
                var response = new XResponse
                {
                    Ok = false,
                    Status = pContext.Response.StatusCode,
                    Data = null,
                    Errors = XUtils.GetExceptionMessages(pEx),
                    Details = XUtils.GetExceptionDetails(pEx),
                };
                await pContext.Response.WriteAsync(XUtils.SerializeString(response));
            }
        }

        private static async Task GetIndex(HttpContext pHttpContext)
        {
            Stream strm = XResource.GetResourceStream<XUseContext>(@"Resource.index.html");
            strm.CopyTo(pHttpContext.Response.Body);
            await Task.CompletedTask;
        }
    }
}
