/// <reference path="../Stage/XStageTabControl.ts" />
/// <reference path="../Net/XHttpClient.ts" />
/// <reference path="../Elements/XWrapPanel.ts" />

class ActionBar extends XWrapPanel
{
    constructor(pOwner: App)
    {
        super(pOwner, "XButtonBar");
        this.App = pOwner;

        this.Edit = new XSVGButton(this);
        this.Edit.HTML.className = "XButtonBarItem";
        this.Edit.SetIcon("svg/edit.svg");
        this.Inactive = new XSVGButton(this);
        this.Inactive.HTML.className = "XButtonBarItem";
        this.Inactive.SetIcon("svg/trash.svg");
        this.Active = new XSVGButton(this);
        this.Active.HTML.className = "XButtonBarItem";
        this.Active.SetIcon("svg/recycle.svg");
        this.Save = new XSVGButton(this);
        this.Save.HTML.className = "XButtonBarItem";
        this.Save.SetIcon("svg/save.svg");
        this.New = new XSVGButton(this);
        this.New.HTML.className = "XButtonBarItem";
        this.New.SetIcon("svg/new.svg");
        this.UpdateState();

        this.Edit.DisplayValue = "inline-block";
        this.Inactive.DisplayValue = "inline-block";
        this.Active.DisplayValue = "inline-block";
        this.Save.DisplayValue = "inline-block";
        this.New.DisplayValue = "inline-block";
    }

    App: App;
    Edit: XSVGButton;
    Inactive: XSVGButton;
    Active: XSVGButton;
    Save: XSVGButton;
    New: XSVGButton;

    public UpdateState(pRows: XArray<XTableRow> | null = [])
    {
        this.Edit.IsVisible = false;
        this.Inactive.IsVisible = false;
        this.Active.IsVisible = false;
        this.Save.IsVisible = false;
        this.New.IsVisible = false;
        switch (this.App.State)
        {
            case XAppState.Inserting:
                this.Edit.IsVisible = false;
                this.Inactive.IsVisible = false;
                this.Active.IsVisible = false;
                this.Save.IsVisible = true;
                this.New.IsVisible = false;
                break;
            case XAppState.Editing:
                this.Edit.IsVisible = false;
                this.Inactive.IsVisible = false;
                this.Active.IsVisible = false;
                this.Save.IsVisible = true;
                this.New.IsVisible = false;
                break;
            case XAppState.Searching:
                this.Edit.IsVisible = pRows != null && pRows.length == 1;
                this.Inactive.IsVisible = pRows != null && pRows.Any(r => r.Tupla.State != XTupleState.Deleted);
                this.Active.IsVisible = pRows != null && pRows.length > 0 && pRows.All(r => r.Tupla.State == XTupleState.Deleted);
                this.Save.IsVisible = false;
                this.New.IsVisible = true;
                break;
            case XAppState.None:
            default:
                this.Edit.IsVisible = false;
                this.Inactive.IsVisible = false;
                this.Active.IsVisible = false;
                this.Save.IsVisible = false;
                this.New.IsVisible = false;
                break;
        }
    }
}

class ActionBarR extends XWrapPanel
{
    constructor(pOwner: App)
    {
        super(pOwner, "XButtonBarR");
        this.App = pOwner;
        this.Close = new XSVGButton(this);
        this.Close.HTML.className = "XButtonBarItem";
        this.Close.SetIcon("svg/close.svg");
        XEventManager.AddEvent(this, this.Close.HTML, XEventType.Click, () => this.App.Close());
    }
    Close: XSVGButton;
    App: App;
}
