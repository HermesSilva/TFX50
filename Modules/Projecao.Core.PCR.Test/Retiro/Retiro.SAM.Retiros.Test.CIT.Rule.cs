using System;
using System.Linq;
using System.Collections.Generic;
using TFX.CIT.Core.Commands;
using TFX.CIT.Core.Model;
using TFX.Core;
using TFX.Core.Reflections;
using Projecao.Core.PCR.Retiro;

namespace Projecao.Core.PCR.Test.Retiro
{
    [XRegister(typeof(RetiroSAMRetirosTestCITRule), "8E66307C-B1F8-4CFC-B87F-C61D465F6926", typeof(RetiroSAM), typeof(RetiroSAMRetirosTestCIT))]
    public class RetiroSAMRetirosTestCITRule : RetiroSAMRetirosTestCIT.XRule
    {
        public RetiroSAMRetirosTestCITRule()
        {
        }

        public override void Execute()
        {
            NewRecord();
            RetiroSAMRetirosTestCIT.xRetiro r = Model.DataRetiro[CurrentPlay];
            ExecuteCommand(CurrentPlay, new XSelectDynamicCommand(RetiroSAM.RetiroFRM.Fields.SYSxEmpresaID, r.SYSxEmpresaID, r.vSYSxEmpresaID, r.pkvSYSxEmpresaID)); // Fazenda
            ExecuteCommand(CurrentPlay, new XSendTextCommand(RetiroSAM.RetiroFRM.Fields.Nome, r.Nome)); // Nome
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(RetiroSAM.RetiroFRM.Fields.Latitude, r.Latitude)); // Latitude
            ExecuteCommand(CurrentPlay, new XSendDecimalCommand(RetiroSAM.RetiroFRM.Fields.Longitude, r.Longitude)); // Longitude
            ExecuteCommand(CurrentPlay, new XSendTextCommand(RetiroSAM.RetiroFRM.Fields.CoordenadasArea, r.CoordenadasArea)); // Coordenadas da Área
            SaveRecord();
            Search(RetiroSVC.RetiroFilterFRM.Fields.Nome, r.Nome);
            DoEditSingle();
            GetDataSet(AfterDataSet);
        }

        public override void Validade()
        {
        }
    }
}
