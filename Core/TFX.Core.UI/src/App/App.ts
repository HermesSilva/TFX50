/// <reference path="../Stage/XStageTabControl.ts" />
/// <reference path="../Net/XHttpClient.ts" />

@AutoInit
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
    @Inject(XHttpClient, XLifetime.Transient)
    Client!: XHttpClient;
    Model!: XAPPModel;
    DataView!: SceneDataView;

    SetModel(pModel: XAPPModel)
    {
        this.Model = pModel;
        this.DataView = new SceneDataView(this.Scanes);
        this.Client?.SendAsync(Paths.ServiceModel, { ID: pModel.SearchServiceID }, (pData: XResponse<XServiceModel>) =>
        {
            this.DataView.SetModel(pData.Data);
        });
        this.Prepare();
    }

    Prepare()
    {
        for (var i = 0; i < this.Model.Forms.length; i++)
        {
            var fmdl = this.Model.Forms[i];
            if (fmdl.Type == XFRMType.SVCFilter)
                continue;
            var frm = new SceneForm(this);
            frm.SetModel(fmdl);
        }
    }
}
