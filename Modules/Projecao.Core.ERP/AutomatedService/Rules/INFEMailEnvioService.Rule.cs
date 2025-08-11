using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Projecao.Core.ERP.AutomatedService.Rules
{
    public class INFEMailEnvioServiceRule : EMailEnvioService.BaseINFEMailEnvioServiceRule
    {
        public INFEMailEnvioServiceRule(EMailEnvioService pService)
               :base(pService)
        {
        }
    }
}