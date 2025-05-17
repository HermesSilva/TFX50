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
        this._Client.SendAsync(Paths.ServiceModel, pModel.ID, this.LoadCallBack);
    }

    Load()
    {
    }

    LoadCallBack(pData: any, pCallData: any, pEvent: ProgressEvent<EventTarget> | null)
    {
    }
}