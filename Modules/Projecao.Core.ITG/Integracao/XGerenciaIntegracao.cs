using System;
using System.Linq;


using Projecao.Core.ERP.GerenciaEmpresa;
using Projecao.Core.ITG.Jobs;
using Projecao.Core.LGC.DB;

using TFX.Core;
using TFX.Core.Exceptions;
using TFX.Core.Model.Cache;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC.SVC;
using TFX.Core.Service.Cache;
using TFX.Core.Service.Data;

namespace Projecao.Core.Integracao.Rule.Integracao
{
    public class XGerenciaIntegracao : XLegacyManager
    {
        private static XILog _Log = XLog.GetLogFor(typeof(XGerenciaIntegracao));
        private static Boolean _Executed = false;
        public const String FAT_CADASTROS = "FAT_CADASTROS";
        public const String FAT_ITEMPEDIDO = "FAT_ITEMPEDIDO";
        public const String FAT_CAPAPEDIDO = "FAT_CAPAPEDIDO";
        public const String FAT_PRODUTOS = "FAT_PRODUTOS";
        public const String FAT_FUNCIONARIOS = "FAT_FUNCIONARIOS";
        public const String FAT_CIDADES = "FAT_CIDADES";
        public const String FAT_ESTADOS = "FAT_ESTADOS";
        public const String FAT_GRUPOPROD = "FAT_GRUPOPROD";
        public const String FAT_LINHA = "FAT_LINHA";
        public const String FAT_MARCAS = "FAT_MARCAS";
        public const String FAT_PRODUTOS_CAMPOS_EMPRESAS = "FAT_PRODUTOS_CAMPOS_EMPRESAS";
        public const String FAT_ESTOQUE = "FAT_ESTOQUE";
        public const String FAT_ESTOQUELOTE = "FAT_ESTOQUELOTE";
        public const String FAT_PROMOCAO = "FAT_PROMOCAO";
        public const String FAT_PROMOCAOITEM = "FAT_PROMOCAOITEM";
        public const String FAT_SERVICOMENSAGEM = "FAT_SERVICOMENSAGEM";
        public const String ITG_DELECAO = "ITG_DELECAO";

        public static String[] KnowTables = new[] { FAT_CADASTROS, FAT_CAPAPEDIDO, FAT_PRODUTOS, FAT_FUNCIONARIOS, FAT_ITEMPEDIDO, FAT_CIDADES, FAT_ESTADOS,
                                                    FAT_GRUPOPROD, FAT_LINHA, FAT_MARCAS, FAT_PRODUTOS_CAMPOS_EMPRESAS, FAT_ESTOQUE, FAT_ESTOQUELOTE, FAT_PROMOCAO,
                                                    FAT_PROMOCAOITEM, ITG_DELECAO, FAT_SERVICOMENSAGEM };
        public static IntegracaoLegadoSVC.XCFGTuple Config;

        public static Boolean IsReverse
        {
            get
            {
                if (Config == null)
                    Config = XConfigCache.GetByResource<IntegracaoLegadoSVC.XCFGTuple>(IntegracaoLegadoSVC.gCID);
                if (XDefault.IsServerSide)
                {
                    ConfigITGFRM.XCFGTuple cfg = XConfigCache.Get<ConfigITGFRM.XCFGTuple>(ConfigITGFRM.gCID, XModelCache.Instance.KEY[0].ID);
                    return cfg?.IntegracaoReversa == true;
                }

                return Config?.PastaIntegracao.IsFull() == true;
            }
        }

        public static bool IsClientReverse
        {
            get;
            set;
        }

        public static XDBConnection GetConfig()
        {
            Config = XConfigCache.GetByResource<IntegracaoLegadoSVC.XCFGTuple>(IntegracaoLegadoSVC.gCID);
            if (Config == null)
                return null;
            XDBConnection cnn = new XDBConnection();

            cnn.Catalog = Config.Usuario;
            cnn.Vendor = XDBVendor.Oracle;
            cnn.Host = Config.Host;
            cnn.Password = Config.Senha;
            cnn.PoolCount = 5;
            cnn.Timeout = 1000;
            cnn.User = Config.Usuario;
            cnn.Port = Config.Porta;
            cnn.Instance = Config.Instancia;
            cnn.NoCheck = true;
            _Log.Warn($"LEGADO Connection [{cnn.ToString()}]");
            return cnn;
        }

        public static void Execute(XExecContext pTarget)
        {
            XDBConnection cnn = XGerenciaIntegracao.GetConfig();
            if (cnn == null)
                throw new XUnconformity("Serviço de integração não configurado.");
            if (XDefault.TestRunning && !_Executed)
            {
                //if (cnn.Host != "VMCI")
                //    throw new XError("Não é permitido rodar intregrção para teste fora do Servidor dase.com.br");
                using (XExecContext ctx = XExecContext.Create(cnn))
                {
                    ctx.DataBase.ExecSQL("call BCO_FUNCAO('GESTCOM')");
                    foreach (XBaseIntegracaoTabela it in ITGTables.OrderBy(i => i.Order))
                        if (ctx.DataBase.TableExists(it.RemoteControlTable, null))
                            ctx.DataBase.ExecSQL($"truncate table {it.RemoteControlTable}");
                    ctx.Commit();
                    _Executed = true;
                }
            }
            using (XExecContext ctx = XExecContext.Create(cnn))
            {
                ctx.DataBase.ExecSQL("call BCO_FUNCAO('GESTCOM')");
                if (XDefault.TestRunning)
                {
                    //if (cnn.Host != "VMCI")
                    //    throw new XError("Não é permitido rodar intregrção para teste fora do Servidor dase.com.br");
                    foreach (XBaseIntegracaoTabela it in ITGTables.OrderBy(i => i.Order))
                        if (ctx.SessionDataBase.TableExists(it.RemoteControlTable, null))
                            ctx.DataBase.ExecSQL($"truncate table {ctx.DataBase.Factory.DBObjectName(it.RemoteControlTable)}");
                }

                if (!XLegacyManager.IsInitialize)
                    LoadRemoteModel(ctx, KnowTables);
                foreach (XBaseIntegracaoTabela it in ITGTables.OrderBy(i => i.Order))
                {
                    if (it.MigrationLevel != XMigrationLevel.ServerToServer || it.LegacySide != XLegacySide.Cloud)
                        continue;
                    try
                    {
                        XConsole.Warn($"Start Execute [{it.RemoteTable}].");
                        it.Prepare(ctx, pTarget);
                        it.Execute(ctx);
                    }
                    finally
                    {
                        it.Prepare(null, null);
                    }
                }
                ctx.Commit();
            }
        }
    }
}