using System;
using System.Linq;
using System.Collections.Generic;
using TFX.CIT.Core.Commands;
using TFX.CIT.Core.Model;
using TFX.Core;
using TFX.Core.Reflections;
using Projecao.Core.PCR.IATF;

namespace Projecao.Core.PCR.Test.IATF
{
    [XRegister(typeof(IATFSAMIATFTestCITRule), "6BB4C831-4F9E-4016-BFCC-FD76AD29370F", typeof(IATFSAM), typeof(IATFSAMIATFTestCIT))]
    public class IATFSAMIATFTestCITRule : IATFSAMIATFTestCIT.XRule
    {
        public IATFSAMIATFTestCITRule()
        {
        }

        public override void Execute()
        {
            NewRecord();
            IATFSAMIATFTestCIT.xIATF r = Model.DataIATF[CurrentPlay];
            ExecuteCommand(CurrentPlay, new XSendTextCommand(IATFSAM.IATFFRM.Fields.Nome, r.Nome)); // Nome

            List<IATFSAMIATFTestCIT.xPCRxIATFID> pcrxiatfid = Model.DataPCRxIATFID[CurrentPlay];
            GridRecordCount[IATFSAM.IATFFRM.Fields.PCRxIATFID] = CurrentPlay;

            foreach (IATFSAMIATFTestCIT.xPCRxIATFID rg in pcrxiatfid)
            {
                GridPKValue[CurrentPlay, IATFSAM.IATFFRM.Fields.PCRxIATFID] = NewRecord<Guid>(IATFSAM.IATFFRM.Fields.PCRxIATFID);
                ExecuteCommand(CurrentPlay, new XSendDecimalCommand(IATFSAM.PCRxIATFIDFRM.Fields.Duracao, rg.Duracao)); // Duração (dias)
                ExecuteCommand(CurrentPlay, new XSendDecimalCommand(IATFSAM.PCRxIATFIDFRM.Fields.Ordem, rg.Ordem)); // Ordem
                ExecuteCommand(CurrentPlay, new XSelectStaticCommand(IATFSAM.PCRxIATFIDFRM.Fields.PCRxIATFFaseTipoID, rg.PCRxIATFFaseTipoID, rg.vPCRxIATFFaseTipoID, rg.pkvPCRxIATFFaseTipoID)); // Tipo de Fase
                ApplyRecord(IATFSAM.IATFFRM.Fields.PCRxIATFID);
            }
            SaveRecord();
            Search(IATFSVC.IATFFilterFRM.Fields.Nome, r.Nome);
            DoEditSingle();
            GetDataSet(AfterDataSet);
        }

        public override void Validade()
        {
        }
    }
}
