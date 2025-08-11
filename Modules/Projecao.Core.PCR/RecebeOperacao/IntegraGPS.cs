using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SQLite;
using System.Globalization;
using System.Linq;
using System.Text;

using Projecao.Core.NTR.DB;
using Projecao.Core.PCR.Modelo;

using Geolocation;

using TFX.Core;
using TFX.Core.Data;
using TFX.Core.Model.Cache;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Service.Data;

namespace Projecao.Core.PCR.RecebeOperacao
{
    public static class IntegraGPS
    {
        public static void LoadGPS(XExecContext pContext, String pFile, Guid pConfigID)
        {
            List<APPxLocation> datasource = new List<APPxLocation>();
            using (SQLiteConnection conn = new SQLiteConnection($"Data Source={pFile}"))
            {
                conn.Open();
                if (conn.TableExists("APPxLocation"))
                {
                    using (SQLiteCommand cmda = new SQLiteCommand("select * from APPxLocation", conn))
                    using (DbDataReader readerAtividade = cmda.ExecuteReader())
                    {
                        while (readerAtividade.Read())
                        {
                            APPxLocation location = new APPxLocation();
                            IntegraUtils.Read(readerAtividade, location);
                            datasource.Add(location);
                        }
                    }
                }
            }
            IntegraGPS.AddLocation(pContext, datasource, pConfigID);
        }

        public static Boolean TableExists(this SQLiteConnection pConnection, String pTable)
        {
            using (SQLiteCommand cmda = new SQLiteCommand($"select count(*) as Count from sqlite_master where name = '{pTable}'", pConnection))
            using (DbDataReader dr = cmda.ExecuteReader())
            {
                return dr.Read() && Int32.TryParse(dr.GetValue(0).AsString(), out Int32 cnt) && cnt > 0;
            }
        }

        public static void AddLocation(XExecContext pContext, List<APPxLocation> pTuples, Guid pConfigID)
        {
            using (NTRx._NTRxMobileAtividade ativ = XPersistencePool.Get<NTRx._NTRxMobileAtividade>(pContext))
            {
                foreach (IGrouping<DateTime, APPxLocation> item in pTuples.GroupBy(t => t.dData.Date))
                {
                    DateTime dataInicial = item.Min(l => l.dData);
                    DateTime dataFinal = item.Max(l => l.dData);
                    NTRx.NTRxMobileAtividade.XTuple tpl;
                    (String Rota, Int32 Distancia) ret;
                    if (ativ.Open(NTRx.NTRxMobileAtividade.NTRxMobileConfigID, pConfigID, NTRx.NTRxMobileAtividade.DataInicial, XOperator.GreaterThanOrEqualTo, dataFinal.Date,
                              NTRx.NTRxMobileAtividade.DataFinal, XOperator.LessThan, dataFinal.Date.AddDays(1)))
                    {
                        tpl = ativ.Tuples.First();
                        ret = Join(tpl.Rota, item);
                        tpl.Rota = ret.Rota;
                        tpl.Distancia = ret.Distancia;
                    }
                    else
                    {
                        tpl = ativ.NewTuple();
                        ret = Join(null, item);
                        tpl.Rota = ret.Rota;
                        tpl.Distancia = ret.Distancia;
                        tpl.DataInicial = dataInicial;
                    }
                    tpl.NTRxMobileConfigID = pConfigID;
                    tpl.DataFinal = dataFinal;
                    ativ.Flush();
                }
            }
        }

        private static (String Rota, Int32 Distancia) Join(String pData, IGrouping<DateTime, APPxLocation> pNewData)
        {
            Double dist = 0;
            List<XTuple<DateTime, Double, Double, Int32>> newloc = new List<XTuple<DateTime, Double, Double, Int32>>();
            if (pData.IsFull())
            {
                String[] old = pData.SafeBreak(XEnvironment.NewLine);
                foreach (String loc in old)
                {
                    String[] lpc = loc.SafeBreak(";");
                    if (lpc.SafeLength() == 4)
                        newloc.Add(new XTuple<DateTime, Double, Double, Int32>(DateTime.Parse(lpc[0], CultureInfo.InvariantCulture), Double.Parse(lpc[1], CultureInfo.InvariantCulture),
                             Double.Parse(lpc[2], CultureInfo.InvariantCulture), Int32.Parse(lpc[3])));
                }
            }
            newloc.AddRange(pNewData.Select(l => new XTuple<DateTime, Double, Double, Int32>(l.dData, l.Latitude, l.Longitude, l.Ordem)));
            XTuple<DateTime, Double, Double, Int32> last = null;
            StringBuilder sb = new StringBuilder();
            foreach (XTuple<DateTime, Double, Double, Int32> tpl in newloc.OrderBy(t => t.Item3))
            {
                if (last != null)
                {
                    Double m = GeoCalculator.GetDistance(last.Item1, last.Item2, tpl.Item1, tpl.Item2, 8, DistanceUnit.Meters);
                    if (m > 5)
                    {
                        dist += m;
                        sb.AppendLineEx($"{tpl.Item0.ToString("s", CultureInfo.InvariantCulture)};{tpl.Item1.ToString(CultureInfo.InvariantCulture)};{tpl.Item2.ToString(CultureInfo.InvariantCulture)};{tpl.Item3.ToString(CultureInfo.InvariantCulture)}");
                    }
                    else
                        continue;
                }
                else
                    sb.AppendLineEx($"{tpl.Item0.ToString("s", CultureInfo.InvariantCulture)};{tpl.Item1.ToString(CultureInfo.InvariantCulture)};{tpl.Item2.ToString(CultureInfo.InvariantCulture)};{tpl.Item3.ToString(CultureInfo.InvariantCulture)}");
                last = tpl;
            }
            return new(sb.ToString(), (Int32)dist);
        }
    }
}