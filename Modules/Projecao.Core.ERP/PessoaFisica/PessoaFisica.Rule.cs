using System;
using System.Linq;

using Projecao.Core.ERP.Pessoa;

using TFX.Core.Exceptions;
using TFX.Core.Model.Data;
using TFX.Core.Reflections;
using TFX.Core.Service.Apps;
using TFX.Core.Service.Data;
using TFX.Core.Utils;

using static Projecao.Core.ERP.DB.ERPx;
using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.ERP.PessoaFisica
{
    [XRegister(typeof(PessoaFisicaRule), sCID, typeof(PessoaFisicaSVC))]
    public class PessoaFisicaRule : PessoaFisicaSVC.XRule
    {
        public const String sCID = "51006A25-F7E8-4BE3-9C0C-24FFE2BAF65B";
        public static Guid gCID = new Guid(sCID);

        public PessoaFisicaRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, PessoaFisicaSVC pModel, PessoaFisicaSVC.XDataSet pDataSet)
        {
            foreach (PessoaFisicaSVC.XTuple tpl in pDataSet.Where(t => !t.State.In(XTupleState.Revoked, XTupleState.Recycled)))
            {
                if (tpl.CPF.IsEmpty() && !pDataSet.DocumentoDataSet.Tuples.Any(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CPF))
                    throw new XUnconformity("Não é permitido Pessoa física sem CPF.");
                if (tpl.CPF.IsFull() && !pDataSet.DocumentoDataSet.Tuples.Any(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CPF))
                {
                    pDataSet.DocumentoDataSet.NewTuple();
                    pDataSet.DocumentoDataSet.Current.ERPxDocumentoTipoID = ERPxDocumentoTipo.XDefault.CPF;
                    pDataSet.DocumentoDataSet.Current.Numero = tpl.CPF;
                }

                if (!pDataSet.EnderecoDataSet.Tuples.Any(t => t.SYSxPessoaID == tpl.SYSxPessoaID))
                    throw new XUnconformity($"Não é permitido Pessoa física [{tpl.Nome}] sem endereço, senão não poderá ser integrado os dados.");
            }
        }

        protected override void AfterFlush(XExecContext pContext, PessoaFisicaSVC pModel, PessoaFisicaSVC.XDataSet pDataSet)
        {
            foreach (PessoaFisicaSVC.XTuple tpl in pDataSet.Where(t => !t.State.In(XTupleState.Revoked, XTupleState.Recycled)))
            {
                DocumentoSVC.XTuple dtpl = pDataSet.DocumentoDataSet.Tuples.FirstOrDefault(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CPF);
                if (dtpl != null)
                {
                    _ERPxPessoaFisica pesf = GetTable<_ERPxPessoaFisica>();
                    XAppUtils.InsertGrupoCPF(pContext, pesf, dtpl.Numero);
                }
            }
        }

        protected override void ExecutePromote(XExecContext pContext, PessoaFisicaSVC.XDataSet pDataSet)
        {
            if (!XUtils.CheckCPF(pDataSet.Current.CPF))
                throw new XUnconformity("O CPF informado é Inválido.");
            if (pDataSet.Count != 1)
                throw new XError("Este serviço não pode ser chamado com um número de tuplas diferente de 1.");
            pDataSet.Current.IsPromoteOk = false;
            _ERPxPessoaFisica pesf = GetTable<_ERPxPessoaFisica>();
            Guid foundpk = XAppUtils.InsertGrupoCPF(pContext, pesf, pDataSet.Current.CPF, true);
            if (foundpk == Guid.Empty)
            {
                DocumentoSVC.XTuple dtpl = pDataSet.DocumentoDataSet.NewTuple();
                dtpl.ERPxDocumentoTipoID = ERPxDocumentoTipo.XDefault.CPF;
                dtpl.Numero = pDataSet.Current.CPF;
                dtpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                dtpl.Tipo = ERPxDocumentoTipo.XDefault.sCPF;
                dtpl.Mascara = PessoaFisicaSVC.CPF.Mask;
            }
            else
            {
                pDataSet.Current.IsPromoteOk = true;
                pDataSet.Current.PKValue = foundpk;
            }
        }
    }
}