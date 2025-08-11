using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.CEP.ReadOnly;

namespace Projecao.Core.CEP.ReadOnly.Rules
{
    public class LogradouroRule : BaseLogradouroRule
    {
        public LogradouroRule(XService pService)
               :base(pService)
        {
        }
    }
}