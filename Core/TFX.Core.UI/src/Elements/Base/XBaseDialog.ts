
/// <reference path="XSizeableElement.ts" />
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

class XBaseButtonBar extends XWrapPanel
{
    constructor(pOwner: XElement, pClass: string)
    {
        super(pOwner, pClass);
        this.Cancel = new XBaseTextButton(this, "XDialogButton");
        this.Cancel.Title = "Cancelar";
    }
    Cancel: XBaseTextButton;
}

class XBaseDialog extends XSizeableElement implements XIDialog
{
    Caption: XBaseDialogCaption;
    constructor(pOwner: XElement)
    {
        super(pOwner, "XDialog");
        this.HTML.parentElement?.removeChild(this.HTML);
        this.AutoIncZIndex = true;
        this.Caption = new XBaseDialogCaption(this, "XDialogCaption");
        this.ButtonBar = new XBaseButtonBar(this, "XButtonBar Right");
        XEventManager.AddEvent(this, this.ButtonBar.Cancel.HTML, XEventType.Click, this.Cancel);
    }
    IsDialog: boolean = true;
    ButtonBar: XBaseButtonBar;
    private _DialogContainer: any;

    Cancel(pArg: MouseEvent)
    {
        if (this.HTML.parentElement == null)
            return;
        this.IsVisible = false;
    }

    get Title(): string
    {
        return this.Caption.Title;
    }
    set Title(pValue: string)
    {
        this.Caption.Title = pValue;
    }

    ShowDialog()
    {
        this.IsVisible = true;
        this.StartMouseDown(<any>null);
    }

    override StartMouseDown(pArg: MouseEvent)
    {
        var r = this.Caption.HTML.GetRect();
        var lb = this.HTML.StyleValue("border-left");
        var tb = this.HTML.StyleValue("border-top");
        this.DragRect = new XRect(lb, tb, r.Width, r.Height);
    }

    override IncZIndex()
    {
        this.HTML.style.zIndex = `${999 + XPopupManager.ZIndex()}`;
    }

    override Show(pValue: boolean = true)
    {
        if (this._DialogContainer == null)
        {
            this._DialogContainer = this.GetDialogContainer();
            if (this._DialogContainer.HTML != this.HTML)
            {
                this.HTML.parentElement?.removeChild(this.HTML);
                this._DialogContainer.DialogContainer.HTML.appendChild(this.HTML);
            }
        }

        super.Show(pValue);
        this._DialogContainer.DialogContainer.IsVisible = pValue;
    }
}