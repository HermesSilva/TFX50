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
    Dialog: XBaseDialog | null = null;

    CreateInput(): HTMLInputElement
    {
        this.Button = new XBaseButton(this, "XLookupButton");
        return <any>this.Button.HTML;
    }

    OnClick(pArg: MouseEvent)
    {
        if (this.Dialog == null)
        {
            var con = this.GetDialogContainer();
            this.Dialog = new XBaseDialog(<any>con);
            this.Dialog.Title = "Mostrando o Dialogo"
        }
        this.Dialog.ShowDialog();
    }
}