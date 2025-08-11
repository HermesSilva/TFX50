using System;
using System.IO;

using TFX.Core;
using TFX.Core.LZMA;
using TFX.Core.Reflections;
using TFX.Core.Service.Cache;
using TFX.Core.Service.Data;

using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.ERP.GerenciaEmpresa
{
    [XRegister(typeof(GerenciaEmpresaRule), sCID, typeof(GerenciaEmpresaSVC))]
    public class GerenciaEmpresaRule : GerenciaEmpresaSVC.XRule
    {
        public const String sCID = "08A3075B-6A6B-4051-8D49-CEA387F520CB";
        public static Guid gCID = new Guid(sCID);

        public GerenciaEmpresaRule()
        {
            ID = gCID;
        }

        protected override void Prepare()
        {
            Model.CheckTenant = false;
        }

        protected override void BeforeFlush(XExecContext pContext, GerenciaEmpresaSVC pModel, GerenciaEmpresaSVC.XDataSet pDataSet)
        {
            _SYSxEmpresaConfig empcfg = GetTable<_SYSxEmpresaConfig>();
            foreach (GerenciaEmpresaSVC.XTuple tpl in pDataSet)
            {
                if (!empcfg.Open(tpl.SYSxEmpresaID))
                    empcfg.NewTuple(tpl.SYSxEmpresaID);
                ConfigITGFRM.XCFGTuple cfg = new ConfigITGFRM.XCFGTuple();
                cfg.SetData(tpl.Configuracao);
                cfg.CompanyID = tpl.SYSxEmpresaID;
                cfg.ID = tpl.SYSxEmpresaID;
                empcfg.Current.Configuracao = cfg.GetBytes();
                String path = Path.Combine(XDefault.StaticFolder, "ConfigITG");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                File.WriteAllBytes(Path.Combine(path, $"{tpl.Numero.OnlyNumbers()}.txt"), XLzma.Encode(cfg.GetBytes()));
                empcfg.Flush();
            }
        }

        protected override void AfterCommit(XExecContext pContext)
        {
            XConfigCache.Reaload();
        }

        protected override void AfterGet(XExecContext pContext, GerenciaEmpresaSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            _SYSxEmpresaConfig empcfg = GetTable<_SYSxEmpresaConfig>();
            foreach (GerenciaEmpresaSVC.XTuple tpl in pDataSet)
            {
                tpl.FormID = ConfigITGFRM.gCID;
                if (empcfg.Open(tpl.SYSxEmpresaID))
                    tpl.Configuracao = empcfg.Current.Configuracao;
                tpl.IDEmpresa = tpl.SYSxEmpresaID.AsString();
            }
        }
    }
}