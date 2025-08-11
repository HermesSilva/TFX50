using System;
using System.Collections.Generic;

using Projecao.Core.ERP.Pessoa;
using Projecao.Core.ERP.Profissional;

using TFX.CIT.Core.Commands;
using TFX.Core.Reflections;

namespace Projecao.Core.ERP.Test.Profissional
{
    [XRegister(typeof(ProfissionalSAMAna_ClaraTestCITRule), "B5066C53-A6AC-4B6A-A3D0-ACC1FC879C42", typeof(ProfissionalSAM), typeof(ProfissionalSAMAna_ClaraTestCIT))]
    public class ProfissionalSAMAna_ClaraTestCITRule : ProfissionalSAMAna_ClaraTestCIT.XRule
    {
        public ProfissionalSAMAna_ClaraTestCITRule()
        {
        }

        public override void Execute()
        {
            ProfissionalSVC.XTuple r = Model.DataSet[CurrentPlay];
            NewRecord(r.SYSxPessoaID.AsString());
            ExecuteCommand(CurrentPlay, new XSendPromoteCommand(ProfissionalSAM.ProfissionalFRM.Fields.Nome, null)); // Nome
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ProfissionalSAM.ProfissionalFRM.Fields.Nome, r.Nome)); // Nome
            ExecuteCommand(CurrentPlay, new XSelectStaticCommand(ProfissionalSAM.ProfissionalFRM.Fields.ERPxGeneroID, r.Genero, r.Genero, r.ERPxGeneroID)); // Gênero
            ExecuteCommand(CurrentPlay, new XSendDateCommand(ProfissionalSAM.ProfissionalFRM.Fields.Nascimento, r.Nascimento, r.Nascimento.ToString("ddMMyyyy"))); // Data de Nascimento
            ExecuteCommand(CurrentPlay, new XShowTabCommand(ProfissionalSAM.TabFormCategoriaFRM.sCID)); // New Tab Control

            IEnumerable<CategoriaSVC.XTuple> sysxpessoacategoriaid = Model.DataSet.CategoriaDataSet.FilteredBy(r);
            GridRecordCount[ProfissionalSAM.TabFormCategoriaFRM.Fields.SYSxPessoaCategoriaID] = CurrentPlay;

            foreach (CategoriaSVC.XTuple rg in sysxpessoacategoriaid)
            {
                GridPKValue[CurrentPlay, ProfissionalSAM.TabFormCategoriaFRM.Fields.SYSxPessoaCategoriaID] = NewRecordTabItem<Guid>(ProfissionalSAM.TabFormCategoriaFRM.Fields.SYSxPessoaCategoriaID);
                ExecuteCommand(CurrentPlay, new XSelectStaticCommand(ProfissionalSAM.SYSxPessoaCategoriaIDFRM.Fields.ERPxCategoriaID, rg.Categoria, rg.Categoria, rg.ERPxCategoriaID)); // Categoria de Profissional
                ApplyRecord(ProfissionalSAM.TabFormCategoriaFRM.Fields.SYSxPessoaCategoriaID);
            }

            //ExecuteCommand(CurrentPlay, new XShowTabCommand(ProfissionalSAM.TabFormEnderecoFRM.sCID)); // New Tab Control
            //List<ProfissionalSAMAna_ClaraTestCIT.xSYSxPessoaEnderecoID> sysxpessoaenderecoid = Model.DataSYSxPessoaEnderecoID[CurrentPlay];
            //GridRecordCount[ProfissionalSAM.TabFormEnderecoFRM.Fields.SYSxPessoaEnderecoID] = CurrentPlay;

            //foreach (ProfissionalSAMAna_ClaraTestCIT.xSYSxPessoaEnderecoID rg in sysxpessoaenderecoid)
            //{
            //    GridPKValue[CurrentPlay, ProfissionalSAM.TabFormEnderecoFRM.Fields.SYSxPessoaEnderecoID] = NewRecordTabItem<Guid>(ProfissionalSAM.TabFormEnderecoFRM.Fields.SYSxPessoaEnderecoID);
            //    ExecuteCommand(CurrentPlay, new XSelectStaticCommand(ProfissionalSAM.SYSxPessoaEnderecoIDFRM.Fields.ERPxFinalidadeID, rg.ERPxFinalidadeID, rg.vERPxFinalidadeID, rg.pkvERPxFinalidadeID)); // Finalidade
            //    ExecuteCommand(CurrentPlay, new XSendTextCommand(ProfissionalSAM.SYSxPessoaEnderecoIDFRM.Fields.Numero, rg.Numero)); // Número
            //    ExecuteCommand(CurrentPlay, new XSendTextCommand(ProfissionalSAM.SYSxPessoaEnderecoIDFRM.Fields.Quadra, rg.Quadra)); // Quadra
            //    ExecuteCommand(CurrentPlay, new XSendTextCommand(ProfissionalSAM.SYSxPessoaEnderecoIDFRM.Fields.Lote, rg.Lote)); // Lote
            //    ExecuteCommand(CurrentPlay, new XSendTextCommand(ProfissionalSAM.SYSxPessoaEnderecoIDFRM.Fields.Observacao, rg.Observacao)); // Observação
            //    ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(ProfissionalSAM.SYSxPessoaEnderecoIDFRM.Fields.CEPxLogradouroID, rg.CEPxLogradouroID, rg.vCEPxLogradouroID, rg.pkvCEPxLogradouroID)); // Logradouro
            //    ExecuteCommand(CurrentPlay, new XSendTextCommand(ProfissionalSAM.SYSxPessoaEnderecoIDFRM.Fields.Complemento, rg.Complemento)); // Complemento
            //    ExecuteCommand(CurrentPlay, new XSendDecimalCommand(ProfissionalSAM.SYSxPessoaEnderecoIDFRM.Fields.Latitude, rg.Latitude)); // Latitude
            //    ExecuteCommand(CurrentPlay, new XSendDecimalCommand(ProfissionalSAM.SYSxPessoaEnderecoIDFRM.Fields.Longitude, rg.Longitude)); // Longitude
            //    ApplyRecord(ProfissionalSAM.TabFormEnderecoFRM.Fields.SYSxPessoaEnderecoID);
            //}

            ExecuteCommand(CurrentPlay, new XShowTabCommand(ProfissionalSAM.TabFormHorarioFRM.sCID)); // New Tab Control

            IEnumerable<HorariosSVC.XTuple> sysxhorariopessoaid = Model.DataSet.HorariosDataSet.FilteredBy(r);
            GridRecordCount[ProfissionalSAM.TabFormHorarioFRM.Fields.SYSxHorarioPessoaID] = CurrentPlay;

            foreach (HorariosSVC.XTuple rg in sysxhorariopessoaid)
            {
                GridPKValue[CurrentPlay, ProfissionalSAM.TabFormHorarioFRM.Fields.SYSxHorarioPessoaID] = NewRecordTabItem<Guid>(ProfissionalSAM.TabFormHorarioFRM.Fields.SYSxHorarioPessoaID);
                ExecuteCommand(CurrentPlay, new XSelectStaticCommand(ProfissionalSAM.SYSxHorarioPessoaIDFRM.Fields.ERPxProfissionalHorarioTipoID, rg.Horario, rg.Horario, rg.ERPxProfissionalHorarioTipoID)); // Tipos de Horários
                ExecuteCommand(CurrentPlay, new XSendTimeCommand(ProfissionalSAM.SYSxHorarioPessoaIDFRM.Fields.Inicio, rg.Inicio)); // Início
                ExecuteCommand(CurrentPlay, new XSendTimeCommand(ProfissionalSAM.SYSxHorarioPessoaIDFRM.Fields.Fim, rg.Fim)); // Fim
                ApplyRecord(ProfissionalSAM.TabFormHorarioFRM.Fields.SYSxHorarioPessoaID);
            }
            ExecuteCommand(CurrentPlay, new XShowTabCommand(ProfissionalSAM.TabFormContatoFRM.sCID)); // New Tab Control

            IEnumerable<ContatoSVC.XTuple> sysxpessoacontatoid = Model.DataSet.ContatoDataSet.FilteredBy(r);
            GridRecordCount[ProfissionalSAM.TabFormContatoFRM.Fields.SYSxPessoaContatoID] = CurrentPlay;

            foreach (ContatoSVC.XTuple rg in sysxpessoacontatoid)
            {
                GridPKValue[CurrentPlay, ProfissionalSAM.TabFormContatoFRM.Fields.SYSxPessoaContatoID] = NewRecordTabItem<Guid>(ProfissionalSAM.TabFormContatoFRM.Fields.SYSxPessoaContatoID);
                ExecuteCommand(CurrentPlay, new XSelectStaticCommand(ProfissionalSAM.SYSxPessoaContatoIDFRM.Fields.ERPxFinalidadeID, rg.Finalidade, rg.Finalidade, rg.ERPxFinalidadeID)); // Finalidade
                ExecuteCommand(CurrentPlay, new XSelectStaticCommand(ProfissionalSAM.SYSxPessoaContatoIDFRM.Fields.ERPxContatoTipoID, rg.Tipo, rg.Tipo, rg.ERPxContatoTipoID)); // Tipo de Contato
                ExecuteCommand(CurrentPlay, new XSendTextCommand(ProfissionalSAM.SYSxPessoaContatoIDFRM.Fields.Contato, rg.Contato)); // E-Mail, Telefone e ETC.
                ExecuteCommand(CurrentPlay, new XSendTextCommand(ProfissionalSAM.SYSxPessoaContatoIDFRM.Fields.Observacao, rg.Observacao)); // Observação
                ApplyRecord(ProfissionalSAM.TabFormContatoFRM.Fields.SYSxPessoaContatoID);
            }
            ExecuteCommand(CurrentPlay, new XShowTabCommand(ProfissionalSAM.TabFormDocumentoFRM.sCID)); // New Tab Control

            IEnumerable<DocumentoSVC.XTuple> sysxpessoadocumentoid = Model.DataSet.DocumentoDataSet.FilteredBy(r);
            GridRecordCount[ProfissionalSAM.TabFormDocumentoFRM.Fields.SYSxPessoaDocumentoID] = CurrentPlay;

            foreach (DocumentoSVC.XTuple rg in sysxpessoadocumentoid)
            {
                GridPKValue[CurrentPlay, ProfissionalSAM.TabFormDocumentoFRM.Fields.SYSxPessoaDocumentoID] = NewRecordTabItem<Guid>(ProfissionalSAM.TabFormDocumentoFRM.Fields.SYSxPessoaDocumentoID);
                ExecuteCommand(CurrentPlay, new XSelectStaticCommand(ProfissionalSAM.SYSxPessoaDocumentoIDFRM.Fields.ERPxDocumentoTipoID, rg.Tipo, rg.Tipo, rg.ERPxDocumentoTipoID)); // Tipo de Documento
                ExecuteCommand(CurrentPlay, new XSendTextCommand(ProfissionalSAM.SYSxPessoaDocumentoIDFRM.Fields.Numero, rg.Numero)); // Doc. Nº
                ApplyRecord(ProfissionalSAM.TabFormDocumentoFRM.Fields.SYSxPessoaDocumentoID);
            }
            SaveRecord();
            Search(ProfissionalSVC.ProfissionalFilterFRM.Fields.Nome, r.Nome);
            DoEditSingle();
            GetDataSet(AfterDataSet);
        }

        public override void Validade()
        {
        }
    }
}