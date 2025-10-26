/// <reference path="Base/XBaseButton.ts" />

class XSVGButton extends XBaseButton 
{
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null = null)
    {
        super(pOwner, pClass ?? "XSVGButton");
        this.SVG = XUtils.AddElement<HTMLImageElement>(this.HTML, "img", "ButtonBarIcon");
        XEventManager.AddEvent(this, this.SVG, XEventType.Click, this.DoClick, true);
    }
    SVG: HTMLImageElement;
    OnClick?: XMethod<MouseEvent>;

    DoClick(pEvent: MouseEvent)
    {
        if (this.OnClick)
            this.OnClick(pEvent);
    }

    SetIcon(pIcon: string)
    {
        this.SVG.src = pIcon;
    }
}
