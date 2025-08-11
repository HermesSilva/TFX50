using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Projecao.Core.ERP.PessoaFisica.Rules
{
    public class INFPessoaFisicaTipoServiceRule : PessoaFisicaTipoService.BaseINFPessoaFisicaTipoServiceRule
    {
        public INFPessoaFisicaTipoServiceRule(PessoaFisicaTipoService pService)
               :base(pService)
        {
        }
    }
}