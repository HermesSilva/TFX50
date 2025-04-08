/// <reference path="XBaseInput.ts" />
class XBaseLoockupInput extends XBaseInput 
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Button = new XBaseButton(this, "XLookupButton");
        this.DropDownContent = this.CreateDropDown();
        XEventManager.AddEvent(this, this.Button.HTML, XEventType.Click, this.OnClick, true);
    }

    Button: XBaseButton;
    DropDownContent: XDropDownElement;

    protected CreateDropDown(): XDropDownElement
    {
        return new XDropDownElement(<any>this.Owner, "XDropDown");
    }

    OnClick(pArg: KeyboardEvent)
    {
        this.DropDownContent.BindTo(this);
        this.DropDownContent.Show();
    }
}