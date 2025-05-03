using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace TFX.Core.Controllers
{
    public class XResponse
    {
        public const String TuplesCount = "Não é permitido Flush sem Tuplas.";
        public const String BadJSon = "JSon inválido.";

        public bool Ok
        {
            get; set;
        }
        public int Status
        {
            get; set;
        }
        public Object Data
        {
            get; set;
        }
        public string Errors
        {
            get;
            internal set;
        }
        public string Details
        {
            get;
            internal set;
        }
    }

    public class XResponseWrapperFilter : IAsyncResultFilter
    {
        public async Task OnResultExecutionAsync(ResultExecutingContext pContext, ResultExecutionDelegate pNext)
        {
            if (pContext.Result is ObjectResult result)
            {
                var pack = new XResponse
                {
                    Ok = true,
                    Status = result.StatusCode ?? 200,
                    Data = result.Value
                };
                pack.Ok = pack.Status >= 200 && pack.Status < 300;
                result.Value = pack;
            }
            await pNext();
        }
    }
}