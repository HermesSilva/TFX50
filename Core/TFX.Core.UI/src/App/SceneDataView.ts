/// <reference path="../Stage/XScene.ts" />
/// <reference path="../Reflection/XReflections.ts" />
/// <reference path="../Net/XHttpClient.ts" />

XObjectCache.AddProvider(XHttpClient, XLifetime.Singleton)
@AutoInit
class SceneDataView extends XScene
{
    constructor(pOwner: XElement)
    {
        super(pOwner);
        this.DataGrid = new MainDataGrid(this);
        this.Filter = new XFilter(this);
    }
    Filter: XFilter;
    DataGrid: MainDataGrid;
    Model: XAPPModel | undefined;
    SVCModel!: XServiceModel;
    Teste: string | undefined;

    @Inject(XHttpClient, XLifetime.Singleton)
    Client!: XHttpClient;

    SetModel(pModel: XAPPModel)
    {
        this.Model = pModel;
        this.Client?.SendAsync(Paths.ServiceModel, { ID: pModel.SearchServiceID }, (pData: XResponse<XServiceModel>) =>
        {
            this.SVCModel = pData.Data;
            this.Load();
        });
    }

    Load()
    {
        this.DataGrid.SetModel(this.SVCModel);
        var fmdl = this.SVCModel.Forms.FirstOrNull(f => f.Type == XFRMType.SVCFilter);
        if (fmdl != null)
            this.Filter.SetModel(fmdl);
        if (this.Model?.SearchPath === undefined)
            return;
        this.Client?.SendAsync(this.Model.SearchPath, {}, (pData: any) =>
        {
            this.DataGrid.SetDataSet(pData.Data);
        });
        this.DataGrid.HTML.style.top = this.Filter.HTML.offsetHeight + "px";
    }
}
