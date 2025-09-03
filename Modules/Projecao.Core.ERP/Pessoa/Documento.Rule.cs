using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TFX.Core.Controllers;
using TFX.Core.Services;
using Projecao.Core.ERP.Pessoa;

namespace Projecao.Core.ERP.Pessoa.Rules
{
    public class DocumentoRule : BaseDocumentoRule
    {
        public DocumentoRule(XService pService)
               :base(pService)
        {
        }
    }
}