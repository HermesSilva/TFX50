using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.ERP.Profissional;

namespace Projecao.Core.ERP.Profissional.Rules
{
    public class CategoriaRule : BaseCategoriaRule
    {
        public CategoriaRule(XService pService)
               :base(pService)
        {
        }
    }
}