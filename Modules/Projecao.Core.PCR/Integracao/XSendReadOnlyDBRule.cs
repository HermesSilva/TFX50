using System;
using System.IO;
using System.Net.Sockets;

using Projecao.Core.NTR.Jobs;

using TFX.Core;
using TFX.Core.Exceptions;
using TFX.Core.Model.HPC;
using TFX.Core.Reflections;
using TFX.Core.Service.Cache;

namespace Projecao.Core.PCR.Integracao
{
    [XRegister(typeof(XSendReadOnlyDBRule), sCID)]
    public class XSendReadOnlyDBRule : XPackageRuleHPCRule
    {
        public const String sCID = "42227DAC-3889-4AF9-BCEF-FE1B0A7E865C";
        public static Guid gCID = new Guid(sCID);
        public static Guid ReceiveRuleID = new Guid("349CE368-EAEB-4819-897B-A689376BC873");

        public override void ExecuteServerSide(XHPCPackage pPackage)
        {
            if (pPackage is XStartGetFile)
                SendData(Socket, SessionID, pPackage.UserID, ((XStartGetFile)pPackage).CRC);
        }

        public static void SendData(Socket pSocket, Guid pSessionID, Guid pUserID, Int32 pCRC)
        {
            if (ImportaDados.LastCRC != null && ImportaDados.LastCRC == pCRC)
            {
                pSocket?.SendIsOk(pSessionID, pUserID);
                return;
            }
            HPCIntegracaoSVC.XCFGTuple cfg = XConfigCache.Get<HPCIntegracaoSVC.XCFGTuple>();
            if (cfg == null)
            {
                pSocket?.SendPack(new XHPCThrowMessage("Serviço de Envido de dados não configurado"), pSessionID, Guid.Empty, null);
                return;
            }

            String sqdb = $"{cfg.PastaTemoraria}\\Mobile.db";
            String file = Path.Combine(XDefault.TempFolder, sqdb);
            if (!File.Exists(file))
            {
                pSocket?.SendPack(new XHPCThrowMessage("Arquivo de dados não preparado"), pSessionID, Guid.Empty, null);
                return;
            }
            String hdfile = Path.Combine(XDefault.TempFolder, pUserID.AsString() + ".txt");
            XPackage bpkg = new XPackage();
            bpkg.RuleID = Guid.Empty;
            bpkg.UserID = pUserID;
            bpkg.CreateHeader(ReceiveRuleID, file);

            using (XHPCWaiter<XIsOkPackage> wt = new XHPCWaiter<XIsOkPackage>(pSessionID, XSyncReader.TimeOut))
            {
                pSocket.SendPack(bpkg, pSessionID, pUserID);
                XIsOkPackage pck = wt.Wait();
                if (pck?.IsOk == false)
                    throw new XError("Falha ao enviar base de dados");
            }
            XPackage.SendParts(pSocket, pSessionID, pUserID, bpkg.PartList);
        }
    }
}