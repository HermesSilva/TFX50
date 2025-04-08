declare enum XKey {
    K_CANCEL = 3,
    K_HELP = 6,
    K_BACK_SPACE = 8,
    K_TAB = 9,
    K_CLEAR = 12,
    K_RETURN = 13,
    K_ENTER = 14,
    K_SHIFT = 16,
    K_CONTROL = 17,
    K_ALT = 18,
    K_PAUSE = 19,
    K_CAPS_LOCK = 20,
    K_ESCAPE = 27,
    K_SPACE = 32,
    K_PAGE_UP = 33,
    K_PAGE_DOWN = 34,
    K_END = 35,
    K_HOME = 36,
    K_LEFT = 37,
    K_UP = 38,
    K_RIGHT = 39,
    K_DOWN = 40,
    K_PRINTSCREEN = 44,
    K_INSERT = 45,
    K_DELETE = 46,
    K_0 = 48,
    K_1 = 49,
    K_2 = 50,
    K_3 = 51,
    K_4 = 52,
    K_5 = 53,
    K_6 = 54,
    K_7 = 55,
    K_8 = 56,
    K_9 = 57,
    K_SEMICOLON = 59,
    K_EQUALS = 61,
    K_A = 65,
    K_B = 66,
    K_C = 67,
    K_D = 68,
    K_E = 69,
    K_F = 70,
    K_G = 71,
    K_H = 72,
    K_I = 73,
    K_J = 74,
    K_K = 75,
    K_L = 76,
    K_M = 77,
    K_N = 78,
    K_O = 79,
    K_P = 80,
    K_Q = 81,
    K_R = 82,
    K_S = 83,
    K_T = 84,
    K_U = 85,
    K_V = 86,
    K_W = 87,
    K_X = 88,
    K_Y = 89,
    K_Z = 90,
    K_CONTEXT_MENU = 93,
    K_NUMPAD0 = 96,
    K_NUMPAD1 = 97,
    K_NUMPAD2 = 98,
    K_NUMPAD3 = 99,
    K_NUMPAD4 = 100,
    K_NUMPAD5 = 101,
    K_NUMPAD6 = 102,
    K_NUMPAD7 = 103,
    K_NUMPAD8 = 104,
    K_NUMPAD9 = 105,
    K_MULTIPLY = 106,
    K_ADD = 107,
    K_SEPARATOR = 108,
    K_SUBTRACT = 109,
    K_DECIMAL = 110,
    K_DIVIDE = 111,
    K_F1 = 112,
    K_F2 = 113,
    K_F3 = 114,
    K_F4 = 115,
    K_F5 = 116,
    K_F6 = 117,
    K_F7 = 118,
    K_F8 = 119,
    K_F9 = 120,
    K_F10 = 121,
    K_F11 = 122,
    K_F12 = 123,
    K_F13 = 124,
    K_F14 = 125,
    K_F15 = 126,
    K_F16 = 127,
    K_F17 = 128,
    K_F18 = 129,
    K_F19 = 130,
    K_F20 = 131,
    K_F21 = 132,
    K_F22 = 133,
    K_F23 = 134,
    K_F24 = 135,
    K_NUM_LOCK = 144,
    K_SCROLL_LOCK = 145,
    K_COMMA = 188,
    K_PERIOD = 190,
    K_SLASH = 191,
    K_BACK_QUOTE = 192,
    K_OPEN_BRACKET = 219,
    K_BACK_SLASH = 220,
    K_CLOSE_BRACKET = 221,
    K_QUOTE = 222,
    K_META = 224
}
declare enum XMouseButton {
    None = 0,
    Left = 1,
    Right = 2
}
declare var Maps: {
    base: string;
    letters: RegExp;
}[];
declare class XDefault {
    static StrNullDate: string;
    static StrBRNullDate: string;
    static NullDate: Date;
    static IsDebug: boolean;
    static NullID: string;
    static DefaultColCount: number;
    static DefaultRowHeight: number;
}
declare enum XEventType {
    MouseMove = "mousemove",
    MouseDown = "mousedown",
    MouseUp = "mouseup",
    MouseEnter = "mouseenter",
    MouseLeave = "mouseleave",
    Input = "input",
    Paste = "paste",
    KeyDown = "keydown",
    KeyUp = "keyup",
    KeyPress = "keypress",
    LostFocus = "focusout",
    Click = "click",
    FocusIn = "focusin",
    Blur = "blur",
    Scroll = "scroll"
}
declare class XCallOnce {
    constructor(pUUID: string, pEvent: any);
    UUID: string;
    Event: any;
    Execute(): void;
}
declare class XEventManager {
    private static _CallOnce;
    static AddExecOnce(pUUID: string, pEvent: any): void;
    static ExecOnce(pUUID: string): void;
    static AddObserver(pContext: any, pConfig: any, pEvent: any): void;
    static AddEvent(pContext: any, pElement: HTMLElement, pEvent: XEventType, pMethod: any, pCheckSource?: boolean): void;
    static RemoveEvent(pContext: any, pElement: any, pEvent: string): void;
    static Call(pCallScope: any, pEvent: any, pHTM: any, pCheckSource: boolean, pArg: any): void;
    static DelayedEvent(pContext: any, pEvent: any, pTime?: number): void;
    static SetTiemOut(pContext: any, pEvent: any, pTime?: number): void;
}
declare class XKeyValue<K, V> {
    Key: K | undefined;
    Value: V | undefined;
}
declare class Guid {
    static Empty: string;
    static NewGuid(): any;
    static IsEmpty(pGuid: string): boolean;
    static IsFull(pValue: string): boolean;
    static NewUUID(): string;
}
interface Document {
    Styles: StyleSheetList;
}
interface Node {
    IsChildOf(pElement: Node, pOrIsSelf?: boolean): boolean;
    Any(pPredicate: XFunc<Node>): boolean;
    Name: string;
}
interface NodeList {
    FirstOrNull<T>(pPredicate?: XFunc<T>): T;
}
interface Array<T> {
    Order: any;
    ToArray(): Array<T>;
    Get(pID: string): T;
    GetAs<Tx>(pIndex: number): Tx;
    FirstOrNull(pPredicate?: XFunc<T>): T;
    LastOrNull(pPredicate?: XFunc<T>): T;
    FirstOr<T>(pPredicate?: XFunc<T>): T;
    Count<T>(pPredicate: XFunc<T>): number;
    Sum(pPredicate: XFuncNumber<T>): number;
    Max(pPredicate: XFuncNumber<T>): number;
    Add(pItem: T): void;
    GetRandom(pCount?: number): Array<T>;
    AddRange(pValue: Array<T>): void;
    Remove(pItem: T): void;
    Clear(): void;
    OrderBy(pOrder: XValue<T>): Array<T>;
    OrderByEx<T>(pOrder: XValue<T>): Array<T>;
    OrderByDescending(pOrder: XValue<T>): Array<T>;
    _Comparer(pLeft: T, pRigh: T): number;
    _ComparerD(pLeft: T, pRigh: T): number;
    Contains(pValue: T, pStart?: number, pEnd?: number): boolean;
    FirstBy<T>(pVvalue: XValue<T>): T;
    Insert(pIndex: number, pValue: T): void;
    Delete(pStart: number, pEnd: number): void;
    LPad(pLength: number): Array<T>;
    Where(pPredicade: XFunc<T>): Array<T>;
    Select<R>(pValue: XValue<T>): Array<R>;
    SelectDistinct<R>(pValue: XValue<T>): Array<R>;
    GroupBy<T>(pValue: XValue<T>): Array<XKeyValue<any, XArray<XArray<T>>>>;
    Any(pPredicate: XFunc<T>): boolean;
    IndexOf(pPredicate: XFunc<T>): number;
    ForEach(pPredicade: XMethod<T>): void;
    Assign(pArgments: IArguments): void;
    NextFrom(pPredicate: XFunc<T>): T;
    PreviousFrom(pPredicate: XFunc<T>): T;
    NextNOrLest(pN: number, pPredicate: XFunc<T>): T;
    PreviousNOrFirst(pN: number, pPredicate: XFunc<T>): T;
    IsEqual(pOther: Array<T>, pPredicate: XFuncEx<T>): boolean;
}
interface Element {
    Object: XElement;
}
interface HTMLElement {
    Location(pReference: HTMLElement): XPoint;
    Name: string;
    GetTextWidth(pText: string, pFont: string): number;
    Swap(pLeft: number, pRight: number): any;
    GetRectRelative(pRelative: HTMLElement): XRect;
    GetRect(pInternal?: boolean): XRect;
    StyleValue(pItemName: string): number;
    StyleStrValue(pItemName: string): string;
    SetRect(pRect: XRect): void;
}
interface String {
    IsEqual(pValue: string): boolean;
    Split(pSeparator: string): XArray<String>;
    Contains(pValue: string[]): boolean;
    Clean(): string;
    IndexOf(pValue: string): number;
    Exist(pValue: String): boolean;
    ReplaceAll(pSearch: string, pValue: string): string;
    Exchange(pPos: number, pChar: string): string;
    Add(pChar: any, pCount: number): string;
    RPad(pCount: number, pChar?: any): string;
    LPad(pCount: number, pChar?: any): string;
    Count(pChar: string): number;
}
declare class XDateTimeResult {
    IsValid: boolean;
    Value: Date;
    IsEmpty: boolean;
}
declare enum XDatePart {
    Year = "year",
    Month = "month",
    Week = "week",
    Day = "day",
    Hour = "hour",
    Minute = "minute",
    Second = "second"
}
interface Date {
    FormatDateTime(pPattern: string): string;
    ToString(): string;
    DateTimeString(): string;
    TimeString(pShort?: boolean): string;
    DateString(): string;
    LocalDateTimeString(pShortY?: boolean, pShortH?: boolean, pShowDecimal?: boolean): string;
    LocalTimeString(pShort?: boolean, pShowDecimal?: boolean): string;
    LocalDateString(pShort?: boolean): string;
    ToISO(): string;
    ToLocalISO(): string;
    Full(): string;
    WeekDay(): string;
    Month(): string;
    IsLeapYear(): boolean;
    GetUTCDaysInMonth(): number;
    AddMonths(pValue: number): Date;
    OnlyDate(): Date;
    OnlyTime(): Date;
    IsToday(): boolean;
    Add(pPart: XDatePart, pValue: number): Date;
}
interface DateConstructor {
    IsBRDateTime(pValue: string): boolean;
    FromBRDateTime(pValue: string): XDateTimeResult;
    FromBRTime(pValue: string): XDateTimeResult;
    IsNullDateOrTime(pValue: any): boolean;
    IsDateOrTime(pValue: any): boolean;
    ToDateTime(pValue: string, pAsUTC?: boolean): Date;
    IsDate(pValue: string): boolean;
    FromDate(pValue: string): XDateTimeResult;
    IsTime(pValue: string): boolean;
    FromTime(pValue: string): XDateTimeResult;
    IsDateTime(pValue: string): boolean;
    FromDateTime(pValue: string): XDateTimeResult;
    FromBRDate(pValue: string): XDateTimeResult;
    IsBRTime(pValue: string): boolean;
    IsBRDate(pValue: string): boolean;
    Parse(pValue: string): XDateTimeResult;
    IsLeapYear(pYear: number): boolean;
    GetDaysInMonth(pYear: number, pMonth: number): number;
    IsEmpty(pValue: Date): boolean;
}
declare class X {
    static DataIsEmpty(pValue: string): boolean;
    static AddNL(pSource: string, ...pValues: string[]): string;
    static Lower(pString: string): string;
    static Split(pValue: string, pSeparetor: string): Array<string>;
    static In<T>(pValue: T, ...pValues: T[]): boolean;
    static Exists(pData: string, ...pValues: string[]): boolean;
    static ToDate(pValue: string): Date;
    static IsNumber(pValue: any): boolean;
    static IsF5(pArg: KeyboardEvent): boolean;
    static IsAlpha(pValue: string): boolean;
    static IsNum(pValue: string): boolean;
    static Sleep(pTime: number): void;
    static Length(pValue: any): any;
    static PadStart(pString: any, pSize: number, pAdd: string): string;
    static IfNull(pString: string, pValue: string): string;
    static As(pValue: any): any;
    static Void(pArg: any): boolean;
    static IsChar(pValue: string): boolean;
    static IsEmpty(pValue: any): boolean;
    static Contains(pArray: Array<any>, pValue: any): boolean;
}
declare class XCall {
    static AddEvent(pContext: any, pElement: any, pEvent: string, pMethod: any): void;
    static RemoveEvent(pContext: any, pElement: any, pEvent: string): void;
    static RemoveAll(pElement: any): void;
    static Call(pCallScope: any, pEvent: any, pArg?: any[]): void;
}
declare class XHotkeyManager {
    static OnKeyDown(pArg: KeyboardEvent): boolean;
}
interface XMouseEvent {
    (pArg: MouseEvent): void;
}
interface Element {
    Owner: XElement | null;
}
interface XIElement {
    Owner: XElement | HTMLElement | null;
}
interface XIDialog {
    IsDialog: boolean;
}
interface XIDialogContainer extends XIElement {
    IsDialogContainer: boolean;
    DialogContainer: XDialogContainer;
}
interface Window {
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
interface XIElement {
    HTML: HTMLElement;
    IsVisible: boolean;
    OrderIndex: number;
}
interface XIEditor extends XIElement {
    Rows: number;
    Cols: number;
    NewLine: boolean;
    Rect: XRect;
    Input: HTMLInputElement;
}
interface XIPopupPanel extends XIElement {
    CallPopupClosed(): void;
    Show(): void;
    AutoClose: boolean;
    OnPopupClosed: XPopupClosedEvent | null;
    CanClose(pSource: HTMLElement): boolean;
}
declare class XMath {
    private static m_w;
    private static m_z;
    private static mask;
    static CreateArrow(pt: XPoint, pt2: XPoint, pWidth: number): XArray<XPoint>;
    static RotatePoints(pCenter: XPoint, pPoints: XPoint[], pDegree: number): XArray<XPoint>;
    static RotatePoint(pCenter: XPoint, pPoint: XPoint, pDegree: number): XPoint;
    static Round(pRect: XRect, pFactor: number): XRect;
    static RoundN(pValue: number, pFactor: number): number;
    static Distance2Points(pPoint: XPoint, pCenter: XPoint): number;
    static LineIntersectsRect(pRect: XRect, p1: XPoint, p2: XPoint): boolean;
    private static LineIntersectsLine;
    static LineIntersection(pP1Line1: XPoint, pP2Line1: XPoint, pP1Line2: XPoint, pP2Line2: XPoint): XPoint;
    static ToPolygonEx(pRect: XRect, pInflateLine?: number): XArray<XArray<XPoint>>;
    static AddCorner(pCorner: XPoint, pRound: number, pP1: XPoint, pP2: XPoint): XArray<XPoint>;
    static PointCircle(pCenter: XPoint, pPoint: XPoint, pRadiusX: number, pRadiusY?: number): XPoint;
    static AngleInRad(pFirst: XPoint, pSecond: XPoint): number;
    static PolarToCartesian(pCenter: XPoint, pRadius: number, pDegrees: number): XPoint;
    static DonutSlice(pCenter: XPoint, pRadius: number, pStartDegrees: number, pEndDegrees: number, pWidth: number): string;
    static PieSlice(pCenter: XPoint, pRadius: number, pStartDegrees: number, pEndDegrees: number): string;
    static Arc(pCenter: XPoint, pRadius: number, pStartDegrees: number, pEndDegrees: number): string;
    static Seed(pSeed?: number): void;
    static Random(): number;
}
interface XPopupClosedEvent {
    (pPopupPanel: XIPopupPanel): void;
}
declare class XPopupManager {
    static _ZIndex: number;
    static ZIndex(): string;
    private static PopupList;
    private static AutoEvent;
    static UseCrl: boolean;
    static AddAutoEvent(pContext: any, pMethod: any, pOnce?: boolean): void;
    static Remove(pView: XIPopupPanel): void;
    static Show(pView: XIPopupPanel): void;
    static Add(pView: XIPopupPanel): void;
    static HideAll(pArg?: MouseEvent, pValid?: boolean): void;
}
interface XSortCompare<T> {
    (pLeft: T, pRight: T): number;
}
interface XSortSwap<T> {
    (pArray: Array<T>, pLeft: number, pRight: number): void;
}
declare class XSort {
    static Sort<T>(pArray: Array<T>, pSwap: XSortSwap<T>, pComparer: XSortCompare<T>, pOwner: any): Array<T>;
    private static QuickSort;
    static Swap<T>(pArray: Array<T>, pLeft: number, pRight: number): void;
}
declare class XUtils {
    static SetCursor(pElement: HTMLElement, pType: XDragType): void;
    static Location(pElement: HTMLElement): XPoint;
    static IsOut(pRect: DOMRect, pLocation: XPoint, pWidth: number, pHeight: number): Boolean;
    static IsNumber(pValue: any): boolean;
    static AddElement<T extends Element>(pOwner: any | HTMLElement | null, pTag: string | null, pClass?: string | null, pInsert?: boolean): T;
}
declare class XSize {
    constructor(pWidth?: number, pHeight?: number);
    Width: number;
    Height: number;
    Equal(pOther: XSize): boolean;
}
declare class XArray<T> extends Array<T> {
    constructor(pArg?: number | T[] | any);
}
declare class XHashSet<T, I> {
    Items: any;
    List: XArray<any>;
    get Count(): number;
    Add(pItem: T, pID: I): T;
    Contains(pID: I): boolean;
    Get(pID: I): T;
    Remove(pID: I): void;
}
declare enum XAction {
    Save = 1,
    Close = 2,
    NewTuple = 3
}
interface XActionEvent {
    (pAction: XAction): void;
}
interface XFunc<T> {
    (pItem: T): Boolean;
}
interface XFuncEx<T> {
    (pItem: T[]): Boolean;
}
interface XFuncNumber<T> {
    (pItem: T): number;
}
interface XMethod<T> {
    (pItem: T): void;
}
interface XEvent {
    (): void;
}
interface XOwnerEvent<T> {
    (pSender: T): void;
}
interface XValue<T> {
    (pValue: T): any;
}
declare enum XDragType {
    LeftTop = 0,
    Top = 1,
    RightTop = 2,
    Right = 3,
    RightBottom = 4,
    Bottom = 5,
    LeftBottom = 6,
    Left = 7,
    Drag = 8,
    Error = 9
}
declare class XHSLColor {
    constructor(pH: number, pS: number, pL: number);
    H: number;
    S: number;
    L: number;
    A: number;
    get RGB(): string;
    static StringToRGB(pColor: string): XArray<number>;
    static RGBToHSL(pR: number, pG: number, pB: number): XHSLColor;
    static HSLToRGB(pH: number, pS: number, pL: number, pA: number): string;
}
declare class XPoint {
    static _Empty: XPoint;
    static get Empty(): XPoint;
    constructor(pX?: number, pY?: number);
    X: number;
    Y: number;
    Tag: any;
    get IsLessZero(): Boolean;
    Equals(pOther: XPoint): boolean;
    LocationType(pW: number, pH: number, pSize?: number, pDragArea?: number): XDragType;
    AsString(): string;
    toString(): string;
}
declare class XRect {
    static _Empty: XRect;
    static get Empty(): XRect;
    static FromPoints(pLeftTop: XPoint, pRightBottom: XPoint): XRect;
    constructor(pLeft?: number | any, pTop?: number, pWidth?: number, pHeight?: number);
    Left: number;
    Top: number;
    Width: number;
    Height: number;
    Bottom: number;
    Right: number;
    Size: XSize | undefined;
    get IsEmpty(): boolean;
    get LeftTop(): XPoint;
    get RightTop(): XPoint;
    get LeftBottom(): XPoint;
    get RightBottom(): XPoint;
    get X(): number;
    get Y(): number;
    get AsPath(): string;
    toString(): string;
    IntersectsWith(pRect: XRect): boolean;
    Clone(): XRect;
    ApplyStyle(pStyle: CSSStyleDeclaration): void;
    Union(pRect: XRect): void;
    private SetValue;
    Inflate(pWidth: number, pHeight: number): void;
    AsSelectPath(pValue?: number): string;
    Center(): XPoint;
    Contains(pPoint: XPoint): boolean;
    Postion(pTarget: XRect): XDragType;
}
declare class XElement implements XIElement {
    static _ID: number;
    static NextID(): number;
    constructor(pOwner: XElement | HTMLElement | null, pClass?: string | null, pTag?: string | null);
    HTML: HTMLElement;
    Element: HTMLElement | null;
    Owner: XElement | HTMLElement | null;
    private _IsVisible;
    UUID: number;
    private _ResizeObserver;
    OnResize: XMethod<XElement> | null;
    OrderIndex: number;
    Rows: number;
    Cols: number;
    Children: XArray<XElement>;
    AutoIncZIndex: boolean;
    GetOwner<T extends XIElement | null>(pPredicate: XFunc<T>): T;
    GetDialogContainer(): XIDialogContainer;
    IncZIndex(): void;
    AddChildren(pElement: XElement): void;
    get Rect(): XRect;
    set Rect(pValue: XRect);
    SizeChanged(): void;
    CheckClose(pElement: HTMLElement): boolean;
    get IsDrawed(): boolean;
    OnHide(): void;
    OnShow(): void;
    Show(pValue?: boolean): void;
    SetContent(pValue: string): void;
    protected CreateChildren(): void;
    protected CreateContainer(pTag?: string | null): HTMLElement;
    CanClose(pSource: HTMLElement): boolean;
    get IsVisible(): boolean;
    set IsVisible(pValue: boolean);
}
declare class XDiv extends XElement {
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null);
    protected CreateContainer(): HTMLElement;
}
declare class XBaseInput extends XDiv implements XIEditor {
    constructor(pOwner: XElement | HTMLElement | null);
    Input: HTMLInputElement;
    protected ELMTitle: XDiv;
    NewLine: boolean;
    OrderIndex: number;
    get Title(): string;
    set Title(pValue: string);
    CreateInput(): HTMLInputElement;
}
declare class XDataGridEditor extends XBaseInput {
    constructor(pOwner: XElement | HTMLElement | null);
    DataGrid: XDataGrid;
    CreateInput(): HTMLInputElement;
}
declare class XBaseLoockupInput extends XBaseInput {
    constructor(pOwner: XElement | HTMLElement | null);
    Button: XBaseButton;
    DropDownContent: XDropDownElement;
    protected CreateDropDown(): XDropDownElement;
    OnClick(pArg: KeyboardEvent): void;
}
declare class XDataGrid extends XDiv {
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null);
    Table: XTable;
}
declare class XDropDownDataGrid extends XDataGrid {
    constructor(pOwner: XDataLoockupEditor, pClass: string);
    Editor: XDataLoockupEditor;
}
declare class XDataLoockupEditor extends XBaseLoockupInput {
    constructor(pOwner: XElement | HTMLElement | null);
    DataGrid: XDataGrid;
    OnSelected(pRows: XArray<XTableRow>): void;
}
declare class XBaseButtonInput extends XBaseInput {
    constructor(pOwner: XElement | HTMLElement | null);
    Button: XBaseButton;
    OnClick(pArg: KeyboardEvent): void;
}
declare class XDatePickerEditor extends XBaseLoockupInput {
    constructor(pOwner: XElement | HTMLElement | null);
    Calendar: XCalendar;
    SelectedDate: Date;
    protected CreateDropDown(): XDropDownElement;
    Selected(pDate: Date): void;
    FormatDate(data: Date, formato: string): string;
    private HandleInput;
    private formatDateSection;
    private formatTimeSection;
    private ValidateDate;
    OnClick(pArg: KeyboardEvent): void;
}
declare class XDecimalEditor extends XBaseInput {
    constructor(pOwner: XElement | HTMLElement | null);
    AllowNegative: boolean;
    MaxIntegerDigits: number;
    DecimalDigits: number;
    private HandleInput;
    private HandleKeydown;
    private ProcessValue;
    private FormatValue;
    private AdjustCursorPosition;
}
declare class XEMailEditor extends XBaseInput {
    constructor(pOwner: XElement | HTMLElement | null);
    Validate(pArg: InputEvent): void;
}
declare class XIntegerEditor extends XBaseInput {
    constructor(pOwner: XElement | HTMLElement | null);
    Mask: string;
    AllowNegative: boolean;
    MaxDigits: number;
    HasSeparator: boolean;
    IsFixedMask: boolean;
    Init(): void;
    SetupEventListeners(): void;
    GandleInput(pArg: any): void;
    handleKeyDown(e: any): void;
    ToggleSign(): void;
    HandleBlur(): void;
    GetRawValue(pValue: string): string;
    ProcessValue(pValue: string): string;
    FormatNumber(pValue: string): string;
    InsertSeparators(pDigits: string): any;
}
declare class XMemoEditor extends XBaseInput {
    constructor(pOwner: XElement | HTMLElement | null);
    CreateInput(): HTMLInputElement;
}
declare class XNormalEditor extends XBaseInput {
    constructor(pOwner: XElement | HTMLElement | null);
}
declare class PhoneFormatter {
    static format(value: string): string;
    static validate(phone: string): boolean;
}
declare class XPhoneEditor extends XBaseInput {
    constructor(pOwner: XElement | HTMLElement | null);
    private lastValue;
    private cursorPos;
    private handleKeyDown;
    private handleInput;
    private calculateCursorPos;
    private updateValidation;
}
interface XSizeableElementSizeEvent {
    (pSender: XElement): void;
}
declare class XSizeableElement extends XDiv {
    constructor(pOwner: XElement, pClass: string);
    protected _Location: XPoint;
    protected _Start: XPoint;
    protected _StartPos: XPoint;
    DragType: XDragType;
    IsCaptured: boolean;
    protected AutoPosition: boolean;
    CustomSizePosition: boolean;
    ParentConstraint: boolean;
    DragPanelSizeEvent: XSizeableElementSizeEvent | any;
    DragRect: XRect;
    private _CanDrag;
    private _CanResize;
    get CanDrag(): boolean;
    get CanResize(): boolean;
    set CanDrag(pValue: boolean);
    set CanResize(pValue: boolean);
    PrepareEvents(): void;
    SelectionChanged(): void;
    get IsDraging(): boolean;
    Focus(): void;
    Resize(): void;
    protected DoAutoPosition(): void;
    GetLineCount(pRect: XRect): number[];
    get DragHeight(): number;
    get ResizeWidth(): number;
    OnMouseLeave(pArg: MouseEvent): void;
    OnMouseDown(pArg: MouseEvent): void;
    StartMouseDown(pArg: MouseEvent): void;
    GetLocationType(pStart: XPoint, pClientWidth: number, pClientHeight: number, pResizeWidth: number, pDragHeight: number): XDragType;
    GetPos(pSource: XRect, pTarget: XRect): XDragType;
    OnMouseUp(pArg: MouseEvent): void;
    EndDrag(): void;
    EndSize(): void;
    OnMouseMove(pArg: MouseEvent): void;
    CanExecute(pType: XDragType): XDragType;
    DragMouseMove(pArg: MouseEvent): void;
    Draging(): void;
}
declare class XPopupElement extends XSizeableElement implements XIPopupPanel {
    constructor(pOwner: XElement, pClass: string);
    AutoClose: boolean;
    OnPopupClosed: XPopupClosedEvent | null;
    ReferenceElement: XElement | null;
    private _DialogContainer;
    CallPopupClosed(): void;
    Show(pValue?: boolean): void;
    CanClose(pElement: HTMLElement): boolean;
}
declare class XDropDownElement extends XPopupElement {
    constructor(pOwner: XElement, pClass: string);
    Selected(): void;
    BindTo(pElement: XElement): void;
}
declare class XCalendar extends XDropDownElement {
    constructor(pOwner: XElement, pClass: string);
    protected Header: XDiv;
    protected LeftArrow: XBaseButton;
    protected CenterButton: XBaseButton;
    protected RightArrow: XBaseButton;
    protected DaysGrid: XDiv;
    protected MonthsGrid: XDiv;
    protected YearsGrid: XDiv;
    private CurrentPanel;
    private ViewDate;
    SelectedDate: Date;
    OnSelectdate: XMethod<Date> | null;
    get CanDrag(): boolean;
    get CanResize(): boolean;
    OnShow(pValue?: boolean): void;
    OnHide(): void;
    CallPopupClosed(): void;
    private ShowYears;
    private ShowMonths;
    private ShowDays;
    SelectDate(pDate: Date): void;
    private Navigate;
    UpdateCalendar(): void;
    protected CreateContainer(): HTMLElement;
}
declare class XMenuButtonItem extends XDiv {
    constructor(pOwner: XElement | HTMLElement | null, pItem: any);
}
declare class XHoverPanel extends XDiv {
    constructor(pOwner: XElement | HTMLElement | null, pItem: any);
    Header: XDiv;
}
declare class XMenuItem extends XDiv {
    constructor(pOwner: XElement | HTMLElement | null, pItem: any);
    Header: XDiv;
    Menu: XMenu | null;
    DataItem: any;
    HoverPanel: XHoverPanel | null;
    HoverItens: XArray<XMenuButtonItem>;
    Title: HTMLLIElement | null;
    Instances: HTMLLIElement | null;
    private CreateItens;
    private CreateHoverPanel;
}
declare class XMenu extends XDiv {
    private menuData;
    constructor(pOwner: XElement | HTMLElement | null);
    ToggleButton: XBaseButton;
    AccordionMenu: XDiv;
    Itens: XArray<XMenuItem>;
    ExpandItem(pItem: XMenuItem): void;
    UnExpand(pItem?: XMenuItem | null): boolean;
    Collaspse(pArg: MouseEvent): void;
    CreateItens(): void;
}
declare class XBaseButton extends XElement {
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null);
    protected CreateContainer(): HTMLElement;
}
declare class XBaseTextButton extends XBaseButton {
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null);
    protected Text: HTMLSpanElement;
    get Title(): string;
    set Title(pValue: string);
}
declare class XTabControlButton extends XBaseTextButton {
    constructor(pOwner: XElement | HTMLElement | null);
    TabControl: XTabControl | null;
    Tab: XTabControlTab | null;
}
declare class XTabControlHeader extends XDiv {
    constructor(pOwner: XElement | HTMLElement | null);
    DropdownButton: XTabControlButtonList | null;
    SelectionChanged(): void;
    SizeChanged(): void;
    private ValidateVisibility;
}
declare class XTabControlTab extends XDiv implements XIDialogContainer {
    constructor(pOwner: XElement | HTMLElement | null);
    Button: XTabControlButton | null;
    DialogContainer: XDialogContainer;
    IsDialogContainer: boolean;
}
declare class XTabControlContainer extends XDiv {
    constructor(pOwner: XElement | HTMLElement | null);
}
declare class XTabControlDropdown extends XPopupElement {
    constructor(pOwner: XElement);
}
declare class XTabControlButtonList extends XBaseTextButton {
    constructor(pOwner: XElement | HTMLElement | null);
}
declare class XTabControl extends XDiv implements XIDialogContainer {
    constructor(pOwner: XElement | HTMLElement | null);
    Header: XTabControlHeader;
    Container: XTabControlContainer;
    Dropdown: XTabControlDropdown;
    ButtonList: XTabControlButtonList;
    DialogContainer: XDialogContainer;
    ActiveTab: XTabControlTab | null;
    protected Tabs: XArray<XTabControlTab>;
    IsDialogContainer: boolean;
    private PopulateDropdown;
    SelectTab(pButton: XTabControlButton): void;
    AddTab(pTitle: string): void;
    CreateTab(): XTabControlTab;
}
interface XColumnConfig {
    Title: string;
    Visible: boolean;
    Width: number;
}
declare class XTableElement extends XElement {
    constructor(pOwner: XElement | HTMLElement | null, pClass?: string | null, pTag?: string | null);
    Cells: XArray<XTableElement>;
    AddCell(pClass: string): void;
    protected CreateContainer(pTag?: string | null): HTMLElement;
}
declare class XDragUtils {
    private static _Data;
    static SetData(pData: any): void;
    static GetData<T>(): T;
    static HasDrag: any;
}
declare class XTableHCell extends XTableElement {
    constructor(pOwner: XTableHRow, pClass?: string | null);
    Table: XTable;
    HRow: XTableHRow;
    Sizer: HTMLDivElement;
    TextArea: HTMLSpanElement;
    Title: HTMLSpanElement;
    Content: HTMLDivElement;
    SortIcon: HTMLSpanElement;
    Data: XColumnConfig | any;
    SortState: {
        Field: string;
        Direction: 'asc' | 'desc';
    };
    SetData(pCell: XColumnConfig): void;
    DragEvents(): void;
    MoveTo(pLeft: XTableHCell, pRight: XTableHCell): void;
    private ResizerEvents;
}
declare class XTableHRow extends XTableElement {
    constructor(pOwner: XTableHeader);
    Header: XTableHeader;
}
declare class XTableHeader extends XElement {
    constructor(pOwner: XElement | HTMLElement | null, pTable: XTable);
    TRows: XTableHRow;
    Columns: XArray<XTableHCell>;
    Table: XTable;
    Clear(): void;
    AddColumns(pClass: string): XTableHCell;
    protected CreateContainer(): HTMLElement;
}
declare class XTableBody extends XElement {
    constructor(pOwner: XElement | HTMLElement, pTable: XTable);
    BRows: XTableElement;
    DataRows: XArray<XTableRow>;
    Table: XTable;
    SortCells: Array<XTableHCell>;
    SortData(pCell: XTableHCell, pAction: number): any;
    Clear(): void;
    AddRow(): XTableRow;
    protected CreateContainer(): HTMLElement;
}
declare class XTableRow extends XTableElement {
    constructor(pOwner: XTableBody);
    Table: XTable;
    Body: XTableBody;
    Tupla: any;
    Cell: XArray<XTableCell>;
    SetData(pTupla: any): void;
    CreateCell(): void;
}
declare class XTableCell extends XTableElement {
    constructor(pOwner: XTableRow, pClass: string);
    Content: HTMLDivElement;
    Text: HTMLSpanElement;
    Table: XTable;
    Row: XTableRow;
    HCell: XTableHCell | any;
    Data: any;
    SetData(pData: any, pHCell: XTableHCell): void;
}
declare class XTable extends XDiv {
    constructor(pOwner: XElement | HTMLElement | null, pClass: string | null);
    Container: HTMLDivElement;
    Header: XTableHeader;
    Body: XTableBody;
    Columns: XColumnConfig[] | null;
    protected DataSet: any[];
    private RowNumberColumn;
    OnRowClick: XMethod<XArray<XTableRow>> | null;
    DoSelectRow(pRow: XTableRow): void;
    PositioningHeader(pArg: MouseEvent): void;
    ResizeColumn(pHeaderCell: XTableHCell, pWidth: number, pCheck?: boolean): void;
    MoveTo(pLeft: XTableHCell, pRight: XTableHCell): void;
    GetVisibleColumns(): Array<XColumnConfig>;
    SetDataSet(pDataSet: any): void;
    CreateBody(): void;
    private AdjustCollumnWidth;
    CreateHeader(): void;
}
declare class XType1 {
    Point: XPoint;
    LeftX: number;
    LeftY: number;
    Used: boolean;
    EndX: number;
    StartX: number;
}
declare class XEditPosition {
    constructor(pLocation: XPoint);
    Used: boolean;
    Point: XPoint;
}
declare class XForm extends XDiv {
    constructor(pOwner: XElement | HTMLElement | null);
    Fields: XArray<XIEditor>;
    SizeChanged(): void;
    ResizeChildren(): void;
    SortRectangles(rectangles: XArray<XIEditor>): XArray<XIEditor>;
}
declare class XStage extends XDiv {
    static Instance: XStage;
    static Run(): void;
    constructor();
    Menu: XMenu;
    TopBar: XTopBar;
    TabControl: XStageTabControl;
    SizeChanged(): void;
    MenuResize(): void;
}
declare class XStageTabControlTab extends XTabControlTab implements XIDialogContainer {
    constructor(pOwner: XElement | HTMLElement | null);
    IsDialogContainer: boolean;
    Form: XForm;
}
declare class XStageTabControl extends XTabControl implements XIDialogContainer {
    constructor(pOwner: XElement | HTMLElement | null);
    CreateTab(): XTabControlTab;
}
declare class XTopBar extends XDiv {
    constructor(pOwner: XElement | HTMLElement | null);
}
declare class XWrapPanel extends XDiv {
    constructor(pOwner: XElement, pClass?: string | null);
}
declare class XBaseDialogCaption extends XDiv {
    constructor(pOwner: XElement, pClass: string);
    protected ELMTitle: XDiv;
    get Title(): string;
    set Title(pValue: string);
}
declare class XBaseButtonBar extends XWrapPanel {
    constructor(pOwner: XElement, pClass: string);
    Cancel: XBaseTextButton;
}
declare class XBaseDialog extends XSizeableElement implements XIDialog {
    Caption: XBaseDialogCaption;
    constructor(pOwner: XElement);
    IsDialog: boolean;
    ButtonBar: XBaseButtonBar;
    private _DialogContainer;
    Cancel(pArg: MouseEvent): void;
    get Title(): string;
    set Title(pValue: string);
    ShowDialog(): void;
    StartMouseDown(pArg: MouseEvent): void;
    IncZIndex(): void;
    Show(pValue?: boolean): void;
}
declare class XButtonEditor extends XBaseInput {
    constructor(pOwner: XElement);
    Button: XBaseButton | any;
    CreateInput(): HTMLInputElement;
    OnClick(pArg: MouseEvent): void;
}
declare class XDialogContainer extends XDiv {
    constructor(pOwner: XElement, pClass: string);
}
