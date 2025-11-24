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
        {
            if (!this.ApplyMask())
                this.RawValue = XUtils.UnMask(this.Input.value, this.Mask);
        }
        else
            this.RawValue = this.Input.value;
    }
    get RawValue(): any
    {
        return this._RawValue;
    }

    set RawValue(value: any)
    {
        if (this._RawValue == value)
            return;
        this._RawValue = value;
        if (this.Mask)
        {
            this.Input.value = value;
            this.ApplyMask();
        }
        else
            if (this.Input.value != value)
                this.Input.value = value;
    }

    protected override ApplyMask(): boolean
    {
        super.ApplyMask();
        if (X.IsEmpty(this.Mask) || X.IsEmpty(this.Input.value))
            return false;
        var msk = XUtils.ApplyMask(this.Input.value, this.Mask);
        var unv = XUtils.UnMask(this.Input.value, this.Mask);
        if (this.Input.value != msk)
            this.Input.value = msk
        this.RawValue = unv;
        return true;
    }

    override CreateInput(): HTMLInputElement
    {
        return XUtils.AddElement<HTMLInputElement>(this.HTML, "input", "XBaseButtonInput");
    }
}