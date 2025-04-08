/// <reference path="XElement.ts" />
class XBaseButton extends XElement
{
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null)
    {
        super(pOwner, pClass);
    }
    
    protected override CreateContainer(): HTMLElement 
    {
        return XUtils.AddElement<HTMLElement>(null, "div", null);
    }
}