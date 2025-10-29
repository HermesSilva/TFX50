using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Tootega.Core.CEP.Localidade.Rules
{
    public class INFLocalidadeControllerRule : LocalidadeController.BaseINFLocalidadeControllerRule
    {
        public INFLocalidadeControllerRule(LocalidadeController pController)
               :base(pController)
        {
        }
    }
}