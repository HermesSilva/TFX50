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
enum XScenes 
{
    SearchGrid = 1,
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

interface XAPPModel
{
    ID: string;
    Title: string;
    Name: string;
    SearchServiceID: string;
    SearchPath: string;
    Forms: XFRMModel[];
}

