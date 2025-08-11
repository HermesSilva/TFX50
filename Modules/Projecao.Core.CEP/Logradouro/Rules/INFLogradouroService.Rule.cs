using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Projecao.Core.CEP.Logradouro.Rules
{
    public class INFLogradouroServiceRule : LogradouroService.BaseINFLogradouroServiceRule
    {
        public INFLogradouroServiceRule(LogradouroService pService)
               :base(pService)
        {
        }
    }
}