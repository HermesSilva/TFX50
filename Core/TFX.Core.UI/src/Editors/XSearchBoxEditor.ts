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
        this.Button.OnClick = (e) => this.DoSerach(e);
        this.Option = new XSVGButton(this, "XSearchBoxEditorButtonOpt");
        this.Option.SVG.className = "XSearchIcon";
        this.Option.SetIcon("svg/option.svg");
    }

    Button: XSVGButton;
    Option: XSVGButton;
    Columns!: XColumnModel[];
    OnSerach?: XMethod<any>;
    Fields: XArray<XEditableTag> = new XArray<XEditableTag>();

    DoSerach(e?: Event): void
    {
        if (this.OnSerach)
            this.OnSerach(this.GetFilter());
    }

    GetFilter(): XFilter
    {
        let filter: any = new Object();
        for (let i = 0; i < this.Fields.length; i++)
        {
            let fld = this.Fields[i];
            if (fld.Value != null && fld.Value != "")
            {
                let ffld = new Object() as XFilterField;
                ffld.Name = fld.Columns.Name;
                ffld.Operator = fld.Columns.Operator;
                ffld.State = XFieldState.NotEmpty;
                ffld.Value = fld.Value;
                filter[fld.Columns.Name] = ffld;
            }
        }

        return filter;  
    }

    SetFields(pColumns: XColumnModel[])
    {
        this.Columns = pColumns;
        this.Columns.ForEach((c) => this.AddField(c));
        if (this.Rows == 2)
        {
            this.Button.HTML.className = "XSearchBoxEditorButtonIL";
            this.Option.HTML.className = "XSearchBoxEditorButtonOptIL";
        }
    }

    AddField(pColumns: XColumnModel)
    {
        let tag = new XEditableTag(this.Input);
        tag.SetModel(pColumns);
        tag.Editor.Title.innerHTML = pColumns.Title;
        tag.OnClick = (pTag: XEditableTag) => this.Close(pTag);
        this.Fields.Add(tag);

        // Dispara a pesquisa ao pressionar Enter dentro do editor do tag
        if (tag.Editor && tag.Editor.Editor && tag.Editor.Editor.Input)
            XEventManager.AddEvent(this, tag.Editor.Editor.Input, XEventType.KeyDown, this.OnFieldKeyDown);
    }

    private OnFieldKeyDown(e: KeyboardEvent)
    {
        if (e.key === "Enter")
            this.DoSerach(e);
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

