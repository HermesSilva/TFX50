using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Tootega.Core.ERP.Pessoa.Rules
{
    public class INFContatoServiceRule : ContatoService.BaseINFContatoServiceRule
    {
        public INFContatoServiceRule(ContatoService pService)
               :base(pService)
        {
        }
    }
}