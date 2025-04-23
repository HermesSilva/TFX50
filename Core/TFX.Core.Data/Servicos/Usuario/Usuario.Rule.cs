using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using TFX.Core.Data.Servicos.Usuario;

namespace TFX.Core.Data.Servicos.Usuario.Rules
{
    public class UsuarioRule : BaseUsuarioRule
    {
        public UsuarioRule(XService pService)
               :base(pService)
        {
        }
    }
}