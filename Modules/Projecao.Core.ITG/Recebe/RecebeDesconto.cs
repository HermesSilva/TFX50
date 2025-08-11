using System;
using System.IO;

using TFX.Core;
using TFX.Core.Model.Cache;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ISE.DB.ISEx;

namespace Projecao.Core.ITG.Recebe
{
    [XRegister(typeof(RecebeDesconto), sCID)]
    public class RecebeDesconto : XClientMessageRule
    {
        public const String sCID = "25B678E6-3028-442B-905D-97BA114490B2";
        public static Guid gCID = new Guid(sCID);

        public override void Execute(XExecContext pContext)
        {
            using (_ISExItemPromocao pro = XPersistencePool.Get<_ISExItemPromocao>(pContext))
            {
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
                        DateTime ini = GetValue<DateTime>(data, "FINAL", XDefault.NullDateTime);
                        DateTime fim = GetValue<DateTime>(data, "INICIO", XDefault.NullDateTime);
                        ISExItemPromocao.XTuple protpl;
                        if (!pro.Open(ISExItemPromocao.Nome, promoid, ISExItemPromocao.SYSxEmpresaID, pContext.Logon.CurrentCompanyID))
                            protpl = pro.NewTuple();
                        else
                            protpl = pro.Current;
                        protpl.Inicio = ini;
                        protpl.Fim = fim;
                        protpl.Nome = promoid;
                        protpl.SYSxEmpresaID = pContext.Logon.CurrentCompanyID;
                        pro.Flush();
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
                //        String promoid = GetValue(data, "PROMOCAO_ID");
                //        if (pro.Open(ISExItemPromocao.Nome, promoid, ISExItemPromocao.SYSxEmpresaID, pContext.Logon.CurrentCompanyID))
                //            pro.DeleteAll();
                //    }
                //}
            }
        }
    }
}