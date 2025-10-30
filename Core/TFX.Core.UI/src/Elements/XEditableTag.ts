/// <reference path="Base/XElement.ts" />
/// <reference path="../Reflection/XReflections.ts" />

class XTagEditor extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null)
    {
        super(pOwner, pClass);
        this.Title = XUtils.AddElement<HTMLSpanElement>(this.HTML, "span", "XTagEditorTitle");
        this.Title.innerHTML = "Tag Editor";
    }

    Editor!: XIEditor;
    Title: HTMLSpanElement;
    SVG!: HTMLImageElement;
    private _LIconSrc?: string;

    SetModel(pColumns: XColumnModel)
    {
        let edttype = XEditorFactory.DataTypeToEditorType(pColumns.Type);
        this.Editor = XEditorFactory.NewEditor(edttype, this) as XIEditor;
        this.Editor.RemoveTitle()
        this.Editor.HTML.className = "XTagEditorInput";
        this.Editor.Input.className = "XTagEditorInput";
        this.Editor.Input.addEventListener('input', () => this.OnInput(), false)
        this.Editor.Mask = pColumns.Mask;
        this.Title.addEventListener('click', () => this.Editor.Input.focus(), false);
        this.SVG = XUtils.AddElement<HTMLImageElement>(this.HTML, "img", "XTagEditorSVG");
        this.UpdateSVGIcon();
    }

    public Clear()
    {
        this.Editor.Input.value = "";
        this.Editor.RawValue = null;
        this.OnInput();
        this.UpdateSVGIcon();
    }

    private OnInput()
    {
        let w = XUtils.ApplySize(this.Editor.HTML, this.Editor.Input.value);
        this.Editor.Input.style.width = w + "px";
        this.UpdateSVGIcon();
    }

    private UpdateSVGIcon()
    {
        const hasContent = this.Editor?.Input?.value?.length > 0;
        const iconSrc = hasContent ? "svg/tinyclosebold.svg" : "svg/tinyclose.svg";
        if (this._LIconSrc != iconSrc)
            this.SVG.src = this._LIconSrc = iconSrc;
    }
}

class XEditableTag extends XDiv 
{
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null = null)
    {
        super(pOwner, pClass ?? "XEditableTag");
    }

    Editor!: XTagEditor;
    Columns!: XColumnModel;
    OnClick!: (pTag: XEditableTag) => void;
    get Value(): any
    {
        return this.Editor.Editor.RawValue;
    }

    SetModel(pColumns: XColumnModel)
    {
        this.Columns = pColumns;
        this.Editor = new XTagEditor(this, "XTagEditor");
        this.Editor.SetModel(pColumns);
        this.Editor.SVG.addEventListener("click", () => this.Editor.Clear(), false);
    }

    DoClick()
    {
        if (this.OnClick != null)
            this.OnClick.apply(this, [this]);
    }

    protected override CreateContainer(): HTMLElement 
    {
        return XUtils.AddElement<HTMLElement>(null, "div", null);
    }
}
