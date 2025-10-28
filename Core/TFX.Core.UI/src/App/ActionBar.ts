/// <reference path="../Stage/XStageTabControl.ts" />
/// <reference path="../Net/XHttpClient.ts" />
/// <reference path="../Elements/XWrapPanel.ts" />

class ActionBar extends XWrapPanel
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
        this.SetButtonsVisible(false, false, false, false);
    }
    App: App;
    Edit: XSVGButton;
    Inactive: XSVGButton;
    Active: XSVGButton;
    Save: XSVGButton;

    UpdateBySelection(rows: XArray<XTableRow> | null)
    {
        if (rows == null || rows.length === 0)
        {
            this.SetButtonsVisible(false, false, false, false);
            return;
        }

        const row = rows[0];
        const tuple = row?.Tupla as XDataTuple;
        const state = tuple?.State as XTupleState | undefined;

        if (!tuple || state === undefined)
        {
            this.SetButtonsVisible(false, false, false, false);
            return;
        }

        let canEdit = true;
        let canSave = false;
        let showInactive = true;
        let showActive = true;

        switch (state)
        {
            case XTupleState.Deleted:
                canEdit = false;
                canSave = false;
                showInactive = false;
                showActive = true;
                break;

            case XTupleState.Added:
            case XTupleState.Insert:
                canEdit = true;
                canSave = true;
                showInactive = false;
                showActive = false;
                break;

            case XTupleState.Modified:
                canEdit = true;
                canSave = true;
                showInactive = true;
                showActive = true;
                break;


            case XTupleState.Detached:
            case XTupleState.Unchanged:
                canEdit = true;
                canSave = false;
                showInactive = true;
                showActive = false;
                break;
        }
        
        if (tuple && tuple.IsReadOnly === true)
        {
            canEdit = false;
            canSave = false;
        }

        this.SetButtonsVisible(canEdit, showInactive, showActive, canSave);
    }

    private SetButtonsVisible(edit: boolean, inactive: boolean, active: boolean, save: boolean)
    {
        this.Edit.HTML.style.display = edit ? "" : "none";
        this.Inactive.HTML.style.display = inactive ? "" : "none";
        this.Active.HTML.style.display = active ? "" : "none";
        this.Save.HTML.style.display = save ? "" : "none";
    }
}

class ActionBarR extends XWrapPanel
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
