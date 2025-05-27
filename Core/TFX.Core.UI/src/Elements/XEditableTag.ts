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

    SetModel(pColumns: XColumnModel)
    {
        var edttype = XEditorFactory.DataTypeToEditorType(pColumns.Type);
        this.Editor = XEditorFactory.NewEditor(edttype, this) as XIEditor;
        this.Editor.RemoveTitle()
        this.Editor.HTML.className = "XTagEditorInput";
        this.Editor.Input.className = "XTagEditorInput";
        this.Editor.Input.addEventListener('input', () => this.OnInput(), false)
        this.Editor.Mask = pColumns.Mask;
        this.Title.addEventListener('click', () => this.Editor.Input.focus(), false);
        this.SVG = XUtils.AddElement<HTMLImageElement>(this.HTML, "img", "XTagEditorSVG");
        this.SVG.src = "svg/closered.svg";
    }

    private OnInput()
    {
        var w = XUtils.ApplySize(this.Editor.HTML, this.Editor.Input.value);
        this.Editor.Input.style.width = w + "px";   
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

    SetModel(pColumns: XColumnModel)
    {
        this.Columns = pColumns;
        this.Editor = new XTagEditor(this, "XTagEditor");
        this.Editor.SetModel(pColumns);
        this.Editor.SVG.addEventListener("click", () => this.DoClick(), false);
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