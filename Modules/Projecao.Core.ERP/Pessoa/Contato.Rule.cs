using System;
using System.Text;

using TFX.Core.Exceptions;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.ERP.Pessoa
{
    [XRegister(typeof(ContatoRule), sCID, typeof(ContatoSVC))]
    public class ContatoRule : ContatoSVC.XRule
    {
        public const String sCID = "52D57C25-F4B2-4D4F-95BE-47044EC1ED65";
        public static Guid gCID = new Guid(sCID);

        public ContatoRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, ContatoSVC pModel, ContatoSVC.XDataSet pDataSet)
        {
            StringBuilder sb = new StringBuilder();
            foreach (ContatoSVC.XTuple tpl in pDataSet.Tuples)
            {
                if (tpl.Contato.IsEmpty())
                    sb.AppendLine($"Meio de contato \"{tpl.Contato}\" está com número/contato vazio, não é permitido.");
                if (tpl.Mascara.IsFull())
                    tpl.Contato = tpl.Contato.OnlyNumbers();
            }
            if (sb.Length > 0)
                throw new XUnconformity(sb.ToString());
        }
    }
}