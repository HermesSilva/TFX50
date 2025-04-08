/// <reference path="XDiv.ts" />
class XWrapPanel extends XDiv 
{
    constructor(pOwner: XElement, pClass: string | null = null)
    {
        super(pOwner, pClass ?? "XWrapPanel");
    }
}

