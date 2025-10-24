using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Tootega.Core.ERP.PessoaJuridica;

namespace Tootega.Core.ERP.PessoaJuridica.Rules
{
    public class PessoaJuridicaRule : BasePessoaJuridicaRule
    {
        public PessoaJuridicaRule(XService pService)
               :base(pService)
        {
        }
    }
}