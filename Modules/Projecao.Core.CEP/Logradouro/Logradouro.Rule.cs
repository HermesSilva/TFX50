using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.CEP.Logradouro;

namespace Projecao.Core.CEP.Logradouro.Rules
{
    public class LogradouroRule : BaseLogradouroRule
    {
        public LogradouroRule(XService pService)
               :base(pService)
        {
        }
    }
}