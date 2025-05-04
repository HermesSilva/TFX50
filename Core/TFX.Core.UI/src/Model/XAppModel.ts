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
    public ID!: string;
    public Title!: string;
    public Name!: string;
    public Steps: XAPPStep[] = [];
    public DataSourceId!: string;
    public FlushAtStep!: boolean;
    public Size!: { width: number; height: number };
    public HasDetail!: boolean;
    public SearchServiceId!: string;
    public SearchPkId!: string;
    public SubjectFields!: string[];
    public SubjectTitle!: string;
    public MainViewId!: string;
    public MainMenuId!: number;
    public PlatformId!: number;
    public IsSystem!: boolean;
    public ConfigFormId!: string;
    public IsDisabled!: boolean;
    public Rights!: number[];
    public Icon!: string;
    public GroupId!: number;
    public AppTypeId!: number;
    public UseTypeId!: number[];
    public OnlyHolding!: boolean;
    public FreeForUsers!: boolean;
}

