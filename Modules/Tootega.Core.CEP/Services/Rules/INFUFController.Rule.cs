using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Tootega.Core.CEP.Services.Rules
{
    public class INFUFControllerRule : UFController.BaseINFUFControllerRule
    {
        public INFUFControllerRule(UFController pController)
               :base(pController)
        {
        }
    }
}