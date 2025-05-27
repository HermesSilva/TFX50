/// <reference path="../XDiv.ts" />
class XBaseInput extends XDiv implements XIEditor
{

    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "InputContainer");
        this.Input = this.CreateInput();
        this.ELMTitle = new XDiv(this, "InputTitle");

    }
    Name!: string;
    Description!: string;
    IsNullable!: boolean;
    AllowEmpty!: boolean;
    IsReadOnly!: boolean;
    IsRequired!: boolean;
    IsFreeSearch!: boolean;
    IsFormInplace!: boolean;
    IsJustifyHeight!: boolean;
    IsSelected: any;
    IsChecked: any;
    State: any;
    Value: any;
    Type: any;
    GeneratorInfo!: XGeneratorInfo;
    DataSourceID!: string;
    TargetDisplayFieldID!: string[];
    SourceDisplayFieldID!: string[];
    TargetFilterFieldID!: string[];
    SourceFilterFieldID!: string[];
    GridFormCID!: string;
    RowsServiceID!: string;
    ColsServiceID!: string;
    AdditionalFieldsID!: string[];
    AdditionalDataFieldsID!: string[];
    LookupPKFieldID!: string;
    OwnerID!: string;
    ParentID!: string;
    Order!: number;
    Input!: HTMLInputElement;
    protected ELMTitle: XDiv;
    NewLine: boolean = false;
    OrderIndex: number = -1;

    private _Mask: string = '';

    public get Mask(): string
    {
        return this._Mask;
    }
    public set Mask(value: string)
    {
        this._Mask = value;
        this.ApplyMask();
    }

    protected ApplyMask()
    {    
    }

    RemoveTitle()
    {
        this.ELMTitle?.Free();
    }
    get Title(): string
    {
        return this.ELMTitle.HTML.innerHTML;
    }
    set Title(pValue: string)
    {
        this.ELMTitle.HTML.innerHTML = pValue;
    }

    CreateInput(): HTMLInputElement
    {
        return XUtils.AddElement<HTMLInputElement>(this.HTML, "input", "XBaseButtonInput");
    }
}