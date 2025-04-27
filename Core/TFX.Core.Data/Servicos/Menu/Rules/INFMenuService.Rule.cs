using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace TFX.Core.Data.Servicos.Menu.Rules
{
    public class INFMenuServiceRule : MenuService.BaseINFMenuServiceRule
    {
        public INFMenuServiceRule(MenuService pService)
               :base(pService)
        {
        }

        public override ResultSet AppModel(AppData pData)
        {
            return base.AppModel(pData);
        }
    }
}