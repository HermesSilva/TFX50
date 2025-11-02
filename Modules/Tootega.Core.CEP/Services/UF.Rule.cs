using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Tootega.Core.CEP.Services;

namespace Tootega.Core.CEP.Services.Rules
{
    public class UFRule : BaseUFRule
    {
        public UFRule(XService pService)
               :base(pService)
        {
        }
    }
}