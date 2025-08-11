using System;
using System.Linq;
using System.Collections.Generic;
using TFX.CIT.Core.Commands;
using TFX.CIT.Core.Model;
using TFX.Core;
using TFX.Core.Reflections;
using Projecao.Core.PCR.Pasto;

namespace Projecao.Core.PCR.Test.Pasto
{
    [XRegister(typeof(PastoSAMPastosTestCITRule), "C0B41C67-42EF-4A6F-8AA6-A5C870719889", typeof(PastoSAM), typeof(PastoSAMPastosTestCIT))]
    public class PastoSAMPastosTestCITRule : PastoSAMPastosTestCIT.XRule
    {
        public PastoSAMPastosTestCITRule()
        {
        }

        public override void Execute()
        {
            NewRecord();
            PastoSAMPastosTestCIT.xPasto r = Model.DataPasto[CurrentPlay];
            ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(PastoSAM.PastoFRM.Fields.PCRxRetiroID, r.PCRxRetiroID, r.vPCRxRetiroID, r.pkvPCRxRetiroID)); // Retiro
            ExecuteCommand(CurrentPlay, new XSendTextCommand(PastoSAM.PastoFRM.Fields.Nome, r.Nome)); // Nome
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(PastoSAM.PastoFRM.Fields.Area, r.Area)); // Área (ha)
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(PastoSAM.PastoFRM.Fields.Animais, r.Animais)); // Animais
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(PastoSAM.PastoFRM.Fields.Latitude, r.Latitude)); // Latitude
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(PastoSAM.PastoFRM.Fields.Longitude, r.Longitude)); // Longitude

            List<PastoSAMPastosTestCIT.xPCRxPastoID> pcrxpastoid = Model.DataPCRxPastoID[CurrentPlay];
            GridRecordCount[PastoSAM.PastoFRM.Fields.PCRxPastoID] = CurrentPlay;

            foreach (PastoSAMPastosTestCIT.xPCRxPastoID rg in pcrxpastoid)
            {
                GridPKValue[CurrentPlay, PastoSAM.PastoFRM.Fields.PCRxPastoID] = NewRecord<Int32>(PastoSAM.PastoFRM.Fields.PCRxPastoID);
                ExecuteCommand(CurrentPlay, new XSendTextCommand(PastoSAM.PCRxPastoIDFRM.Fields.Nome, rg.Nome)); // Nome
                ExecuteCommand(CurrentPlay, new XSendDecimalCommand(PastoSAM.PCRxPastoIDFRM.Fields.Longitude, rg.Longitude)); // Longitude
                ExecuteCommand(CurrentPlay, new XSendDecimalCommand(PastoSAM.PCRxPastoIDFRM.Fields.Latitude, rg.Latitude)); // Latitude
                ExecuteCommand(CurrentPlay, new XSelectStaticCommand(PastoSAM.PCRxPastoIDFRM.Fields.PCRxElementoTipoID, rg.PCRxElementoTipoID, rg.vPCRxElementoTipoID, rg.pkvPCRxElementoTipoID)); // Tipo de Elemento
                ApplyRecord(PastoSAM.PastoFRM.Fields.PCRxPastoID);
            }
            ExecuteCommand(CurrentPlay, new XSendTextCommand(PastoSAM.PastoFRM.Fields.CoordenadasArea, r.CoordenadasArea)); // Coordenadas da Área
            SaveRecord();
            Search(PastoSVC.PastoFilterFRM.Fields.Nome, r.Nome);
            DoEditSingle();
            GetDataSet(AfterDataSet);
        }

        public override void Validade()
        {
        }
    }
}
