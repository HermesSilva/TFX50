/// <reference path="../Elements/Base/XBaseInput.ts" />
class XMemoEditor extends XBaseInput
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Input.className = "XMemoEditor";
        this.Title = "Digite um Texto";
    }

    override CreateInput(): HTMLInputElement
    {
        return XUtils.AddElement<HTMLInputElement>(this.HTML, "textarea", "XBaseButtonInput");
    }
}