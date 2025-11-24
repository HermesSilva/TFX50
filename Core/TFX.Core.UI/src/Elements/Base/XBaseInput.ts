/// <reference path="../XDiv.ts" />
class XBaseInput extends XDiv implements XIEditor
{
    constructor(pOwner: XElement | HTMLElement | null)
    {
        super(pOwner, "InputContainer");
        this.ELMTitle = new XDiv(this, "InputTitle");
        this.Input = this.CreateInput();
    }

    Name!: string;
    Description!: string;
    IsNullable!: boolean;
    AllowEmpty!: boolean;
    private _IsReadOnly: boolean = false;
    IsRequired!: boolean;
    IsFreeSearch!: boolean;
    IsFormInplace!: boolean;
    IsJustifyHeight!: boolean;
    IsSelected: any;
    IsChecked: any;
    State: any;
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
    protected _RawValue: any;
    private _Mask: string = '';
    Field!: XFRMField;
    Tuple!: XTuple | any;
    SVCModel!: XIServiceModel;

    BindNotify(pSouce: any, pField: string, pValue: any)
    {
        this.RefreshData(pValue)
    }

    RefreshData(pValue: any)
    {
        this.RawValue = pValue;
    }

    get RawValue(): any
    {
        return this._RawValue;
    }
    set RawValue(value: any)
    {
        this._RawValue = value;
        if (this.Input)
            this.Input.value = value;
    }

    public get IsReadOnly(): boolean
    {
        return this._IsReadOnly;
    }

    public set IsReadOnly(value: boolean)
    {
        this._IsReadOnly = value;
    }
    Clear(): void
    {
        this._RawValue = null;
        this.Value = null;
    }
    get Value(): any
    {
        if (this.Input && this.Input.value)
            return this.Input.value;
        return null;
    }

    SetField(pField: XFRMField, pSVCModel: XIServiceModel)
    {
        this.Field = pField;
        this.SVCModel = pSVCModel;
    }
    set Value(pValue: any)
    {
        if (this.Input && this.Input.value)
            this.Input.value = pValue;
    }

    public get Mask(): string
    {
        return this._Mask;
    }
    public set Mask(value: string)
    {
        this._Mask = value;
        this.ApplyMask();
    }

    protected ApplyMask(): boolean
    {
        return false;
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