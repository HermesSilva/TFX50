using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Projecao.Core.ERP.Profissional.Rules
{
    public class INFCategoriaServiceRule : CategoriaService.BaseINFCategoriaServiceRule
    {
        public INFCategoriaServiceRule(CategoriaService pService)
               :base(pService)
        {
        }
    }
}