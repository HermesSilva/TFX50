/// <reference path="XStringEditor.ts" />

class XSearchBoxEditor extends XBaseInput 
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.ELMTitle.HTML.innerHTML = "Pesquisa ";
        this.Button = new XSVGButton(this, "XSearchBoxEditorButton");
        this.Button.SVG.className = "XSearchIcon";
        this.Button.SetIcon("svg/search.svg");
        for (var i = 0; i < 10; i++)
        {
            this.EditableTag = new XEditableTag(this.Input);
            this.EditableTag.OnClick = (pTag: XEditableTag) => this.Close(pTag);
        }

    }
    Button: XSVGButton;
    EditableTag!: XEditableTag;

    Close(pTag: XEditableTag)
    {
        pTag.Free();
    }

    CreateInput(): HTMLInputElement
    {
        return XUtils.AddElement<HTMLInputElement>(this.HTML, "div", "XSearchBoxEditor");
    }

    get Title(): string
    {
        return this.ELMTitle.HTML.innerHTML;
    }
    set Title(pValue: string)
    {

    }
}