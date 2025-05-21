/// <reference path="../Stage/XStage.ts" />

class Stage extends XStage
{
    static Instance: Stage;

    constructor()
    {
        super();
        this.Menu = new MainMenu(this);
        this.Menu.OnResize = () => this.MenuResize();
        this.Menu.OnLaunch = (arg: XDataMenuItem) => this.DoLounch(arg);
        this.Loaded();
    }

    Menu: MainMenu;

    Loaded()
    {
        this.Menu.Load();
    }

    static Run()
    {
        window.onmousedown = (arg) => XPopupManager.HideAll(arg);
        window.onkeydown = (a) => XHotkeyManager.OnKeyDown(a);
        this.Instance = new Stage();
    }

    override SizeChanged()
    {
        this.MenuResize();
    }

    LoadApp(pLoadApp: XAPPModel)
    {
        var tab = <App>this.TabControl.AddTab(pLoadApp.Title);
        tab.SetModel(pLoadApp);
    }

    DoLounch(pItem: XDataMenuItem)
    {
        XMainCache.GetApp(pItem.ResourceID, this, this.LoadApp);
    }

    MenuResize()
    {
        var r = this.Menu.HTML.GetRect();
        this.TabControl.HTML.style.left = `${r.Width}px`;
        this.TabControl.HTML.style.width = `${this.Rect.Width - r.Width - 1}px`;
        this.TopBar.HTML.style.left = `${r.Width}px`;
        this.TopBar.HTML.style.width = `${this.Rect.Width - r.Width - 1}px`;
    }



    CreateTabControl(): XStageTabControl
    {
        return new StageTabControl(this);
    }
}
