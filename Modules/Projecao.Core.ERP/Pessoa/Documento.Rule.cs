using System;
using System.Linq;
using System.Text;

using TFX.Core.Exceptions;
using TFX.Core.Reflections;
using TFX.Core.Service.Data;

using static Projecao.Core.ERP.DB.ERPx;

namespace Projecao.Core.ERP.Pessoa
{
    [XRegister(typeof(DocumentoRule), sCID, typeof(DocumentoSVC))]
    public class DocumentoRule : DocumentoSVC.XRule
    {
        public const String sCID = "2776AF22-2CEB-4954-BB70-872C74EF1A73";
        public static Guid gCID = new Guid(sCID);

        public DocumentoRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, DocumentoSVC pModel, DocumentoSVC.XDataSet pDataSet)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var tplg in pDataSet.Tuples.Where(t => !t.ERPxDocumentoTipoID.In(ERPxDocumentoTipo.XDefault.Outros)).GroupBy(t => t.ERPxDocumentoTipoID))
            {
                if (tplg.Count() > 1)
                {
                    DocumentoSVC.XTuple tpl = tplg.FirstOrDefault();
                    sb.AppendLine($"Documento \"{tpl?.Numero}\" do tipo \"{tpl?.Tipo}\", duplicado.");
                }
            }

            foreach (DocumentoSVC.XTuple tpl in pDataSet.Tuples)
            {
                if (tpl.Numero.IsEmpty())
                    sb.AppendLine($"Documento \"{tpl?.Numero}\" está com número vazio, não é permitido.");
                if (tpl.Mascara.IsFull())
                    tpl.Numero = tpl.Numero.OnlyNumbers();
            }

            if (sb.Length > 0)
                throw new XUnconformity(sb.ToString());
        }
    }
}