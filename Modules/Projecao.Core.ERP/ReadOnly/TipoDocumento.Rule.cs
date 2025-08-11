using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.ERP.ReadOnly;

namespace Projecao.Core.ERP.ReadOnly.Rules
{
    public class TipoDocumentoRule : BaseTipoDocumentoRule
    {
        public TipoDocumentoRule(XService pService)
               :base(pService)
        {
        }
    }
}