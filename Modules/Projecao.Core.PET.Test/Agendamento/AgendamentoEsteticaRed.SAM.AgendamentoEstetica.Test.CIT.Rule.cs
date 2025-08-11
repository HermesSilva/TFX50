using System;
using System.Linq;

using Projecao.Core.PET.Agendamento;

using TFX.CIT.Core;
using TFX.CIT.Core.Commands;
using TFX.Core.Reflections;

namespace Projecao.Core.PET.Test.Agendamento
{
    [XRegister(typeof(AgendamentoEsteticaRedSAMAgendamentoEsteticaTestCITRule), "E5EEBA2E-501C-40CB-9241-B3B525531C7B", typeof(AgendamentoEsteticaRedSAM), typeof(AgendamentoEsteticaRedSAMAgendamentoEsteticaTestCIT))]
    public class AgendamentoEsteticaRedSAMAgendamentoEsteticaTestCITRule : AgendamentoEsteticaRedSAMAgendamentoEsteticaTestCIT.XRule
    {
        public AgendamentoEsteticaRedSAMAgendamentoEsteticaTestCITRule()
        {
            IsEnabled = false;
        }

        public override void Execute()
        {
            NewRecord();
            AgendamentoSVC.XTuple r = Model.DataSet[CurrentPlay];
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.Numero, r.Numero)); // Número
            ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.PETxTutorID, r.NomeTutor, r.NomeTutor)); // Tutor
            ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.PETxAnimalTutorID, r.NomeAnimal, r.NomeAnimal)); // PET

            ExecuteCommand(CurrentPlay, new XShowTabCommand(AgendamentoEsteticaRedSAM.TabFormPricipalFRM.sCID)); // New Tab Control
            ExecuteCommand(CurrentPlay, new XSendTextCommand(AgendamentoEsteticaRedSAM.TabFormPricipalFRM.Fields.Observacao, r.Observacao)); // Observação
            NewRecord<Guid>(AgendamentoEsteticaRedSAM.TabFormPricipalFRM.Fields.PETxAtendimentoProfiID);
            ExecuteCommand(CurrentPlay, new XSelectMultiRecordsCommand(XCIDs.StanAloneSearchGrid, AgendamentoProfiEsteSVC.xPETxAtendimentoProfissional.PETxAtendimentoProfissionalID.ID, Model.DataSet.AgendamentoProfiEsteDataSet.FilteredBy(r).Select(t => t.PETxAtendimentoProfissionalID.AsString()).ToArray())); // Profissional por Atendimento

            ExecuteCommand(CurrentPlay, new XShowTabCommand(AgendamentoEsteticaRedSAM.TabFormSevicoFRM.sCID)); // New Tab Control
            ExecuteCommand(CurrentPlay, new XSelectMultiRecordsCommand(XCIDs.StanAloneSearchGrid, AgendamentoServicoSVC.xPETxAtendimentoServicos.PETxAtendimentoServicosID.ID, Model.DataSet.AgendamentoServicoDataSet.FilteredBy(r).Select(t => t.PETxAtendimentoServicosID.AsString()).ToArray())); // Serviços por Atendimento

            ExecuteCommand(CurrentPlay, new XShowTabCommand(AgendamentoEsteticaRedSAM.TabFormDetalheFRM.sCID)); // New Tab Control
            ExecuteCommand(CurrentPlay, new XSelectMultiRecordsCommand(XCIDs.StanAloneSearchGrid, AgendamentoDetalheSVC.xPETxAtendimentoDetalhamento.PETxAtendimentoDetalhamentoID.ID, Model.DataSet.AgendamentoDetalheDataSet.FilteredBy(r).Select(t => t.PETxAtendimentoDetalhamentoID.AsString()).ToArray())); // Detalhamento do Atendimento

            ExecuteCommand(CurrentPlay, new XShowTabCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.sCID)); // New Tab Control
            ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.Fields.ERPxEnderecoBuscaID, r.LogradouroBusca, r.LogradouroBusca, r.ERPxEnderecoBuscaID)); // Buscar no Endereço
            ExecuteCommand(CurrentPlay, new XSendDateCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.Fields.DataBusca, r.DataBusca)); // Data e Hora (buscar)
            ExecuteCommand(CurrentPlay, new XSendTextCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.Fields.ObservacaoBusca, r.ObservacaoBusca)); // Observação para Busca
            ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.Fields.ERPxEnderecoEntregaID, r.LogradouroEntrega, r.LogradouroEntrega, r.ERPxEnderecoEntregaID)); // Entregar no Endereço
            ExecuteCommand(CurrentPlay, new XSendDateCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.Fields.DataEntrega, r.DataEntrega)); // Data e Hora (entregar)
            ExecuteCommand(CurrentPlay, new XSendTextCommand(AgendamentoEsteticaRedSAM.TabFormTaxiFRM.Fields.ObservacaoEntrega, r.ObservacaoEntrega)); // Observação para Entrega
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.ValorTotal, r.ValorTotal)); // Valor Total
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.Desconto, r.Desconto)); // Desconto
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(AgendamentoEsteticaRedSAM.AgendamentoEsteticaRedFRM.Fields.ValorCobrado, r.ValorCobrado)); // Valor Cobrado

            SaveRecord();
            Search(AgendamentoSVC.AgendamentoFilterFRM.Fields.Nome, r.NomeAnimal);
            DoEditSingle();
            GetDataSet(AfterDataSet);
        }

        public override void Validade()
        {
        }
    }
}