enum XOperator
{
    EqualTo = 5,
    NotEqualTo = 6,
    GreaterThan = 7,
    GreaterThanOrEqualTo = 8,
    LessThanOrEqualTo = 9,
    LessThan = 10,
    IsNull = 11,
    IsNotNull = 12,
    Like = 13,
    LikeBegin = 14,
    LikeEnd = 15,
    In = 16,
    NotIn = 17
}

enum XLogic
{
    AND = 1,
    OR = 2
}

enum XParentheses
{
    Open = 3,
    Close = 4
}
enum XFontStyle
{
    Unset = 10,
    Regular = 0,
    Bold = 1,
    Italic = 2,
    BoldItalic = 3,
    Underline = 4,
    Strikeout = 8,
    Normal = 11
}

enum XTextAlignment
{
    Unset = 0,
    BaseLineLeft = 1,
    TopLeft = 2,
    CenterLeft = 3,
    BottomLeft = 4,
    BaseLineCenter = 5,
    TopCenter = 6,
    Center = 7,
    BottomCenter = 8,
    BaseLineRight = 9,
    TopRight = 10,
    CenterRight = 11,
    BottomRight = 12
}

enum XHorizontalAlignment
{
    None = 0,
    Left = 1,
    Right = 2,
    Center = 3,
    Stretch = 4
}

enum XVerticalAlignment
{
    None = 0,
    Top = 1,
    Bottom = 2,
    Center = 3,
    Stretch = 4
}

enum XFRMFieldFilterType
{
    Contains = 1,
    NotContains = 2
}

enum XFRMEditorType
{
    AdhocEditor = 1,
    AnswerStringEditor = 2,
    BooleanEditor = 3,
    ButtonEditor = 4,
    ComboStringEditor = 5,
    ConstantLabelBox = 6,
    DateEditor = 7,
    DateTimeEditor = 8,
    DBDataLabelBox = 9,
    DecimalEditor = 10,
    DescriptionEditor = 11,
    DetailBinaryBox = 12,
    DetailSchedulerEditor = 13,
    DetailsDataGridEditor = 14,
    DynamicFormBox = 15,
    FileUpload = 16,
    FingerprintEditor = 17,
    HTMLEditor = 18,
    ImageFileEditor = 19,
    Int32Editor = 20,
    Int64Editor = 21,
    LabelBox = 22,
    MemoEditor = 23,
    PasswordEditor = 24,
    RepeatableDetailEditor = 25,
    SchedulerBoxEditor = 26,
    ServiceSelectorEditor = 27,
    StaticCrossDataGridEditor = 28,
    StaticSelectorEditor = 29,
    StringDiscreetEditor = 30,
    StringDiscreetParagraphEditor = 31,
    StringEditor = 32,
    TimeEditor = 33,
    TreeViewEditor = 34
}

enum XFRMStyle
{
    Normal = 0,
    Document = 1
}

enum XFRMType
{
    None = 0,
    Activity = 1,
    SVCFilter = 2,
    DetailGrid = 3,
    Config = 4,
    StandAlone = 5,
    PAMAdditionalForm = 6,
    RPTFilter = 7
}
interface XObject
{
    Description: string
    Owner: XObject
    ID: string
    Name: string
    Title: string
}

interface XGeneratorInfo
{
    GeneratorID: string
    Values: any
    DataSourceID: string
    FilterInactive: boolean
    AllowEmpty: boolean
    AllowDuplicity: boolean
    ConstantValue: any
    TestFieldsID: string
    DataSourceFieldsID: string
}

interface XFRMField extends XObject
{
    IsSelected: any
    IsChecked: any
    State: any
    Type: any
    GeneratorInfo: XGeneratorInfo
    EditorType: XFRMEditorType
    DataSourceID: string
    Location: number
    EditorCID: string
    DefaultValue: any
    IsNullable: boolean
    AllowEmpty: boolean
    RowCount: number
    ColCount: number
    OwnerID: string
    ParentID: string
    TargetDisplayFieldID: string[]
    SourceDisplayFieldID: string[]
    TargetFilterFieldID: string[]
    SourceFilterFieldID: string[]
    GridFormCID: string
    RowsServiceID: string
    ColsServiceID: string
    JustifyHeight: boolean
    IsRequired: boolean
    AdditionalFieldsID: string[]
    AdditionalDataFieldsID: string[]
    Order: number
    Model: XFRMModel
    IsFreeSearch: boolean
    Mask: string
    LookupPKFieldID: string
    FormImplace: boolean
    IsReadOnly: boolean
    FormFieldID: string
    TypeID: string
    Length: number
    Scale: number
    PAMID: string
    SourceFieldID: string
    AutoLoad: boolean
    RowFieldID: string
    ForceRW: boolean
    FilterData: number[]
    FilterType: XFRMFieldFilterType
    CanInsert: boolean
    CanUpdate: boolean
    Operator: XOperator
    FilterInative: boolean
    SearchServiceID: string
    SearchPKFieldID: string
    SearchFilterFieldsID: number[]
    ValueMath: string
    FontColor: string
    FontStyle: XFontStyle
    ShowFooter: boolean
    ValueItems: string[]
    IsAnswer: boolean
    AllowMultiSelect: boolean
    NewLine: boolean
    FontSize: number
    ViewSAM: string
    IsHidden: boolean
    AllwaysPrint: boolean
    Rules: string[];
    Value: any
}

interface XFRMModel extends XObject
{
    Icon(Icon: any): unknown
    Fields: XFRMField[]
    IsLineForm: boolean
    MinRows: number
    ConfigCID: string
    ConfigPKFilter: boolean
    CompanyFilter: boolean
    Style: XFRMStyle
    Type: XFRMType
}
