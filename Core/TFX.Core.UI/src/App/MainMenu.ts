/// <reference path="../Elements/XMenu.ts" />

interface MenuTuple extends XTuple
{
    Menu: XField;
    Icone: XField;
    Item: XField;
    CORxMenuItemID: XField;
}
class MainMenu extends XMenu
{

    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
    }

    Load()
    {
        var clt = new XHttpClient(this, Routes.Menu);
        clt.OnLoad = this.LoadCallBack;
        clt.SendAsync();
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
            const key = `${tuple.Menu.Value}|${tuple.Icone.Value}`;

            const group = groups[key] ?? (groups[key] = {
                Icon: tuple.Icone.Value,
                Title: tuple.Menu.Value,
                Items: []
            });

            group.Items.push({
                Title: tuple.Item.Value,
                ID: tuple.CORxMenuItemID.Value
            });

            return groups;
        }, {});

        const data = Object.keys(grupos).map(key => grupos[key]);
        return data;
    }
}
