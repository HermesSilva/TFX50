using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.ERP.AutomatedService;

namespace Projecao.Core.ERP.AutomatedService.Rules
{
    public class EMailEnvioRule : BaseEMailEnvioRule
    {
        public EMailEnvioRule(XService pService)
               :base(pService)
        {
        }
    }
}