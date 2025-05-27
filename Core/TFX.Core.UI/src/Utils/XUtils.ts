class XUtils
{
    private static CanvasContext = document.createElement('canvas').getContext('2d')!


    static ApplyMask(pValue: string | number, pMaskPattern: string): string
    {
        const CleanValue = typeof pValue === 'number' ? pValue.toString() : pValue.replace(/\D/g, '')
        const Masks = pMaskPattern.split('|').map(mask => ({ Mask: mask, Count: (mask.match(/#/g) || []).length })).OrderBy(a => a.Count)

        let SelectedMask = Masks.FirstOrNull(item => item.Count >= CleanValue.length)?.Mask ?? Masks[Masks.length - 1].Mask

        let Result = ''
        let DigitIndex = 0

        for (let i = 0; i < SelectedMask.length && DigitIndex < CleanValue.length; i++)
        {
            const Char = SelectedMask[i]
            Result += Char === '#' ? CleanValue[DigitIndex++] : Char
        }

        return Result
    }


    static ApplySize(pInput: HTMLElement, pText: string, pFont?: string): number
    {
        const style = getComputedStyle(pInput)

        const font = pFont || `${style.fontStyle || 'normal'} ${style.fontWeight || 'normal'} ${style.fontSize} ${style.fontFamily}`
        XUtils.CanvasContext.font = font

        switch (style.textTransform) 
        {
            case 'uppercase':
                pText = pText.toUpperCase();
                break
            case 'lowercase':
                pText = pText.toLowerCase();
                break
            case 'capitalize':
                pText = pText.replace(/\b\w/g, c => c.toUpperCase());
                break
        }

        const metrics = XUtils.CanvasContext.measureText(pText)
        const letterSpacing = parseFloat(style.letterSpacing) || 0
        const spacing = pText.length > 1 ? letterSpacing * (pText.length - 1) : 0
        const textWidth = metrics.width + spacing

        const pl = parseFloat(style.paddingLeft) || 0
        const pr = parseFloat(style.paddingRight) || 0
        const bl = parseFloat(style.borderLeftWidth) || 0
        const br = parseFloat(style.borderRightWidth) || 0

        let finalWidth = textWidth
        if (style.boxSizing === 'content-box')
            finalWidth += pl + pr + bl + br

        pInput.style.width = `${Math.ceil(finalWidth)}px`
        return Math.ceil(finalWidth);
    }


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