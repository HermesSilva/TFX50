/// <reference path="XDiv.ts" />


class XDataGrid extends XDiv
{        
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null)
    {
        super(pOwner, pClass);
        this.Table = new XTable(this, "XTable");
    }
    Table: XTable;
}

