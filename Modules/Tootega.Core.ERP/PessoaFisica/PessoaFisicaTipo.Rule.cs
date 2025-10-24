using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Tootega.Core.ERP.PessoaFisica;

namespace Tootega.Core.ERP.PessoaFisica.Rules
{
    public class PessoaFisicaTipoRule : BasePessoaFisicaTipoRule
    {
        public PessoaFisicaTipoRule(XService pService)
               :base(pService)
        {
        }
    }
}