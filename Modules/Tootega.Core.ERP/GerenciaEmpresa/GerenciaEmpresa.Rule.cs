using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Tootega.Core.ERP.GerenciaEmpresa;

namespace Tootega.Core.ERP.GerenciaEmpresa.Rules
{
    public class GerenciaEmpresaRule : BaseGerenciaEmpresaRule
    {
        public GerenciaEmpresaRule(XService pService)
               :base(pService)
        {
        }
    }
}