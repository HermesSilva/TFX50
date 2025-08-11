using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Projecao.Core.CEP.Localidade.Rules
{
    public class INFLocalidadeServiceRule : LocalidadeService.BaseINFLocalidadeServiceRule
    {
        public INFLocalidadeServiceRule(LocalidadeService pService)
               :base(pService)
        {
        }
    }
}