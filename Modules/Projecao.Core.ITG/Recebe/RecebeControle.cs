using System;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.ITG.Recebe
{
    [XRegister(typeof(RecebeControle), sCID)]
    public class RecebeControle : XClientMessageRule
    {
        public const String sCID = "F9B3E324-0D70-4C00-ABF8-465339714867";
        public static Guid gCID = new Guid(sCID);

        public override void Execute(XExecContext pContext)
        {
            //using (_ISExItemPromocao pro = XPersistencePool.Get<_ISExItemPromocao>(pContext))
            //{
            //    using (XMemoryStream ms = new XMemoryStream(Data))
            //    using (StreamReader sr = new StreamReader(ms, XEncoding.Default))
            //    {
            //        Fields = sr.ReadLine().SafeBreak(";");
            //        while (!sr.EndOfStream)
            //        {
            //            String[] data = sr.ReadLine().SafeBreak(";", StringSplitOptions.None);
            //            if (data.IsEmpty())
            //                continue;
            //            Count++;

            //            String promoid = GetValue(data, "PROMOCAO_ID");
            //            DateTime ini = GetValue<DateTime>(data, "FINAL", XDefault.NullDateTime);
            //            DateTime fim = GetValue<DateTime>(data, "INICIO", XDefault.NullDateTime);
            //            ISExItemPromocao.XTuple protpl;
            //            if (!pro.Open(ISExItemPromocao.Nome, promoid, ISExItemPromocao.SYSxEmpresaID, pContext.Logon.CurrentCompanyID))
            //                protpl = pro.NewTuple();
            //            else
            //                protpl = pro.Current;
            //            protpl.Inicio = ini;
            //            protpl.Fim = fim;
            //            protpl.Nome = promoid;
            //            protpl.SYSxEmpresaID = pContext.Logon.CurrentCompanyID;
            //            pro.Flush();
            //        }
            //    }
        }
    }
}