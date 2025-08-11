using System;
using System.Linq;

using Projecao.Core.PET.Agendamento;

using TFX.CIT.Core.Commands;
using TFX.Core.Reflections;

namespace Projecao.Core.PET.Test.Agendamento
{
    [XRegister(typeof(AgendamentoEsteticaRedSAMLidianeTestCITRule), "EA6FBF9E-37BD-4331-B033-5EC1217F5FFC", typeof(AgendamentoEsteticaRedSAM), typeof(AgendamentoEsteticaRedSAMLidianeTestCIT))]
    public class AgendamentoEsteticaRedSAMLidianeTestCITRule : AgendamentoEsteticaRedSAMLidianeTestCIT.XRule
    {
        public AgendamentoEsteticaRedSAMLidianeTestCITRule()
        {
        }

        public override void Execute()
        {
            NewRecord();
            AgendamentoEsteticaRedSAMLidianeTestCIT.xAgendamentoEsteticaRed r = Model.DataAgendamentoEsteticaRed[CurrentPlay];
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.ValorTotal, r.ValorTotal)); // Valor Total
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.Numero, r.Numero)); // Número
            ExecuteCommand(CurrentPlay, new XShowTabCommand(AgendamentoEsteticaRedSAM.TabFormPricipalFRM.sCID)); // New Tab Control 
            ExecuteCommand(CurrentPlay, new XSendTextCommand(AgendamentoEsteticaRedSAM.TabFormPricipalFRM.Fields.Observacao, r.Observacao)); // Observação
            ExecuteCommand(CurrentPlay, new XSelectMultiRecordsCommand(AgendamentoEsteticaRedSAM.TabFormPricipalFRM.Fields.PETxAtendimentoProfiID, Model.DataPETxAtendimentoProfiID[CurrentPlay].Select(t => t.PETxAtendimentoProfissionalID.AsString()).ToArray())); // Profissional por Atendimento
            ExecuteCommand(CurrentPlay, new XShowTabCommand(AgendamentoEsteticaRedSAM.TabFormSevicoFRM.sCID)); // New Tab Control 
            ExecuteCommand(CurrentPlay, new XSelectMultiRecordsCommand(AgendamentoEsteticaRedSAM.TabFormSevicoFRM.Fields.PETxAtendimentoServicoID, Model.DataPETxAtendimentoServicoID[CurrentPlay].Select(t => t.PETxAtendimentoServicosID.AsString()).ToArray())); // Serviços por Atendimento
            ExecuteCommand(CurrentPlay, new XShowTabCommand(AgendamentoEsteticaRedSAM.TabFormDetalheFRM.sCID)); // New Tab Control 
            ExecuteCommand(CurrentPlay, new XSelectMultiRecordsCommand(AgendamentoEsteticaRedSAM.TabFormDetalheFRM.Fields.PETxAtendimentoDetalheID, Model.DataPETxAtendimentoDetalheID[CurrentPlay].Select(t => t.PETxAtendimentoDetalhamentoID.AsString()).ToArray())); // Detalhamento do Atendimento
            ExecuteCommand(CurrentPlay, new XShowTabCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.sCID)); // New Tab Control 
            ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.Fields.ERPxEnderecoBuscaID, r.ERPxEnderecoBuscaID, r.vERPxEnderecoBuscaID, r.pkvERPxEnderecoBuscaID)); // Buscar no Endereço
            ExecuteCommand(CurrentPlay, new XSendDateCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.Fields.DataBusca, r.DataBusca, r.mDataBusca)); // Data e Hora (buscar)
            ExecuteCommand(CurrentPlay, new XSendDateCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.Fields.DataEntrega, r.DataEntrega, r.mDataEntrega)); // Data e Hora (entregar)
            ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.Fields.ERPxEnderecoEntregaID, r.ERPxEnderecoEntregaID, r.vERPxEnderecoEntregaID, r.pkvERPxEnderecoEntregaID)); // Entregar no Endereço
            ExecuteCommand(CurrentPlay, new XSendTextCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.Fields.ObservacaoBusca, r.ObservacaoBusca)); // Observação para Busca
            ExecuteCommand(CurrentPlay, new XSendTextCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.Fields.ObservacaoEntrega, r.ObservacaoEntrega)); // Observação para Entrega
            ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.PETxTutorID, r.PETxTutorID, r.vPETxTutorID, r.pkvPETxTutorID)); // Tutor
            ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.PETxAnimalTutorID, r.PETxAnimalTutorID, r.vPETxAnimalTutorID, r.pkvPETxAnimalTutorID)); // PET
            ExecuteCommand(CurrentPlay, new XSendTextCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.NewCleanArea, r.NewCleanArea)); // 
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.Desconto, r.Desconto)); // Desconto
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.ValorCobrado, r.ValorCobrado)); // Valor Cobrado
            SaveRecord();
            Search(AgendamentoSVC.AgendamentoFilterFRM.Fields.Nome, r.Nome);
            DoEditSingle();
            GetDataSet(AfterDataSet);
        }

        public override void Validade()
        {
        }
    }
}
