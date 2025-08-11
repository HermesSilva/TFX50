using Projecao.Core.NTR.DB;
using Projecao.Core.PCR.Modelo;

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.IO;
using System.Linq;

using TFX.Core;
using TFX.Core.Model.Cache;
using TFX.Core.Service.Data;
using TFX.Core.Utils;

using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.PCR.RecebeOperacao
{
    public static class IntegraAtividade
    {
        private static XILog _Log = XLog.GetLogFor(typeof(IntegraAtividade));

        public static void ProcessAtividade(XExecContext pContext, List<String> pFiles)
        {
            foreach (String file in pFiles.OrderBy(f => f))
            {
                try
                {
                    using (XExecContext ctx = pContext.Clone())
                    using (NTRx._NTRxMobileConfig tblcfg = XPersistencePool.Get<NTRx._NTRxMobileConfig>(ctx))
                    {
                        APPxConfig config = new APPxConfig();

                        using (SQLiteConnection conn = new SQLiteConnection($"Data Source={file}"))
                        {
                            conn.Open();
                            if (conn.TableExists("APPxConfig"))
                            {
                                using (SQLiteCommand cmda = new SQLiteCommand("select * from APPxConfig", conn))
                                using (DbDataReader readerAtividade = cmda.ExecuteReader())
                                {
                                    if (readerAtividade.Read())
                                    {
                                        IntegraUtils.Read(readerAtividade, config);
                                    }
                                }
                            }
                        }
                        if (!tblcfg.Open(config.UserID))
                        {
                            NTRx.NTRxMobileConfig.XTuple tpl = tblcfg.NewTuple(config.UserID);
                            tpl.Data = DateTime.Now;
                            tpl.Dispositivo = tpl.Dispositivo ?? "NI";
                            tpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                            tblcfg.Flush();
                        }

                        List<Tuple<APPxAtividade, APPxAtividadeItem, Guid>> datasource = LoadDataSource(file, config.UserID);
                        List<Tuple<APPxErrorLog, Guid>> errorlist = LoadErrors(file, config.UserID);
                        IntegraGPS.LoadGPS(ctx, file, config.UserID);
                        IntegraErros.AddErros(ctx, errorlist);
                        IntegraAnimal.AddAnimal(ctx, datasource, config.UserID);
                        IntegraCampeio.AddCampeio(ctx, datasource, config.UserID);
                        IntegraPesagem.AddPesagem(ctx, datasource, config.UserID);
                        IntegraVacinacao.AddVacinacao(ctx, datasource, config.UserID);
                        IntegraIATF.AddIATF(ctx, datasource, config.UserID);
                        IntegraDesmama.AddDesmama(ctx, datasource, config.UserID);
                        MoveDB(file);
                        ctx.Commit();
                    }
                }
                catch (Exception pEx)
                {
                    _Log.Error(pEx);
                }
            }
        }

        private static void MoveDB(String pFileName)
        {
            String path = Path.Combine(Path.GetDirectoryName(pFileName), "Processados");
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            String name = $"{DateTime.Now.ToString("yyyyy-MM-dd HH-MM-ss")} {Path.GetFileName(pFileName)}";
            if (Path.GetFileName(pFileName).SafeLength() > 55)
                name = Path.GetFileName(pFileName);
            String nfile = XUtils.FileUniqueName(Path.Combine(path, name));
            File.Move(pFileName, nfile);
        }

        private static List<Tuple<APPxErrorLog, Guid>> LoadErrors(String pFiles, Guid pConfigID)
        {
            List<Tuple<APPxErrorLog, Guid>> datasource = new List<Tuple<APPxErrorLog, Guid>>();
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={pFiles}"))
            {
                conn.Open();
                if (conn.TableExists("APPxErrorLog"))
                {
                    using (SQLiteCommand cmda = new SQLiteCommand("select * from APPxErrorLog", conn))
                    using (DbDataReader readerAtividade = cmda.ExecuteReader())
                    {
                        while (readerAtividade.Read())
                        {
                            APPxErrorLog error = new APPxErrorLog();
                            IntegraUtils.Read(readerAtividade, error);
                            datasource.Add(new Tuple<APPxErrorLog, Guid>(error, pConfigID));
                        }
                    }
                }
            }
            return datasource;
        }

        private static List<Tuple<APPxAtividade, APPxAtividadeItem, Guid>> LoadDataSource(String pFile, Guid pConfigID)
        {
            List<Tuple<APPxAtividade, APPxAtividadeItem, Guid>> datasource = new List<Tuple<APPxAtividade, APPxAtividadeItem, Guid>>();
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={pFile}"))
            {
                conn.Open();
                if (conn.TableExists("APPxAtividade"))
                {
                    using (SQLiteCommand cmda = new SQLiteCommand("select * from APPxAtividade", conn))
                    {
                        using (DbDataReader readerAtividade = cmda.ExecuteReader())
                        {
                            while (readerAtividade.Read())
                            {
                                APPxAtividade atividade = new APPxAtividade();
                                IntegraUtils.Read(readerAtividade, atividade);
                                using (SQLiteCommand cmd = new SQLiteCommand($"select * from APPxAtividadeItem WHERE APPxAtividadeID = '{atividade.APPxAtividadeID}' or APPxAtividadeID = '9a6b34be-77d8-4027-8c0e-b6c7ac3f6941'", conn))
                                {
                                    using (DbDataReader reader = cmd.ExecuteReader())
                                    {
                                        while (reader.Read())
                                        {
                                            APPxAtividadeItem atividadeItem = new APPxAtividadeItem();
                                            IntegraUtils.Read(reader, atividadeItem);
                                            datasource.Add(new Tuple<APPxAtividade, APPxAtividadeItem, Guid>(atividade, atividadeItem, pConfigID));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return datasource;
        }
    }
}