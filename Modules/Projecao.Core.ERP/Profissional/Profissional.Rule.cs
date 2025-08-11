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

namespace Projecao.Core.ERP.Profissional
{
    [XRegister(typeof(ProfissionalRule), sCID, typeof(ProfissionalSVC))]
    public class ProfissionalRule : ProfissionalSVC.XRule
    {
        public const String sCID = "4DBEF887-BA7F-4A20-8AD9-2B49281B5052";
        public static Guid gCID = new Guid(sCID);

        public ProfissionalRule()
        {
            ID = gCID;
        }

        protected override void AfterGet(XExecContext pContext, ProfissionalSVC.XDataSet pDataSet, bool pIsPKGet)
        {
            if (!pIsPKGet)
            {
                CategoriaSVC.XService cat = GetService<CategoriaSVC.XService>();
                foreach (ProfissionalSVC.XTuple tpl in pDataSet)
                {
                    if (cat.Open(CategoriaSVC.xERPxProfissionalCategoria.ERPxProfissionalID, tpl.SYSxPessoaID))
                        tpl.Categoria = String.Join(", ", cat.DataSet.Tuples.Select(t => t.Categoria));
                }
            }
        }

        protected override void BeforeFlush(XExecContext pContext, ProfissionalSVC pModel, ProfissionalSVC.XDataSet pDataSet)
        {
            foreach (ProfissionalSVC.XTuple tpl in pDataSet.Where(t => !t.State.In(XTupleState.Revoked, XTupleState.Recycled)))
            {
                if (pDataSet.CategoriaDataSet.Tuples.Where(t => t.ERPxProfissionalID == tpl.SYSxPessoaID).GroupBy(t => t.ERPxCategoriaID).Any(g => g.Count() > 1))
                    throw new XUnconformity("Não é permitido Profissional com categoria duplicada.");
                if (pDataSet.CategoriaDataSet.Tuples.Count(t => t.ERPxProfissionalID == tpl.SYSxPessoaID) == 0)
                    throw new XUnconformity("Não é permitido Profissional sem ao menos uma categoria.");
                if (tpl.CPF.IsEmpty() && !pDataSet.DocumentoDataSet.Tuples.Any(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CPF))
                    throw new XUnconformity("Não é permitido Profissional sem CPF.");
                if (tpl.CPF.IsFull() && !pDataSet.DocumentoDataSet.Tuples.Any(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CPF))
                {
                    pDataSet.DocumentoDataSet.NewTuple();
                    pDataSet.DocumentoDataSet.Current.ERPxDocumentoTipoID = ERPxDocumentoTipo.XDefault.CPF;
                    pDataSet.DocumentoDataSet.Current.Numero = tpl.CPF;
                }
            }
        }

        protected override void AfterFlush(XExecContext pContext, ProfissionalSVC pModel, ProfissionalSVC.XDataSet pDataSet)
        {
            foreach (ProfissionalSVC.XTuple tpl in pDataSet.Where(t => !t.State.In(XTupleState.Revoked, XTupleState.Recycled)))
            {
                DocumentoSVC.XTuple dtpl = pDataSet.DocumentoDataSet.Tuples.FirstOrDefault(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CPF);
                _ERPxProfissional tt = GetTable<_ERPxProfissional>();
                if (dtpl != null)
                {
                    XAppUtils.InsertGrupoCPF(pContext, tt, dtpl.Numero);
                }
            }
        }

        protected override void ExecutePromote(XExecContext pContext, ProfissionalSVC.XDataSet pDataSet)
        {
            if (!XUtils.CheckCPF(pDataSet.Current.CPF))
                throw new XUnconformity("O CPF informado é Inválido.");
            if (pDataSet.Count != 1)
                throw new XError("Este serviço não pode ser chamado com um número de tuplas diferente de 1.");
            pDataSet.Current.IsPromoteOk = false;
            _ERPxProfissional tt = GetTable<_ERPxProfissional>();
            Guid foundpk = XAppUtils.InsertGrupoCPF(pContext, tt, pDataSet.Current.CPF, true);
            if (foundpk == Guid.Empty)
            {
                DocumentoSVC.XTuple dtpl = pDataSet.DocumentoDataSet.NewTuple();
                dtpl.ERPxDocumentoTipoID = ERPxDocumentoTipo.XDefault.CPF;
                dtpl.Numero = pDataSet.Current.CPF;
                dtpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                dtpl.Tipo = ERPxDocumentoTipo.XDefault.sCPF;
                dtpl.Mascara = ProfissionalSVC.CPF.Mask;
            }
            else
            {
                pDataSet.Current.IsPromoteOk = true;
                pDataSet.Current.PKValue = foundpk;
            }
        }
    }
}