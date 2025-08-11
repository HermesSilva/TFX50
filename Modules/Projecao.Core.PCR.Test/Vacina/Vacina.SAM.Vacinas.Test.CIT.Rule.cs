using System;
using System.Collections.Generic;

using Projecao.Core.PCR.Vacina;

using TFX.CIT.Core.Commands;
using TFX.Core.Reflections;

namespace Projecao.Core.PCR.Test.Vacina
{
    [XRegister(typeof(VacinaSAMVacinasTestCITRule), "E82681F7-1481-4A07-A061-8EB26A883B63", typeof(VacinaSAM), typeof(VacinaSAMVacinasTestCIT))]
    public class VacinaSAMVacinasTestCITRule : VacinaSAMVacinasTestCIT.XRule
    {
        public VacinaSAMVacinasTestCITRule()
        {
        }

        public override void Execute()
        {
            NewRecord();
            VacinaSAMVacinasTestCIT.xVacina r = Model.DataVacina[CurrentPlay];
            ExecuteCommand(CurrentPlay, new XSendTextCommand(VacinaSAM.VacinaFRM.Fields.Nome, r.Nome)); // Nome

            List<VacinaSAMVacinasTestCIT.xISExItemCategoria> isexitemcategoria = Model.DataISExItemCategoria[CurrentPlay];
            GridRecordCount[VacinaSAM.VacinaFRM.Fields.ISExItemCategoria] = CurrentPlay;

            foreach (VacinaSAMVacinasTestCIT.xISExItemCategoria rg in isexitemcategoria)
            {
                GridPKValue[CurrentPlay, VacinaSAM.VacinaFRM.Fields.ISExItemCategoria] = NewRecord<Guid>(VacinaSAM.VacinaFRM.Fields.ISExItemCategoria);
                ExecuteCommand(CurrentPlay, new XSelectStaticCommand(VacinaSAM.ISExItemCategoriaFRM.Fields.ISExCategoriaID, rg.ISExCategoriaID, rg.vISExCategoriaID, rg.pkvISExCategoriaID)); // Categorização de Itens
                ApplyRecord(VacinaSAM.VacinaFRM.Fields.ISExItemCategoria);
            }
            SaveRecord();
            Search(VacinaSVC.VacinaFilterFRM.Fields.Nome, r.Nome);
            DoEditSingle();
            GetDataSet(AfterDataSet);
        }

        public override void Validade()
        {
        }
    }
}