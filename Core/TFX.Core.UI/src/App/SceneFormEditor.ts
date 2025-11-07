/// <reference path="../Stage/XScene.ts" />
/// <reference path="../Reflection/XReflections.ts" />
/// <reference path="../Net/XHttpClient.ts" />
/// <reference path="../Elements/Base/XBaseDialog.ts" />

@AutoInit
class SceneFormEditor extends XBaseCleanDialog
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

    App: App | undefined;
    Form: XForm;
    SVCModel!: XIServiceModel;
    Model!: XFRMModel;
    DataSet!: XDataSet;
    OnClose: XMethod<any> | null = null;

    private _TitleBar: XDiv;

    SetModel(pModel: XFRMModel, pSVCModel: XIServiceModel, pDataSet: XDataSet)
    {
        this.Model = pModel;
        this.SVCModel = pSVCModel;
        this.DataSet = pDataSet;
        this.Form.SetModel(this.Model, this.SVCModel);
        this.Form.SetDataSet(pDataSet);
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

    override OnShow()
    {
        if (!this.Form)
            return;
        XEventManager.SetTiemOut(this.Form, this.Form.FocusFirstInput, 0);
    }

    Close()
    {
        if (this.OnClose)
            this.OnClose.apply(this, [null]);
        this.IsVisible = false;
        this.Free();
    }
}
