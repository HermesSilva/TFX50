/// <reference path="../Elements/Base/XBaseLoockupInput.ts" />
/// <reference path="../Elements/XDataGrid.ts" />
class XDropDownDataGrid extends XDataGrid
{
    constructor(pOwner:  XDataLoockupEditor, pClass: string)
    {
        super(pOwner.DropDownContent, pClass);
        this.Editor = pOwner;
    }
    Editor: XDataLoockupEditor;
}

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
    DataGrid: XDataGrid;

    OnSelected(pRows: XArray<XTableRow>)
    {
        this.Input.value = pRows[0].Tupla.nome;
        this.DropDownContent.Selected();
    }
}