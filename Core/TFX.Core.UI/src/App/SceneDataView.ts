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
        this.Filter.OnResize = () => this.UpdateDataGridPosition();
    }

    Filter: XFilter;
    DataGrid: MainDataGrid;
    SVCModel!: XIServiceModel;
    Teste: string | undefined;

    @Inject(XHttpClient, XLifetime.Singleton)
    Client!: XHttpClient;

    SetModel(pModel: XIServiceModel)
    {
        this.SVCModel = pModel;
        this.Load();
    }

    DoSerach(pData: any): void
    {
        const data = this.Filter.GetFilter();
        this.Client?.GetData(this.SVCModel.SearchPath, this.SVCModel, data, (pData: XDataSet) =>
        {
            this.DataGrid.SetDataSet(pData);
        });
    }

    Load()
    {
        this.DataGrid.SetModel(this.SVCModel);
        let fmdl = this.SVCModel.Forms.FirstOrNull(f => f.Type == XFRMType.SVCFilter);
        if (fmdl != null)
            this.Filter.SetModel(this.SVCModel, fmdl, this.DataGrid.Table.Columns);
        if (this.SVCModel?.SearchPath === undefined)
            return;

        this.UpdateDataGridPosition();
    }

    override SizeChanged()
    {
        super.SizeChanged();
        this.UpdateDataGridPosition();
    }

    private UpdateDataGridPosition()
    {
        if (this.Filter && this.DataGrid)
        {
            const filterHeight = this.Filter.HTML.scrollHeight;
            this.DataGrid.HTML.style.top = (filterHeight + 10) + "px";
            this.DataGrid.HTML.style.height = `calc(100% - ${filterHeight + 10}px)`;
        }
    }
}

