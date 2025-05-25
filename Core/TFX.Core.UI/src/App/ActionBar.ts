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
    }
    App: App;
    Edit: XSVGButton;
    Inactive: XSVGButton;
    Active: XSVGButton;
    Save: XSVGButton;
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
