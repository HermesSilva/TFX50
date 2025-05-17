/// <reference path="../Elements/XTabControl.ts" />


class XStageTabControlTab extends XTabControlTab implements XIDialogContainer
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.IsDialogContainer = true;
    }
    IsDialogContainer: boolean;
}

class XStageTabControl extends XTabControl implements XIDialogContainer
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.IsDialogContainer = true;
        this.HTML.classList.add("Main");
    }

    CreateTab(): XTabControlTab
    {
        return new XStageTabControlTab(this.Container);;
    }
}

