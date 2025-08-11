using System;

using Projecao.Core.ISE.DB;
using Projecao.Core.NTR.Jobs;
using Projecao.Core.PCR.DB;
using Projecao.Core.PCR.Integracao;

using TFX.Core;
using TFX.Core.Model.DIC;
using TFX.Core.Reflections;
using TFX.Core.Service.Cache;
using TFX.Core.Service.Data;
using TFX.Core.Service.DB;
using TFX.Core.Service.Apps;
using TFX.Core.Service.SVC;

namespace Projecao.Core.PCR.PessoaFisica
{
    [XRegister(typeof(AlmaCorePCRRule), sCID, typeof(ProjecaoCorePCR))]
    public class AlmaCorePCRRule : XModuleRule
    {
        public const String sCID = "9DF1DF17-6BF6-4E6E-847E-E8E80F431866";
        public static Guid gCID = new Guid(sCID);

        static AlmaCorePCRRule()
        {
            DataChanged = true;
        }

        public static Boolean DataChanged
        {
            get;
            set;
        }

        public AlmaCorePCRRule()
        {
            ID = gCID;
        }

        protected override void OnLoad()
        {
            XCustomerKey.OnLoadKey += KeyLoad;
            if (!XEnvironment.ModelUpdate)
                XThreadPool.DelayExecute(() => PreparaDados(), 5000);
            XTableChangeEvents.AddTable(PCRx.PCRxAnimal.Instance, TableChanged);
            XTableChangeEvents.AddTable(PCRx.PCRxAnimalFiliacao.Instance, TableChanged);
            XTableChangeEvents.AddTable(ISEx.ISExItem.Instance, TableChanged);
            XTableChangeEvents.AddTable(PCRx.PCRxReprodutor.Instance, TableChanged);
            XTableChangeEvents.AddTable(PCRx.PCRxAnimalLote.Instance, TableChanged);
            XTableChangeEvents.AddTable(PCRx.PCRxIATF.Instance, TableChanged);
            XTableChangeEvents.AddTable(PCRx.PCRxIATFFase.Instance, TableChanged);
            XTableChangeEvents.AddTable(PCRx.PCRxRetiro.Instance, TableChanged);
            XTableChangeEvents.AddTable(PCRx.PCRxPasto.Instance, TableChanged);
            XTableChangeEvents.AddTable(PCRx.PCRxFazenda.Instance, TableChanged);
            XTableChangeEvents.AddTable(SYSx.SYSxEmpresa.Instance, TableChanged);
            XTableChangeEvents.AddTable(CTLx.CTLxUsuario.Instance, TableChanged);
        }

        private void TableChanged(XDBTable pTable)
        {
            XActionManager.SetTimeOut(sCID.GetHashCode(), () => PreparaDados(), 10000);
        }

        private void PreparaDados()
        {
            HPCIntegracaoSVC.XCFGTuple cfg = XConfigCache.Get<HPCIntegracaoSVC.XCFGTuple>();
            if (cfg == null)
                return;
            ImportaDados prepa = new ImportaDados();
            using (XExecContext ctx = XExecContext.Create())
            {
                prepa.MigraDados(ctx, cfg);
            }
        }

        private void KeyLoad(XCustomerKey pKey)
        {
            PCRx.PCRxFazenda.XTuple pjtpl = PCRx.PCRxFazenda.Instance.StaticData.NewTuple<PCRx.PCRxFazenda.XTuple>(pKey.ID);
            pjtpl.SYSxEstadoID = SYSx.SYSxEstado.XDefault.Ativo;
        }
    }
}