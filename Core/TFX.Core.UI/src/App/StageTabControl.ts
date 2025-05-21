/// <reference path="../Stage/XStageTabControl.ts" />

class StageTabControl extends XStageTabControl
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
    }

    CreateTab(): XTabControlTab
    {
        return new App(this.Container);;
    }
}
