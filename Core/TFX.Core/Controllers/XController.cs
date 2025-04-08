using System;
using System.ComponentModel;
using System.Diagnostics;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

using TFX.Core.Authorize;
using TFX.Core.Exceptions;

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
			Console.WriteLine($"Ellapsed {stp?.Elapsed.TotalMilliseconds.ToString("#,##0.0")}ms {pContext.HttpContext.Request.Path}");
		}
	}

	[XStopwatch]
	[XAuthorizeFilter]
	public abstract class XController : ControllerBase
	{

		public XController(ILogger<XController> pLogger)
		{
			Log = pLogger;
		}

		protected readonly ILogger<XController> Log;
	}
}
