/// <reference path="../Stage/XStageTabControl.ts" />

class App extends XStageTabControlTab
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.ButtonBar = new XButtonBar(this);
        this.Scanes = new XDiv(this, "Scenes");
    }

    Scanes: XDiv;
    ButtonBar: XButtonBar;

    SetModel(pLoadApp: XAPPModel)
    {
        var dv = new SceneDataView(this.Scanes);
        dv.SetModel(pLoadApp);
    }
}
