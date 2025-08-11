using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.ERP.ReadOnly;

namespace Projecao.Core.ERP.ReadOnly.Rules
{
    public class ProfissionalRule : BaseProfissionalRule
    {
        public ProfissionalRule(XService pService)
               :base(pService)
        {
        }
    }
}