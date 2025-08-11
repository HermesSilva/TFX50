using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.ERP.PessoaFisica;

namespace Projecao.Core.ERP.PessoaFisica.Rules
{
    public class PessoaFisicaTipoRule : BasePessoaFisicaTipoRule
    {
        public PessoaFisicaTipoRule(XService pService)
               :base(pService)
        {
        }
    }
}