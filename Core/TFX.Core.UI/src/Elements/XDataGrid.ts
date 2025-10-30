/// <reference path="XDiv.ts" />


class XDataGrid extends XDiv
{        
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null)
    {
        super(pOwner, pClass);
        this.Table = new XTable(this, "XTable");
        this.Table.OnRowClick = (r) => this.OnClickRow(r);
        this.Table.OnRowDoubleClick = (r) => this.OnDoubleClickRow(r);

    }

    Table: XTable;
    OnSelectionChanged: XMethod<XArray<XTableRow>> | null = null;
    OnRowDoubleClick: XMethod<XArray<XTableRow>> | null = null;

    get SelectedRows(): XArray<XTableRow>
    {
        return this.Table.Body.DataRows.Where(r => r.IsSelected);
    }

    OnClickRow(pRows: XArray<XTableRow>): void
    {
        if (this.OnSelectionChanged != null)
            this.OnSelectionChanged.apply(this, [pRows]);
    }

    OnDoubleClickRow(pRows: XArray<XTableRow>): void
    {
        if (this.OnRowDoubleClick != null)
            this.OnRowDoubleClick.apply(this, [pRows]);
    }
}

