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
        this.Option = new XSVGButton(this, "XSearchBoxEditorButtonOpt");
        this.Option.SVG.className = "XSearchIcon";
        this.Option.SetIcon("svg/option.svg");
    }

    Button: XSVGButton;
    Option: XSVGButton;
    Columns!: XColumnModel[];

    SetFields(pColumns: XColumnModel[])
    {
        this.Columns = pColumns;
        this.Columns.ForEach((c) =>this.AddField(c));
    }

    AddField(pColumns: XColumnModel)
    {
        var tag = new XEditableTag(this.Input);
        tag.SetModel(pColumns);
        tag.Editor.Title.innerHTML = pColumns.Title;
        tag.OnClick = (pTag: XEditableTag) => this.Close(pTag);
    }

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