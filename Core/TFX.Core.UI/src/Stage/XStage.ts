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
    }

    Menu: XMenu;
    TopBar: XTopBar;
    TabControl: XStageTabControl;
    
    override SizeChanged()
    {
        this.MenuResize();
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

