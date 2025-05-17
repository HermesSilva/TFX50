enum XPAMActionView
{
    None = 0,
    Search = 1,
    SearchByRow = 2,
    Edit = 3,
    View = 4,
    EditView = 5,
    AllTime = 6,
    New = 7
}

class XAddButton 
{
    ServiceID!: number;
    IconURL!: string;
    AskMessage!: string;
    Hint!: string;
    AllRecords!: boolean;
    ActionType!: XPAMActionView;
    Rights!: string[];
}

class XAPPStep 
{
    public DataSourceID!: string;
    public DataSetID!: string;
    public NextStepsID!: string[];
    public PreviousStepsID!: string[];
    public FormID!: string;
    public GroupID!: string;
    public Paths!: string[];
    public Rect!: XRect;
    public SourceFieldID!: string;
    public Flow!: number;
    public Values!: bigint[];
    public AditionalFormsID!: string[];
    public SourceLinkFieldID!: string;
    public TargetLinkFieldID!: string;
    public TitleFieldID!: string;
    public StateFieldID!: string;
    public IsRepeatable!: boolean;
    public Previous!: string;
    public PreviousDescription!: string;
    public Next!: string;
    public NextDescription!: string;
    public FlushService!: boolean;
}

class XAPPModel
{
    ID!: string;
    Title!: string;
    Name!: string;
    Steps: XAPPStep[] = [];
    DataSourceId!: string;
    FlushAtStep!: boolean;
    Size!: { width: number; height: number };
    HasDetail!: boolean;
    SearchServiceId!: string;
    SearchPkId!: string;
    SubjectFields!: string[];
    SubjectTitle!: string;
    MainViewId!: string;
    MainMenuId!: number;
    PlatformId!: number;
    IsSystem!: boolean;
    ConfigFormId!: string;
    IsDisabled!: boolean;
    Rights!: number[];
    Icon!: string;
    GroupId!: number;
    AppTypeId!: number;
    UseTypeId!: number[];
    OnlyHolding!: boolean;
    FreeForUsers!: boolean;
    SearchPath: string | undefined;
}

