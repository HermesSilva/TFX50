using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Tootega.Core.ERP.Empresa.Rules
{
    public class INFEmpresaControllerRule : EmpresaController.BaseINFEmpresaControllerRule
    {
        public INFEmpresaControllerRule(EmpresaController pController)
               :base(pController)
        {
        }
    }
}