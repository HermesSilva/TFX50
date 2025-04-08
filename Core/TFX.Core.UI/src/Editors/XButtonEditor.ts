/// <reference path="../Elements/Base/XBaseInput.ts" />
class XButtonEditor extends XBaseInput
{
    constructor(pOwner: XElement)
    {
        super(pOwner);
        this.Title = "Clique no Botão";
        XEventManager.AddEvent(this, this.Button.HTML, XEventType.Click, this.OnClick, true);
    }

    Button: XBaseButton | any;

    CreateInput(): HTMLInputElement
    {
        this.Button = new XBaseButton(this, "XLookupButton");
        return <any>this.Button.HTML;
    }
    OnClick(pArg: MouseEvent)
    {
        var con = this.GetDialogContainer();
        var dlg = new XBaseDialog(<any>con);
        dlg.Title = "Mostrando o Dialogo"
        dlg.ShowDialog();
    }
}