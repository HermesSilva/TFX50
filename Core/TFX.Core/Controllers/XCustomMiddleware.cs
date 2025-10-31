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
            var idexpage = @"<!DOCTYPE html>
<html xmlns=""http://www.w3.org/1999/xhtml"">
<head>
    <meta charset=""utf-8"" />
    <title>TFX Core</title>
    <script src=""js/TFX.Core.js""></script>
    <link href=""css/TFX.Core.css"" rel=""stylesheet"" />
    <link rel=""icon"" href=""svg/favicon.svg"" type=""image/svg+xml"">
    <link rel=""shortcut icon"" href=""svg/favicon.svg"" type=""image/svg+xml"">
</head>
<body onload=""Stage.Run()"">
</body>
</html>";
            await pHttpContext.Response.BodyWriter.WriteAsync(Encoding.UTF8.GetBytes(idexpage));
        }
    }
}
