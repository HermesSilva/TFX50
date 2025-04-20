using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using TFX.Core.Authorize;
using TFX.Core.Cache;
using TFX.Core.Exceptions;
using TFX.Core.IDs;
using TFX.Core.IDs.Model;
using TFX.Core.Interfaces;

namespace TFX.Core.Controllers
{
    public enum XMLStatus
    {
        /// <summary>
        /// Documento validado e recebido para ser inserido na base de dados.
        /// </summary>
        [Description("Documento Aceito.")]
        Aceito = 1,

        /// <summary>
        /// A chamada da API entregou um documento vazio.
        /// </summary>
        [Description("Documento Vazio.")]
        DocumentoVazio = 2,

        /// <summary>
        /// O documento recebido é inválido para os tipos esperados
        /// </summary>
        [Description("O documento recebido não um XML é válido.")]
        DocumentoInvalido = 3,

        /// <summary>
        /// O JSon enviado é inválido
        /// </summary>
        [Description("JSon inválido.")]
        JSonInvalido = 4,

        /// <summary>
        /// Foi excedido o limite de chamdas por minuto que deve ficar, no máximo em 100 por minuto.
        /// Aguarde um (1) minuto para voltar a enviar documento.
        /// </summary>
        [Description("A quantidade de requisições é maior que a permitida em um determinado intervalo, veja documentação para não exceder")]
        MuitasRequisicoes = 5,

        ///// <summary>
        ///// A Assinatura do XML é inválida
        ///// </summary>
        //[Description("A Assinatura do Documento não é valida.")]
        //AssinaturaInvalida = 6,

        /// <summary>
        /// Houve um erro não previsto.
        /// </summary>
        [Description("Houve um erro não previsto.")]
        FalhaInprevista = 7,
    }

    public class XMLResponse
    {
        public static readonly XMLResponse IsOk = new XMLResponse { Status = XMLStatus.Aceito, Message = "Documento Aceito." };
        public static readonly XMLResponse Empty = new XMLResponse { Status = XMLStatus.DocumentoVazio, Message = "Documento Vazio." };
        public static XMLResponse BadFormat = new XMLResponse { Status = XMLStatus.DocumentoInvalido, Message = "O documento recebido não é um XML válido." };
        public static XMLResponse BadJSon = new XMLResponse { Status = XMLStatus.JSonInvalido, Message = "JSon inválido." };
        public static XMLResponse TooManyRequest = new XMLResponse { Status = XMLStatus.MuitasRequisicoes, Message = "A quantidade de requisições é maior que a permitida em um determinado intervalo, veja documentação para não exceder." };
        //public static XMLResponse InvalidSign = new XMLResponse { Status = XMLStatus.AssinaturaInvalida, Message = "A Assinatura do Documento não é valida." };


        /// <summary>
        /// Estado do recebimento, se verdadeiro (true), documento aceito.
        /// </summary>
        public XMLStatus Status
        {
            get; set;
        }

        /// <summary>
        /// Mesagem do estado do documento recebido.
        /// </summary>
        public string Message
        {
            get; set;
        }
    }

    public class XContextManagerAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext pContext)
        {
            var ctlr = pContext.Controller;
            if (ctlr is XBaseController cancl)
            {
                cancl.SetContextData(pContext.HttpContext.RequestAborted, null);

                var fields = ctlr.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance);
                foreach (var field in fields.Where(f => typeof(XIUseContext).IsAssignableFrom(f.FieldType)))
                {
                    var obj = field.GetValue(ctlr);
                    if (obj is XIUseContext cancelable)
                        cancelable.SetContextData(pContext.HttpContext.RequestAborted, cancl.Session);
                }
            }
        }

        public override void OnResultExecuted(ResultExecutedContext pContext)
        {
        }
    }

    public class XStopwatchAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext pContext)
        {
            Stopwatch stp = new Stopwatch();
            pContext.HttpContext.Items["Stopwatch"] = stp;
            stp.Start();
        }

        public override void OnResultExecuted(ResultExecutedContext pContext)
        {
            Stopwatch stp = pContext.HttpContext.Items["Stopwatch"] as Stopwatch;
            stp?.Stop();
            XConsole.Warn($"Ellapsed {stp?.Elapsed.TotalMilliseconds.ToString("#,##0.0")}ms {pContext.HttpContext.Request.Path}");
        }
    }

    [XStopwatch]
    [XContextManager]
    public abstract class XBaseController : ControllerBase, XIUseContext
    {

        public XBaseController(ILogger<XBaseController> pLogger)
        {
            Log = pLogger;
        }

        protected readonly ILogger<XBaseController> Log;
        private XUserSession _Session;
        public XUserSession Session => _Session;

        protected CancellationToken CancellationToken
        {
            get;
            private set;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public void SetContextData(CancellationToken pCancellationToken, XUserSession pSession)
        {
            CancellationToken = pCancellationToken;
            _Session = XSessionCache.GetSession(Guid.Empty);
        }
    }
}
