using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Projecao.Core.ERP.Profissional.Rules
{
    public class INFProfissionalServiceRule : ProfissionalService.BaseINFProfissionalServiceRule
    {
        public INFProfissionalServiceRule(ProfissionalService pService)
               :base(pService)
        {
        }
    }
}