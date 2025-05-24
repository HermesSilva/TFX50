/// <reference path="XStringEditor.ts" />

class XSearchBoxEditor extends XStringEditor 
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Title = "Digite um Texto";
    }
}