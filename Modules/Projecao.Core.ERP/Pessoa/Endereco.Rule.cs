using System;

using TFX.Core.Reflections;
using TFX.Core.Service.Data;

namespace Projecao.Core.ERP.Pessoa
{
    [XRegister(typeof(EnderecoRule), sCID, typeof(EnderecoSVC))]
    public class EnderecoRule : EnderecoSVC.XRule
    {
        public const String sCID = "E810F4D2-F100-4C15-9C73-65DFA027F672";
        public static Guid gCID = new Guid(sCID);

        public EnderecoRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, EnderecoSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            pDataSet.Tuples.ForEach(t =>
            {
                t.Logradouro = t.Tipo + " " + t.Logradouro;
                t.Endereco = $"{t.Tipo} {t.Logradouro} {t.Numero}, Q-{t.Quadra}, L-{t.Lote}, {t.Complemento}";
            });
        }
    }
}