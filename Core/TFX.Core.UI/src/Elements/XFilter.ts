/// <reference path="XForm.ts" />
/// <reference path="XDiv.ts" />
class XFilter extends XForm
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner);
    }

    DoSerach?: XMethod<any>;

    override SetModel(pForm: XFRMModel, pSVCModel: XServiceModel)
    {
        super.SetModel(pForm, pSVCModel)
        let srcbox = <XSearchBoxEditor>this.Fields.FirstOrNull(f => f instanceof XSearchBoxEditor);
        if (srcbox && this.DoSerach)
        {
            srcbox.OnSerach = this.DoSerach;
        }
    }
}

