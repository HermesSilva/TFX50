/// <reference path="../Stage/XScene.ts" />

class SceneDataView extends XScene
{    
    constructor(pOwner: XElement)
    {
        super(pOwner);
        this.DataGrid = new MainDataGrid(this);
    }
    DataGrid: MainDataGrid;
    Path: string | undefined;

    Load()
    {
        if (this.Path == undefined)
            return;
        var clt = new XHttpClient(this, this.Path);
        clt.OnLoad = this.LoadCallBack;
        clt.SendAsync();
    }

    LoadCallBack(pData: any, pCallData: any, pEvent: ProgressEvent<EventTarget> | null)
    {
    }
}