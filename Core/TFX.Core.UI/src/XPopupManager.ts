interface XPopupClosedEvent { (pPopupPanel: XIPopupPanel): void; }

class XPopupManager
{
    static _ZIndex: number = 999;
    static ZIndex(): string
    {
        this._ZIndex++;
        return this._ZIndex.toString();
    }

    private static PopupList: Array<XIPopupPanel> = new Array<XIPopupPanel>();
    private static AutoEvent: XArray<{ Context: any, Method: XEvent, Once: boolean }> = new Array<{ Context: any, Method: XEvent, Once: boolean }>();
    static UseCrl: boolean = false;

    static AddAutoEvent(pContext: any, pMethod: any, pOnce: boolean = true)
    {
        var obj = { Context: pContext, Method: pMethod, Once: pOnce };
        this.AutoEvent.Add(obj);
    }

    static Remove(pView: XIPopupPanel)
    {
        XPopupManager.PopupList.Remove(pView);
    }

    static Show(pView: XIPopupPanel)
    {
        pView.Show();
        //pView.HTML.scrollIntoView();
    }

    static Add(pView: XIPopupPanel)
    {
        XPopupManager.PopupList.Add(pView);
    }

    static HideAll(pArg?: MouseEvent, pValid: boolean = false)
    {
        if (pArg != null && this.UseCrl && !pArg.ctrlKey)
            return;
        var ar = XPopupManager.AutoEvent.ToArray();
        for (var i = 0; i < ar.length; i++)
        {
            var m = ar[i];
            if (pArg != null && !m.Context.CanClose(<HTMLElement>pArg.srcElement))
                continue;
            m.Method.apply(m.Context);
            if (m.Once)
                XPopupManager.AutoEvent.Remove(m);
        }
        for (var i = 0; i < XPopupManager.PopupList.length; i++)
        {
            var elm = XPopupManager.PopupList[i];
            if (!elm.IsVisible)
                continue;
            if (pArg == null || elm.CanClose(<HTMLElement>pArg.srcElement))
            {
                if (!pValid)
                    elm.CallPopupClosed();
                elm.IsVisible = false;
            }
        }
    }
}