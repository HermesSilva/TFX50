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
        this.AutoIncZIndex = true;
    }

    Form: XForm;
    SVCModel!: XServiceModel;
    Model!: XFRMModel;

    OnClose: XMethod<any> | null = null;

    private _DialogContainer: XIDialogContainer | null = null;

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

    private ResizeToContainer()
    {
        //if (this._DialogContainer == null)
        //    return;
        //const r = this._DialogContainer.DialogContainer.HTML.GetRect(true);
        //const l =15;
        //const t =15;
        //const w = Math.max(0, r.Width -30);
        //const h = Math.max(0, r.Height -30);
        //this.HTML.style.left = l + "px";
        //this.HTML.style.top = t + "px";
        //this.HTML.style.width = w + "px";
        //this.HTML.style.height = h + "px";
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
            this.ResizeToContainer();
    }

    override SizeChanged()
    {
        super.SizeChanged();
        this.ResizeToContainer();
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
