/// <reference path="../Stage/XScene.ts" />
/// <reference path="../Reflection/XReflections.ts" />
/// <reference path="../Net/XHttpClient.ts" />

@AutoInit
class SceneDataView extends XScene
{
    constructor(pOwner: XElement)
    {
        super(pOwner);
        this.DataGrid = new MainDataGrid(this);
        this.Filter = new XFilter(this);
        this.Teste = "Maria";
        //this.X123456 = new XScene(this);
    }
    //X123456: XScene
    Filter: XFilter;
    DataGrid: MainDataGrid;
    Model: XAPPModel | undefined;
    Teste: string | undefined;

    @Inject(XHttpClient, XLifetime.Transient)
    ClientT1!: XHttpClient;
    @Inject(XHttpClient, XLifetime.Transient)
    ClientT2!: XHttpClient;
    @Inject(XHttpClient, XLifetime.Transient)
    ClientT3!: XHttpClient;

    @Inject(XHttpClient, XLifetime.Singleton)
    ClientS1!: XHttpClient;
    @Inject(XHttpClient, XLifetime.Singleton)
    ClientS2!: XHttpClient;
    @Inject(XHttpClient, XLifetime.Singleton)
    ClientS3!: XHttpClient;

    @Inject(XHttpClient, XLifetime.Scoped)
    ClientC1!: XHttpClient;
    @Inject(XHttpClient, XLifetime.Scoped)
    ClientC2!: XHttpClient;
    @Inject(XHttpClient, XLifetime.Scoped)
    ClientC3!: XHttpClient;

    SetModel(pModel: XAPPModel)
    {
        this.Model = pModel;
        //this.Client?.SendAsync(Paths.ServiceModel, { ID: pModel.SearchServiceID }, (pData: any) =>
        //{
        //    this.DataGrid.SetModel(pData.Data);
        //    this.Load();
        //});
    }

    Load()
    {
        if (this.Model?.SearchPath === undefined)
            return;
        //this.Client?.SendAsync(this.Model.SearchPath, {}, (pData: any) =>
        //{
        //    this.DataGrid.SetDataSet(pData.Data);
        //});
    }
}
