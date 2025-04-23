using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using TFX.Core.Data.Servicos.Menu;

namespace TFX.Core.Data.Servicos.Menu.Rules
{
    public class MenuRule : BaseMenuRule
    {
        public MenuRule(XService pService)
               :base(pService)
        {
        }
    }
}