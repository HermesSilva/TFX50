using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace TFX.Core.Data.Servicos.Rules
{
    public class INFUsuarioServiceRule : UsuarioService.BaseINFUsuarioServiceRule
    {
        public INFUsuarioServiceRule(UsuarioService pService)
               :base(pService)
        {
        }
    }
}