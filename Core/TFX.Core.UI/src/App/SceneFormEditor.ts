/// <reference path="../Stage/XScene.ts" />
/// <reference path="../Reflection/XReflections.ts" />
/// <reference path="../Net/XHttpClient.ts" />

class SceneFormEditor extends XScene
{
    constructor(pOwner: XElement)
    {
        super(pOwner);
        this.HTML.className = "SceneFormEditor";
        this.Form = new XForm(this);
        this.IsVisible = false;
    }

    Form: XForm;
    SVCModel!: XServiceModel;
    Model!: XFRMModel;

    // callback invoked when the editor is closed
    OnClose: XMethod<any> | null = null;

    SetModel(pModel: XFRMModel, pSVCModel: XServiceModel)
    {
        this.Model = pModel;
        this.SVCModel = pSVCModel;
        this.Load();
    }

    Load()
    {
        if (!this.Form)
            return;
        this.Form.SetModel(this.Model, this.SVCModel);
    }

    Close()
    {
        if (this.OnClose)
            this.OnClose.apply(this, [null]);
        this.Free();
    }
}
