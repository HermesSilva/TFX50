using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Tootega.Core.ERP.Pessoa;

namespace Tootega.Core.ERP.Pessoa.Rules
{
    public class BuscaPessoaRule : BaseBuscaPessoaRule
    {
        public BuscaPessoaRule(XService pService)
               :base(pService)
        {
        }
    }
}