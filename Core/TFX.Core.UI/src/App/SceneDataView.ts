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
    Teste: string | undefined;

    @Inject(XHttpClient)
    ClientT1!: XHttpClient;
    @Inject(XHttpClient)
    ClientT2!: XHttpClient;
    @Inject(XHttpClient)
    ClientT3!: XHttpClient;

    @Inject(XHttpClient)
    ClientS1!: XHttpClient;
    @Inject(XHttpClient)
    ClientS2!: XHttpClient;

    @Inject(XHttpClient)
    ClientS3!: XHttpClient;
    @Inject(XHttpClient, XLifetime.Transient)
    ClientC1!: XHttpClient;
    @Inject(XHttpClient, XLifetime.Scoped)
    ClientC2!: XHttpClient;
    @Inject(XHttpClient, XLifetime.Singleton)
    Client!: XHttpClient;

    SetModel(pModel: XAPPModel)
    {
        this.Model = pModel;
        this.Client?.SendAsync(Paths.ServiceModel, { ID: pModel.SearchServiceID }, (pData: any) =>
        {
            this.DataGrid.SetModel(pData.Data);
            this.Load();
        });
    }

    Load()
    {
        if (this.Model?.SearchPath === undefined)
            return;
        this.Client?.SendAsync(this.Model.SearchPath, {}, (pData: any) =>
        {
            this.DataGrid.SetDataSet(pData.Data);
        });
    }
}
