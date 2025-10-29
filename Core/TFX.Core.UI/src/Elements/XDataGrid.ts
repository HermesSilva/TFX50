/// <reference path="XDiv.ts" />


class XDataGrid extends XDiv
{        
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null)
    {
        super(pOwner, pClass);
        this.Table = new XTable(this, "XTable");
        this.Table.OnRowClick = (r) => this.OnClickRow(r);

    }

    Table: XTable;
    OnSelectionChanged: XMethod<XArray<XTableRow>> | null = null;

    get SelectedRows(): XArray<XTableRow>
    {
        return this.Table.Body.DataRows.Where(r => r.IsSelected);
    }

    OnClickRow(pRow: XArray<XTableRow>): void
    {
        if (this.OnSelectionChanged != null)
            this.OnSelectionChanged.apply(this, [pRow]);
    }
}

