/// <reference path="../Stage/XStage.ts" />

class Stage extends XStage
{
    static Instance: Stage;

    static Run()
    {
        window.onmousedown = (arg) => XPopupManager.HideAll(arg);
        window.onkeydown = (a) => XHotkeyManager.OnKeyDown(a);
        this.Instance = new Stage();
    }

    static Test()
    {
        window.onmousedown = (arg) => XPopupManager.HideAll(arg);
        window.onkeydown = (a) => XHotkeyManager.OnKeyDown(a);
        this.Instance = new Stage();
        var tabx = this.Instance.TabControl.AddTab("Test App");
    }


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


    override SizeChanged()
    {
        this.MenuResize();
    }

    LoadApp(pLoadApp: XAPPModel)
    {
        let tab = <App>this.TabControl.AddTab(pLoadApp.Title);
        tab.SetModel(pLoadApp);
    }

    DoLounch(pItem: XDataMenuItem)
    {
        XMainCache.GetApp(pItem.ResourceID, this, this.LoadApp);
    }

    MenuResize()
    {
        let r = this.Menu.HTML.GetRect();
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

