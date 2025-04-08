/// <reference path="../Elements/Base/XBaseInput.ts" />

class XDataGridEditor extends XBaseInput
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Title = "Grade de Dados";
        this.DataGrid = new XDataGrid(this, "XDataGridEditor");
        this.Input = <any>this.DataGrid.HTML;
    }

    DataGrid: XDataGrid;

    CreateInput(): HTMLInputElement
    {
        return <any>null;
    }    
}
