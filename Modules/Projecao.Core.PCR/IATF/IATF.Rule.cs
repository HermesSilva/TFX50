using System;
using System.Linq;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.PCR.DB.PCRx;
using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.PCR.IATF
{
    [XRegister(typeof(IATFRule), sCID, typeof(IATFSVC))]
    public class IATFRule : IATFSVC.XRule
    {
        public const String sCID = "14D89C5F-ED37-4F36-8BCE-F6545F5A07F0";
        public static Guid gCID = new Guid(sCID);

        public IATFRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, IATFSVC pModel, IATFSVC.XDataSet pDataSet)
        {
            foreach (IATFSVC.XTuple itpl in pDataSet)
            {
                Int16 ord = 1;
                Int32 dias = 0;
                foreach (IATFFasesSVC.XTuple tpl in pDataSet.IATFFasesDataSet.Tuples.Where(t => t.PCRxIATFID == itpl.PCRxIATFID).OrderBy(t => t.Ordem))
                {
                    tpl.Ordem = ord++;
                    dias += tpl.Duracao;
                }
                itpl.Duracao = dias;
                itpl.Fases = pDataSet.IATFFasesDataSet.Tuples.Count(t => t.SYSxEstadoID == SYSxEstado.XDefault.Ativo);
            }
        }

        protected override void AfterFlush(XExecContext pContext, IATFSVC pModel, IATFSVC.XDataSet pDataSet)
        {
            _PCRxIATFFase fase = GetTable<_PCRxIATFFase>();
            foreach (IATFSVC.XTuple itpl in pDataSet)
            {
                PCRxIATFFase.XTuple aftpl = null;
                PCRxIATFFase.XTuple ftpl = null;
                fase.Clear();
                foreach (IATFFasesSVC.XTuple tpl in pDataSet.IATFFasesDataSet.Tuples.Where(t => t.PCRxIATFID == itpl.PCRxIATFID).OrderBy(t => t.Ordem))
                {
                    fase.OpenAppend(tpl.PCRxIATFFaseID);
                    ftpl = fase.Current;
                    if (aftpl != null)
                    {
                        aftpl.PCRxIATFFaseProximaID = ftpl.PCRxIATFFaseID;
                        ftpl.PCRxIATFFaseAnteriorID = aftpl.PCRxIATFFaseID;
                    }
                    else
                        ftpl.PCRxIATFFaseAnteriorID = Guid.Empty;
                    aftpl = fase.Current;
                }
                if (aftpl != null)
                    aftpl.PCRxIATFFaseProximaID = Guid.Empty;
                fase.Flush();
            }
        }
    }
}