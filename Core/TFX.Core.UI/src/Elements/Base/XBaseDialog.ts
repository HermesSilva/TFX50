
/// <reference path="XSizeableElement.ts" />
/// <reference path="../XWrapPanelEx.ts" />
/// <reference path="../XWrapPanel.ts" />
class XBaseDialogCaption extends XDiv
{
    constructor(pOwner: XElement, pClass: string)
    {
        super(pOwner, pClass);
        this.ELMTitle = new XDiv(this, "XDialogTitle");
    }
    protected ELMTitle: XDiv;
    get Title(): string
    {
        return this.ELMTitle.HTML.innerHTML;
    }
    set Title(pValue: string)
    {
        this.ELMTitle.HTML.innerHTML = pValue;
    }

}

class XBaseButtonBar extends XWrapPanelEx
{
    constructor(pOwner: XElement, pClass: string)
    {
        super(pOwner, pClass);
        this.Ok = new XBaseTextButton(this, "XDialogButton");
        this.Ok.Title = "Ok";
        this.Cancel = new XBaseTextButton(this, "XDialogButton");
        this.Cancel.Title = "Cancelar";
    }
    Cancel: XBaseTextButton;
    Ok: XBaseTextButton;
}

class XBaseCleanDialog extends XSizeableElement implements XIDialog
{
    constructor(pOwner: XElement)
    {
        super(pOwner, "XDialog");
        this.HTML.parentElement?.removeChild(this.HTML);
        this.AutoIncZIndex = true;
    }

    IsDialog: boolean = true;
    DialogContainer: any;
    AskClose!: XFunc<XBaseCleanDialog>;

    Cancel(pArg: MouseEvent)
    {
        if (this.AskClose && !this.AskClose(this))
            return;
        if (this.HTML.parentElement == null)
            return;
        this.IsVisible = false;
    }

    Ok(pArg: MouseEvent)
    {
        if (this.AskClose && !this.AskClose(this))
            return;
        if (this.HTML.parentElement == null)
            return;
        this.IsVisible = false;
    }

    ShowDialog()
    {
        this.IsVisible = true;
        this.StartMouseDown(<any>null);
    }

    override IncZIndex()
    {
        this.HTML.style.zIndex = `${999 + XPopupManager.ZIndex()}`;
    }

    override Show(pValue: boolean = true)
    {
        if (this.DialogContainer == null)
        {
            this.DialogContainer = this.GetDialogContainer();
            if (this.DialogContainer.HTML != this.HTML)
            {
                this.HTML.parentElement?.removeChild(this.HTML);
                this.DialogContainer.DialogContainer.HTML.appendChild(this.HTML);
            }
        }

        super.Show(pValue);
        this.DialogContainer.DialogContainer.IsVisible = pValue;
    }
}

class XBaseDialog extends XBaseCleanDialog
{
    Caption: XBaseDialogCaption;
    constructor(pOwner: XElement)
    {
        super(pOwner);
        this.HTML.parentElement?.removeChild(this.HTML);
        this.AutoIncZIndex = true;
        this.Caption = new XBaseDialogCaption(this, "XDialogCaption");
        this.ButtonBar = new XBaseButtonBar(this, "XDialogButtonBar");
        this.ButtonBar.StartSide = XWrapStartSide.Right;
        this.ButtonBar.StartMargin = 6;
        this._Text = XUtils.AddElement<HTMLTextAreaElement>(this, "textarea", "XDialogTextArea");
        this._Text.setAttribute("readonly", "true");
        XEventManager.AddEvent(this, this.ButtonBar.Cancel.HTML, XEventType.Click, this.Cancel);
        XEventManager.AddEvent(this, this.ButtonBar.Ok.HTML, XEventType.Click, this.Ok);        
    }

    ButtonBar: XBaseButtonBar;
    private _Text: HTMLTextAreaElement;
    override SizeChanged()
    {
        super.SizeChanged();
        this.ButtonBar.SizeChanged();
    }
    get Text(): string
    {
        return this._Text.value;
    }
    set Text(pValue: string)
    {
        this._Text.value = pValue;
    }
    get Title(): string
    {
        return this.Caption.Title;
    }
    set Title(pValue: string)
    {
        this.Caption.Title = pValue;
    }

    override StartMouseDown(pArg: MouseEvent)
    {
        let r = this.Caption.HTML.GetRect();
        let lb = this.HTML.StyleValue("border-left");
        let tb = this.HTML.StyleValue("border-top");
        this.DragRect = new XRect(lb, tb, r.Width, r.Height);
    }
}
