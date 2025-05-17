using System;
using System.ComponentModel;
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
using TFX.Core.Exceptions;
using TFX.Core.IDs;
using TFX.Core.IDs.Model;
using TFX.Core.Interfaces;
using TFX.Core.Model.APP;
using TFX.Core.Model.Payload;
using TFX.Core.Model.Service;

namespace TFX.Core.Controllers
{
    [Route("Model")]
    public class XModelController : XBaseController
    {

        [HttpPost, Route("App")]
        public async Task<XAPPModel> App([FromBody] XModelPayload pPayload)
        {
            var mdl = await XMainCache.GetAppModel(pPayload);
            return mdl;
        }
        [HttpPost, Route("Service")]
        public async Task<XServiceModel> Service([FromBody] XModelPayload pPayload)
        {
            var mdl = await XMainCache.GetServiceModel(pPayload);
            return mdl;
        }
    }
}
