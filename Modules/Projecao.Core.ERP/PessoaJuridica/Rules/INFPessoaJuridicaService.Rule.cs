using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Projecao.Core.ERP.PessoaJuridica.Rules
{
    public class INFPessoaJuridicaServiceRule : PessoaJuridicaService.BaseINFPessoaJuridicaServiceRule
    {
        public INFPessoaJuridicaServiceRule(PessoaJuridicaService pService)
               :base(pService)
        {
        }
    }
}