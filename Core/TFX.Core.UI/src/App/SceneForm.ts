/// <reference path="../Stage/XScene.ts" />
/// <reference path="../Reflection/XReflections.ts" />
/// <reference path="../Net/XHttpClient.ts" />

class SceneForm extends XScene
{
    constructor(pOwner: App)
    {
        super(pOwner);
        this.HTML.className = "Scenes";
        this.App = pOwner;
        this.Form = new XForm(this);
        this.IsVisible = false;
    }

    App: App;
    Model!: XFRMModel;
    Form: XForm;

    SetModel(pModel: XFRMModel)
    {
        this.Model = pModel;
        this.Load();
    }

    Load()
    {
        this.Form.SetModel(this.Model);
    }
}
