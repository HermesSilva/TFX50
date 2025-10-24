using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Tootega.Core.ERP.Profissional.Rules
{
    public class INFHorariosServiceRule : HorariosService.BaseINFHorariosServiceRule
    {
        public INFHorariosServiceRule(HorariosService pService)
               :base(pService)
        {
        }
    }
}