using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using TFX.ESC.Core.Escritorios;

namespace TFX.ESC.Core.Escritorios.Rules
{
    public class EscritorioRule : BaseEscritorioRule
    {
        public EscritorioRule(XService pService)
               :base(pService)
        {
        }
    }
}