/// <reference path="XDiv.ts" />

type XDataMenuItem = { Title: string; ID: string };
type XDataMenu = { Icon: string; Title: string; Items: XDataMenuItem[] };

class XMenuButtonItem extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null, pItem: XDataMenuItem)
    {
        super(pOwner, "hover-item");
        this.HTML.textContent = pItem.Title;
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

class XMenuItem extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null, pItem: XDataMenu)
    {
        super(pOwner, "accordion-item");
        this.Header = new XDiv(this, 'accordion-header');
        this.DataItem = pItem;

        this.Header.HTML.addEventListener('click', () =>this.Menu?.ExpandItem(this))
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
    Title: HTMLLIElement | null = null;
    Instances: HTMLLIElement | null = null;
    ID: string | null = null;

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
                const li = XUtils.AddElement<HTMLLIElement>(submenu, 'li', "XAppItem");
                this.Title = XUtils.AddElement<HTMLLIElement>(li, 'span', null);
                this.Instances = XUtils.AddElement<HTMLLIElement>(li, 'span', "XAppCount");
                this.Title.innerText = subitem.Title;
                this.ID = subitem.ID;
                this.Instances.innerText = "(5)";
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
    Itens = new XArray<XMenuItem>();

    ExpandItem(pItem: XMenuItem)
    {
        if (this.AccordionMenu.HTML.classList.contains('collapsed'))
            return;
        if (this.UnExpand(pItem))
            return;

        this.Itens.forEach(i => i.HTML.classList.remove('active'));
        if (pItem.DataItem.Items)
            pItem.HTML.classList.add('active');
    }

    UnExpand(pItem: XMenuItem | null = null): boolean
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
            var item = new XMenuItem(this.AccordionMenu, mitem);
            item.Menu = this;
            this.Itens.Add(item);
        }
    }
}


