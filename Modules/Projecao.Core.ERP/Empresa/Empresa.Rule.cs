using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.ERP.Empresa;

namespace Projecao.Core.ERP.Empresa.Rules
{
    public class EmpresaRule : BaseEmpresaRule
    {
        public EmpresaRule(XService pService)
               :base(pService)
        {
        }
    }
}