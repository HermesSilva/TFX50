using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace TFX.ESC.Core.Escritori.Rules
{
    public class INFEscritorioControllerRule : EscritorioController.BaseINFEscritorioControllerRule
    {
        public INFEscritorioControllerRule(EscritorioController pController)
               :base(pController)
        {
        }
    }
}