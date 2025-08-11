using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Projecao.Core.ERP.ReadOnly.Rules
{
    public class INFEmpresaConfigServiceRule : EmpresaConfigService.BaseINFEmpresaConfigServiceRule
    {
        public INFEmpresaConfigServiceRule(EmpresaConfigService pService)
               :base(pService)
        {
        }
    }
}