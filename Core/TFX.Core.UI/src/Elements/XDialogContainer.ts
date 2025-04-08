/// <reference path="XDiv.ts" />
class XDialogContainer extends XDiv 
{
    constructor(pOwner: XElement, pClass: string)
    {
        super(pOwner, pClass);
        this.IsVisible = false;
        this.AutoIncZIndex = true;
    }
}