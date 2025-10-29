/// <reference path="../Stage/XStageTabControl.ts" />
/// <reference path="../Net/XHttpClient.ts" />
/// <reference path="SceneFormEditor.ts" />
enum XAppState
{
    None = 0,
    Searching = 1,
    Editing = 2,
}

@AutoInit
class App extends XStageTabControlTab
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.ButtonBar = new ActionBar(this);
        this.ButtonBarR = new ActionBarR(this);
        this.Scanes = new XDiv(this, "Scenes");
        this._FormEditor = null;
        XEventManager.AddEvent(this, this.ButtonBar.Edit.HTML, XEventType.Click, () => this.OnEdit());
    }

    @Inject(XHttpClient, XLifetime.Transient)
    Client!: XHttpClient;

    Scanes: XDiv;
    ButtonBar: ActionBar;
    ButtonBarR: ActionBarR;
    Model!: XAPPModel;
    DataView!: SceneDataView;
    private _FormEditor: SceneFormEditor | null;

    SetModel(pModel: XAPPModel)
    {
        this.Model = pModel;
        this.DataView = new SceneDataView(this.Scanes);
        this.Client?.SendAsync(Paths.ServiceModel, { ID: pModel.SearchServiceID }, (pData: XResponse<XServiceModel>) =>
        {
            this.DataView.SetModel(pData.Data);
            if (this.DataView?.DataGrid)
                this.DataView.DataGrid.OnSelectionChanged = (rows) => this.ButtonBar.UpdateState(XAppState.Searching, rows);
            this.ButtonBar.UpdateState(XAppState.None, null);
            this.SizeChanged();
        });
        this.Prepare();
    }

    override SizeChanged()
    {
        this.Scanes.HTML.style.top = this.ButtonBar.HTML.offsetHeight + "px";
        this.Scanes.HTML.style.height = (this.HTML.offsetHeight - this.ButtonBar.HTML.offsetHeight) + "px";
        if (this._FormEditor)
        {
            this._FormEditor.HTML.style.top = this.ButtonBar.HTML.offsetHeight + "px";
            this._FormEditor.HTML.style.height = (this.HTML.offsetHeight - this.ButtonBar.HTML.offsetHeight) + "px";
        }
    }

    Prepare()
    {
        for (let i = 0; i < this.Model.Forms.length; i++)
        {
            let fmdl = this.Model.Forms[i];
            if (fmdl.Type == XFRMType.SVCFilter)
                continue;
            let frm = new SceneForm(this);
            frm.SetModel(fmdl);
        }
    }

    Close()
    {
        if (this._FormEditor != null)
        {
            this._FormEditor.Close();
            this._FormEditor = null;
            if (this.DataView)
                this.DataView.IsVisible = true;
            this.ButtonBar.UpdateState(XAppState.Searching, this.DataView.DataGrid.SelectedRows);
        }
        else
            super.Close();
    }

    private OnEdit()
    {
        if (!this.DataView || !this.DataView.DataGrid)
            return;

        const fmdl = this.Model.Forms.FirstOrNull(f => f.Type != XFRMType.SVCFilter) as XFRMModel | null;
        if (!fmdl)
            return;

        if (this._FormEditor == null)
        {
            this._FormEditor = new SceneFormEditor(this.Scanes);
            this._FormEditor.OnClose = (_pArg: any) => this.CloseEditor();
        }

        this.DataView.IsVisible = false;
        this.ButtonBar.UpdateState(XAppState.Editing, this.DataView.DataGrid.SelectedRows);
        this._FormEditor.SetModel(fmdl, this.DataView.SVCModel);
        this._FormEditor.IsVisible = true;
    }

    CloseEditor()
    {
        this._FormEditor = null;
        this.DataView.IsVisible = true;
        this.ButtonBar.UpdateState(XAppState.Searching, this.DataView.DataGrid.SelectedRows);
    }
}

