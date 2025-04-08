/// <reference path="../Elements/Base/XBaseInput.ts" />
class XEMailEditor extends XBaseInput
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.Input.className = "XEMailEditor";
        this.Input.placeholder = "e-mail";
        XEventManager.AddEvent(this, this.HTML, XEventType.Input, this.Validate);
        this.Title = "Digite um E-Mail";
    }

    Validate(pArg: InputEvent)
    {
        pArg.preventDefault(); // Impede o envio do formulário
        const email = this.Input.value;
        var isvalid = email.match(/^[^\s@]+@[^\s@]+\.[^\s@]+$/);

        this.Input.classList.remove('Error');
        if (isvalid)
            return;
        if (!isvalid)
            this.Input.classList.add('Error');

    }
}