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
    }

    Fields: XArray<XIEditor> = new XArray<XIEditor>();
    Model!: XFRMModel;
    SVCModel!: XServiceModel;

    SetModel(pForm: XFRMModel, pSVCModel: XServiceModel)
    {
        this.Model = pForm;
        this.SVCModel = pSVCModel;
        this.Fields.Clear();
        this.SetTitle(pForm.Title);
        this.SetDescription(pForm.Description);
        this.SetIcon(pForm.Icon);
        for (const field of pForm.Fields)
        {
            var editor = XEditorFactory.CreateEditor(this, field);
            this.Fields.Add(editor);
            if (editor instanceof XSearchBoxEditor)
            {
                editor.SetFields(pSVCModel.DataView.Columns.Where(c => c.IsFreeSearch));
            }
        }
        this.ResizeChildren();

    }

    SetTitle(pTitle: string)
    {
    }

    SetDescription(pDescription: string)
    {
    }

    SetIcon(pIcon: any)
    {
    }

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

        let maxBottom = 0;

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

                        // Track the bottom of the last field
                        const bottom = y + crows * cellh;
                        if (bottom > maxBottom)
                            maxBottom = bottom;

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

        // Set form height based on positioned fields
        if (maxBottom > 0)
        {
            this.HTML.style.height = `${Math.ceil(maxBottom)}px`;
        }
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

