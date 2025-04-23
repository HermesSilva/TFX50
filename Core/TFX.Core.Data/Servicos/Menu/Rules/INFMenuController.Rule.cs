using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace TFX.Core.Data.Servicos.Menu.Rules
{
    public class INFMenuControllerRule : MenuController.BaseINFMenuControllerRule
    {
        public INFMenuControllerRule(MenuController pController)
               :base(pController)
        {
        }
    }
}