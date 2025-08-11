using System;
using System.IO;
using System.Linq;

using TFX.Core;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.DB;
using TFX.Core.Service.DB.Model;

using static Projecao.Core.CEP.DB.CEPx;

namespace Projecao.Core.CEP.DataPack
{
    [XRegister(typeof(CEPxDataPack), "2B0D98A3-034C-4428-B674-AFD617A9941E")]
    [XModule("86C8282F-31CF-419F-99DF-18D84C561EC7")]
    public class CEPxDataPack : XIScriptExecutorEX
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
            if (XDefault.IsDebugTime)
                return;
            Stream st = XResources.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxUF.txt");
            CEPxUF.Instance.UpdateFromCSV(st, new[] { "CEPFinal", "CEPInicial" });
            st = XResources.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxLocalidade.txt");
            CEPxLocalidade.Instance.LoadFromCSV(st);
            //if (XDefault.Verbose)
            XConsole.Debug($"CEPxLocalidade StaticData=[{CEPxLocalidade.Instance.StaticData.Count}]");
        }

        public void UploadDefaultData(XExecContext pContext, Boolean pIsMasterDB)
        {
            if (XDefault.IgnorePack)
                return;
            if ((XDefault.IsNewDB && !XDefault.IsDebugTime) || XDefault.ForceDefaultData)
            {
                LoadDefault(pContext, true);
            }
        }

        private static void LoadDefault(XExecContext pContext, Boolean pUpload)
        {
            if (!XEnvironment.NewDB)
            {
                AddLograd(pContext);
                return;
            }
            if (pContext.DataBase.Factory.DBVendor.In(XDBVendor.MSSQLLocalDB, XDBVendor.MSSQLServer))
            {
                using (XDBTable bairro = (XDBTable)XPersistencePool.Get<_CEPxBairro>(pContext))
                {
                    bairro.ReadFrom(XResources.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxBairro.txt"), pSeparator: "@");
                }
                using (XDBTable bairro = (XDBTable)XPersistencePool.Get<_CEPxLogradouro>(pContext))
                {
                    bairro.ReadFrom(XResources.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxLogradouro.txt"), pSeparator: "@");
                }
            }
            else
            {
                Stream st;

                if (CEPxBairro.Instance.StaticData.Count == 1)
                {
                    st = XResources.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxBairro.txt");
                    CEPxBairro.Instance.LoadFromCSV(st);
                }

                if (pUpload)
                    XScriptExecutor.WriteDefaultData(pContext, CEPxBairro.Instance);
                if (XDefault.Verbosity > 1)
                    XConsole.Debug($"CEPxBairro StaticData=[{CEPxBairro.Instance.StaticData.Count}]");
                if (Environment.Is64BitOperatingSystem)
                {
                    if (CEPxLogradouro.Instance.StaticData.Count == 1)
                    {
                        st = XResources.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxLogradouro.txt");
                        CEPxLogradouro.Instance.LoadFromCSV(st);
                    }

                    if (pUpload)
                        XScriptExecutor.WriteDefaultData(pContext, CEPxLogradouro.Instance);
                    if (XDefault.Verbosity > 1)
                        XConsole.Debug($"CEPxLogradouro StaticData=[{CEPxLogradouro.Instance.StaticData.Count}]");
                }
            }
            AddLograd(pContext);
        }

        private static void AddLograd(XExecContext pContext)
        {
            using (_CEPxLocalidade loc = XPersistencePool.Get<_CEPxLocalidade>(pContext))
            using (_CEPxLogradouro log = XPersistencePool.Get<_CEPxLogradouro>(pContext))
            using (_CEPxBairro bai = XPersistencePool.Get<_CEPxBairro>(pContext))
            {
                loc.MaxRows = 0;
                loc.FilterActives = true;
                loc.Open();
                XConsole.Debug($"GERANDO LOGRADOUROS INEXISTENTES=[{loc.Count}]");

                Int32 cnt = 0;
                foreach (CEPxLocalidade.XTuple loctpl in loc)
                {
                    if (log.Open(CEPxLogradouro.CEPxLocalidadeID, loctpl.CEPxLocalidadeID))
                        continue;
                    CEPxBairro.XTuple btpl;
                    if (!bai.Open(CEPxBairro.CEPxLocalidadeID, loctpl.CEPxLocalidadeID))
                    {
                        btpl = bai.NewTuple();
                        bai.Current.CEPxLocalidadeID = loctpl.CEPxLocalidadeID;
                        bai.Current.Nome = "Centro";
                        bai.Flush();
                        cnt++;
                    }
                    else
                        btpl = bai.Tuples.FirstOrDefault();
                    log.NewTuple();
                    log.Current.CEPxLocalidadeID = loctpl.CEPxLocalidadeID;
                    log.Current.Nome = "Setor Central";
                    log.Current.CEP = loctpl.CEPGeral;
                    log.Current.Tipo = "Setor";
                    log.Current.CEPxBairroID = btpl.CEPxBairroID;
                    log.Flush();
                }
                XConsole.Debug($"LOGRADOUROS INEXISTENTES gerados=[{cnt}]");
            }
        }
    }
}