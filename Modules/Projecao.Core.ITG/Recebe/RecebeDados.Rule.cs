using System;

using TFX.Core;
using TFX.Core.Cache;
using TFX.Core.Exceptions;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.ITG.Recebe
{
    [XRegister(typeof(RecebeDadosRule), sCID, typeof(RecebeDadosSVC))]
    public class RecebeDadosRule : RecebeDadosSVC.XRule
    {
        public const String sCID = "A4DFD8C7-66A3-482F-9C0B-58855BA6FCD3";
        public static Guid gCID = new Guid(sCID);

        public RecebeDadosRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, RecebeDadosSVC pModel, RecebeDadosSVC.XDataSet pDataSet)
        {
            foreach (RecebeDadosSVC.XTuple tpl in pDataSet)
            {
                using (XMemoryStream ms2 = new XMemoryStream(tpl.Dados))
                {
                    ms2.Read(out Guid cid);
                    ms2.Read(out Int64 size);
                    if (tpl.Dados.SafeLength() != size)
                        throw new XError($"Tamanho de do dado [{tpl.Dados.SafeLength().ToString("#,##0")}] diferente do esperado [{size.ToString("#,##0")}]");
                    XClientMessageRule cmr = XTypeCache.CreateInstance<XClientMessageRule>(cid);
                    ms2.Read(out cmr.Data);
                    ms2.Read(out cmr.ToDeleteData);
                    cmr.Execute(pContext);
                    pDataSet.ReloadOnPost = true;
                    pDataSet.Clear();
                    pDataSet.NewTuple();
                    pDataSet.Current.IsOk = true;
                    XConsole.Debug($"Final Recebimento [{cmr}] Count[{cmr.Count}]");
                }
            }
        }
    }
}