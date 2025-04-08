/// <reference path="XBaseInput.ts" />
class XBaseButtonInput extends XBaseInput 
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Button = new XBaseButton(this, "XLookupButton");
        XEventManager.AddEvent(this, this.Button.HTML, XEventType.Click, this.OnClick, true);
    }

    Button: XBaseButton;

    OnClick(pArg: KeyboardEvent)
    {
    }
}