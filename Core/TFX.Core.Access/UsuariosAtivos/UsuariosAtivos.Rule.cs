using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using TFX.Core.Access.UsuariosAtivos;

namespace TFX.Core.Access.UsuariosAtivos.Rules
{
    public class UsuariosAtivosRule : BaseUsuariosAtivosRule
    {
        public UsuariosAtivosRule(XService pService)
               :base(pService)
        {
        }
    }
}