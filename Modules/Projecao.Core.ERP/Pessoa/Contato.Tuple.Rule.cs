using System;

using Projecao.Core.ERP.DB;

using TFX.Core.Model.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;

namespace Projecao.Core.ERP.Pessoa
{
    [XRegister(typeof(ContatoSVCTupleRule), "E30B5E57-3597-4FEC-8ED9-1098610737BF", typeof(ContatoSVC))]
    [XToTypeScript(typeof(ContatoSVC))]
    public class ContatoSVCTupleRule : XDataTupleRule<ContatoSVC.XDataSet, ContatoSVC.XTuple>
    {
        public ContatoSVCTupleRule(ContatoSVC.XTuple pTuple)
          : base(pTuple)
        {
        }

        public override bool CanTotalize(XORMField pField)
        {
            return true;
        }

        public override void RefreshValues(ContatoSVC.XDataSet pDataSet)
        {
            if (X.In(Tuple.ERPxContatoTipoID, ERPx.ERPxContatoTipo.XDefault.EMail, ERPx.ERPxContatoTipo.XDefault.Telefone_Celular))
                Tuple.SetFieldReadOnly(Tuple.Fields.Validar, false);
            else
                Tuple.SetFieldReadOnly(Tuple.Fields.Validar, true);
        }
    }
}