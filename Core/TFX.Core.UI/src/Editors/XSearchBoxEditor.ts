/// <reference path="XStringEditor.ts" />

class XSearchBoxEditor extends XStringEditor 
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Input.className = "XSearchBoxEditor";
        this.ELMTitle.HTML.innerHTML = "Pesquisa ";
        this.Button = new XSVGButton(this, "XSearchBoxEditorButton");
        this.Button.SVG.className = "XSearchIcon";    
        this.Button.SetIcon("svg/search.svg");
        //this.Campo = new XDiv(this, "InputTitle");
    }
    Button: XSVGButton;

    get Title(): string
    {
        return this.ELMTitle.HTML.innerHTML;
    }
    set Title(pValue: string)
    {
        
    }
}