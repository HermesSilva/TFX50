using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Projecao.Core.ERP.GerenciaEmpresa.Rules
{
    public class INFGerenciaEmpresaServiceRule : GerenciaEmpresaService.BaseINFGerenciaEmpresaServiceRule
    {
        public INFGerenciaEmpresaServiceRule(GerenciaEmpresaService pService)
               :base(pService)
        {
        }
    }
}