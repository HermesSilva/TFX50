using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Tootega.Core.CEP.ReadOnly;

namespace Tootega.Core.CEP.ReadOnly.Rules
{
    public class LogradouroRule : BaseLogradouroRule
    {
        public LogradouroRule(XService pService)
               :base(pService)
        {
        }
    }
}