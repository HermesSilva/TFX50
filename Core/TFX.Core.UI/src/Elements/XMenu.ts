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
        XEventManager.TrackChange(pItem, "Count", (campo: any, antigo: any, novo: any) => this.Change(campo, antigo, novo));
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
    ID: string | null = null;

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
        XEventManager.TrackChange(pItem, "Count", (campo: any, antigo: any, novo: any) => this.Change(campo, antigo, novo));
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

        this.CreateHoverPanel();
        this.CreateItens();
    }

    Header: XDiv;
    Menu: XMenu | null = null;
    DataItem: XDataMenu;
    HoverPanel: XHoverPanel | null = null;
    HoverItens = new XArray<XMenuButtonItem>();

    private CreateItens()
    {
        if (this.DataItem)
        {
            const submenu = XUtils.AddElement<HTMLUListElement>(this, 'ul', 'accordion-submenu');
            if (this.DataItem.Items.length > 8)
                submenu.classList.add('has-scroll');

            for (var i = 0; i < this.DataItem.Items.length; i++)
            {
                var subitem = this.DataItem.Items[i];
                let mi = new XMenuItem(this, submenu);
                mi.Menu = this.Menu;
                mi.SetData(subitem);
            };
        }
    }

    private CreateHoverPanel()
    {
        if (this.DataItem.Title)
        {
            this.HoverPanel = new XHoverPanel(this, this.DataItem);

            for (var i = 0; i < this.DataItem.Items.length; i++)
            {
                var subitem = this.DataItem.Items[i];
                var hitem = new XMenuButtonItem(this.HoverPanel, subitem);
                XEventManager.AddEvent(this.Menu, hitem.HTML, XEventType.Click, () => this.Menu?.Launch(subitem));

                this.HoverItens.Add(hitem);
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
        var ret = false;
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
        for (var i = 0; i < pData.length; i++)
        {
            var mitem = pData[i];
            var item = new XMenuItemGroup(this.AccordionMenu, mitem);
            this.Itens.Add(item);
        }
    }
}


