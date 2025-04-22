using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace TFX.Core.Access.UsuariosAtivos.Rules
{
    public class INFUsuariosAtivosServiceRule : UsuariosAtivosService.BaseINFUsuariosAtivosServiceRule
    {
        public INFUsuariosAtivosServiceRule(UsuariosAtivosService pService)
               :base(pService)
        {
        }
    }
}