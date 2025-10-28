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
        this.Filter.DoSerach = (f) => this.DoSerach(f);
    }

    Filter: XFilter;
    DataGrid: MainDataGrid;
    SVCModel!: XServiceModel;
    Teste: string | undefined;

    @Inject(XHttpClient, XLifetime.Singleton)
    Client!: XHttpClient;

    SetModel(pModel: XServiceModel)
    {
        this.SVCModel = pModel;
        this.Load();
    }

    DoSerach(pData: any): void
    {
        this.Client?.SendAsync(this.SVCModel.SearchPath, pData, (pData: any) =>
        {
            this.DataGrid.SetDataSet(pData.Data);
        }); 
    }

    Load()
    {
        this.DataGrid.SetModel(this.SVCModel);
        var fmdl = this.SVCModel.Forms.FirstOrNull(f => f.Type == XFRMType.SVCFilter);
        if (fmdl != null)
            this.Filter.SetModel(fmdl, this.SVCModel);
        if (this.SVCModel?.SearchPath === undefined)
            return;

        this.DataGrid.HTML.style.top = this.Filter.HTML.offsetHeight + "px";
    }
}
