/// <reference path="../Elements/XDiv.ts" />

class XStage extends XDiv
{
    constructor()
    {
        super(document.body, "MainDiv");
        this.TopBar = new XTopBar(this);
        this.TabControl = new XStageTabControl(this);
        this.TabControl.Dropdown.HTML.classList.add("Main");
        if (XStage.SessionID == null)
            XStage.SessionID = crypto.randomUUID();
    }

    TopBar: XTopBar;
    TabControl: XStageTabControl;
    static SessionID: string | any = null;

    override SizeChanged()
    {
        this.MenuResize();
    }

    LoadApp(pLoadApp: XAPPModel)
    {
        var tab = this.TabControl.AddTab(pLoadApp.Title);
    }

}

