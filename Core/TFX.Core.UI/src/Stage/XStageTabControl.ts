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
        this.AddTab("Aninha");
        this.AddTab("Maria");
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
    }


    CreateTab(): XTabControlTab
    {
        return new XStageTabControlTab(this.Container);;
    }
}

