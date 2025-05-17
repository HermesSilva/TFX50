/// <reference path="../Stage/XScene.ts" />
/// <reference path="../Elements/XDataGrid.ts" />

class MainDataGrid extends XDataGrid
{
    
    constructor(pOwner: XElement)
    {
        super(pOwner, "MainDataGrid");
    }

    SetModel(pModel: XServiceModel)
    {
        this.Table.SetColumns(pModel.DataView.Columns);
    }

    SetDataSet(pDataSet: XDataSet)
    {
        this.Table.SetDataSet(pDataSet);
    }

}