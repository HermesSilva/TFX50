interface XMouseEvent { (pArg: MouseEvent): void; }
interface XISplashable
{
    BeginWait():void;

    EndWait(): void;

    ShowError(pError: any): void;

    LastSplash: any;
}

interface Element
{
    Owner: XElement | null;
    Class: string;
}

interface XIElement
{
    Owner: XElement | HTMLElement | null;
    HTML: HTMLElement;
    IsVisible: boolean;
    OrderIndex: number;
    GetOwnerOrSelf(pContext: XIElement): XISplashable;
}

interface XIDialog
{
    IsDialog: boolean;
}

interface XIDialogContainer extends XIElement
{
    IsDialogContainer: boolean;
    DialogContainer: XDialogContainer;
}

interface Window 
{
    ErrorDialog: any;
    Wait: any;

    ShowError(pError: Error): void;

    BeginWait(): void;

    EndWait(): void;

    Canvas: any;
    LogonForm: any;
    ConfigForm: any;

    ShowRemoteCMD(): void;

    InitializeMap: any;
    CITHook: any;
}

interface XIEditor extends XIElement
{
    RemoveTitle(): void;
    Description: string;
    IsNullable: boolean;
    AllowEmpty: boolean;
    IsReadOnly: boolean;
    IsRequired: boolean;
    IsFreeSearch: boolean;
    IsFormInplace: boolean;
    IsJustifyHeight: boolean;
    IsSelected: any;
    IsChecked: any;
    State: any;
    ID: string;
    Name: string;
    Value: any;
    Type: any;
    GeneratorInfo: XGeneratorInfo;
    DataSourceID: string;
    TargetDisplayFieldID: string[];
    SourceDisplayFieldID: string[];
    TargetFilterFieldID: string[];
    SourceFilterFieldID: string[];
    GridFormCID: string;
    RowsServiceID: string;
    ColsServiceID: string;
    AdditionalFieldsID: string[];
    AdditionalDataFieldsID: string[];
    Mask: string;
    LookupPKFieldID: string;
    OwnerID: string;
    ParentID: string;
    Order: number;
    Rows: number;
    Cols: number;
    NewLine: boolean;
    Rect: XRect;
    Input: HTMLInputElement;
    Title: string;
}

interface XIPopupPanel extends XIElement
{
    CallPopupClosed(): void;
    Show(): void;

    AutoClose: boolean;
    OnPopupClosed: XPopupClosedEvent | null;
    CanClose(pSource: HTMLElement): boolean;
}