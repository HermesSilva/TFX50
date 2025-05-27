/// <reference path="../Elements/Base/XBaseInput.ts" />
class XStringEditor extends XBaseInput
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Title = "Digite um Texto";
        XEventManager.AddEvent(this, this.Input, XEventType.Input, this.OnInput);
    }

    private OnInput()
    {
        if (this.Mask)
            this.ApplyMask()
    }

    protected override ApplyMask()
    {
        if (X.IsEmpty(this.Mask) || X.IsEmpty(this.Input.value))
            return;
        
        this.Input.value = XUtils.ApplyMask(this.Input.value, this.Mask);
    }

    override CreateInput(): HTMLInputElement
    {
        return XUtils.AddElement<HTMLInputElement>(this.HTML, "input", "XBaseButtonInput");
    }
}