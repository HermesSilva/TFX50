/// <reference path="../Stage/XStageTabControl.ts" />
/// <reference path="../Net/XHttpClient.ts" />
/// <reference path="../Elements/XWrapPanel.ts" />

class XButtonBar extends XWrapPanel
{
    constructor(pOwner: App)
    {
        super(pOwner, "XButtonBar");
        this.App = pOwner;

        this.Edit = new XSVGButton(this);
        this.Edit.HTML.className = "XButtonBarItem";
        this.Edit.SetIcon("svg/edit.svg");
        this.Inactive = new XSVGButton(this);
        this.Inactive.HTML.className = "XButtonBarItem";
        this.Inactive.SetIcon("svg/trash.svg");
        this.Active = new XSVGButton(this);
        this.Active.HTML.className = "XButtonBarItem";
        this.Active.SetIcon("svg/recycle.svg");
        this.Save = new XSVGButton(this);
        this.Save.HTML.className = "XButtonBarItem";
        this.Save.SetIcon("svg/save.svg");
    }
    App: App;
    Edit: XSVGButton;
    Inactive: XSVGButton;
    Active: XSVGButton;
    Save: XSVGButton;
}

class XButtonBarR extends XWrapPanel
{
    constructor(pOwner: App)
    {
        super(pOwner, "XButtonBarR");
        this.App = pOwner;
        this.Close = new XSVGButton(this);
        this.Close.HTML.className = "XButtonBarItem";
        this.Close.SetIcon("svg/close.svg");
        XEventManager.AddEvent(this, this.Close.HTML, XEventType.Click, () => this.App.Close());
    }
    Close: XSVGButton;
    App: App;
}


@AutoInit
class App extends XStageTabControlTab
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.ButtonBar = new XButtonBar(this);
        this.ButtonBarR = new XButtonBarR(this);
        this.Scanes = new XDiv(this, "Scenes");
    }

    @Inject(XHttpClient, XLifetime.Transient)
    Client!: XHttpClient;

    Scanes: XDiv;
    ButtonBar: XButtonBar;
    ButtonBarR: XButtonBarR;
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
        this.Scanes.HTML.style.top = this.ButtonBar.HTML.offsetHeight + "px";
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
