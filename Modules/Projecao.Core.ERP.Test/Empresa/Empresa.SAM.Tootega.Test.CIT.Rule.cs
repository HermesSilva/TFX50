using System;
using System.Collections.Generic;

using Projecao.Core.ERP.Empresa;
using Projecao.Core.ERP.Pessoa;

using TFX.CIT.Core.Commands;
using TFX.Core.Reflections;

namespace Projecao.Core.ERP.Test.Empresa
{
    [XRegister(typeof(EmpresaSAMTootegaTestCITRule), "5E33665E-7038-4B5E-9B93-7106CFD1FFDE", typeof(EmpresaSAM), typeof(EmpresaSAMTootegaTestCIT))]
    public class EmpresaSAMTootegaTestCITRule : EmpresaSAMTootegaTestCIT.XRule
    {
        public EmpresaSAMTootegaTestCITRule()
        {
        }

        public override void Execute()
        {
            EmpresaSVC.XTuple r = Model.DataSet[CurrentPlay];
            NewRecord(r.SYSxPessoaID.AsString());
            ExecuteCommand(CurrentPlay, new XSendTextCommand(EmpresaSAM.EmpresaFRM.Fields.RazaoSocial, r.RazaoSocial)); // Razão Social
            ExecuteCommand(CurrentPlay, new XSendTextCommand(EmpresaSAM.EmpresaFRM.Fields.Nome, r.Nome)); // Nome de Fantasia
            ExecuteCommand(CurrentPlay, new XSendTextCommand(EmpresaSAM.EmpresaFRM.Fields.Sigla, r.Sigla)); // Sigla

            //ExecuteCommand(CurrentPlay, new XShowTabCommand(EmpresaSAM.TabFormEnderecosFRM.sCID)); // New Tab Control

            //List<EmpresaSAMTootegaTestCIT.xEnderecos> enderecos = Model.DataEnderecos[CurrentPlay];
            //GridRecordCount[EmpresaSAM.TabFormEnderecosFRM.Fields.Enderecos] = CurrentPlay;

            //foreach (EmpresaSAMTootegaTestCIT.xEnderecos rg in enderecos)
            //{
            //    GridPKValue[CurrentPlay, EmpresaSAM.TabFormEnderecosFRM.Fields.Enderecos] = NewRecordTabItem<Guid>(EmpresaSAM.TabFormEnderecosFRM.Fields.Enderecos);
            //    ExecuteCommand(CurrentPlay, new XSelectStaticCommand(EmpresaSAM.EnderecosFRM.Fields.ERPxFinalidadeID, rg.ERPxFinalidadeID, rg.vERPxFinalidadeID, rg.pkvERPxFinalidadeID)); // Finalidade
            //    ExecuteCommand(CurrentPlay, new XSendTextCommand(EmpresaSAM.EnderecosFRM.Fields.Numero, rg.Numero)); // Número
            //    ExecuteCommand(CurrentPlay, new XSendTextCommand(EmpresaSAM.EnderecosFRM.Fields.Quadra, rg.Quadra)); // Quadra
            //    ExecuteCommand(CurrentPlay, new XSendTextCommand(EmpresaSAM.EnderecosFRM.Fields.Lote, rg.Lote)); // Lote
            //    ExecuteCommand(CurrentPlay, new XSendTextCommand(EmpresaSAM.EnderecosFRM.Fields.Observacao, rg.Observacao)); // Observação
            //    ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(EmpresaSAM.EnderecosFRM.Fields.CEPxLogradouroID, rg.CEPxLogradouroID, rg.vCEPxLogradouroID, rg.pkvCEPxLogradouroID)); // Logradouro
            //    ExecuteCommand(CurrentPlay, new XSendTextCommand(EmpresaSAM.EnderecosFRM.Fields.Complemento, rg.Complemento)); // Complemento
            //    ExecuteCommand(CurrentPlay, new XSendDecimalCommand(EmpresaSAM.EnderecosFRM.Fields.Latitude, rg.Latitude)); // Latitude
            //    ExecuteCommand(CurrentPlay, new XSendDecimalCommand(EmpresaSAM.EnderecosFRM.Fields.Longitude, rg.Longitude)); // Longitude
            //    ApplyRecord(EmpresaSAM.TabFormEnderecosFRM.Fields.Enderecos);
            //}

            ExecuteCommand(CurrentPlay, new XShowTabCommand(EmpresaSAM.TabFormDocumentoFRM.sCID)); // New Tab Control

            IEnumerable<DocumentoSVC.XTuple> documentos = Model.DataSet.DocumentoDataSet.FilteredBy(r);
            GridRecordCount[EmpresaSAM.TabFormDocumentoFRM.Fields.Documentos] = CurrentPlay;

            foreach (DocumentoSVC.XTuple rg in documentos)
            {
                GridPKValue[CurrentPlay, EmpresaSAM.TabFormDocumentoFRM.Fields.Documentos] = NewRecordTabItem<Guid>(EmpresaSAM.TabFormDocumentoFRM.Fields.Documentos);
                ExecuteCommand(CurrentPlay, new XSelectStaticCommand(EmpresaSAM.DocumentosFRM.Fields.ERPxDocumentoTipoID, rg.Tipo, rg.Tipo, rg.ERPxDocumentoTipoID)); // Tipo de Documento
                ExecuteCommand(CurrentPlay, new XSendTextCommand(EmpresaSAM.DocumentosFRM.Fields.Numero, rg.Numero, ".-/,")); // Doc. Nº
                ApplyRecord(EmpresaSAM.TabFormDocumentoFRM.Fields.Documentos);
            }

            //ExecuteCommand(CurrentPlay, new XShowTabCommand(EmpresaSAM.TabFormContatoFRM.sCID)); // New Tab Control

            //List<EmpresaSAMTootegaTestCIT.xContatos> contatos = Model.DataContatos[CurrentPlay];
            //GridRecordCount[EmpresaSAM.TabFormContatoFRM.Fields.Contatos] = CurrentPlay;

            //foreach (EmpresaSAMTootegaTestCIT.xContatos rg in contatos)
            //{
            //    GridPKValue[CurrentPlay, EmpresaSAM.TabFormContatoFRM.Fields.Contatos] = NewRecordTabItem<Guid>(EmpresaSAM.TabFormContatoFRM.Fields.Contatos);
            //    ExecuteCommand(CurrentPlay, new XSelectStaticCommand(EmpresaSAM.ContatosFRM.Fields.ERPxFinalidadeID, rg.ERPxFinalidadeID, rg.vERPxFinalidadeID, rg.pkvERPxFinalidadeID)); // Finalidade
            //    ExecuteCommand(CurrentPlay, new XSelectStaticCommand(EmpresaSAM.ContatosFRM.Fields.ERPxContatoTipoID, rg.ERPxContatoTipoID, rg.vERPxContatoTipoID, rg.pkvERPxContatoTipoID)); // Tipo de Contato
            //    ExecuteCommand(CurrentPlay, new XSendTextCommand(EmpresaSAM.ContatosFRM.Fields.Contato, rg.Contato)); // E-Mail, Telefone e ETC.
            //    ExecuteCommand(CurrentPlay, new XSendTextCommand(EmpresaSAM.ContatosFRM.Fields.Observacao, rg.Observacao)); // Observação
            //    ApplyRecord(EmpresaSAM.TabFormContatoFRM.Fields.Contatos);
            //}

            SaveRecord();
            Search(EmpresaSVC.EmpresaFilterFRM.Fields.RazaoSocial, r.RazaoSocial);
            DoEditSingle();
            GetDataSet(AfterDataSet);
        }

        public override void Validade()
        {
        }
    }
}