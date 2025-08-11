using System;
using System.Collections.Generic;
using System.Linq;

using Projecao.Core.ERP.Pessoa;

using TFX.Core.Data;
using TFX.Core.Exceptions;
using TFX.Core.Model.Data;
using TFX.Core.Model.DIC.ORM;
using TFX.Core.Reflections;
using TFX.Core.Service.Apps;
using TFX.Core.Service.Data;
using TFX.Core.Utils;

using static Projecao.Core.ERP.DB.ERPx;
using static Projecao.Core.PET.DB.PETx;
using static TFX.Core.Service.Apps.SYSx;

namespace Projecao.Core.PET.Tutor
{
    [XRegister(typeof(TutorRule), sCID, typeof(TutorSVC))]
    public class TutorRule : TutorSVC.XRule
    {
        public const String sCID = "EEE08A0C-1A8C-4C5E-A2DA-B3155B019B05";
        public static Guid gCID = new Guid(sCID);

        public TutorRule()
        {
            ID = gCID;
        }

        protected override void GetWhere(XExecContext pContext, TutorSVC.XDataSet pDataSet, List<object> pWhere, Dictionary<Guid, XTuple<Dictionary<Guid, object>, List<object>>> pRawFilter, bool pIsPKGet)
        {
            if (pContext.SourceID != TutorSAM.gCID)
                pWhere.Add(TutorSVC.xSYSxPessoa.SYSxPessoaID, XOperator.In,
                    $"(select {PETxAnimalTutor.PETxTutorID.Name} from {PETxAnimalTutor.Instance.Name} where " +
                    $"{PETxAnimalTutor.PETxTutorID.Name} = {Model.SYSxPessoa.Name}.{TutorSVC.xSYSxPessoa.SYSxPessoaID.Name})");
        }

        protected override void AfterGet(XExecContext pContext, TutorSVC.XDataSet pDataSet, Boolean pIsPKGet)
        {
            if (pDataSet.Count == 0 || pDataSet.Count > 100)
                return;
            AnimalSVC.XTuple tpl = pDataSet.AnimalDataSet.Tuples.FirstOrDefault(t => t.PETxAnimalID.IsEmpty());
            if (tpl != null)
                pDataSet.AnimalDataSet.Remove(tpl);

            Object[] ttrs = pDataSet.Tuples.Select(t => t.PKValue).ToArray();

            AnimalSVC.XService anml = GetService<AnimalSVC.XService>();
            anml.MaxRows = 0;
            anml.FilterInactive = true;
            anml.Open(AnimalSVC.xPETxAnimalTutor.PETxTutorID, XOperator.In, ttrs);
            DocumentoSVC.XService doc = GetService<DocumentoSVC.XService>();
            doc.MaxRows = 0;
            doc.FilterInactive = true;
            doc.Open(DocumentoSVC.xERPxDocumento.ERPxDocumentoTipoID, XOperator.In, new Object[] { ERPxDocumentoTipo.XDefault.CNPJ, ERPxDocumentoTipo.XDefault.CPF },
                      DocumentoSVC.xERPxDocumento.SYSxPessoaID, XOperator.In, ttrs);

            IEnumerable<IGrouping<Guid, AnimalSVC.XTuple>> ganmls = anml.Tuples.GroupBy(t => t.PETxTutorID);
            foreach (TutorSVC.XTuple ttpl in pDataSet.ToArray())
            {
                IGrouping<Guid, AnimalSVC.XTuple> anms = ganmls.FirstOrDefault(a => a.Key == ttpl.SYSxPessoaID);
                if (anms != null)
                    ttpl.Animais = String.Join(", ", anms.Select(t => $"{t.Nome}({t.Designacao})"));
                DocumentoSVC.XTuple dtpl = doc.Tuples.FirstOrDefault(t => t.SYSxPessoaID == ttpl.SYSxPessoaID);
                if (dtpl != null)
                    ttpl.CPF = XUtils.FormatCPFCNPJ(dtpl.Numero);
            }
        }

        protected override void BeforeFlush(XExecContext pContext, TutorSVC pModel, TutorSVC.XDataSet pDataSet)
        {
            foreach (TutorSVC.XTuple tpl in pDataSet.Where(t => !t.State.In(XTupleState.Revoked, XTupleState.Recycled)))
            {
                if (tpl.CPF.IsEmpty() && !pDataSet.DocumentoDataSet.Tuples.Any(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CPF))
                    throw new XUnconformity("Não é permitido Tutor sem CPF.");
                if (!pDataSet.ContatoDataSet.Tuples.Any(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxContatoTipoID.In(ERPxContatoTipo.XDefault.Telefone_Celular, ERPxContatoTipo.XDefault.Mensagem_WhatsApp, ERPxContatoTipo.XDefault.Telefone_Fixo)))
                    throw new XUnconformity("Não é permitido Tutor sem Telefone.");
                if (tpl.CPF.IsFull() && !pDataSet.DocumentoDataSet.Tuples.Any(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CPF))
                {
                    pDataSet.DocumentoDataSet.NewTuple();
                    pDataSet.DocumentoDataSet.Current.ERPxDocumentoTipoID = ERPxDocumentoTipo.XDefault.CPF;
                    pDataSet.DocumentoDataSet.Current.Numero = tpl.CPF;
                }

                if (!pDataSet.EnderecoDataSet.Tuples.Any(t => t.SYSxPessoaID == tpl.SYSxPessoaID))
                    throw new XUnconformity($"Não é permitido Tutor [{tpl.Nome}] sem endereço, senão não poderá ser integrado os dados.");
            }
        }

        protected override void AfterFlush(XExecContext pContext, TutorSVC pModel, TutorSVC.XDataSet pDataSet)
        {
            foreach (TutorSVC.XTuple tpl in pDataSet.Where(t => !t.State.In(XTupleState.Revoked, XTupleState.Recycled)))
            {
                DocumentoSVC.XTuple dtpl = pDataSet.DocumentoDataSet.Tuples.FirstOrDefault(t => t.SYSxPessoaID == tpl.SYSxPessoaID && t.ERPxDocumentoTipoID == ERPxDocumentoTipo.XDefault.CPF);
                if (dtpl != null)
                {
                    _PETxTutor tt = GetTable<_PETxTutor>();
                    XAppUtils.InsertGrupoCPF(pContext, tt, dtpl.Numero);
                }
                if (tpl.State == XTupleState.New)
                {
                    _PETxAnimalTutor at = GetTable<_PETxAnimalTutor>();
                    at.NewTuple();
                    at.Current.PETxAnimalID = Guid.Empty;
                    at.Current.SYSxEstadoID = SYSxEstado.XDefault.Inativo;
                    at.Current.PETxTutorID = tpl.SYSxPessoaID;
                    at.Flush();
                }
            }
        }

        protected override void ExecutePromote(XExecContext pContext, TutorSVC.XDataSet pDataSet)
        {
            if (!XUtils.CheckCPF(pDataSet.Current.CPF))
                throw new XUnconformity("O CPF informado é Inválido.");
            if (pDataSet.Count != 1)
                throw new XError("Este serviço não pode ser chamado com um número de tuplas diferente de 1.");
            pDataSet.Current.IsPromoteOk = false;
            _PETxTutor tt = GetTable<_PETxTutor>();

            Guid foundpk = XAppUtils.InsertGrupoCPF(pContext, tt, pDataSet.Current.CPF, true);
            if (foundpk.IsEmpty())
            {
                DocumentoSVC.XTuple dtpl = pDataSet.DocumentoDataSet.NewTuple();
                dtpl.ERPxDocumentoTipoID = ERPxDocumentoTipo.XDefault.CPF;
                dtpl.Numero = pDataSet.Current.CPF;
                dtpl.SYSxEstadoID = SYSxEstado.XDefault.Ativo;
                dtpl.Tipo = ERPxDocumentoTipo.XDefault.sCPF;
                dtpl.Mascara = TutorSVC.CPF.Mask;
            }
            else
            {
                pDataSet.Current.IsPromoteOk = true;
                pDataSet.Current.PKValue = foundpk;
            }
        }
    }
}