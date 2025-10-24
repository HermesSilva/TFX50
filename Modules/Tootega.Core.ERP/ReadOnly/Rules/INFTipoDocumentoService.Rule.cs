using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;

namespace Tootega.Core.ERP.ReadOnly.Rules
{
    public class INFTipoDocumentoServiceRule : TipoDocumentoService.BaseINFTipoDocumentoServiceRule
    {
        public INFTipoDocumentoServiceRule(TipoDocumentoService pService)
               :base(pService)
        {
        }
    }
}