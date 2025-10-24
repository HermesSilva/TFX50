using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Tootega.Core.ERP.Profissional;

namespace Tootega.Core.ERP.Profissional.Rules
{
    public class HorariosRule : BaseHorariosRule
    {
        public HorariosRule(XService pService)
               :base(pService)
        {
        }
    }
}