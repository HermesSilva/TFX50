/// <reference path="../XDiv.ts" />
class XBaseInput extends XDiv implements XIEditor
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "InputContainer");
        this.Input = this.CreateInput();
        this.ELMTitle = new XDiv(this, "InputTitle");

    }
    Input: HTMLInputElement;
    protected ELMTitle: XDiv;
    NewLine: boolean = false;
    OrderIndex: number = -1;

    get Title(): string
    {
        return this.ELMTitle.HTML.innerHTML;
    }
    set Title(pValue: string)
    {
        this.ELMTitle.HTML.innerHTML = pValue;
    }

    CreateInput(): HTMLInputElement
    {
        return XUtils.AddElement<HTMLInputElement>(this.HTML, "input", "XBaseButtonInput");
    }
}