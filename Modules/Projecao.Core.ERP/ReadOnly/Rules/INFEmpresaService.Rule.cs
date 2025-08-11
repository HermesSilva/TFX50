using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Projecao.Core.ERP.ReadOnly.Rules
{
    public class INFEmpresaServiceRule : EmpresaService.BaseINFEmpresaServiceRule
    {
        public INFEmpresaServiceRule(EmpresaService pService)
               :base(pService)
        {
        }
    }
}