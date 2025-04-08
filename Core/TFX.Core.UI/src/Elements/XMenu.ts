/// <reference path="XDiv.ts" />

class XMenuButtonItem extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null, pItem: any)
    {
        super(pOwner, "hover-item");
        this.HTML.textContent = pItem;
    }
}


class XHoverPanel extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null, pItem: any)
    {
        super(pOwner, "hover-panel");
        this.Header = new XDiv(this, 'accordion-header');
        const icon = new XDiv(this.Header, 'icon');
        icon.HTML.innerHTML = pItem.icon;
        const headerText = XUtils.AddElement<HTMLSpanElement>(this.Header, "span", null);
        headerText.textContent = pItem.title;
    }
    Header: XDiv;
}

class XMenuItem extends XDiv
{
    constructor(pOwner: XElement | HTMLElement | null, pItem: any)
    {
        super(pOwner, "accordion-item");
        this.Header = new XDiv(this, 'accordion-header');
        this.DataItem = pItem;

        this.Header.HTML.addEventListener('click', () => this.Menu?.ExpandItem(this))


        const icon = new XDiv(this.Header, 'icon');
        icon.HTML.innerHTML = pItem.icon;
        const headerText = XUtils.AddElement<HTMLSpanElement>(this.Header, "span", "menu-span");
        headerText.textContent = pItem.title;

        this.CreateHoverPanel();
        this.CreateItens();
    }

    Header: XDiv;
    Menu: XMenu | null = null;
    DataItem: any;
    HoverPanel: XHoverPanel | null = null;
    HoverItens = new XArray<XMenuButtonItem>();
    Title: HTMLLIElement | null = null;
    Instances: HTMLLIElement | null = null;

    private CreateItens()
    {
        if (this.DataItem.subItems)
        {
            const submenu = XUtils.AddElement<HTMLUListElement>(this, 'ul', 'accordion-submenu');
            if (this.DataItem.subItems.length > 8)
                submenu.classList.add('has-scroll');

            for (var i = 0; i < this.DataItem.subItems.length; i++)
            {
                var subitem = this.DataItem.subItems[i];
                const li = XUtils.AddElement<HTMLLIElement>(submenu, 'li', "XAppItem");
                this.Title = XUtils.AddElement<HTMLLIElement>(li, 'span', null);
                this.Instances = XUtils.AddElement<HTMLLIElement>(li, 'span', "XAppCount");
                this.Title.innerText = subitem;
                this.Instances.innerText = "(5)";
            };
        }
    }

    private CreateHoverPanel()
    {
        if (this.DataItem.subItems)
        {
            this.HoverPanel = new XHoverPanel(this, this.DataItem);

            for (var i = 0; i < this.DataItem.subItems.length; i++)
            {
                var subitem = this.DataItem.subItems[i];
                var hitem = new XMenuButtonItem(this.HoverPanel, subitem);
                this.HoverItens.Add(hitem);
            }
        }
    }
}


class XMenu extends XDiv
{
    private menuData = [
        {
            icon: '🏠',
            title: 'Home',
            subItems: ['Nossa História', 'Equipe', 'Parceiros']
        },
        {
            icon: '🛠️',
            title: 'Serviços',
            subItems: Array.from({ length: 10 }, (_, i) => `Serviço ${i + 1}`)
        },
        {
            icon: '📚',
            title: 'Sobre',
            subItems: ['Nossa História', 'Equipe', 'Parceiros']
        },
        {
            icon: '📦',
            title: 'Produtos',
            subItems: Array.from({ length: 12 }, (_, i) => `Produto ${i + 1}`)
        },
        {
            icon: '📞',
            title: 'Contato',
            subItems: ['Nossa História', 'Equipe', 'Parceiros']
        }
    ];
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "XMenu");
        this.ToggleButton = new XBaseButton(this, "collapse-toggle");
        this.AccordionMenu = new XDiv(this, "accordion-menu");
        this.ToggleButton.HTML.addEventListener('click', (e) => this.Collaspse(e));
        this.CreateItens();
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
        //if (this.AccordionMenu.HTML.classList.contains('collapsed'))
        //    this.AccordionMenu.HTML.classList.remove('collapsed');

        this.Itens.forEach(i => i.HTML.classList.remove('active'));
        if (pItem.DataItem.subItems)
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

    CreateItens()
    {
        for (var i = 0; i < this.menuData.length; i++)
        {
            var mitem = this.menuData[i];
            var item = new XMenuItem(this.AccordionMenu, mitem);
            item.Menu = this;
            this.Itens.Add(item);
        }
    }
}


