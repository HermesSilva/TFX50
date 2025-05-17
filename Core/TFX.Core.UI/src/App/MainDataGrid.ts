/// <reference path="../Stage/XScene.ts" />
/// <reference path="../Elements/XDataGrid.ts" />

class MainDataGrid extends XDataGrid
{
    SetModel(pModel: XAPPModel) {
        throw new Error("Method not implemented.");
    }
    constructor(pOwner: XElement)
    {
        super(pOwner, "MainDataGrid");
    }
}