using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;

using Microsoft.Extensions.DependencyInjection;

using TFX.Core;
using TFX.Core.Reflections;
using EFCore.BulkExtensions;

namespace TFX.Core.Data.CEP.DataPack
{
    public static class CEPxDataPack
    {
        public static void ProcessStream<T>(Stream pStream, Int32 pBatchSize, Action<List<T>> pOnBatch) where T : new()
        {
            using var reader = new StreamReader(pStream);
            string headerLine = reader.ReadLine();
            if (headerLine == null)
                return;

            var headers = headerLine.Split('@');
            var propsPorNome = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance).ToDictionary(p => p.Name, p => p, StringComparer.OrdinalIgnoreCase);

            var mapeamento = headers
                .Select((nome, i) => new { Index = i, Propriedade = propsPorNome.GetValueOrDefault(nome.Trim()) })
                .Where(x => x.Propriedade != null)
                .ToList();

            var batch = new List<T>(pBatchSize);
            string line;

            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                var campos = line.Split('@');
                var instancia = new T();

                foreach (var item in mapeamento)
                {
                    if (item.Index >= campos.Length)
                        continue;

                    var texto = campos[item.Index];
                    if (string.IsNullOrWhiteSpace(texto))
                        continue;
                    try
                    {
                        object valor = ConverterTexto(texto, item.Propriedade!.PropertyType);
                        if (valor != null)
                            item.Propriedade.SetValue(instancia, valor);
                    }
                    catch (Exception ex)
                    {
                    }
                }
                batch.Add(instancia);
                if (batch.Count >= pBatchSize)
                {
                    pOnBatch(batch);
                    batch = new List<T>(pBatchSize);
                }
            }

            if (batch.Count > 0)
                pOnBatch(batch);
        }


        private static object ConverterTexto(string texto, Type tipo)
        {
            if (string.IsNullOrWhiteSpace(texto))
                return null;

            Type tipoBase = Nullable.GetUnderlyingType(tipo) ?? tipo;

            return tipoBase switch
            {
                Type t when t == typeof(string) => texto,
                Type t when t == typeof(bool) => bool.Parse(texto),
                Type t when t == typeof(byte) => byte.Parse(texto),
                Type t when t == typeof(char) => texto[0],
                Type t when t == typeof(short) => short.Parse(texto),
                Type t when t == typeof(int) => int.Parse(texto),
                Type t when t == typeof(long) => long.Parse(texto),
                Type t when t == typeof(float) => float.Parse(texto, CultureInfo.InvariantCulture),
                Type t when t == typeof(double) => double.Parse(texto, CultureInfo.InvariantCulture),
                Type t when t == typeof(decimal) => decimal.Parse(texto, CultureInfo.InvariantCulture),
                Type t when t == typeof(DateTime) => DateTime.Parse(texto, CultureInfo.InvariantCulture),
                Type t when t == typeof(Guid) => Guid.Parse(texto),
                _ => throw new NotSupportedException($"Tipo não suportado: {tipo.Name}")
            };
        }

        public static void Apply()
        {
            if (XDefault.IsDebugTime)
                return;
            var cnt = 0;
            Stream st = XResource.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxLocalidade.txt");
            //ProcessStream<CEPxDBContext._CEPxLocalidade>(st, 1000, (batch) =>
            //{
            //    InsertLocalidade(batch);
            //});
            //st = XResource.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxBairro.txt");
            //ProcessStream<CEPxDBContext._CEPxBairro>(st, 10000, (batch) =>
            //{
            //    InsertBairro(batch);

            //});
            st = XResource.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxLogradouro.txt");
            ProcessStream<CEPxDBContext._CEPxLogradouro>(st, 10000, (batch) =>
            {
                InsertLogradouro(batch);
                Console.WriteLine($"Lote de CEPxLogradouro inserido: {(cnt += batch.Count)} registros.");
            });
        }

        private static void InsertLogradouro(List<CEPxDBContext._CEPxLogradouro> batch)
        {
            try
            {
                using (var scope = XEnvironment.Services.CreateScope())
                using (var ctx = scope.ServiceProvider.GetRequiredService<CEPxDBContext>())
                {
                    ctx.BulkInsert(batch);
                }
            }
            catch (Exception ex)
            {
                XConsole.Error($"Erro ao inserir lote de CEPxLogradouro: {ex.Message}");
            }

        }

        private static void InsertBairro(List<CEPxDBContext._CEPxBairro> batch)
        {
            try
            {
                using (var scope = XEnvironment.Services.CreateScope())
                using (var ctx = scope.ServiceProvider.GetRequiredService<CEPxDBContext>())
                {
                    ctx.BulkInsert(batch);
                }
            }
            catch (Exception ex)
            {
                XConsole.Error($"Erro ao inserir lote de CEPxBairro: {ex.Message}");
            }

        }

        private static void InsertLocalidade(List<CEPxDBContext._CEPxLocalidade> batch)
        {
            try
            {
                using (var scope = XEnvironment.Services.CreateScope())
                using (var ctx = scope.ServiceProvider.GetRequiredService<CEPxDBContext>())
                {
                    foreach (var item in batch)
                    {
                        ctx.CEPxLocalidade.Add(item);
                    }
                    ctx.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                XConsole.Error($"Erro ao inserir lote de CEPxBairro: {ex.Message}");
            }
        }

        //public void UploadDefaultData(XExecContext pContext, Boolean pIsMasterDB)
        //{
        //    if (XDefault.IgnorePack)
        //        return;
        //    if ((XDefault.IsNewDB && !XDefault.IsDebugTime) || XDefault.ForceDefaultData)
        //    {
        //        LoadDefault(pContext, true);
        //    }
        //}

        //private static void LoadDefault(XExecContext pContext, Boolean pUpload)
        //{
        //    if (!XEnvironment.NewDB)
        //    {
        //        AddLograd(pContext);
        //        return;
        //    }
        //    if (pContext.DataBase.Factory.DBVendor.In(XDBVendor.MSSQLLocalDB, XDBVendor.MSSQLServer))
        //    {
        //        using (XDBTable bairro = (XDBTable)XPersistencePool.Get<_CEPxBairro>(pContext))
        //        {
        //            bairro.ReadFrom(XResources.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxBairro.txt"), pSeparator: "@");
        //        }
        //        using (XDBTable bairro = (XDBTable)XPersistencePool.Get<_CEPxLogradouro>(pContext))
        //        {
        //            bairro.ReadFrom(XResources.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxLogradouro.txt"), pSeparator: "@");
        //        }
        //    }
        //    else
        //    {
        //        Stream st;

        //        if (CEPxBairro.Instance.StaticData.Count == 1)
        //        {
        //            st = XResources.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxBairro.txt");
        //            CEPxBairro.Instance.LoadFromCSV(st);
        //        }

        //        if (pUpload)
        //            XScriptExecutor.WriteDefaultData(pContext, CEPxBairro.Instance);
        //        if (XDefault.Verbosity > 1)
        //            XConsole.Debug($"CEPxBairro StaticData=[{CEPxBairro.Instance.StaticData.Count}]");
        //        if (Environment.Is64BitOperatingSystem)
        //        {
        //            if (CEPxLogradouro.Instance.StaticData.Count == 1)
        //            {
        //                st = XResources.GetResourceStream(typeof(CEPxDataPack).Assembly, "DataPack.CEPxLogradouro.txt");
        //                CEPxLogradouro.Instance.LoadFromCSV(st);
        //            }

        //            if (pUpload)
        //                XScriptExecutor.WriteDefaultData(pContext, CEPxLogradouro.Instance);
        //            if (XDefault.Verbosity > 1)
        //                XConsole.Debug($"CEPxLogradouro StaticData=[{CEPxLogradouro.Instance.StaticData.Count}]");
        //        }
        //    }
        //    AddLograd(pContext);
        //}

        //private static void AddLograd(XExecContext pContext)
        //{
        //    using (_CEPxLocalidade loc = XPersistencePool.Get<_CEPxLocalidade>(pContext))
        //    using (_CEPxLogradouro log = XPersistencePool.Get<_CEPxLogradouro>(pContext))
        //    using (_CEPxBairro bai = XPersistencePool.Get<_CEPxBairro>(pContext))
        //    {
        //        loc.MaxRows = 0;
        //        loc.FilterActives = true;
        //        loc.Open();
        //        XConsole.Debug($"GERANDO LOGRADOUROS INEXISTENTES=[{loc.Count}]");

        //        Int32 cnt = 0;
        //        foreach (CEPxLocalidade.XTuple loctpl in loc)
        //        {
        //            if (log.Open(CEPxLogradouro.CEPxLocalidadeID, loctpl.CEPxLocalidadeID))
        //                continue;
        //            CEPxBairro.XTuple btpl;
        //            if (!bai.Open(CEPxBairro.CEPxLocalidadeID, loctpl.CEPxLocalidadeID))
        //            {
        //                btpl = bai.NewTuple();
        //                bai.Current.CEPxLocalidadeID = loctpl.CEPxLocalidadeID;
        //                bai.Current.Nome = "Centro";
        //                bai.Flush();
        //                cnt++;
        //            }
        //            else
        //                btpl = bai.Tuples.FirstOrDefault();
        //            log.NewTuple();
        //            log.Current.CEPxLocalidadeID = loctpl.CEPxLocalidadeID;
        //            log.Current.Nome = "Setor Central";
        //            log.Current.CEP = loctpl.CEPGeral;
        //            log.Current.Tipo = "Setor";
        //            log.Current.CEPxBairroID = btpl.CEPxBairroID;
        //            log.Flush();
        //        }
        //        XConsole.Debug($"LOGRADOUROS INEXISTENTES gerados=[{cnt}]");
        //    }
        //}
    }
}