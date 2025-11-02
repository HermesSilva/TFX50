/// <reference path="XStringEditor.ts" />
class XSearchBoxEditor extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "XFilter");

        this.FTitle = XUtils.AddElement<HTMLDivElement>(this.HTML, "div", "XEditorTitle");
        this.Title = "Pesquisa ";

        this.Rail = new XDiv(this, "XFilterRail");
        this.Container = new XWrapPanelEx(this, "XFilterContainerEditor");
        this.Container.OnResize = () => this.ChildSizeChanged();
        this.Option = new XSVGButton(this.Rail, "Search   ");
        this.Option.SVG.className = "XSearchIcon";
        this.Option.SetIcon("svg/option.svg");

        this.Button = new XSVGButton(this.Rail, "Dots");
        this.Button.SVG.className = "XSearchIcon";
        this.Button.SetIcon("svg/search.svg");
        this.Button.OnClick = (e) => this.DoSerach(e);
    }


    FTitle: HTMLDivElement;
    Rail: XDiv;
    Container: XWrapPanelEx;
    Button: XSVGButton;
    Option: XSVGButton;
    Columns!: XColumnModel[];
    Form!: XFRMModel;
    OnSerach?: XMethod<any>;
    Fields: XArray<XEditableTag> = new XArray<XEditableTag>();
    AppSVCModel?: XServiceModel;

    ChildSizeChanged()
    {
        var cr = this.Container.HTML.GetRect();
        var top = this.Container.HTML.StyleValue("top");
        if (cr.Height > 0)
            this.HTML.style.minHeight = (top + cr.Height + 5) + "px";
    }

    SetModel(pSVCModel: XServiceModel, pForm: XFRMModel, pColumns: XColumnModel[])
    {
        this.AppSVCModel = pSVCModel;
        this.Form = pForm;
        this.Columns = pColumns;
        this.SetFields(this.Columns.Where(c => c.IsFreeSearch));
    }

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
    }

    AddField(pColumn: XColumnModel)
    {
        let tag = new XEditableTag(this.Container);
        let ffld = this.Form.Fields.FirstOrNull(f => f.TargetDisplayFieldID.Any(dfld => pColumn.FieldID == dfld));
        ffld = ffld ?? this.Form.Fields.FirstOrNull(f => f.Name == pColumn.Name);
        tag.SetModel(pColumn, ffld);
        tag.Title.innerHTML = pColumn.Title;
        this.Fields.Add(tag);

        if (tag.Input)
            XEventManager.AddEvent(this, tag.Input, XEventType.KeyDown, this.OnFieldKeyDown);
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
        return <any>this.HTML;
    }

    get Title(): string
    {
        return this.FTitle.innerHTML;
    }

    set Title(pValue: string)
    {
        this.FTitle.innerHTML = pValue;
    }
}

