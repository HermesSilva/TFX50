using Projecao.Core.NTR.Jobs;

using System;
using System.Collections.Generic;
using System.IO;

using TFX.Core;
using TFX.Core.Exceptions;
using TFX.Core.Reflections;
using TFX.Core.Service.Cache;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.RecebeOperacao
{
    [XRegister(typeof(IntegraRulex), sCID, typeof(HPCIntegracaoSVC))]
    public class IntegraRulex : HPCIntegracaoSVC.XRule
    {
        public const String sCID = "71639080-83A7-4336-A4ED-5A2B01961E4A";
        public static Guid gCID = new Guid(sCID);
    }

    [XRegister(typeof(IntegraRule), sCID, typeof(HPCIntegracaoSVC))]
    public class IntegraRule : HPCIntegracaoSVC.XRule
    {
        public const String sCID = "71639080-83A7-4336-A4ED-5A2B01961E40";
        public static Guid gCID = new Guid(sCID);
        private static XILog _Log = XLog.GetLogFor<IntegraRule>();

        public IntegraRule()
        {
            ID = gCID;
        }

        private static Boolean _Executando = false;

        protected override void Execute(XExecContext pContext)
        {
            Executa(pContext);
        }

        public static void Executa(XExecContext pContext)
        {
            HPCIntegracaoSVC.XCFGTuple cfg = XConfigCache.Get<HPCIntegracaoSVC.XCFGTuple>();
            if (cfg == null)
                throw new XError("Integrador de Dados, não foi configurado, importação de Log de operações não é possivel.");
            String pasta = Path.Combine(cfg.PastaTemoraria, "Recebido");

            if (_Executando || !Directory.Exists(pasta))
                return;

            try
            {
                _Executando = true;
                String[] files = Directory.GetFiles(pasta, "*.db");
                if (files.SafeLength() == 0)
                    return;
                IntegraAtividade.ProcessAtividade(pContext, new List<String>(files));
            }
            finally
            {
                _Executando = false;
            }
        }
    }
}