/// <reference path="Base/XBaseButton.ts" />

class XSVGButton extends XBaseButton 
{
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null = null)
    {
        super(pOwner, pClass ?? "XSVGButton");
        this.SVG = XUtils.AddElement<HTMLImageElement>(this.HTML, "img", "ButtonBarIcon");
    }
    SVG: HTMLImageElement;
    SetIcon(pIcon: string)
    {
        this.SVG.src = pIcon;
    }
}