using System;
using System.ComponentModel;
using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using TFX.Core.Authorize;
using TFX.Core.Exceptions;

namespace TFX.Core.Controllers
{


    //[XAuthorizeFilter]
    public abstract class XController : XBaseController
    {

    }
}
