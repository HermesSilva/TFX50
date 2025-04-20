using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace TFX.ESC.Core.Escritorio.Rules
{
    public class INFEscritorioServiceRule : EscritorioService.BaseINFEscritorioServiceRule
    {
        public INFEscritorioServiceRule(EscritorioService pService)
               :base(pService)
        {
        }
    }
}