/// <reference path="Base/XBaseDialog.ts" />
/// <reference path="XDiv.ts" />

class XErrorDialog extends XBaseDialog
{
    constructor(pOwner: XElement)
    {
        super(pOwner);
        this.HTML.classList.add("XErrorDialog");
        this.Title = "Error";
        this._Content = new XDiv(this, "XDialogContent");
        this._Message = new XDiv(this._Content, "XErrorMessage");
        this._Detail = new XDiv(this._Content, "XErrorDetail");
        this._Detail.IsVisible = false;
        this.ButtonBar.Cancel.Title = "Close";
        this._Toggle = new XBaseTextButton(this.ButtonBar, "XDialogButton");
        this._Toggle.Title = "Details";
        XEventManager.AddEvent(this, this._Toggle.HTML, XEventType.Click, () => this.ToggleDetail());
    }

    private _Content: XDiv;
    private _Message: XDiv;
    private _Detail: XDiv;
    private _Toggle: XBaseTextButton;

    SetError(pError: Error)
    {
        const msg = pError?.message ?? "";
        const stk = pError?.stack ?? "";
        this._Message.SetContent(msg);
        this._Detail.SetContent(stk);
        this.Layout();
    }

    private ToggleDetail()
    {
        this._Detail.IsVisible = !this._Detail.IsVisible;
        this.Layout();
    }

    private Layout()
    {
        const cw = this.HTML.offsetWidth;
        const ch = this.HTML.offsetHeight;
        const top = this.Caption.HTML.offsetHeight;
        const bh = this.ButtonBar.HTML.offsetHeight;
        const h = Math.max(40, ch - top - bh - 10);
        this._Content.Rect = new XRect(5, top + 5, cw - 10, h);
        this._Message.Rect = new XRect(5, 5, this._Content.Rect.Width - 10, this._Detail.IsVisible ? Math.max(40, Math.floor(h * 0.4)) : h - 10);
        this._Detail.Rect = new XRect(5, this._Message.Rect.Top + this._Message.Rect.Height + 5, this._Content.Rect.Width - 10, this._Detail.IsVisible ? Math.max(60, Math.floor(h * 0.5)) : 0);
    }

    override SizeChanged()
    {
        this.Layout();
    }
}
