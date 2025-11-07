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
    SVCModel!: XIServiceModel;

    SetModel(pModel: XIServiceModel)
    {
        this.SVCModel = pModel;
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

    override SetField(pField: XFRMField, pSVCModel: XIServiceModel)
    {
        super.SetField(pField, pSVCModel);
        this.Client?.GetSVCModel(this.Field.DataSourceID, (pModel) =>
        {
            this.DataGrid.SetModel(pModel);
            this.RawValue = this._RawValue;
            this.SizeChanged();
            this.RefreshData(this._RawValue);
        });
    }

    RefreshData(value: any)
    {
        this._RawValue = value;
        if (!this.SVCModel)
            return;
        const sdisp = this.SVCModel.GetColumn(this.Field.TargetFieldID[0]);
        this.Input.value = this.Tuple[sdisp.Name].Value;
    }

    override set RawValue(value: any)
    {
        if (this._RawValue == value)
            return;
        this.RefreshData(value);
    }

    DoSerach()
    {
        if (!this.DataGrid.IsVisible)
            return;

        this.Client?.SendAsync(this.DataGrid.SVCModel.SearchPath, {}, (pData: any) =>
        {
            this.DataGrid.SetDataSet(pData.Data);
        });
    }

    OnSelected(pRows: XArray<XTableRow>)
    {
        this.DropDownContent.Selected();
        const dgsvc = this.DataGrid.SVCModel;
        if (X.IsEmpty(pRows) || !dgsvc)
            return;
        const tpl = pRows[0].Tuple;
        for (var i = 0; i < this.Field.SourceFieldID.length; i++)    
        {
            const sdisp = dgsvc.GetColumn(this.Field.SourceFieldID[i]);
            const tdisp = this.SVCModel.GetColumn(this.Field.TargetFieldID[i]);
            this.Tuple[tdisp.Name].Value = tpl[sdisp.Name].Value;
        }
        const pkcol = dgsvc.PKColumn;
        this.RefreshData(tpl[pkcol.Name].Value)
    }
}