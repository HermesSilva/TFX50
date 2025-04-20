using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using TFX.ESC.Core.Escritorio;

namespace TFX.ESC.Core.Escritorio.Rules
{
    public class EscritorioRule : BaseEscritorioRule
    {
        public EscritorioRule(XService pService)
               :base(pService)
        {
        }
    }
}