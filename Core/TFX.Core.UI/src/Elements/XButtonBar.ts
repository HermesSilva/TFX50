/// <reference path="XDiv.ts" />
class XButtonBar extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "XButtonBar");
        this.Close = new XSVGButton(this);
    }

    Close: XSVGButton;

}