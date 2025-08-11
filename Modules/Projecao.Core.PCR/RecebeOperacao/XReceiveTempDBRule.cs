using System;
using System.IO;
using System.Net.Sockets;

using Projecao.Core.NTR.Jobs;

using TFX.Core;
using TFX.Core.Jobs;
using TFX.Core.Model.HPC;
using TFX.Core.Reflections;
using TFX.Core.Service.Cache;

namespace Projecao.Core.PCR.RecebeOperacao
{
    [XRegister(typeof(XReceiveTempDBRule), sCID)]
    public class XReceiveTempDBRule : XPackageRuleHPCRule
    {
        public const String sCID = "FE852887-BED6-4A1A-91AA-E5DBEE008F3D";
        public static Guid gCID = new Guid(sCID);

        public override void ExecuteServerSide(XHPCPackage pPackage)
        {
            HPCIntegracaoSVC.XCFGTuple cfg = XConfigCache.Get<HPCIntegracaoSVC.XCFGTuple>();
            if (cfg == null)
            {
                Socket.SendPack(new XHPCThrowMessage("Serviço de Envido de dados não configurado"), SessionID, UserID, null);
                return;
            }
            if (pPackage is XPackage)
                RestartReceive((XPackage)pPackage, cfg);
            if (pPackage is XPackagePart)
                XPackage.SavePart(Socket, Path.Combine(cfg.PastaTemoraria, "Recebido"), (XPackagePart)pPackage, pPackage.UserID, SessionID, pProgress: OnProgress);
        }

        private void OnProgress(Int32 pSize, Int32 pProgress, Boolean pIsFinished, Boolean pSuccess)
        {
            if (pIsFinished)
            {
                XConsole.Warn("Base de dados recebida.");
                XThreadPool.Execute(() => XJobRunner.StartJob(HPCIntegracaoSVC.gCID));
            }
        }

        public override void ExecuteClientSide(XHPCPackage pPackage)
        {
        }

        private void RestartReceive(XPackage pPackage, HPCIntegracaoSVC.XCFGTuple pConfig)
        {
            String tmppath = Path.Combine(pConfig.PastaTemoraria, "Recebido");
            if (!Directory.Exists(tmppath))
                Directory.CreateDirectory(tmppath);
            String hdfile = Path.Combine(tmppath, pPackage.PackageID.AsString() + ".txt");
            pPackage.PartList.FileName = pPackage.PackageID.AsString() + "-" + pPackage.PartList.FileName;
            File.WriteAllText(hdfile, pPackage.PartList.ToString());
            Socket.SendIsOk(SessionID, pPackage.UserID);
        }
    }
}