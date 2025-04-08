/// <reference path="../Elements/Base/XBaseInput.ts" />
class XNormalEditor extends XBaseInput
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Input.className = "XNormalEditor";
        this.Title = "Digite uma Frase";
    }
}