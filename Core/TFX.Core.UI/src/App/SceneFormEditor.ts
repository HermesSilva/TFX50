/// <reference path="../Stage/XScene.ts" />
/// <reference path="../Reflection/XReflections.ts" />
/// <reference path="../Net/XHttpClient.ts" />

class SceneFormEditor extends XScene
{
    constructor(pOwner: XElement)
    {
        super(pOwner);
        this.HTML.className = "SceneFormEditor";
        this._TitleBar = new XDiv(this, "SceneFormEditorTitle");
        this.Form = new XForm(this);
        this.Form.HTML.className = "ScenePopupForm";
        this.IsVisible = false;
        this.AutoIncZIndex = true;
    }

    Form: XForm;
    SVCModel!: XServiceModel;
    Model!: XFRMModel;

    OnClose: XMethod<any> | null = null;

    private _DialogContainer: XIDialogContainer | null = null;
    private _TitleBar: XDiv;

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
        this.UpdateTitle();
    }

      override Show(pValue: boolean = true)
    {
        if (this._DialogContainer == null)
        {
            this._DialogContainer = this.GetDialogContainer();
            if (this._DialogContainer && this._DialogContainer.DialogContainer && this._DialogContainer.DialogContainer.HTML !== this.HTML.parentElement)
            {
                this.HTML.parentElement?.removeChild(this.HTML);
                this._DialogContainer.DialogContainer.HTML.appendChild(this.HTML);
            }
        }

        super.Show(pValue);

        if (this._DialogContainer)
            this._DialogContainer.DialogContainer.IsVisible = pValue;

        if (pValue)
            this.UpdateTitle();
    }


    private UpdateTitle()
    {
        const at = this.App?.Model?.Title ?? "";
        let act = "";
        switch (this.App?.State)
        {
            case XAppState.Inserting:
                act = "Incluindo";
                break;
            case XAppState.Editing:
                act = "Editando";
                break;
            case XAppState.Searching:
                act = "Pesquisando";
                break;
            default:
                act = "";
                break;
        }
        const ttl = act ? `${at} - ${act}` : at;
        this._TitleBar.HTML.innerText = ttl;
    }

    Close()
    {
        if (this._DialogContainer)
            this._DialogContainer.DialogContainer.IsVisible = false;
        if (this.OnClose)
            this.OnClose.apply(this, [null]);
        this.Free();
    }
}
