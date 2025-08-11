using Projecao.Core.PCR.Reprodutor;

using TFX.CIT.Core.Commands;
using TFX.Core.Reflections;
using TFX.Core;
namespace Projecao.Core.PCR.Test.Reprodutor
{
    [XRegister(typeof(ReprodutorSAMReprodutoresTestCITRule), "BDB905CE-1804-40C7-9D79-AA446263B521", typeof(ReprodutorSAM), typeof(ReprodutorSAMReprodutoresTestCIT))]
    public class ReprodutorSAMReprodutoresTestCITRule : ReprodutorSAMReprodutoresTestCIT.XRule
    {
        public ReprodutorSAMReprodutoresTestCITRule()
        {
            if (XDefault.FastTests)
                PlayCount = 4;
        }

        public override void Execute()
        {
            NewRecord();
            
            ReprodutorSAMReprodutoresTestCIT.xReprodutor r = Model.DataReprodutor[CurrentPlay];
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ReprodutorSAM.ReprodutorFRM.Fields.Nome, r.Nome)); // Nome
            //ExecuteCommand(CurrentPlay, new XSendTextCommand(ReprodutorSAM.ReprodutorFRM.Fields.ApenasSemem, r.ApenasSemem)); // Apenas Sêmem
            ExecuteCommand(CurrentPlay, new XSendDateCommand(ReprodutorSAM.ReprodutorFRM.Fields.Nascimento, r.Nascimento, r.mNascimento)); // Nascimento
            ExecuteCommand(CurrentPlay, new XSelectStaticCommand(ReprodutorSAM.ReprodutorFRM.Fields.PCRxRacaID, r.PCRxRacaID, r.vPCRxRacaID, r.pkvPCRxRacaID)); // Raça
            ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(ReprodutorSAM.ReprodutorFRM.Fields.PCRxPaiID, r.PCRxPaiID, r.vPCRxPaiID, r.pkvPCRxPaiID)); // Nome da Pai
            ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(ReprodutorSAM.ReprodutorFRM.Fields.PCRxMaeID, r.PCRxMaeID, r.vPCRxMaeID, r.pkvPCRxMaeID)); // Nome do Mãe
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ReprodutorSAM.ReprodutorFRM.Fields.Numero, r.Numero)); // Brinco
            SaveRecord();
            ExecuteCommand(CurrentPlay, new XSendTextCommand(ReprodutorSVC.ReprodutorFilterFRM.Fields.Numero, r.Numero)); // Brinco
            Search(ReprodutorSVC.ReprodutorFilterFRM.Fields.Nome, r.Nome);
            DoEditSingle();
            GetDataSet(AfterDataSet);
        }

        public override void Validade()
        {
        }
    }
}