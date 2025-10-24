using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Tootega.Core.ERP.ReadOnly;

namespace Tootega.Core.ERP.ReadOnly.Rules
{
    public class EmpresaRule : BaseEmpresaRule
    {
        public EmpresaRule(XService pService)
               :base(pService)
        {
        }
    }
}