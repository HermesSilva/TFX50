using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

using iText.Commons.Actions.Contexts;

using Newtonsoft.Json;

using Projecao.Core.ERP.GerenciaEmpresa;
using Projecao.Core.IMC.Mensagens;
using Projecao.Core.Integracao.Rule.Integracao;
using Projecao.Core.ITG.Envio;
using Projecao.Core.ITG.Jobs;
using Projecao.Core.ITG.Sincronizacao;
using Projecao.Core.LGC.DB;
using Projecao.Core.PET.Agendamento;

using TFX.Core;
using TFX.Core.Cache;
using TFX.Core.Exceptions;
using TFX.Core.LZMA;
using TFX.Core.Model.Data;
using TFX.Core.Model.Net;
using TFX.Core.Objects;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;
using TFX.Core.Service.DB;
using TFX.Core.Service.SVC;
using TFX.Core.Utils;

using static Projecao.Core.ERP.DB.ERPx;

using static Projecao.Core.IMC.DB.IMCx;
using static Projecao.Core.PET.DB.PETx;

namespace Projecao.Core.ITG
{
    [XRegister(typeof(XIntegrationManager), sCID)]
    public class XIntegrationManager : XIntegrationRule
    {
        public class Servicos
        {
            public String SERVICO
            {
                get; set;
            }
            public String EXAME
            {
                get; set;
            }
            public String VACINA
            {
                get; set;
            }
        }
        public const String sCID = "797D697B-8EAC-4786-9619-46839AA9C13C";
        public static Guid gCID = new Guid(sCID);
        public static ConfigITGFRM.XCFGTuple Config = new ConfigITGFRM.XCFGTuple();
        private static Timer _Timer;
        private static Boolean _Working = false;
        public override Int32 RunnOrder => -1;

        protected override void Execute()
        {
            if (_Timer == null)
            {
                _Timer = new Timer(Watchdog);
                if (XDefault.IsDebugTime)
                    _Timer.Change(10 * 1000, Timeout.Infinite);
                else
                    _Timer.Change(5 * 60 * 1000, Timeout.Infinite);
            }

            PrepareConfig();
            XGerenciaIntegracao.IsClientReverse = Config.IntegracaoReversa;
            if (Config.GrupoServico.IsFull())
            {
                IntegraMesagens();
                RecebeDados();
                EnviaDados();
            }
            if (Config.IntegracaoReversa)
                IntegracaoReversa();
        }

        private void IntegracaoReversa()
        {
            XGerenciaIntegracao.Config = new IntegracaoLegadoSVC.XCFGTuple();
            String[] param = Config.Parametros.SafeBreak(XEnvironment.NewLine);
            if (param.SafeLength() > 0)
            {
                Servicos par = new Servicos();
                JsonConvert.PopulateObject(param[0], par);
                XGerenciaIntegracao.Config.GrupoServico = par.SERVICO;
                XGerenciaIntegracao.Config.GrupoExame = par.EXAME;
                XGerenciaIntegracao.Config.GrupoServico = par.VACINA;
            }

            if (Directory.Exists(XDefault.DataTemp))
                Directory.Delete(XDefault.DataTemp, true);
            if (!Directory.Exists(XDefault.DataTemp))
                Directory.CreateDirectory(XDefault.DataTemp);
            using (XExecContext ctx = XExecContext.Create())
            {
                if (!XLegacyManager.IsInitialize)
                    XLegacyManager.LoadRemoteModel(ctx, XGerenciaIntegracao.KnowTables);

                foreach (XBaseIntegracaoTabela it in XLegacyManager.ITGTables.OrderBy(i => i.Order))
                {
                    if (it.MigrationLevel != XMigrationLevel.ServerToServer || it.LegacySide != XLegacySide.Cloud)
                        continue;
                    try
                    {
                        XConsole.Warn($"Start Execute [{it.RemoteTable}].");
                        it.Prepare(ctx, null);
                        it.Execute(ctx);
                    }
                    finally
                    {
                        it.Prepare(null, null);
                    }
                }
                ctx.Commit();
            }

            try
            {
                String[] files = Directory.GetFiles(XDefault.DataTemp, "*.bin");
                if (files.SafeLength() > 0)
                {
                    using (XHttpBroker broker = new XHttpBroker("http://" + Config.HostInt))
                    {
                        broker.UseThrow = true;
                        broker.Timeout = 30 * 60 * 1000;
                        broker.Connect(Config.UsuarioInt, Config.SenhaInt);
                        using (XMemoryStream ms = new XMemoryStream())
                        {
                            foreach (String file in files)
                            {
                                Byte[] data = File.ReadAllBytes(file);
                                if (data.SafeLength() == 0)
                                    continue;
                                ms.Write(Path.GetFileNameWithoutExtension(file));
                                ms.Write(data);
                            }
                            broker.Put("reverseintegration", ms.ToArray());
                        }
                    }
                }
            }
            catch (Exception pEx)
            {
                Log.Exception(pEx);
            }
        }

        private void IntegraMesagens()
        {
            try
            {
                using (XHttpBroker broker = new XHttpBroker("http://" + Config.HostInt))
                {
                    broker.UseThrow = true;
                    broker.Timeout = 30 * 60 * 1000;
                    broker.Connect(Config.UsuarioInt, Config.SenhaInt);

                    XSearchData sdata = XHttpRequest.GetWhere("0", IntegraMensagensSVC.xIMCxMensagem.Integrado);
                    IntegraMensagensSVC.XDataSet srcmsg = broker.Get<IntegraMensagensSVC, IntegraMensagensSVC.XDataSet>(sdata, 0);
                    if (srcmsg.Count == 0)
                        return;
                    using (XExecContext ctx = XExecContext.Create())
                    using (XDBLegacyTable msg = XLegacyManager.Create(XGerenciaIntegracao.FAT_SERVICOMENSAGEM, ctx, XGerenciaIntegracao.KnowTables))
                    {
                        String cnpj = ctx.DataBase.ExecScalar<String>("SELECT EMPRESA_ID FROM FAT_PARAMETROS");
                        ctx.DataBase.ExecSQL("call BCO_FUNCAO('GESTCOM')");
                        Log.Warn($"Integrando mensagem [{srcmsg.Count}]");
                        foreach (IntegraMensagensSVC.XTuple tpl in srcmsg)
                        {
                            try
                            {

                                Int32 pk = ctx.DataBase.ExecScalar<Int32>("select SEQ_SERVICOMENSAGEM.NEXTVAL from dual");
                                msg.Clear();
                                msg.NewTuple(pk);
                                msg.Current["SERVICOMENSAGEM_ID"] = pk;
                                msg.Current["EMPRESA_ID"] = cnpj;
                                msg.Current["CANAL"] = "WHATSAPP";
                                msg.Current["NUMERO_DESTINO"] = tpl.Destino;
                                msg.Current["MENSAGEM"] = XEncoding.Default.GetString(tpl.Mensagem);
                                msg.Flush();
                                sdata = XHttpRequest.GetWhere(tpl.IMCxMensagemID.AsString(), IntegraMensagensSVC.xIMCxMensagem.IMCxMensagemID);
                                IntegraMensagensSVC.XDataSet tflush = broker.Get<IntegraMensagensSVC, IntegraMensagensSVC.XDataSet>(sdata, 0);
                                if (tflush.Count > 0)
                                {
                                    var tplf = tflush.Tuples.FirstOrDefault();
                                    tpl.Integrado = true;
                                    broker.OnlyPut(srcmsg);
                                }
                            }
                            catch (Exception pEx)
                            {
                                Log.Error(pEx);
                            }

                        }
                        ctx.Commit();
                    }
                }
            }
            catch (Exception pEx)
            {
                Log.Exception(pEx);
            }
        }

        private void Watchdog(object state)
        {
            try
            {
                if (!XWSBroker.IsLive)
                    WSConnect();
            }
            catch
            {
            }
            finally
            {
                try
                {
                    if (XDefault.IsDebugTime)
                        _Timer.Change(10 * 1000, Timeout.Infinite);
                    else
                        _Timer.Change(5 * 60 * 1000, Timeout.Infinite);
                }
                catch
                {
                }
            }
        }

        private void PrepareConfig()
        {
            Log.Warn("EXECUTANDO PrepareConfig");
            String cfgfile = Path.Combine(XDefault.AppPath, "Config", "Config.cfg");
            if (!File.Exists(cfgfile))
                throw new XError($"Arquivo [{cfgfile}] de configuração não foi encontro.");
            Byte[] data = XLzma.Decode(File.ReadAllBytes(cfgfile));
            Config.SetData(data);
            if (!XDefault.IsDebugTime)
                RunInterval = Math.Max(300, Config.LatenciaIntegracao) * 1000;
            WSConnect();
            XDBConnection.DefualtConfig = new XDBConnection();
            XDBConnection.DefualtConfig.Vendor = XDBVendor.Oracle;
            XDBConnection.DefualtConfig.Host = Config.Host;
            XDBConnection.DefualtConfig.Port = Config.Porta;
            XDBConnection.DefualtConfig.Instance = Config.Instancia;
            XDBConnection.DefualtConfig.User = Config.Usuario;
            XDBConnection.DefualtConfig.Password = Config.Senha;
            XDBConnection.DefualtConfig.PoolCount = 50;
            XDBConnection.DefualtConfig.Timeout = 5000;
        }

        private static void WSConnect()
        {
            Log.Warn("EXECUTANDO WSConnect");
            if (XWSBroker.IsLive)
                return;
            XWSIndetity id = new XWSIndetity();
            id.CompanyID = Config.CompanyID;
            id.Type = XWSIdentityType.Wakeup;
            XWSBroker.ConnectWebSocket($"ws://{Config.HostInt}", id);
            XWSBroker.AddMessageCatcher(ReceiveMessage);
        }

        private static void ReceiveMessage(XJSonData pMessage)
        {
            try
            {
                if (pMessage is XWSWakeup)
                {
                    XIntegrationManager im = new XIntegrationManager();
                    im.Execute();
                }
            }
            catch (Exception pEx)
            {
                Log.Error(pEx);
            }
        }

        private void RecebeDados()
        {
            if (_Working)
                return;
            try
            {
                Log.Warn("EXECUTANDO RecebeDados");
                _Working = true;
                using (XExecContext ctx = XExecContext.Create())
                using (XHttpBroker broker = new XHttpBroker("http://" + Config.HostInt))
                {
                    broker.UseThrow = true;
                    broker.Timeout = 30 * 60 * 1000;
                    broker.Connect(Config.UsuarioInt, Config.SenhaInt);

                    ctx.DataBase.ExecSQL("call BCO_FUNCAO('GESTCOM')");
                    XLegacyManager.LoadRemoteModel(ctx, XGerenciaIntegracao.KnowTables);
                    List<XSincronize> runners = new List<XSincronize>();
                    foreach (Type tp in XTypeCache.GetTypes<XSincronize>())
                        runners.Add(tp.CreateInstance<XSincronize>());
                    foreach (XSincronize rule in runners.OrderBy(i => i.RunnOrder))
                    {
                        rule.Config = Config;
                        rule.Execute(ctx, broker);
                    }
                    ctx.Commit();
                }
            }
            catch (Exception pEx)
            {
                Log.Error(pEx);
            }
            finally
            {
                _Working = false;
            }
        }

        private void EnviaDados()
        {
            try
            {
                using (XHttpBroker broker = new XHttpBroker("http://" + Config.HostInt))
                {
                    broker.UseThrow = true;
                    broker.Timeout = 30 * 60 * 1000;
                    broker.Connect(Config.UsuarioInt, Config.SenhaInt);

                    List<XEnvios> runners = new List<XEnvios>();
                    foreach (Type tp in XTypeCache.GetTypes<XEnvios>())
                        runners.Add(tp.CreateInstance<XEnvios>());
                    foreach (XEnvios rule in runners.OrderBy(i => i.RunnOrder))
                    {
                        try
                        {
                            using (XExecContext ctx = XExecContext.Create())
                            {
                                rule.Config = Config;
                                if (!rule.CanExecute)
                                    continue;
                                ctx.DataBase.ExecSQL("call BCO_FUNCAO('GESTCOM')");
                                XLegacyManager.LoadRemoteModel(ctx, XGerenciaIntegracao.KnowTables);
                                rule.Execute(ctx, broker);
                                Log.Warn($"ENVIADO [{rule.Table.RemoteTable}] RecordCount [{rule.RecordCount}] Size [{rule.Size.ToString("#,##0")}]");
                                ctx.Commit();
                            }
                        }
                        catch (Exception pEx)
                        {
                            Log.Error(pEx);
                        }
                    }
                }
            }
            catch (Exception pEx)
            {
                Log.Error(pEx);
            }
        }
    }
}