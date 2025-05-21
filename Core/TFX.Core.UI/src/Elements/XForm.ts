class XType1
{
    Point: XPoint = new XPoint();
    LeftX: number = 0;
    LeftY: number = 0;
    Used: boolean = false;
    EndX: number = -1;
    StartX: number = -1;

}

class XEditPosition
{
    constructor(pLocation: XPoint)
    {
        this.Point = pLocation;
    }

    Used: boolean = false;
    Point: XPoint;
}
class XForm extends XDiv
{

    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "XForm");
        var edt: XIEditor | any;

        edt = new XNormalEditor(this);
        edt.Rows = 1;
        edt.Cols = 4;
        edt.OrderIndex = 1;
        this.Fields.Add(<any>edt);

        this.ResizeChildren();
    }
    Fields: XArray<XIEditor> = new XArray<XIEditor>();

    override SizeChanged()
    {
        this.ResizeChildren();
    }

    ResizeChildren()
    {
        const cols = XDefault.DefaultColCount;
        const rows = 80;
        const cellw = this.HTML.GetRect(true).Width / cols;
        const cellh = XDefault.DefaultRowHeight;

        const ordered = this.Fields.OrderBy(c => c.OrderIndex);

        const grid: boolean[][] = Array.from({ length: rows }, () => new Array(cols).fill(false));

        for (const child of ordered)
        {
            const ccols = child.Cols;
            const crows = child.Rows;

            if (ccols > cols || crows > rows)
                continue;

            let placed = false;

            for (let row = 0; row <= rows - crows; row++)
            {
                for (let col = 0; col <= cols - ccols; col++)
                {
                    let fplace = true;
                    for (let r = row; r < row + crows; r++)
                    {
                        for (let c = col; c < col + ccols; c++)
                        {
                            if (grid[r][c])
                            {
                                fplace = false;
                                break;
                            }
                        }
                        if (!fplace)
                            break;
                    }

                    if (fplace)
                    {
                        for (let r = row; r < row + crows; r++)
                            for (let c = col; c < col + ccols; c++)
                                grid[r][c] = true;

                        const x = col * cellw;
                        const y = row * cellh;
                        var r = new XRect(x, y, ccols * cellw, crows * cellh);
                        r.Inflate(-2, -2);
                        child.Rect = r;

                        placed = true;
                        break;
                    }
                }
                if (placed)
                    break;
            }
        }
        var tidx = 1;
        var tabs = this.SortRectangles(this.Fields);
        for (const child of tabs)
            child.Input.tabIndex = tidx++;
    }

    SortRectangles(rectangles: XArray<XIEditor>): XArray<XIEditor>
    {
        return rectangles.sort((a, b) =>
        {

            if (a.Rect.Top < b.Rect.Top)
                return -1;
            if (a.Rect.Top > b.Rect.Top)
                return 1;

            if (a.Rect.Left < b.Rect.Left)
                return -1;
            if (a.Rect.Left > b.Rect.Left)
                return 1;

            return 0;
        });
    }

}

