using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.CEP.Localidade;

namespace Projecao.Core.CEP.Localidade.Rules
{
    public class LocalidadeRule : BaseLocalidadeRule
    {
        public LocalidadeRule(XService pService)
               :base(pService)
        {
        }
    }
}