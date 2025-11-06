/// <reference path="XDiv.ts" />

class XDataMenuItem
{
    Title: string | any;
    ID: string | any;
    ResourceID: string | any;
    Count: number | any;

};

type XDataMenu = {
    Icon: string;
    Title: string;
    ID: string;
    Items: XDataMenuItem[]
};

class XMenuButtonItem extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null, pItem: XDataMenuItem)
    {
        super(pOwner, "hover-item");
        this.HTML.textContent = pItem.Title;
        this.Instances = XUtils.AddElement<HTMLLIElement>(this.HTML, 'span', "XAppCount");
        XBinding.TrackChange(pItem, "Count", (campo: any, antigo: any, novo: any) => this.Change(campo, antigo, novo));
    }

    Instances: HTMLLIElement;

    Change(campo: any, antigo: any, novo: any): void
    {
        this.Instances.innerText = "(" + novo + ")";
    }
}

class XHoverPanel extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null, pItem: XDataMenu)
    {
        super(pOwner, "hover-panel");
        this.Header = new XDiv(this, 'accordion-header');
        const icon = new XDiv(this.Header, 'icon');
        icon.HTML.innerHTML = pItem.Icon;
        const headerText = XUtils.AddElement<HTMLSpanElement>(this.Header, "span", null);
        headerText.textContent = pItem.Title;
    }
    Header: XDiv;

    public AdjustWidth()
    {
        let mx = 0;
        const els = this.HTML.querySelectorAll('.hover-item, .accordion-header');
        for (let i = 0; i < els.length; i++)
        {
            const el = els[i] as HTMLElement;
            el.style.whiteSpace = 'nowrap';
            const w = el.scrollWidth;
            if (w > mx) mx = w;
        }
        if (mx > 0)
        {
            const pad = 20;
            this.HTML.style.width = (mx + pad) + 'px';
        }
    }
}
class XMenuItem extends XElement
{
    constructor(pOwner: XMenuItemGroup, pHTMLOuner: HTMLElement)
    {
        super(pOwner, "XAppItem");
        this.HTML = XUtils.AddElement<HTMLElement>(pHTMLOuner, "li");
    }
    Menu: XMenu | null = null;
    Item: XDataMenuItem | undefined;
    Title: HTMLLIElement | null = null;
    Instances: HTMLLIElement | null = null;

    protected override CreateContainer(): HTMLElement 
    {
        return <any>null;
    }

    SetData(pItem: XDataMenuItem)
    {
        this.Item = pItem;
        XEventManager.AddEvent(this.Menu, this.HTML, XEventType.Click, () => this.Menu?.Launch(pItem));
        this.Title = XUtils.AddElement<HTMLLIElement>(this.HTML, 'span', null);
        this.Instances = XUtils.AddElement<HTMLLIElement>(this.HTML, 'span', "XAppCount");
        this.Title.innerText = pItem.Title;
        this.ID = pItem.ID;
        XBinding.TrackChange(pItem, "Count", (campo: any, antigo: any, novo: any) => this.Change(campo, antigo, novo));
    }

    Change(campo: any, antigo: any, novo: any)
    {
        if (this.Instances != null)
            this.Instances.innerText = "(" + novo + ")";
    }

}
class XMenuItemGroup extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null, pItem: XDataMenu)
    {
        super(pOwner, "accordion-item");
        this.Menu = <XMenu>pOwner?.Owner;
        this.Header = new XDiv(this, 'accordion-header');
        this.DataItem = pItem;

        this.Header.HTML.addEventListener('click', () => this.Menu?.ExpandItem(this))
        const icon = new XDiv(this.Header, 'icon');
        icon.HTML.innerHTML = pItem.Icon ?? '✔';
        const headerText = XUtils.AddElement<HTMLSpanElement>(this.Header, "span", "menu-span");
        headerText.textContent = pItem.Title;

        this.CreateItens();
        this.CreateHoverPanel();
    }

    Header: XDiv;
    Menu: XMenu | null = null;
    DataItem: XDataMenu;
    HoverPanel: XHoverPanel | null = null;
    HoverItens = new XArray<XMenuButtonItem>();
    SubItems = new XArray<XMenuItem>();

    private CreateItens()
    {
        if (this.DataItem)
        {
            const submenu = XUtils.AddElement<HTMLUListElement>(this, 'ul', 'accordion-submenu');
            if (this.DataItem.Items.length > 8)
                submenu.classList.add('has-scroll');

            for (let i = 0; i < this.DataItem.Items.length; i++)
            {
                const subitem = this.DataItem.Items[i];
                let mi = new XMenuItem(this, submenu);
                mi.Menu = this.Menu;
                mi.SetData(subitem);
                this.SubItems.Add(mi);
            };
        }
    }

    private CreateHoverPanel()
    {
        if (this.DataItem.Title)
        {
            this.HoverPanel = new XHoverPanel(this, this.DataItem);

            for (let i = 0; i < this.DataItem.Items.length; i++)
            {
                const subitem = this.DataItem.Items[i];
                const hitem = new XMenuButtonItem(this.HoverPanel, subitem);
                XEventManager.AddEvent(this.Menu, hitem.HTML, XEventType.Click, () => this.Menu?.Launch(subitem));
                this.HoverItens.Add(hitem);
            }

            this.HoverPanel.AdjustWidth();
            XEventManager.SetTiemOut(this.HoverPanel, this.HoverPanel.AdjustWidth, 0);

            (this as any)._hoverCloseTimer = null as any;

            const cancelClose = () =>
            {
                if ((this as any)._hoverCloseTimer != null)
                {
                    window.clearTimeout((this as any)._hoverCloseTimer);
                    (this as any)._hoverCloseTimer = null;
                }
            };

            const scheduleClose = () =>
            {
                cancelClose();
                (this as any)._hoverCloseTimer = window.setTimeout(() =>
                {
                        if (this.HoverPanel && this.HoverPanel.HTML)
                            this.HoverPanel.HTML.style.display = 'none';
                }, 300);
            };

            this.HTML.addEventListener('mouseenter', () =>
            {
                cancelClose();
                if (this.Menu && this.Menu.AccordionMenu && this.Menu.AccordionMenu.HTML.classList.contains('collapsed'))
                    if (this.HoverPanel && this.HoverPanel.HTML)
                        this.HoverPanel.HTML.style.display = 'block';
                    else
                        if (this.HoverPanel && this.HoverPanel.HTML)
                            this.HoverPanel.HTML.style.display = 'none';
            });

            this.HTML.addEventListener('mouseleave', () => scheduleClose());

            if (this.HoverPanel && this.HoverPanel.HTML)
            {
                this.HoverPanel.HTML.addEventListener('mouseenter', () => cancelClose());
                this.HoverPanel.HTML.addEventListener('mouseleave', () => scheduleClose());
            }
        }
    }
}

class XMenu extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "XMenu");
        this.ToggleButton = new XBaseButton(this, "collapse-toggle");
        this.AccordionMenu = new XDiv(this, "accordion-menu");
        this.ToggleButton.HTML.addEventListener('click', (e) => this.Collaspse(e));
    }

    ToggleButton: XBaseButton;
    AccordionMenu: XDiv;
    Itens = new XArray<XMenuItemGroup>();
    OnLaunch: XMethod<XDataMenuItem> | any;

    Launch(pItem: XDataMenuItem)
    {
        pItem.Count++;
        this.OnLaunch?.apply(this, [pItem]);
    }

    ExpandItem(pItem: XMenuItemGroup)
    {
        if (this.AccordionMenu.HTML.classList.contains('collapsed'))
            return;
        if (this.UnExpand(pItem))
            return;

        this.Itens.forEach(i => i.HTML.classList.remove('active'));
        if (pItem.DataItem.Items)
            pItem.HTML.classList.add('active');
    }

    UnExpand(pItem: XMenuItemGroup | null = null): boolean
    {
        let ret = false;
        if (pItem != null && !pItem.HTML.classList.contains('active'))
            return ret;

        this.Itens.forEach(i => i.HTML.classList.remove('active'));
        return true;
    }

    Collaspse(pArg: MouseEvent)
    {
        this.UnExpand();
        this.AccordionMenu.HTML.classList.toggle('collapsed');
        this.HTML.classList.toggle('Collapsed');
    }

    SetData(pData: Array<XDataMenu>)
    {
        for (let i = 0; i < pData.length; i++)
        {
            let mitem = pData[i];
            let item = new XMenuItemGroup(this.AccordionMenu, mitem);
            this.Itens.Add(item);
        }
    }
}



