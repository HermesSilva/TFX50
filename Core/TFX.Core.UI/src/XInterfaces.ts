interface XMouseEvent { (pArg: MouseEvent): void; }
interface Element
{
    Owner: XElement | null;
}
interface XIElement
{
    Owner: XElement | HTMLElement | null;
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
interface XIElement
{
    HTML: HTMLElement;
    IsVisible: boolean;
    OrderIndex: number;
}

interface XIEditor extends XIElement
{
    Rows: number;
    Cols: number;
    NewLine: boolean;
    Rect: XRect;
    Input: HTMLInputElement;
}

interface XIPopupPanel extends XIElement
{
    CallPopupClosed(): void;
    Show(): void;

    AutoClose: boolean;
    OnPopupClosed: XPopupClosedEvent | null;
    CanClose(pSource: HTMLElement): boolean;
}