using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Tootega.Core.CEP.Services.Rules
{
    public class INFUFServiceRule : UFService.BaseINFUFServiceRule
    {
        public INFUFServiceRule(UFService pService)
               :base(pService)
        {
        }
    }
}