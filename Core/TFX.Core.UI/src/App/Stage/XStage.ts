/// <reference path="../../Elements/XDiv.ts" />

class XStage extends XDiv
{
    static Instance: XStage;



    static Run()
    {
        window.onmousedown = (arg) => XPopupManager.HideAll(arg);
        window.onkeydown = (a) => XHotkeyManager.OnKeyDown(a);

        this.Instance = new XStage();
    }

    constructor()
    {
        super(document.body, "MainDiv");
        this.Menu = new MainMenu(this);
        this.Menu.OnLaunch = (arg: XDataMenuItem) => this.DoLounch(arg);
        this.TopBar = new XTopBar(this);
        this.Menu.OnResize = () => this.MenuResize();
        this.TabControl = new XStageTabControl(this);
        this.TabControl.Dropdown.HTML.classList.add("Main");
        if (XStage.SessionID == null)
            XStage.SessionID = crypto.randomUUID();
        this.Loaded();
    }

    Menu: MainMenu;
    TopBar: XTopBar;
    TabControl: XStageTabControl;
    static SessionID: string | any = null;

    DoLounch(pItem: XDataMenuItem)
    {
        var tab = this.TabControl.AddTab(pItem.Title);
    }

    override SizeChanged()
    {
        this.MenuResize();
    }

    Loaded()
    {
        this.Menu.Load();
    }

    ErroCallBack(pError: Error, pCallData: any | null, pEvent: ProgressEvent | null)
    {
    }

    LoadCallBack(pData: JSON, pCallData: any | null, pEvent: ProgressEvent | null)
    {
    }

    MenuResize()
    {
        var r = this.Menu.HTML.GetRect();
        this.TabControl.HTML.style.left = `${r.Width}px`;
        this.TabControl.HTML.style.width = `${this.Rect.Width - r.Width - 1}px`;
        this.TopBar.HTML.style.left = `${r.Width}px`;
        this.TopBar.HTML.style.width = `${this.Rect.Width - r.Width - 1}px`;
    }
}

