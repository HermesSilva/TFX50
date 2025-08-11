using System;
using System.IO;

using TFX.Core;
using TFX.Core.Model.Cache;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;

namespace Projecao.Core.ITG.Recebe
{
    [XRegister(typeof(RecebeDescontoDetalhe), sCID)]
    public class RecebeDescontoDetalhe : XClientMessageRule
    {
        public const String sCID = "EDF71823-2F12-4D62-8931-6AB3C2AC1603";
        public static Guid gCID = new Guid(sCID);

        public override void Execute(XExecContext pContext)
        {
            using (_ISExItem item = XPersistencePool.Get<_ISExItem>(pContext))
            using (_ISExItemPromocao pro = XPersistencePool.Get<_ISExItemPromocao>(pContext))
            using (_ISExItemPromocaoDetalhe prodet = XPersistencePool.Get<_ISExItemPromocaoDetalhe>(pContext))
            {
                pro.Open(ISExItemPromocao.SYSxEmpresaID, pContext.Logon.CurrentCompanyID);
                pro.AddIndex("IDX01", ISExItemPromocao.Nome);
                item.AddIndex("IDX01", ISExItem.ProdutoID);
                using (XMemoryStream ms = new XMemoryStream(Data))
                using (StreamReader sr = new StreamReader(ms, XEncoding.Default))
                {
                    Fields = sr.ReadLine().SafeBreak(";");
                    while (!sr.EndOfStream)
                    {
                        String[] data = sr.ReadLine().SafeBreak(";", StringSplitOptions.None);
                        if (data.IsEmpty())
                            continue;
                        Count++;

                        String promoid = GetValue(data, "PROMOCAO_ID");
                        Decimal vlr = GetValue<Decimal>(data, "PRECO_PROMOCAO");
                        String prod = GetValue(data, "CODIGO_PRO");
                        Guid prodpk = Guid.Empty;
                        if (item.LocateByIndex("IDX01", prod))
                            prodpk = item.Current.ISExItemID;
                        if (item.OpenAppend(ISExItem.ProdutoID, prod))
                            prodpk = item.Current.ISExItemID;
                        if (prodpk.IsEmpty())
                        {
                            Log.Warn($"PROMOCAO_ITEM PRODUTO [{prod}] NÃO FOI ENCONTRADO, NÃO SERÁ INTEGRADO, EMPRESA[{pContext.Logon.CompanyName}].");
                            continue;
                        }
                        ISExItemPromocao.XTuple protpl = pro.FindByIndex("IDX01", promoid);
                        if (protpl == null)
                        {
                            Log.Warn($"PROMOCAO_ITEM PRODUTO [{prod}], PROMOCAO_ITEM [{promoid}] NÃO FOI ENCONTRADO, NÃO SERÁ INTEGRADO, EMPRESA[{pContext.Logon.CompanyName}].");
                            continue;
                        }
                        ISExItemPromocaoDetalhe.XTuple prodettpl;
                        if (!prodet.Open(ISExItemPromocaoDetalhe.Nome, promoid, ISExItemPromocaoDetalhe.ISExItemID, prodpk, ISExItemPromocaoDetalhe.SYSxEmpresaID, pContext.Logon.CurrentCompanyID))
                            prodettpl = prodet.NewTuple();
                        else
                            prodettpl = prodet.Current;
                        prodettpl.ISExItemPromocaoID = protpl.ISExItemPromocaoID;
                        prodettpl.Nome = promoid;
                        prodettpl.SYSxEmpresaID = pContext.Logon.CurrentCompanyID;
                        prodettpl.ISExItemID = prodpk;
                        prodettpl.Valor = vlr;
                        prodet.Flush();
                    }
                }
                //pkg.Read(out Byte[] delete);
                //using (XMemoryStream ms = new XMemoryStream(delete))
                //using (StreamReader sr = new StreamReader(ms, XEncoding.Default))
                //{
                //    Fields = sr.ReadLine().SafeBreak(";");
                //    while (!sr.EndOfStream)
                //    {
                //        String[] data = sr.ReadLine().SafeBreak(";", StringSplitOptions.None);
                //        if (data.IsEmpty())
                //            continue;
                //        String tbl = GetValue(data, "TABELA");
                //        String[] cxpk = GetValue(data, "CHAVE").SafeBreak("@");
                //        switch (tbl)
                //        {
                //            case "FAT_PROMOCAOITEM":
                //                if (cxpk.SafeLength() != 2)
                //                    break;
                //                String ipk = cxpk[0] + "-" + cxpk[1];
                //                Int32 prodetpk = GetPK<Int32>(pContext.Logon.CurrentCompanyID, prodet.TableInfo, ipk, false, true);
                //                if (prodetpk == 0)
                //                    continue;
                //                if (prodet.Open(prodetpk))
                //                    prodet.DeleteAll();
                //                break;

                //            default:
                //                throw new XError($"Não foi implementando ação para a tabela [{tbl}].");
                //        }
                //    }
                //}
            }
        }
    }
}