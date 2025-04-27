/// <reference path="../../Elements/XTabControl.ts" />


class XStageTabControlTab extends XTabControlTab implements XIDialogContainer
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.IsDialogContainer = true;
        this.Form = new XForm(this);
    }
    IsDialogContainer: boolean;
    Form: XForm;
}

class XStageTabControl extends XTabControl implements XIDialogContainer
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
        this.IsDialogContainer = true;
        this.HTML.classList.add("Main");
        //this.AddTab("Aninha");
        //this.AddTab("Maria");
        //this.AddTab("Joana");
        //this.AddTab("Rebeca");
        //this.AddTab("Antonieta");
        //this.AddTab("Valentina");
        //this.AddTab("Amanda");
        //this.AddTab("Jaqueline");
        //this.AddTab("Helena");
        //this.AddTab("Fernanda");
        //this.AddTab("Sonia");
        //this.AddTab("Larissa");
        //this.AddTab("Eleonora");
        //this.AddTab("Sara");
        //this.AddTab("Sebastina");
        //this.AddTab("Sabrina");
        //this.AddTab("a Aninha");
        //this.AddTab("a Maria");
        //this.AddTab("a Joana");
        //this.AddTab("a Rebeca");
        //this.AddTab("a Antonieta");
        //this.AddTab("a Valentina");
        //this.AddTab("a Amanda");
        //this.AddTab("a Jaqueline");
        //this.AddTab("a Helena");
        //this.AddTab("a Fernanda");
        //this.AddTab("a Sonia");
        //this.AddTab("a Larissa");
        //this.AddTab("a Eleonora");
        //this.AddTab("a Sara");
        //this.AddTab("a Sebastina");
        //this.AddTab("a Sabrina");
    }


    CreateTab(): XTabControlTab
    {
        return new XStageTabControlTab(this.Container);;
    }
}

