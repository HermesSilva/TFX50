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

namespace Projecao.Core.ERP.PessoaJuridica
{
    [XRegister(typeof(PessoaJuridicaRule), sCID, typeof(PessoaJuridicaSVC))]
    public class PessoaJuridicaRule : PessoaJuridicaSVC.XRule
    {
        public const String sCID = "E381F082-82FB-4AFF-AA1D-147CB9F82055";
        public static Guid gCID = new Guid(sCID);

        public PessoaJuridicaRule()
        {
            ID = gCID;
        }

        protected override void BeforeFlush(XExecContext pContext, PessoaJuridicaSVC pModel, PessoaJuridicaSVC.XDataSet pDataSet)
        {
            foreach (PessoaJuridicaSVC.XTuple tpl in pDataSet.Where(t => !t.State.In(XTupleState.Revoked, XTupleState.Recycled)))
            {
                if (tpl.CNPJ.IsEmpty() && !pDataSet.DocumentoDataSet.Tuples.Any(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CNPJ))
                    throw new XUnconformity("Não é permitido Pessoa Jurídica sem CNPJ.");
                if (tpl.CNPJ.IsFull() && !pDataSet.DocumentoDataSet.Tuples.Any(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CNPJ))
                {
                    pDataSet.DocumentoDataSet.NewTuple();
                    pDataSet.DocumentoDataSet.Current.ERPxDocumentoTipoID = ERPxDocumentoTipo.XDefault.CNPJ;
                    pDataSet.DocumentoDataSet.Current.Numero = tpl.CNPJ;
                }

                if (!pDataSet.EnderecoDataSet.Tuples.Any(t => t.SYSxPessoaID == tpl.SYSxPessoaID))
                    throw new XUnconformity($"Não é permitido Pessoa Jurídica [{tpl.Nome}] sem endereço, senão não poderá ser integrado os dados.");
            }
        }

        protected override void AfterFlush(XExecContext pContext, PessoaJuridicaSVC pModel, PessoaJuridicaSVC.XDataSet pDataSet)
        {
            foreach (PessoaJuridicaSVC.XTuple tpl in pDataSet.Where(t => !t.State.In(XTupleState.Revoked, XTupleState.Recycled)))
            {
                DocumentoSVC.XTuple dtpl = pDataSet.DocumentoDataSet.Tuples.FirstOrDefault(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CNPJ);
                if (dtpl != null)
                {
                    _ERPxPessoaJuridica pesj = GetTable<_ERPxPessoaJuridica>();
                    XAppUtils.InsertGrupoCNPJ(pContext, pesj, dtpl.Numero);
                }
            }
        }

        protected override void ExecutePromote(XExecContext pContext, PessoaJuridicaSVC.XDataSet pDataSet)
        {
            if (!XUtils.CheckCNPJ(pDataSet.Current.CNPJ))
                throw new XUnconformity("O CNPJ informado é Inválido.");

            if (pDataSet.Count != 1)
                throw new XError("Este serviço não pode ser chamado com um número de tuplas diferente de 1.");
            pDataSet.Current.IsPromoteOk = false;
            _ERPxPessoaJuridica pesj = GetTable<_ERPxPessoaJuridica>();

            Guid foundpk = XAppUtils.InsertGrupoCNPJ(pContext, pesj, pDataSet.Current.CNPJ, true);
            if (foundpk == Guid.Empty)
            {
                DocumentoSVC.XTuple dtpl = pDataSet.DocumentoDataSet.NewTuple();
                dtpl.ERPxDocumentoTipoID = ERPxDocumentoTipo.XDefault.CNPJ;
                dtpl.Numero = pDataSet.Current.CNPJ;
                dtpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                dtpl.Tipo = ERPxDocumentoTipo.XDefault.sCNPJ;
                dtpl.Mascara = PessoaJuridicaSVC.CNPJ.Mask;
            }
            else
            {
                pDataSet.Current.IsPromoteOk = true;
                pDataSet.Current.PKValue = foundpk;
            }
        }
    }
}