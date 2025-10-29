using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Tootega.Core.CEP.Logradouro.Rules
{
    public class INFLogradouroControllerRule : LogradouroController.BaseINFLogradouroControllerRule
    {
        public INFLogradouroControllerRule(LogradouroController pController)
               :base(pController)
        {
        }
    }
}