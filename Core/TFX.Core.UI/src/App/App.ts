/// <reference path="../Stage/XStageTabControl.ts" />
/// <reference path="../Net/XHttpClient.ts" />
/// <reference path="SceneFormEditor.ts" />
enum XAppState
{
    None = 0,
    Searching = 1,
    Editing = 2,
    Inserting = 3,
}

@AutoInit
class App extends XStageTabControlTab
{

    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.ButtonBar = new ActionBar(this);
        this.ButtonBar.App = this;
        this.ButtonBarR = new ActionBarR(this);
        this.Scanes = new XDiv(this, "Scenes");
        this._FormEditor = null;
        XEventManager.AddEvent(this, this.ButtonBar.Edit.HTML, XEventType.Click, () => this.OnEdit(XAppState.Editing));
        XEventManager.AddEvent(this, this.ButtonBar.New.HTML, XEventType.Click, () => this.OnEdit(XAppState.Inserting));
        this.Dialog = new XMessageDialog(this);
    }

    @Inject(XHttpClient, XLifetime.Transient)
    Client!: XHttpClient;

    Dialog: XMessageDialog;
    State: XAppState = XAppState.None;
    Scanes: XDiv;
    ButtonBar: ActionBar;
    ButtonBarR: ActionBarR;
    Model!: XAPPModel;
    DataView!: SceneDataView;
    SVCModel!: XServiceModel;
    private _FormEditor: SceneFormEditor | null;

    SetModel(pModel: XAPPModel)
    {
        this.State = XAppState.Searching;
        this.Model = pModel;
        this.DataView = new SceneDataView(this.Scanes);
        this.Client?.SendAsync(Paths.ServiceModel, { ID: pModel.SearchServiceID }, (pData: XResponse<XServiceModel>) =>
        {
            this.SVCModel = pData.Data
            this.DataView.SetModel(pData.Data);
            if (this.DataView?.DataGrid)
            {
                this.DataView.DataGrid.OnSelectionChanged = (rows) => this.ButtonBar.UpdateState(rows);
                this.DataView.DataGrid.OnRowDoubleClick = (rows) => this.OnEdit(XAppState.Editing);
            }
            this.SizeChanged();
        });
        this.ButtonBar.UpdateState();
    }

    override SizeChanged()
    {
        this.Scanes.HTML.style.top = this.ButtonBar.HTML.offsetHeight + "px";
        this.Scanes.HTML.style.height = (this.HTML.offsetHeight - this.ButtonBar.HTML.offsetHeight) + "px";
    }


    Close()
    {
        if (this._FormEditor != null)
        {
            this._FormEditor.Close();
            this._FormEditor = null;
            if (this.DataView)
                this.DataView.IsVisible = true;
            this.ButtonBar.UpdateState();
        }
        else
            super.Close();
    }

    private OnEdit(pState: XAppState)
    {
        if (!this.DataView || !this.DataView.DataGrid)
            return;

        const fmdl = this.Model.Forms.FirstOrNull(f => f.Type != XFRMType.SVCFilter) as XFRMModel | null;
        if (!fmdl)
            return;
        let filter: any = new Object();
        let ffld = new Object() as XFilterField;
        ffld.Operator = XOperator.EqualTo;
        ffld.State = XFieldState.NotEmpty;
        ffld.Value = this.DataView.DataGrid.SelectedRows[0].Tupla[this.SVCModel.PKFieldName].Value;
        filter[this.SVCModel.PKFieldName] = ffld;

        this.Client?.SendAsync(this.SVCModel.GetPath, filter, (pData: any) =>
        {
            this.ShowForm(pState, fmdl, pData.Data);
        });
    }

    private ShowForm(pState: XAppState, pModel: XFRMModel, pDataSet: XDataSet)
    {
        this.State = pState;

        if (this._FormEditor == null)
        {
            this._FormEditor = new SceneFormEditor(this.Scanes);
            this._FormEditor.OnClose = (_pArg: any) => this.CloseEditor();
            this._FormEditor.App = this;
            this._FormEditor.ShowDialog();
        }

        this.ButtonBar.UpdateState(this.DataView.DataGrid.SelectedRows);
        this._FormEditor.SetModel(pModel, this.DataView.SVCModel, pDataSet);
        this._FormEditor.IsVisible = true;
    }

    CloseEditor()
    {
        this.State = XAppState.Searching;
        this._FormEditor = null;
        this.DataView.IsVisible = true;
        this.ButtonBar.UpdateState();
    }
}

