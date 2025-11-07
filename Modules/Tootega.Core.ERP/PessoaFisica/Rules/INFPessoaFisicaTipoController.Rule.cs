using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Tootega.Core.ERP.PessoaFisica.Rules
{
    public class INFPessoaFisicaTipoControllerRule : PessoaFisicaTipoController.BaseINFPessoaFisicaTipoControllerRule
    {
        public INFPessoaFisicaTipoControllerRule(PessoaFisicaTipoController pController)
               :base(pController)
        {
        }
    }
}