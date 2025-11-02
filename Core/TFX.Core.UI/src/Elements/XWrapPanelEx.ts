/// <reference path="XDiv.ts" />

enum XWrapStartSide
{
    Left = 0,
    Right = 1
}

class XWrapPanelEx extends XDiv 
{
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null = null)
    {
        super(pOwner, pClass ?? "XWrapPanelEx");
        XEventManager.AddObserver(this, { childList: true, subtree: false }, this.AlignChildren);
    }

    public HorizontalSpacing: number = 4;
    public VerticalSpacing: number = 4;
    public StartSide: XWrapStartSide = XWrapStartSide.Left;

    override SizeChanged()
    {
        super.SizeChanged();
        this.AlignChildren();
    }

    public AlignChildren()
    {
        const rect = this.HTML.GetRect(true);
        const maxHeight = rect.Height;
        const hgap = this.HorizontalSpacing || 0;
        const vgap = this.VerticalSpacing || 0;

        if (maxHeight <= 0)
            return;

        let items: HTMLElement[];
        if (this.StartSide === XWrapStartSide.Left)
            items = <HTMLElement[]>this.Children.Where(e => e.IsVisible).OrderBy(c => c.OrderIndex).Select(c => c.HTML).ToArray();
        else
            items = <HTMLElement[]>this.Children.Where(e => e.IsVisible).OrderByDescending(c => c.OrderIndex).Select(c => c.HTML).ToArray();

        let colWidth = rect.Width;
        let y = 0;
        let x = 0;
        let my = maxHeight;

        for (let i = 0; i < items.length; i++)
        {
            const el = items[i];
            if (!el)
                continue;

            const elRect = el.GetRect();
            const w = elRect.Width;
            const h = elRect.Height;

            if (x + w > colWidth)
            {
                y += h + vgap;
                x = 0;
            }

            const left = this.StartSide === XWrapStartSide.Left ? x : (colWidth - x - w);
            el.style.left = left + 'px';
            el.style.top = y + 'px';
            x += w + hgap;
            my = Math.max(my, y + h);
        }
        this.HTML.style.height = my + 'px';
    }
}
