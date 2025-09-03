using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.ERP.Pessoa;

namespace Projecao.Core.ERP.Pessoa.Rules
{
    public class ContatoRule : BaseContatoRule
    {
        public ContatoRule(XService pService)
               :base(pService)
        {
        }
    }
}