/// <reference path="../Elements/Base/XBaseLoockupInput.ts" />
/// <reference path="../Elements/XDataGrid.ts" />
class XDropDownDataGrid extends XDataGrid
{
    constructor(pOwner: XDataLoockupEditor, pClass: string)
    {
        super(pOwner.DropDownContent, pClass);
        this.Editor = pOwner;
    }
    Editor: XDataLoockupEditor;

    SetModel(pModel: XServiceModel)
    {
        this.Table.SetColumns(pModel.DataView.Columns);
    }

    SetDataSet(pDataSet: XDataSet)
    {
        this.Table.SetDataSet(pDataSet);
    }
}

@AutoInit
class XDataLoockupEditor extends XBaseLoockupInput
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Input.className = "XDataLoockupEditor";
        this.Title = "Digite uma Data";
        this.DataGrid = new XDropDownDataGrid(this, "XDropDownGrid");
        this.DataGrid.Table.OnRowClick = (rows) => this.OnSelected(rows);
    }

    @Inject(XHttpClient, XLifetime.Transient)
    Client!: XHttpClient;
    DataGrid: XDropDownDataGrid;
    SVCModel!: XServiceModel;
    override SetField(pField: XFRMField)
    {
        super.SetField(pField);
        this.Client?.SendAsync(Paths.ServiceModel, { ID: this.Field.DataSourceID }, (pData: XResponse<XServiceModel>) =>
        {
            this.DataGrid.SetModel(pData.Data);
            this.SVCModel = pData.Data;
            this.SizeChanged();
        });
    }

    DoSerach()
    {
        this.Client?.SendAsync(this.SVCModel.SearchPath, {}, (pData: any) =>
        {
            this.DataGrid.SetDataSet(pData.Data);
        });


    }

    OnSelected(pRows: XArray<XTableRow>)
    {
        this.Input.value = pRows[0].Tupla.nome;
        this.DropDownContent.Selected();
    }
}