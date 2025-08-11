using System;
using System.Linq;
using System.Text;

using Projecao.Core.PCR.IATF;

using TFX.Core;
using TFX.Core.Exceptions;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Model.DIC.SVC;
using TFX.Core.Reflections;
using TFX.Core.Service;
using TFX.Core.Service.Data;
using TFX.Core.Service.DB;
using TFX.Core.Service.DB.Model;
using TFX.Core.Service.Apps;
using TFX.Core.Service.SVC;

using static Projecao.Core.PCR.DB.PCRx;

namespace Projecao.Core.PCR
{

    [XRegister(typeof(XScriptExecutorPCR), "BCE18F54-1AE0-4C42-83E3-F7140DC70D34")]
    [XModule("02C4E7C5-1B4F-43DB-AC6C-CD36BBE428B7")]
    public class XScriptExecutorPCR : XIScriptExecutorEX
    {
        public void AfterChangeDDL(XExecContext pContext, Boolean pIsMasterDB)
        {

        }

        public void BeforeChangeDDL(XExecContext pContext, Boolean pIsMasterDB)
        {
        }

        public void DoCheck(XExecContext pContext, XMORMScriptBuilder pBuilder, XModelCache pReverseModel, Boolean pIsMasterDB)
        {
        }

        public void DoExecute(XExecContext pContext, Boolean pIsMasterDB)
        {
        }

        public void DoReverse(XExecContext pContext, Boolean pIsMasterDB)
        {
        }

        public void UploadDefaultData(XExecContext pContext, Boolean pIsMasterDB)
        {
            AddIATF(pContext);
        }

        private static void AddIATF(XExecContext pContext)
        {
            using (IATFSVC.XService iatf = XServicePool.Get<IATFSVC.XService>(pContext))
            {
                IATFSVC.XTuple iatpl = iatf.NewTuple(1);
                iatpl.Nome = "IATF Inicial";
                iatpl.SYSxEstadoID = 1;
                IATFFasesSVC.XDataSet iafsdst = iatf.DataSet.IATFFasesDataSet;
                Guid[] _FasePK = {new Guid("00000000-0000-0000-0000-000000000000"), new Guid("00CA3D69-3298-448A-B412-2AEA437F5971"), new Guid("435F1657-FC99-437E-A6CD-2C07996FE489"),
                                  new Guid("9EEEE923-68FF-491C-97AD-2F6F60946564"), new Guid("CA906A77-4F8C-4165-ACA2-3EAB92F42881"), new Guid("D33685E8-0370-48BB-A1F2-4435BF949DF4"),
                                  new Guid("DD2DE115-68BB-4C4E-8B49-477634D33841"), new Guid("384092CC-F50C-41F5-88DD-484D9D5C8CE2"), new Guid("7E4A1FA0-DEB6-4129-9DBB-543315EA5BE7"),
                                  new Guid("2832BDAB-8FC9-455E-BF7D-5AAE09165BA7"), new Guid("2A0BC9B7-44D1-49C3-A106-5C73259B1471"), new Guid("774BF768-B666-441F-9368-685077AB3660"),
                                  new Guid("0819E2DA-0FE9-49C8-8477-76A2695A165F"), new Guid("AC89A051-A74D-42AC-A991-860C9CE2E586"), new Guid("1CC8C769-C112-4316-9270-902E529935CD"),
                                  new Guid("A9CF743B-434A-40EA-B869-91D3BF4E1638"), new Guid("A2AE2BA6-9F56-4471-8B2D-9485583C9C0F"), new Guid("A828C117-41FC-4CFA-8510-9D0D26CC520F"),
                                  new Guid("6151CD75-E491-4EA4-9A20-9F7194FCBFB9"), new Guid("8799CD2D-03E2-4218-96A8-A90B47EAD08F"), new Guid("CA1602D0-6A28-42E2-A4A9-AB4ABBDF27D4"),
                                  new Guid("AA750983-6792-4C50-B698-D045ECA84FC8"), new Guid("EE0A47A8-BF07-4F5A-BDA2-D3117772BBF1"), new Guid("29B251CB-4DEA-429C-937E-D63B85959783"),
                                  new Guid("26C8F2DF-4271-4C8D-B8FE-D6807191B810"), new Guid("4065A9D5-FCE1-4544-8F77-E5D803D10C80"), new Guid("E393FC37-8030-41DE-BEA0-E9D311A389D1")};

                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Implante_Dispositivo, 0, 1, _FasePK[1]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Intervalo, 9, 2, _FasePK[2]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Retirada_do_Implante, 0, 3, _FasePK[3]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Aplicacao_de_Hormonio, 0, 4, _FasePK[4]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Intervalo, 2, 5, _FasePK[5]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.IATF1, 0, 6, _FasePK[6]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Intervalo, 28, 7, _FasePK[7]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.DG1, 0, 8, _FasePK[8]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Implante_Dispositivo, 0, 9, _FasePK[9]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Aplicacao_de_Hormonio, 0, 10, _FasePK[10]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Intervalo, 9, 11, _FasePK[11]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Retirada_do_Implante, 0, 12, _FasePK[12]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Aplicacao_de_Hormonio, 0, 13, _FasePK[13]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Intervalo, 2, 14, _FasePK[14]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.IATF2, 0, 15, _FasePK[15]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Intervalo, 28, 16, _FasePK[16]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.DG2, 0, 17, _FasePK[17]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Implante_Dispositivo, 0, 18, _FasePK[18]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Aplicacao_de_Hormonio, 0, 19, _FasePK[19]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Intervalo, 9, 20, _FasePK[20]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Retirada_do_Implante, 0, 21, _FasePK[21]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Aplicacao_de_Hormonio, 0, 22, _FasePK[22]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Intervalo, 2, 23, _FasePK[23]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.IATF3, 0, 24, _FasePK[24]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.Intervalo, 28, 25, _FasePK[25]);
                AddFase(iatpl.PCRxIATFID, iafsdst, PCRxIATFFaseTipo.XDefault.DG3, 0, 26, _FasePK[26]);

                iatf.Flush();
            }
        }
        private static void AddFase(Int32 pIATFPK, IATFFasesSVC.XDataSet pFases, Int16 pTipo, Int32 pDur, Int32 pOrdem, Guid pFasePK)
        {
            IATFFasesSVC.XTuple iafstpl = pFases.NewTuple(pFasePK);
            iafstpl.PCRxIATFFaseTipoID = pTipo;
            iafstpl.Duracao = pDur;
            iafstpl.PCRxIATFID = pIATFPK;
            iafstpl.Ordem = (Int16)pOrdem;
            iafstpl.SYSxEstadoID = (Int16)1;
        }

    }
}