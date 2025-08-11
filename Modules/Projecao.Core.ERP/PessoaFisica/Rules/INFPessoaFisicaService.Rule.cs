using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Projecao.Core.ERP.PessoaFisica.Rules
{
    public class INFPessoaFisicaServiceRule : PessoaFisicaService.BaseINFPessoaFisicaServiceRule
    {
        public INFPessoaFisicaServiceRule(PessoaFisicaService pService)
               :base(pService)
        {
        }
    }
}