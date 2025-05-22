/// <reference path="../Elements/XMenu.ts" />

interface MenuTuple extends XTuple
{
    Menu: XField;
    Icone: XField;
    Item: XField;
    CORxMenuItemID: XField;
    CORxRecursoID: XField;
    CORxMenuID: XField;
}

class MainMenu extends XMenu
{

    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
    }

    Load()
    {
        var clt = new XHttpClient();
        clt.Context = this; 
        clt.OnLoad = this.LoadCallBack;
        clt.SendAsync(Paths.Menu);
    }

    LoadCallBack(pData: XResponse<XData<MenuTuple>>, pCallData: any | null, pEvent: ProgressEvent | null)
    {
        let data = this.GroupMenu(pData.Data.Tuples);
        this.SetData(data);
    }

    private GroupMenu(pTuples: MenuTuple[]): Array<XDataMenu>
    {
        const grupos = pTuples.reduce<Record<string, XDataMenu>>((groups, tuple) =>
        {
            const key = `${tuple.CORxMenuID.Value}|{tuple.Menu.Value}|${tuple.Icone.Value}`;

            const group = groups[key] ?? (groups[key] = {
                Icon: tuple.Icone.Value,
                Title: tuple.Menu.Value,
                ID: tuple.Menu.Value,
                Items: []
            });
            var item = new XDataMenuItem();
            item.Title = tuple.Item.Value;
            item.ID = tuple.CORxMenuItemID.Value;
            item.ResourceID = tuple.CORxRecursoID.Value;
            item.Count = 0;
            group.Items.push(item);

            return groups;
        }, {});

        const data = Object.keys(grupos).map(key => grupos[key]);
        return data;
    }
}
