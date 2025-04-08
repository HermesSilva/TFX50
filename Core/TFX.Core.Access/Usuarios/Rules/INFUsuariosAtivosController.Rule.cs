using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace TFX.Core.Access.Usuarios.Rules
{
    public class INFUsuariosAtivosControllerRule : UsuariosAtivosController.BaseINFUsuariosAtivosControllerRule
    {
        public INFUsuariosAtivosControllerRule(UsuariosAtivosController pController)
               :base(pController)
        {
        }
    }
}