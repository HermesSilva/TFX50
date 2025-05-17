/// <reference path="../Stage/XScene.ts" />

class SceneDataView extends XScene
{


    constructor(pOwner: XElement)
    {
        super(pOwner);
        this.DataGrid = new MainDataGrid(this);
        this._Client = new XHttpClient(this);
    }
    DataGrid: MainDataGrid;
    Model: XAPPModel | undefined;

    private _Client: XHttpClient;

    SetModel(pModel: XAPPModel)
    {
        this.Model = pModel;
        this._Client.SendAsync(Paths.ServiceModel, { ID: pModel.SearchServiceID }, (pData: any) =>
        {
            this.DataGrid.SetModel(pData.Data);
            this.Load();
        });
    }

    Load()
    {
        if (this.Model?.SearchPath === undefined)
            return;
        this._Client.SendAsync(this.Model.SearchPath, {}, (pData: any) =>
        {
            this.DataGrid.SetDataSet(pData.Data);
        });
    }

  
}