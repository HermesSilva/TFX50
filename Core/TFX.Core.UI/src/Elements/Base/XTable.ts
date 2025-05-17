/// <reference path="../XDiv.ts" />
class XColumnConfig
{
    Name!: string;
    Visible!: boolean;
    Width!: number;
    Title!: string;    
}

class XTableElement extends XElement
{
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null = null, pTag: string | null = null)
    {
        super(pOwner, pClass, pTag);
    }
    Cells = new XArray<XTableElement>();

    AddCell(pClass: string)
    {
        var cell = new XTableElement(this, pClass, "tr");
        this.Cells.Add(cell);
    }

    protected override CreateContainer(pTag: string | null = null): HTMLElement 
    {
        return XUtils.AddElement<HTMLTableElement>(null, pTag, null);
    }
}

class XDragUtils
{
    private static _Data: any;
    static SetData(pData: any)
    {
        this._Data = pData;
    }
    static GetData<T>(): T
    {
        return <T>this._Data;
    }
    static HasDrag: any;
}

class XTableHCell extends XTableElement
{
    constructor(pOwner: XTableHRow, pClass: string | null = null)
    {
        super(pOwner, pClass, "th");
        this.HRow = pOwner;
        this.Table = this.HRow.Header.Table;
        this.Content = XUtils.AddElement<HTMLDivElement>(this, "div", "XTableHContent");
        this.Sizer = XUtils.AddElement<HTMLDivElement>(this.Content, "div", "XTableResizer");
        this.TextArea = XUtils.AddElement<HTMLSpanElement>(this.Content, "div", "XTableHTitle");
        this.Title = XUtils.AddElement<HTMLSpanElement>(this.TextArea, "span");
        this.SortIcon = XUtils.AddElement<HTMLSpanElement>(this.TextArea, "span", "sort-icon");
        this.SortState = { Field: "", Direction: 'asc' };
        this.ResizerEvents()
        this.DragEvents()
    }
    Table: XTable;
    HRow: XTableHRow;
    Sizer: HTMLDivElement;
    TextArea: HTMLSpanElement;
    Title: HTMLSpanElement;
    Content: HTMLDivElement;
    SortIcon: HTMLSpanElement;
    Data: XColumnConfig | any = null;
    SortState: { Field: string; Direction: 'asc' | 'desc' };

    SetData(pCell: XColumnConfig)
    {
        this.SortState = { Field: "", Direction: 'asc' };
        this.Data = pCell;
        this.Title.innerHTML = "<spans>" + this.Data.Title + "</span>";
    }

    DragEvents()
    {
        this.Content.addEventListener('click', (e) =>
        {
            if (e.target == this.Sizer)
                return;
            var act = 0;
            if (e.ctrlKey)
                act = 1;
            if (e.ctrlKey && e.shiftKey)
                act = 2;
            this.Table.Body.SortData(this, act);
            this.Table.ResizeColumn(this, this.HTML.GetRect().Width);
        });

        this.HTML.draggable = true;
        this.HTML.addEventListener('dragstart', (e) =>
        {
            XDragUtils.SetData(this);
            this.HTML.classList.add('dragging');
        });

        this.HTML.addEventListener('dragend', (e) =>
        {
            this.HTML.classList.remove('dragging');
        });

        this.HTML.addEventListener('dragover', (e) =>
        {
            e.preventDefault();
            var elm = XDragUtils.GetData<XElement>();
            if (elm == null || elm.UUID == this.UUID)
                return;

            var w = this.HTML.GetRect().Width;
            if (e.offsetX <= 5 || e.offsetX + 6 >= w)
                return;

            this.HTML.classList.remove('ldrag-over');
            this.HTML.classList.remove('rdrag-over');
            if (e.offsetX > w / 2)
                this.HTML.classList.add('rdrag-over');
            else
                this.HTML.classList.add('ldrag-over');
        });

        this.HTML.addEventListener('dragleave', () =>
        {
            this.HTML.classList.remove('ldrag-over');
            this.HTML.classList.remove('rdrag-over');
        });

        this.HTML.addEventListener('drop', (e) =>
        {
            e.preventDefault();
            this.HTML.classList.remove('ldrag-over');
            this.HTML.classList.remove('rdrag-over');
            const elm = XDragUtils.GetData<XTableHCell>();
            if (this.Owner instanceof XElement && elm.UUID != this.UUID)
            {
                var w = this.HTML.clientWidth / 2;
                if (e.offsetX > w)
                    this.MoveTo(this, elm);
                else
                    this.MoveTo(elm, this);
            }
        });
    }

    MoveTo(pLeft: XTableHCell, pRight: XTableHCell)
    {
        if (this.Owner instanceof XElement)
        {
            this.Owner.HTML.insertBefore(pLeft.HTML, pRight.HTML);
            this.Table.MoveTo(pLeft, pRight);
        }

    }

    private ResizerEvents()
    {
        let isResizing = false;
        let startX = 0;
        let startWidth = 0;

        this.Sizer.addEventListener('mousedown', (e) =>
        {
            e.stopPropagation();
            e.preventDefault();

            isResizing = true;
            startX = e.clientX;
            startWidth = this.Content.offsetWidth;

            const handleMouseMove = (e: MouseEvent) =>
            {
                if (!isResizing)
                    return;
                const newWidth = startWidth + (e.clientX - startX);
                this.Content.style.width = `${newWidth}px`;
                this.Data.Width = newWidth;
                this.Table.ResizeColumn(this, newWidth);
            };

            const handleMouseUp = () =>
            {
                isResizing = false;
                document.removeEventListener('mousemove', handleMouseMove);
                document.removeEventListener('mouseup', handleMouseUp);
                this.Table.ResizeColumn(this, this.Content.GetRect().Width, true);
            };

            document.addEventListener('mousemove', handleMouseMove);
            document.addEventListener('mouseup', handleMouseUp, { once: true });
        });
    }
}

class XTableHRow extends XTableElement
{
    constructor(pOwner: XTableHeader)
    {
        super(pOwner, null, "tr");
        this.Header = pOwner;
    }
    Header: XTableHeader;
}

class XTableHeader extends XElement
{
    constructor(pOwner: XElement | HTMLElement | null, pTable: XTable)
    {
        super(pOwner, "XTableHeader");
        this.TRows = new XTableHRow(this);
        this.Table = pTable;

    }
    TRows: XTableHRow;
    Columns = new XArray<XTableHCell>();
    Table: XTable;

    Clear()
    {
        this.TRows.HTML.innerHTML = "";
    }

    AddColumns(pClass: string): XTableHCell
    {
        var cell = new XTableHCell(this.TRows, pClass);
        cell.Table = this.Table;
        this.Columns.Add(cell);
        return cell;
    }

    protected override CreateContainer(): HTMLElement 
    {
        return XUtils.AddElement<HTMLTableElement>(null, "thead", null);
    }
}

class XTableBody extends XElement
{
    constructor(pOwner: XElement | HTMLElement, pTable: XTable)
    {
        super(pOwner, "");
        this.Table = pTable;
        this.BRows = new XTableRow(this);
    }
    BRows: XTableElement;
    DataRows = new XArray<XTableRow>();
    Table: XTable;
    SortCells: Array<XTableHCell> = new Array<XTableHCell>();

    SortData(pCell: XTableHCell, pAction: number): any
    {
        switch (pAction)
        {
            case 0:
                this.SortCells = new Array<XTableHCell>();
                break;
            case 2:
                this.SortCells.Remove(pCell);
                break;
        }
        if (!this.SortCells.Any(c => c == pCell) && pAction != 2)
            this.SortCells.Add(pCell);
        let field = pCell.Data.Title;
        this.Table.Header.Columns.ForEach(c =>
        {
            if (!this.SortCells.Any(cc => cc == c))
            {
                c.SortIcon.innerHTML = "";
                c.Table.ResizeColumn(c, c.HTML.GetRect().Width);

            }
        });
        if (pAction != 2)
        {
            if (!X.IsEmpty(pCell.SortIcon.innerHTML))
                pCell.SortState.Direction = pCell.SortState.Direction === 'asc' ? 'desc' : 'asc';
            else
                pCell.SortState = { Field: field, Direction: 'asc' };

            pCell.SortIcon.innerHTML = pCell.SortState.Direction === 'asc' ? ' ▲' : ' ▼';
        }
        this.DataRows.sort((a, b) =>
        {

            for (var i = 0; i < this.SortCells.length; i++)
            {
                let cell = this.SortCells[i];
                if (a.Tupla[cell.Data.Title] > b.Tupla[cell.Data.Title])
                    return cell.SortState.Direction === 'asc' ? 1 : -1;
                if (a.Tupla[cell.Data.Title] < b.Tupla[cell.Data.Title])
                    return cell.SortState.Direction === 'asc' ? -1 : 1;

            }
            return 0;
        });

        while (this.HTML.firstChild)
            this.HTML.removeChild(this.HTML.firstChild);

        for (var i = 0; i < this.DataRows.length; i++)
        {
            var row = this.DataRows[i];
            if (i % 2 != 0)
                row.HTML.className = "XTableRowEven";
            else
                row.HTML.className = "XTableRow";
            this.HTML.appendChild(row.HTML);
        }
    }

    Clear()
    {
        this.HTML.innerHTML = "";
    }

    AddRow(): XTableRow
    {
        var row = new XTableRow(this);
        this.DataRows.Add(row);
        return row;
    }

    protected override CreateContainer(): HTMLElement 
    {
        return XUtils.AddElement<HTMLTableElement>(null, "tbody", null);
    }
}

class XTableRow extends XTableElement
{
    constructor(pOwner: XTableBody)
    {
        super(pOwner, "XTableRow", "tr");
        this.Body = pOwner;
        this.Table = pOwner.Table;
        XEventManager.AddEvent(this.Table, this.HTML, XEventType.Click, () => this.Table.DoSelectRow(this));
    }
    Table: XTable;
    Body: XTableBody;
    Tupla: any;
    Cell = new XArray<XTableCell>();

    SetData(pTupla: XDataTuple)
    {
        this.Tupla = pTupla;
        this.CreateCell();
    }

    CreateCell()
    {
        if (this.Table.Columns == null)
            return;
        for (var i = 0; i < this.Table.Columns.length; i++)
        {
            let cell = new XTableCell(this, "XTd");
            cell.SetData(this.Tupla[this.Table.Columns[i].Name].Value, this.Table.Header.Columns[i]);
            this.Cell.Add(cell);
        }
    }
}

class XTableCell extends XTableElement
{

    constructor(pOwner: XTableRow, pClass: string)
    {
        super(pOwner, pClass, "td");
        this.Content = XUtils.AddElement<HTMLDivElement>(this, "div", "XTableCellContent");
        this.Table = pOwner.Body.Table;
        this.Row = pOwner;
        this.Text = XUtils.AddElement<HTMLSpanElement>(this.Content, "div", "XTableCellTitle");

    }
    Content: HTMLDivElement;
    Text: HTMLSpanElement;
    Table: XTable;
    Row: XTableRow;
    HCell: XTableHCell | any;
    Data: any;

    SetData(pData: any, pHCell: XTableHCell)
    {
        this.HCell = pHCell;
        this.Data = pData
        this.Text.innerHTML = "<spans>" + this.Data + "</span>";
    }
}

class XTable extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null)
    {
        super(pOwner, pClass);
        this.Container = XUtils.AddElement<HTMLDivElement>(this, "table");
        this.Header = new XTableHeader(this.Owner, this);
        this.Body = new XTableBody(this.Container, this);
        XEventManager.AddEvent(this, this.HTML, XEventType.Scroll, this.PositioningHeader);
        this.RowNumberColumn = <XColumnConfig>{ Name: "RowNumber", Visible: true, Width: 50 };
    }
    Container: HTMLDivElement;
    Header: XTableHeader;
    Body: XTableBody;
    Columns: XColumnConfig[] | null = null;
    protected DataSet!: XDataSet;
    private RowNumberColumn: XColumnConfig;
    OnRowClick: XMethod<XArray<XTableRow>> | null = null;

    DoSelectRow(pRow: XTableRow)
    {
        if (this.OnRowClick != null)
            this.OnRowClick.apply(this, [[pRow]]);
    }


    PositioningHeader(pArg: MouseEvent)
    {
        this.Header.HTML.style.left = `-${this.HTML.scrollLeft}px`;
    }

    ResizeColumn(pHeaderCell: XTableHCell, pWidth: number, pCheck: boolean = false)
    {
        if (this.Body.DataRows.length == 0)
            return;
        var dcell = this.Body.DataRows[0].Cell.FirstOrNull(c => c.HCell == pHeaderCell);
        if (dcell != null)
        {
            if (pCheck)
            {
                if (pWidth > dcell.Content.clientWidth)
                    dcell.Content.style.width = `${pWidth}px`;
                else
                    pHeaderCell.Content.style.width = `${dcell.Content.GetRect().Width}px`;
            }
            else
                dcell.Content.style.width = `${pWidth}px`;
        }
    }


    MoveTo(pLeft: XTableHCell, pRight: XTableHCell)
    {
        if (this.Columns == null)
            return;
        var left = this.Body.DataRows[0].Cell.IndexOf(c => c.HCell == pLeft);
        var right = this.Body.DataRows[0].Cell.IndexOf(c => c.HCell == pRight);
        for (var i = 0; i < this.Body.DataRows.length; i++)
        {
            var row = this.Body.DataRows[i];
            var cl = row.Cell[left];
            var cr = row.Cell[right];
            row.HTML.insertBefore(cl.HTML, cr.HTML);
        }
    }

    GetVisibleColumns(): Array<XColumnConfig>
    {
        if (this.Columns == null)
            return new Array<XColumnConfig>();
        return [this.RowNumberColumn, ...this.Columns.filter(c => c.Visible)];
    }

    SetColumns(pColumn: XColumnModel[])
    {
        this.Columns = pColumn.Select(c => <XColumnConfig>{ Name: c.Name, Title: c.Name, Visible: true, Width: 100 });
        this.CreateHeader();
    }

    SetDataSet(pDataSet: XDataSet)
    {
        this.DataSet = pDataSet;
        this.CreateBody();
    }

    CreateBody()
    {
        this.Body.Clear();
        if (this.Columns == null)
            return;
        for (var i = 0; i < this.DataSet.Tuples.length; i++)
        {
            let row = this.Body.AddRow();
            if (i % 2 != 0)
                row.HTML.className = "XTableRowEven";
            row.SetData(this.DataSet.Tuples[i]);
        }
        XEventManager.SetTiemOut(this, this.AdjustCollumnWidth, 100);
    }

    private AdjustCollumnWidth()
    {
        if (this.Body.DataRows.length > 0)
        {
            var row = this.Body.DataRows[0];
            for (var i = 0; i < row.Cell.length; i++)
            {
                let bcell = row.Cell[i];
                let hcell = this.Header.Columns[i];
                let bw = bcell.HTML.clientWidth;
                let hw = hcell.HTML.clientWidth;
                if (bw > hw)
                    hcell.Content.style.width = `${bw}px`;

                else
                    bcell.Content.style.width = `${hw}px`;

            }
        }
    }

    CreateHeader()
    {
        this.Body.Clear();
        if (this.Columns == null)
            return;
        for (var i = 0; i < this.Columns.length; i++)
        {
            let col = this.Columns[i];
            let cell = this.Header.AddColumns("XTh");
            cell.SetData(col);
        }
    }
}