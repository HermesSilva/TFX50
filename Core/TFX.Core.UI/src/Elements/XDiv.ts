/// <reference path="Base/XElement.ts" />
/// <reference path="../Reflection/XReflections.ts" />

class X21 extends XElement
{
    constructor()
    {
        super(null, "X21", "div");
    }

    protected override CreateContainer(): HTMLElement 
    {
        return XUtils.AddElement<HTMLElement>(null, "div", null);
    }

}

class XDiv extends XElement
{
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null)
    {
        super(pOwner, pClass);
    }

    protected override CreateContainer(): HTMLElement 
    {
        return XUtils.AddElement<HTMLElement>(null, "div", null);
    }

    @Inject(X21)
    X23456: X21 | null = null;
}