using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.ERP.GerenciaEmpresa;

namespace Projecao.Core.ERP.GerenciaEmpresa.Rules
{
    public class GerenciaEmpresaRule : BaseGerenciaEmpresaRule
    {
        public GerenciaEmpresaRule(XService pService)
               :base(pService)
        {
        }
    }
}