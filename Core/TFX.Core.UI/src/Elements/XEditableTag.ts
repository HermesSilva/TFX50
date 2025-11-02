/// <reference path="Base/XElement.ts" />
/// <reference path="../Reflection/XReflections.ts" />

class XEditableTag extends XDiv 
{

    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "XTagEditor");

    }

    SVG!: HTMLImageElement;
    Title!: HTMLSpanElement;
    Input!: HTMLInputElement;
    Columns!: XColumnModel;
    Field!: XFRMField;
    Editor!: XIEditor;
    private _LIconSrc!: string;
    get Value(): any
    {
        return "";
    }

    private OnInput()
    {
        let w = XUtils.ApplySize(this.Editor.HTML, this.Editor.Input.value) + this.SVG.clientWidth + this.Title.clientWidth + 50;
        this.HTML.style.minWidth = w + "px";
        this.UpdateSVGIcon();
        if (this.Owner instanceof XElement)
            this.Owner.SizeChanged();
    }

    private UpdateSVGIcon()
    {
        const hasContent = this.Input?.value?.length > 0;
        const iconSrc = hasContent ? "svg/tinyclosebold.svg" : "svg/tinyclose.svg";
        if (this._LIconSrc != iconSrc)
            this.SVG.src = this._LIconSrc = iconSrc;
    }

    SetModel(pColumns: XColumnModel, pField: XFRMField)
    {
        this.Columns = pColumns;
        this.Field = pField;
        this.Editor = XEditorFactory.CreateEditor(this, this.Field);
        this.Editor.Input.parentNode?.removeChild(this.Editor.Input);
        this.Editor.HTML.parentNode?.removeChild(this.Editor.HTML);
        this.Input = this.Editor.Input;
        XEventManager.AddEvent(this, this.Input, XEventType.Input, () => this.OnInput());
        this.Title = XUtils.AddElement<HTMLSpanElement>(this.HTML, "span", "XTagTitle");
        this.Title.innerHTML = "Tag Editor";
        this.HTML.appendChild(this.Input);
        this.Input.className = "XTagInput";
        this.SVG = XUtils.AddElement<HTMLImageElement>(this.HTML, "img", "XTagClear");
        this.SVG.addEventListener("click", () => { this.Editor.Clear(); this.OnInput() }, false);
        this.UpdateSVGIcon();
    }

    DoClick()
    {
    }

    protected override CreateContainer(): HTMLElement 
    {
        return XUtils.AddElement<HTMLElement>(null, "div", null);
    }
}
