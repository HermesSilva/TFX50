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
    SVCModel!: XIServiceModel;
    private _FocusTarget: HTMLElement | null = null;
    private _FocusTries: number = 0;

    SetDataSet(pDataSet: XDataSet)
    {
        var tpl = <any>pDataSet.Tuples.FirstOrNull();
        for (const field of this.Fields)
        {
            field.Tuple = tpl;
            XBinding.Bind(tpl[field.Field.Name], field, "Value", "RawValue", true);
        }
    }

    SetModel(pForm: XFRMModel, pSVCModel: XIServiceModel)
    {
        this.Model = pForm;
        this.SVCModel = pSVCModel;
        this.Fields.Clear();
        this.SetTitle(pForm.Title);
        this.SetDescription(pForm.Description);
        this.SetIcon(pForm.Icon);
        for (const field of pForm.Fields)
        {
            let editor = XEditorFactory.CreateEditor(this, field);
            editor.SetField(field, pSVCModel);
            this.Fields.Add(editor);

        }
        this.ResizeChildren();
    }

    FocusFirstInput(pTries: number = 10)
    {
        const el = this.GetFirstFocusable();
        if (!el)
        {
            if (pTries > 0)
                XEventManager.SetTiemOut(this, () => this.FocusFirstInput(pTries - 1), 30);
            return;
        }
        this._FocusTarget = el;
        this._FocusTries = Math.max(4, pTries);
        XEventManager.SetTiemOut(this, this.EnsureFocusTick, 0);
    }

    private EnsureFocusTick()
    {
        const el = this._FocusTarget;
        if (!el)
            return;
        if (document.activeElement === el)
            return;
        this.TryFocusElement(el);
        if (document.activeElement !== el && this._FocusTries > 0)
        {
            this._FocusTries--;
            XEventManager.SetTiemOut(this, this.EnsureFocusTick, 30);
        }
    }

    private TryFocusElement(pEl: HTMLElement)
    {
        try { (pEl as any).focus({ preventScroll: true }); }
        catch { try { pEl.focus(); } catch { } }
    }

    private GetFirstFocusable(): HTMLElement | null
    {
        const tabs = this.SortRectangles(this.Fields);
        for (let i = 0; i < tabs.length; i++)
        {
            const ed = tabs[i];
            if (!ed)
                continue;
            const el = this.GetEditorFocusTarget(ed);
            if (el)
                return el;
        }
        return null;
    }

    private GetEditorFocusTarget(pEditor: XIEditor): HTMLElement | null
    {
        const el = pEditor.Input as any as HTMLElement;
        if (this.IsAcceptableInput(el))
            return el;
        const nodes = pEditor.HTML.querySelectorAll('input,textarea,select');
        for (let i = 0; i < nodes.length; i++)
        {
            const nd = nodes[i] as HTMLElement;
            if (this.IsAcceptableInput(nd))
                return nd;
        }
        return null;
    }

    private IsAcceptableInput(pEl: HTMLElement | null): boolean
    {
        if (!pEl)
            return false;
        if (!this._IsVisibleInput(pEl))
            return false;
        const tag = pEl.tagName ? pEl.tagName.toUpperCase() : "";
        if (tag === 'INPUT')
        {
            const inp = pEl as HTMLInputElement;
            if (inp.type === 'hidden')
                return false;
            if (inp.disabled)
                return false;
            if (inp.readOnly)
                return false;
            return true;
        }
        if (tag === 'TEXTAREA')
        {
            const ta = pEl as HTMLTextAreaElement;
            if (ta.disabled)
                return false;
            if (ta.readOnly)
                return false;
            return true;
        }
        if (tag === 'SELECT')
        {
            const sl = pEl as HTMLSelectElement;
            if (sl.disabled)
                return false;
            return true;
        }
        return false;
    }

    private _IsVisibleInput(pEl: HTMLElement): boolean
    {
        if (!document.body.contains(pEl))
            return false;
        const rects = pEl.getClientRects();
        if (!rects || rects.length === 0)
            return false;
        const cs = window.getComputedStyle(pEl);
        if (cs.visibility === 'hidden' || cs.display === 'none')
            return false;
        return true;
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
                        let r = new XRect(x, y, ccols * cellw, crows * cellh);
                        r.Inflate(-2, -2);
                        child.Rect = r;

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
        let tidx = 1;
        let tabs = this.SortRectangles(this.Fields);
        for (const child of tabs)
            child.Input.tabIndex = tidx++;

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


