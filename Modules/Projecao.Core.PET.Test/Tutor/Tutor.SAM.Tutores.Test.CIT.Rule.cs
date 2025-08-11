using System;
using System.Collections.Generic;

using Projecao.Core.ERP.Pessoa;
using Projecao.Core.PET.Tutor;

using TFX.CIT.Core.Commands;
using TFX.Core.Reflections;

namespace Projecao.Core.PET.Test.Tutor
{
    [XRegister(typeof(TutorSAMTutoresTestCITRule), "5AC67C8A-DBC5-4F84-A754-022E56F01518", typeof(TutorSAM), typeof(TutorSAMTutoresTestCIT))]
    public class TutorSAMTutoresTestCITRule : TutorSAMTutoresTestCIT.XRule
    {
        public TutorSAMTutoresTestCITRule()
        {
            //PlayCount = 500;
        }

        public override void Execute()
        {
            TutorSVC.XTuple r = Model.DataSet[0];
            if (CurrentPlay < 2)
            {
                r = Model.DataSet[CurrentPlay];
                NewRecord();

                ExecuteCommand(CurrentPlay, new XSendPromoteCommand(TutorSAM.TutorFRM.Fields.Nome, null)); // Nome
                ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.TutorFRM.Fields.Nome, r.Nome)); // Nome
                ExecuteCommand(CurrentPlay, new XSelectStaticCommand(TutorSAM.TutorFRM.Fields.ERPxGeneroID, r.Genero, r.Genero, r.ERPxGeneroID)); // Gênero
                ExecuteCommand(CurrentPlay, new XSendDateCommand(TutorSAM.TutorFRM.Fields.Nascimento, r.Nascimento)); // Data de Nascimento
                ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.TutorFRM.Fields.Observacao, r.Observacao)); // Observação
                ExecuteCommand(CurrentPlay, new XShowTabCommand(TutorSAM.TabFormPETFRM.sCID)); // New Tab Control

                ExecuteCommand(CurrentPlay, new XShowTabCommand(TutorSAM.TabFormEnderecoFRM.sCID)); // New Tab Control
                IEnumerable<EnderecoSVC.XTuple> pessoaenderecoid = Model.DataSet.EnderecoDataSet.FilteredBy(r);
                GridRecordCount[TutorSAM.TabFormEnderecoFRM.Fields.PessoaEnderecoID] = CurrentPlay;
                foreach (EnderecoSVC.XTuple rg in pessoaenderecoid)
                {
                    GridPKValue[CurrentPlay, TutorSAM.TabFormEnderecoFRM.Fields.PessoaEnderecoID] = NewRecordTabItem<Guid>(TutorSAM.TabFormEnderecoFRM.Fields.PessoaEnderecoID);
                    ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(TutorSAM.PessoaEnderecoIDFRM.Fields.CEP, rg.CEP, rg.CEP, null)); // CEP
                    ExecuteCommand(CurrentPlay, new XSelectStaticCommand(TutorSAM.PessoaEnderecoIDFRM.Fields.ERPxFinalidadeID, rg.Finalidade, rg.Finalidade, rg.ERPxFinalidadeID)); // Finalidade
                    ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.PessoaEnderecoIDFRM.Fields.Numero, rg.Numero)); // Número
                    ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.PessoaEnderecoIDFRM.Fields.Quadra, rg.Quadra)); // Quadra
                    ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.PessoaEnderecoIDFRM.Fields.Lote, rg.Lote)); // Lote
                    ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.PessoaEnderecoIDFRM.Fields.Observacao, rg.Observacao)); // Observação
                    ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.PessoaEnderecoIDFRM.Fields.Complemento, rg.Complemento)); // Complemento
                    ExecuteCommand(CurrentPlay, new XSendDecimalCommand(TutorSAM.PessoaEnderecoIDFRM.Fields.Latitude, rg.Latitude)); // Latitude
                    ExecuteCommand(CurrentPlay, new XSendDecimalCommand(TutorSAM.PessoaEnderecoIDFRM.Fields.Longitude, rg.Longitude)); // Longitude
                    ApplyRecord(TutorSAM.TabFormEnderecoFRM.Fields.PessoaEnderecoID);
                }

                ExecuteCommand(CurrentPlay, new XShowTabCommand(TutorSAM.TabFormPETFRM.sCID)); // New Tab Control
                IEnumerable<AnimalSVC.XTuple> animaisid = Model.DataSet.AnimalDataSet.FilteredBy(r);
                GridRecordCount[TutorSAM.TabFormPETFRM.Fields.AnimaisID] = CurrentPlay;

                foreach (AnimalSVC.XTuple rg in animaisid)
                {
                    GridPKValue[CurrentPlay, TutorSAM.TabFormPETFRM.Fields.AnimaisID] = NewRecordTabItem<Guid>(TutorSAM.TabFormPETFRM.Fields.AnimaisID);
                    ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.AnimaisIDFRM.Fields.Nome, rg.Nome)); // Nome
                    ExecuteCommand(CurrentPlay, new XSelectStaticCommand(TutorSAM.AnimaisIDFRM.Fields.ERPxSexoID, rg.Genero, rg.Genero, rg.ERPxSexoID)); // Gênero
                    ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(TutorSAM.AnimaisIDFRM.Fields.PETxRacaID, rg.Raca, rg.Raca, rg.PETxRacaID)); // Raça
                    ExecuteCommand(CurrentPlay, new XSelectStaticCommand(TutorSAM.AnimaisIDFRM.Fields.PETxPelagemID, rg.Pelagem, rg.Pelagem, rg.PETxPelagemID)); // Pelagem
                    ExecuteCommand(CurrentPlay, new XSendDateCommand(TutorSAM.AnimaisIDFRM.Fields.Nascimento, rg.Nascimento)); // Nascimento
                    ExecuteCommand(CurrentPlay, new XSelectStaticCommand(TutorSAM.AnimaisIDFRM.Fields.PETxPorteID, rg.Porte, rg.Porte, rg.PETxPorteID)); // Porte
                    ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.AnimaisIDFRM.Fields.Observacao, rg.Observacao)); // Observação
                    ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.AnimaisIDFRM.Fields.Cor, rg.Cor)); // Cor
                    ApplyRecord(TutorSAM.TabFormPETFRM.Fields.AnimaisID);
                }
                ExecuteCommand(CurrentPlay, new XShowTabCommand(TutorSAM.TabFormDocumentoFRM.sCID)); // New Tab Control

                IEnumerable<DocumentoSVC.XTuple> pessoadocumentoid = Model.DataSet.DocumentoDataSet.FilteredBy(r);
                GridRecordCount[TutorSAM.TabFormDocumentoFRM.Fields.PessoaDocumentoID] = CurrentPlay;

                foreach (DocumentoSVC.XTuple rg in pessoadocumentoid)
                {
                    GridPKValue[CurrentPlay, TutorSAM.TabFormDocumentoFRM.Fields.PessoaDocumentoID] = NewRecordTabItem<Guid>(TutorSAM.TabFormDocumentoFRM.Fields.PessoaDocumentoID);
                    ExecuteCommand(CurrentPlay, new XSelectStaticCommand(TutorSAM.PessoaDocumentoIDFRM.Fields.ERPxDocumentoTipoID, rg.Tipo, rg.Tipo, rg.ERPxDocumentoTipoID)); // Tipo de Documento
                    ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.PessoaDocumentoIDFRM.Fields.Numero, rg.Numero)); // Doc. Nº
                    ApplyRecord(TutorSAM.TabFormDocumentoFRM.Fields.PessoaDocumentoID);
                }

                ExecuteCommand(CurrentPlay, new XShowTabCommand(TutorSAM.TabFormContatoFRM.sCID)); // New Tab Control

                IEnumerable<ContatoSVC.XTuple> pessoacontatoid = Model.DataSet.ContatoDataSet.FilteredBy(r);
                GridRecordCount[TutorSAM.TabFormContatoFRM.Fields.PessoaContatoID] = CurrentPlay;

                foreach (ContatoSVC.XTuple rg in pessoacontatoid)
                {
                    GridPKValue[CurrentPlay, TutorSAM.TabFormContatoFRM.Fields.PessoaContatoID] = NewRecordTabItem<Guid>(TutorSAM.TabFormContatoFRM.Fields.PessoaContatoID);
                    ExecuteCommand(CurrentPlay, new XSelectStaticCommand(TutorSAM.PessoaContatoIDFRM.Fields.ERPxFinalidadeID, rg.Finalidade, rg.Finalidade, rg.ERPxFinalidadeID)); // Finalidade
                    ExecuteCommand(CurrentPlay, new XSelectStaticCommand(TutorSAM.PessoaContatoIDFRM.Fields.ERPxContatoTipoID, rg.Tipo, rg.Tipo, rg.ERPxContatoTipoID)); // Tipo de Contato
                    ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.PessoaContatoIDFRM.Fields.Contato, rg.Contato)); // E-Mail, Telefone e ETC.
                    ExecuteCommand(CurrentPlay, new XSendTextCommand(TutorSAM.PessoaContatoIDFRM.Fields.Observacao, rg.Observacao)); // Observação
                    ApplyRecord(TutorSAM.TabFormContatoFRM.Fields.PessoaContatoID);
                }
                SaveRecord();
            }
            Search(TutorSVC.TutorFilterFRM.Fields.Nome, r.Nome);
            DoEditSingle();
            GetDataSet(AfterDataSet);
        }

        public override void Validade()
        {
        }
    }
}