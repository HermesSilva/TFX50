/// <reference path="../Elements/XDiv.ts" />

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
        this.Menu = new XMenu(this);
        this.TopBar = new XTopBar(this);
        this.Menu.OnResize = () => this.MenuResize();
        this.TabControl = new XStageTabControl(this);
        this.TabControl.Dropdown.HTML.classList.add("Main");
        if (XStage.SessionID == null)
            XStage.SessionID = crypto.randomUUID();
        this.Loaded();
    }
    Menu: XMenu;
    TopBar: XTopBar;
    TabControl: XStageTabControl;
    static SessionID: string | any = null;

    override SizeChanged()
    {
        this.MenuResize();
    }

    Loaded()
    {
        var clt = new XHttpClient(this, "Access/Login");
        clt.SetHeader("SessionID", XStage.SessionID);
        clt.OnLoad = this.LoadCallBack;
        clt.OnError = this.ErroCallBack;
        var data: any = new Object();
        data.Login = "teste";
        clt.SendAsync(data);
        //setTimeout(() =>
        //{
        //    clt.Abort();
        //    console.log("abortado");
        //}, 5000);
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

