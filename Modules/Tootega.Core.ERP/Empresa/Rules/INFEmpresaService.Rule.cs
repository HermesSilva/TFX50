using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Tootega.Core.ERP.Empresa.Rules
{
    public class INFEmpresaServiceRule : EmpresaService.BaseINFEmpresaServiceRule
    {
        public INFEmpresaServiceRule(EmpresaService pService)
               :base(pService)
        {
        }
    }
}