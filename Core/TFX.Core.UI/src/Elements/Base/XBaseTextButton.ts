/// <reference path="XBaseButton.ts" />
class XBaseTextButton extends XBaseButton
{
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null)
    {
        super(pOwner, pClass);
        this.Text = XUtils.AddElement<HTMLSpanElement>(this, "span");
    }

    protected Text: HTMLSpanElement;

    get Title(): string
    {
        return this.Text.innerHTML;
    }
    set Title(pValue: string)
    {
        this.Text.innerHTML = pValue;
    }
}