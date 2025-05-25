/// <reference path="XStringEditor.ts" />

class XSearchBoxEditor extends XStringEditor 
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Input.className = "XSearchBoxEditor";
        this.Title = "Digite um Texto";
        this.Button = new XSVGButton(this, "XSearchBoxEditorButton");
        this.Button.SVG.className = "XSearchIcon";    
        this.Button.SetIcon("svg/search.svg");
    }
    Button: XSVGButton;
}