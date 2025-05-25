/// <reference path="Base/XBaseTextButton.ts" />
/// <reference path="Base/XPopupElement.ts" />

class XTabControlButton extends XBaseTextButton
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "XTabControlButton");
        const button = <HTMLElement>XUtils.AddElement(this, "span", 'XTabControlButtonIcon');
        button.innerHTML = '×';
        XEventManager.AddEvent(this, button, XEventType.Click, () => this.TabControl?.CloseTab(this));
        XEventManager.AddEvent(this, this.HTML, XEventType.Click, () => this.TabControl?.SelectTab(this));
    }

    TabControl: XTabControl | null = null;
    Tab: XTabControlTab | null = null;
}

class XTabControlHeader extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "XTabControlHeader");
    }

    DropdownButton: XTabControlButtonList | null = null;

    SelectionChanged()
    {
        this.ValidateVisibility();
    }

    override SizeChanged()
    {
        if (this.DropdownButton != null)
            this.DropdownButton.IsVisible = this.HTML.scrollWidth > this.HTML.offsetWidth;
        this.ValidateVisibility();
    }

    private ValidateVisibility()
    {
        const painelRect = this.HTML.getBoundingClientRect();
        this.HTML.childNodes.forEach(item =>
        {
            var elm = <HTMLElement>item;
            const rect = elm.getBoundingClientRect();

            if (rect.left < painelRect.left || rect.right > painelRect.right)
                elm.style.display = 'none';

            else
                elm.style.display = 'block';
        });
    }
}

class XTabControlTab extends XDiv implements XIDialogContainer
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "XTabControlTab");
        this.TabControl = <XTabControl>pOwner?.Owner;
        this.DialogContainer = new XDialogContainer(this, "XDialogContainer");
    }
    Button: XTabControlButton | null = null;
    DialogContainer: XDialogContainer;
    IsDialogContainer: boolean = true;
    TabControl!: XTabControl;
    Close()
    {
        this.TabControl.CloseTab(this.Button!);
    }
}

class XTabControlContainer extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "XTabControlContainer");
    }
}

class XTabControlDropdown extends XPopupElement
{
    constructor(pOwner: XElement)
    {
        super(pOwner, "XTabControlDropdown");
        this.HTML.addEventListener('wheel', function (event)
        {
            const { deltaY } = event;
            const { scrollTop, scrollHeight, clientHeight } = this;

            if ((deltaY > 0 && (scrollTop + clientHeight >= scrollHeight)) || (deltaY < 0 && scrollTop <= 0))
                event.preventDefault();
        });
    }
}

class XTabControlButtonList extends XBaseTextButton
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "XTabControlButtonList");
        this.IsVisible = false;
    }
}

class XTabControl extends XDiv implements XIDialogContainer
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "XTabControl");
        this.IsDialogContainer = true;
        this.DialogContainer = new XDialogContainer(this, "XDialogContainer");
        this.Header = new XTabControlHeader(this);
        this.Container = new XTabControlContainer(this);
        this.Dropdown = new XTabControlDropdown(this);
        XPopupManager.Add(this.Dropdown);
        this.ButtonList = new XTabControlButtonList(this);
        this.ButtonList.Title = "Abas";
        this.Header.DropdownButton = this.ButtonList;

        this.ButtonList.HTML.addEventListener('click', () =>
        {
            this.PopulateDropdown();
        });

    }
    Header: XTabControlHeader;
    Container: XTabControlContainer;
    Dropdown: XTabControlDropdown;
    ButtonList: XTabControlButtonList;
    DialogContainer: XDialogContainer;
    ActiveTab: XTabControlTab | null = null;
    protected Tabs: XArray<XTabControlTab> = new XArray<XTabControlTab>();
    IsDialogContainer: boolean = false;

    private PopulateDropdown()
    {
        this.Dropdown.HTML.innerHTML = '';
        this.Tabs.forEach((tab, index) =>
        {
            const item = new XTabControlButton(this.Dropdown);
            item.HTML.className = "XTabControlButtonL";
            if (tab.Button != null)
            {
                if (tab?.Button?.HTML.classList.contains('Active'))
                    item.HTML.classList.add('Active');
                item.TabControl = tab.Button.TabControl;
                item.Tab = tab.Button.Tab;
                item.Title = tab.Button?.Title;
            }

        });
        this.Dropdown.IsVisible = true;
    }

    CloseTab(pButton: XTabControlButton)
    {
        pButton.Tab?.Free();
        pButton.Free();
    }

    SelectTab(pButton: XTabControlButton)
    {
        if (pButton == null)
            return;
        this.Tabs.ForEach(t =>
        {
            if (t.Button != null && t.Button.Tab != null)
            {
                t.Button?.HTML.classList.remove('Active');
                t.Button.Tab.IsVisible = false;
            }
        });
        if (pButton != null && pButton.Tab != null && pButton.Tab.Button != null)
        {
            pButton.Tab.Button.HTML.classList.add('Active');
            pButton.HTML.classList.add('Active');
            pButton.Tab.IsVisible = true;
        }
        var rbtn = pButton?.Tab?.Button?.HTML.getBoundingClientRect();
        var rctn = this.Header.HTML.getBoundingClientRect();
        var offw = (<HTMLElement>pButton?.Tab?.Button?.HTML?.previousElementSibling)?.offsetWidth ?? 0;
        if (rbtn != null)
        {
            if (rbtn.left < rctn.left)
                this.Header.HTML.scrollLeft -= (rctn.left - rbtn.left) + offw;
            else
                if (rbtn.right > rctn.right)
                    this.Header.HTML.scrollLeft += (rbtn.right - rctn.right) + offw;
        }
        this.Dropdown.IsVisible = false;
        this.ActiveTab = pButton.Tab;
        this.Header.SelectionChanged();
    }

    AddTab(pTitle: string): XTabControlTab
    {
        var btn = new XTabControlButton(this.Header);
        btn.Title = pTitle;
        btn.TabControl = this;
        var tab = this.CreateTab();
        tab.Button = btn;
        btn.Tab = tab;
        this.Tabs.Add(tab);
        tab.IsVisible = false;
        this.SelectTab(tab.Button);
        return tab;
    }

    CreateTab(): XTabControlTab
    {
        return new XTabControlTab(this.Container);;
    }
}

