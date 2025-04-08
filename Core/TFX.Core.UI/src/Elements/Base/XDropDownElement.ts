class XDropDownElement extends XPopupElement 
{
    constructor(pOwner: XElement, pClass: string)
    {
        super(pOwner, pClass);
        this.IsVisible = false;
    }

    Selected()
    {
        XPopupManager.HideAll();
    }

    BindTo(pElement: XElement)
    {
        const editorRect = pElement.HTML.getBoundingClientRect();
        const dropdownRect = this.HTML.getBoundingClientRect();
        const viewportWidth = window.innerWidth;
        const viewportHeight = window.innerHeight;

        let top;
        const spaceBelow = viewportHeight - editorRect.bottom;
        const spaceAbove = editorRect.top;

        if (dropdownRect.height <= spaceBelow)
            top = editorRect.bottom;
        else
            if (dropdownRect.height <= spaceAbove)
                top = editorRect.top - dropdownRect.height;
            else
                top = spaceBelow > spaceAbove ? editorRect.bottom : editorRect.top - dropdownRect.height;

        let left;
        const spaceRight = viewportWidth - editorRect.left;

        if (dropdownRect.width <= spaceRight)
            left = editorRect.left;
        else
        {
            const spaceLeft = editorRect.right;
            if (dropdownRect.width <= spaceLeft)
                left = editorRect.right - dropdownRect.width;
            else
                left = Math.max(0, viewportWidth - dropdownRect.width);
        }

        this.HTML.style.position = 'fixed';
        this.HTML.style.top = `${top}px`;
        this.HTML.style.left = `${left}px`;

    }

}