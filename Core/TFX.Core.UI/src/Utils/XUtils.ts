class XUtils
{
    static SetCursor(pElement: HTMLElement, pType: XDragType)
    {
        switch (pType)
        {
            case XDragType.LeftTop:
                pElement.style.cursor = "nw-resize;";
                break;
            case XDragType.Top:
                pElement.style.cursor = "n-resize";
                break;
            case XDragType.RightTop:
                pElement.style.cursor = "ne-resize";
                break;
            case XDragType.Right:
                pElement.style.cursor = "e-resize";
                break;
            case XDragType.RightBottom:
                pElement.style.cursor = "se-resize";
                break;
            case XDragType.Bottom:
                pElement.style.cursor = "s-resize";
                break;
            case XDragType.LeftBottom:
                pElement.style.cursor = "sw-resize";
                break;
            case XDragType.Left:
                pElement.style.cursor = "w-resize";
                break;
            case XDragType.Drag:
                pElement.style.cursor = "move";
                break;
            default:
                pElement.style.cursor = "default";
                break;
        }
    }

    static Location(pElement: HTMLElement): XPoint
    {
        var prect: DOMRect | any = null;
        if (pElement.parentElement != null)
            prect = pElement.parentElement.getBoundingClientRect();
        var rect: DOMRect = pElement.getBoundingClientRect();
        if (prect != null)
            return new XPoint(rect.left - prect.left, rect.top - prect.top);
        return new XPoint(rect.left, rect.top);
    }

    static IsOut(pRect: DOMRect, pLocation: XPoint, pWidth: number, pHeight: number): Boolean
    {
        return (pLocation.IsLessZero || (pRect.width < pWidth + pLocation.X) || (pRect.height < pHeight + pLocation.Y));
    }


    public static IsNumber(pValue: any): boolean
    {
        return !isNaN(parseFloat(pValue)) && isFinite(pValue);
    }

    public static AddElement<T extends Element>(pOwner: any | HTMLElement | null, pTag: string | null, pClass: string | null = null, pInsert: boolean = false): T
    {
        if (pTag == null)
            throw new Error(`Parameter "pTag" can´t be null`);
        var own: Element;
        if (pOwner == null)
            own = document.body;
        else
            if (pOwner instanceof HTMLElement)
                own = pOwner;
            else
                own = pOwner.HTML;

        var elm = <Element>document.createElement(pTag);

        if (pClass != null)
            elm.className = pClass;

        if (pInsert && own.childNodes.length > 0)
            own.insertBefore(elm, elm.childNodes[0]);
        else
            own.appendChild(elm);

        if (pOwner == null)
            elm.Owner = pOwner;
        else
            if (pOwner instanceof XElement)
                elm.Owner = pOwner;
        return <T>elm;
    }
}