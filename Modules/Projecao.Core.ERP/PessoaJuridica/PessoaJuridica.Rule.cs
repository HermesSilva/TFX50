using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.ERP.PessoaJuridica;

namespace Projecao.Core.ERP.PessoaJuridica.Rules
{
    public class PessoaJuridicaRule : BasePessoaJuridicaRule
    {
        public PessoaJuridicaRule(XService pService)
               :base(pService)
        {
        }
    }
}