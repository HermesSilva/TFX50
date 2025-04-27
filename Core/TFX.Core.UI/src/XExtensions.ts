class XKeyValue<K, V>
{
    Key: K | undefined;
    Value: V | undefined;
}

class Guid
{
    static Empty = "00000000-0000-0000-0000-000000000000";

    static NewGuid(): any
    {
        return Guid.NewUUID();
    }

    static IsEmpty(pGuid: string)
    {
        return !this.IsFull(pGuid);
    }

    static IsFull(pValue: string): boolean
    {
        return !X.IsEmpty(pValue) && pValue.length == 36 && pValue != this.Empty;
    }

    static NewUUID(): string
    {
        var d = new Date().getTime();
        var d2 = ((typeof performance !== 'undefined') && performance.now && (performance.now() * 1000)) || 0;
        return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c)
        {
            var r = Math.random() * 16;
            if (d > 0)
            {
                r = (d + r) % 16 | 0;
                d = Math.floor(d / 16);
            }
            else
            {
                r = (d2 + r) % 16 | 0;
                d2 = Math.floor(d2 / 16);
            }
            return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16).toUpperCase();
        });
    }
}

interface Document
{
    Styles: StyleSheetList;
}

document.Styles = document.styleSheets;

interface Node
{
    IsChildOf(pElement: Node, pOrIsSelf?: boolean): boolean;

    Any(pPredicate: XFunc<Node>): boolean;

    Name: string;

}

Node.prototype.IsChildOf = function (pElement: Node, pOrIsSelf?: boolean): boolean
{
    var elm: Node | null = this;
    if (pOrIsSelf && elm == pElement)
        return true;
    while (elm != null)
    {
        if (elm.parentElement == pElement)
            return true;
        elm = elm.parentElement;
    }
    return false;
};
Node.prototype.Any = function (pPredicate: XFunc<Node>): boolean
{
    var elm: Node | null = this;
    while (elm != null)
    {
        if (pPredicate(elm))
            return true;
        elm = elm.parentElement;
    }
    return false;
};

Node.prototype.IsChildOf = function (pElement: Node, pOrIsSelf?: boolean): boolean
{
    var elm: Node | null = this;
    if (pOrIsSelf && elm == pElement)
        return true;
    while (elm != null)
    {
        if (elm.parentElement == pElement)
            return true;
        elm = elm.parentElement;
    }
    return false;
};
Node.prototype.Any = function (pPredicate: XFunc<Node>): boolean
{
    var elm: Node | null = this;
    while (elm != null)
    {
        if (pPredicate(elm))
            return true;
        elm = elm.parentElement;
    }
    return false;
};

interface NodeList
{
    FirstOrNull<T>(pPredicate?: XFunc<T>): T
}

NodeList.prototype.FirstOrNull = function <T>(pPredicate?: XFunc<T>): T
{
    for (var i = 0; i < this.length; i++)
    {
        var item = this[i];
        if (pPredicate == null || pPredicate(<T>item))
            return <T>item;
    }
    return <T>null;
};

interface Array<T>
{
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

Array.prototype.GroupBy = function <T>(pValue: XValue<T>): Array<XKeyValue<any, XArray<XArray<T>>>>
{
    var ar = new Array<XKeyValue<any, XArray<XArray<T>>>>();
    var ord = this.OrderBy(e => pValue(e));
    var lvlr = null;
    var lar: XKeyValue<any, XArray<XArray<T>>> | null = null;

    for (var i = 0; i < ord.length; i++)
    {
        let vlr = pValue(ord[i]);
        if (lvlr != vlr)
        {
            lar = new XKeyValue<any, XArray<XArray<T>>>();
            lar.Key = vlr;
            lar.Value = new XArray<XArray<T>>();
            ar.Add(lar);
        }
        if (lar != null && lar.Value != null)
            lar.Value.Add(ord[i]);
        lvlr = vlr;
    }
    return ar;
};

Array.prototype.IsEqual = function <T>(pOther: Array<T>, pPredicate: XFuncEx<T>): boolean
{
    if (pOther == null || this.length != pOther.length)
        return false;
    for (var i = 0; i < this.length; i++)
        if (!pPredicate([this[i], pOther[i]]))
            return false;
    return true;
}

Array.prototype.GetRandom = function <T>(pCount?: number): Array<T>
{
    var ar = this.ToArray();
    if (pCount == null || pCount <= 0)
        pCount = this.length;
    var curidx = Math.min(pCount, ar.length);
    var rndidx = 0;
    XMath.Seed();
    while (0 !== curidx)
    {
        rndidx = Math.floor(XMath.Random() * curidx);
        curidx--;
        [ar[curidx], ar[rndidx]] = [ar[rndidx], ar[curidx]];
    }
    return ar;
};
Array.prototype.PreviousNOrFirst = function <T>(pN: number, pPredicate?: XFunc<T>): T | null
{
    for (var i = this.length - 1; i >= 0; i--)
    {
        if ((pPredicate == null || pPredicate(<T>this[i])))
            if (i - pN > 0)
                return this[i - pN];
            else
                return this[0];
    }
    return null;
};
Array.prototype.NextNOrLest = function <T>(pN: number, pPredicate?: XFunc<T>): T | null
{
    for (var i = 0; i < this.length; i++)
    {
        if ((pPredicate == null || pPredicate(<T>this[i])))
            if (this.length > i + pN)
                return this[i + pN];
            else
                return this[this.length - 1];
    }
    return null;
};
Array.prototype.PreviousFrom = function <T>(pPredicate?: XFunc<T>): T | null
{
    for (var i = this.length - 1; i >= 0; i--)
    {
        var item = this[i];
        if ((pPredicate == null || pPredicate(<T>item)) && i > 0)
            return this[i - 1];
    }
    return null;
};
Array.prototype.NextFrom = function <T>(pPredicate?: XFunc<T>): T | null
{
    for (var i = 0; i < this.length; i++)
    {
        var item = this[i];
        if ((pPredicate == null || pPredicate(<T>item)) && this.length > i + 1)
            return this[i + 1];
    }
    return null;
};
Array.prototype.ToArray = function (): any
{
    var ar = new XArray<any>();
    for (var i = 0; i < this.length; i++)
        ar[i] = this[i];
    return ar;
};
Array.prototype.Assign = function (pArgments: IArguments)
{
    for (var i = 0; i < pArgments.length; i++)
        this[i] = pArgments[i];
};

Array.prototype._ComparerD = function <T>(pLeft: T, pRigh: T): number
{
    var l = this.Order(pLeft);
    var r = this.Order(pRigh);
    if (l.localeCompare)
        return -l.localeCompare(r);
    if (r > l)
        return 1;
    else
        if (r < l)
            return -1;
    return 0;
};
Array.prototype.FirstOr = function <T>(pPredicate?: XFunc<T>): any
{
    for (var i = 0; i < this.length; i++)
    {
        var item = this[i];
        if (pPredicate == null || pPredicate(<T>item))
            return item;
    }
    return null;
};
Array.prototype.ForEach = function <T>(pPredicade: XMethod<T>)
{
    for (var i = 0; i < this.length; i++)
        pPredicade(this[i]);
};
Array.prototype.GetAs = function <Tx>(pIndex: number): Tx
{
    return <Tx>this[pIndex];
};
Array.prototype.Any = function <T>(pPredicade: XFunc<T>): boolean
{
    for (var i = 0; i < this.length; i++)
        if (pPredicade(this[i]))
            return true;
    return false;
};
Array.prototype.IndexOf = function <T>(pPredicade: XFunc<T>): number
{
    var ar = new XArray<T>();
    for (var i = 0; i < this.length; i++)
    {
        var data = this[i];
        if (pPredicade(data))
            return i;
    }
    return -1;
};
Array.prototype.SelectDistinct = function <T>(pValue: XValue<T>): Array<T>
{
    var ar = new XArray<T>();
    for (var i = 0; i < this.length; i++)
    {
        let vlr = pValue(this[i]);
        if (!ar.Contains(vlr))
            ar.Add(vlr);
    }
    return ar;
};
Array.prototype.Select = function <T>(pValue: XValue<T>): Array<T>
{
    var ar = new XArray<T>();
    for (var i = 0; i < this.length; i++)
        ar.Add(pValue(this[i]));
    return ar;
};
Array.prototype.Where = function <T>(pPredicade: XFunc<T>): Array<T>
{
    var ar = new XArray<T>();
    for (var i = 0; i < this.length; i++)
    {
        var data = this[i];
        if (pPredicade(data))
            ar[ar.length] = data;
    }
    return ar;
};
Array.prototype.LPad = function <T>(pLength: number): Array<T>
{
    var ar = new XArray<T>();
    ar.length = pLength;
    var ed = this.length - 1;
    for (var i = pLength - 1; i >= 0; i--, ed--)
        ar[i] = this[ed];
    return ar;
};
Array.prototype.Delete = function (pStart: number, pEnd: number)
{
    var p = 0;
    for (var i = 0; i < this.length; i++)
    {
        if (i >= pStart && i <= pEnd)
            continue;
        this[p++] = this[i];
    }
    this.length = p;
};
Array.prototype.Insert = function (pIndex: number, pValue: any)
{
    this.length = this.length + 1;
    for (var i = this.length - 1; i >= pIndex; i--)
        this[i] = this[i - 1];
    this[pIndex] = pValue;
};
Array.prototype.FirstBy = function <T>(pValue: XValue<T>): T
{
    var v = null;
    var r = null;
    for (let value of this)
    {
        var vv = pValue(value);
        if (vv < v || v == null)
        {
            r = value;
            v = vv;
        }
    }
    return r;
};
Array.prototype.Contains = function (pValue: any, pStart?: number, pEnd?: number): boolean
{
    if (pStart == null || pStart == null)
    {
        for (var i = 0; i < this.length; i++)
            if (pValue == this[i])
                return true;
    }
    else
    {
        var e = Math.max(pStart, <any>pEnd);
        var s = Math.min(pStart, <any>pEnd);
        if (e == s)
            return false;
        for (var i = s; i < e; i++)
        {
            if (i >= 0 && i < this.length)
                if (this[i] == pValue)
                    return true;
        }
    }
    return false;
};

Array.prototype.Count = function (pPredicate: XFunc<any>): number
{
    var vlr = 0;
    for (var i = 0; i < this.length; i++)
    {
        if (pPredicate(this[i]))
            vlr++;
    }
    return vlr;
};
Array.prototype.Sum = function <T>(pPredicate: XFuncNumber<T>): number
{
    var vlr = 0;
    for (var i = 0; i < this.length; i++)
        vlr += pPredicate(this[i]);
    return vlr;
};
Array.prototype.Max = function <T>(pPredicate: XFuncNumber<T>): number
{
    var vlr = 0;
    for (var i = 0; i < this.length; i++)
        vlr = Math.max(pPredicate(this[i]), vlr);
    return vlr;
};
Array.prototype.Clear = function (): void
{
    this.length = 0;
};
Array.prototype.Add = function (pItem: any): void
{
    this[this.length] = pItem;
};
Array.prototype.AddRange = function <T>(pValue: Array<T>): void
{
    if (X.IsEmpty(pValue))
        return;
    for (var i = 0; i < pValue.length; i++)
        this.Add(pValue[i]);
};
Array.prototype.Remove = function (pItem: any): void
{
    var idx = this.indexOf(pItem);
    if (idx >= 0)
    {
        for (var i = idx; i < this.length - 1; i++)
            this[i] = this[i + 1];
        this.length -= 1;
    }
};
Array.prototype.Get = function <T>(pID: string): T | null
{
    var r: T | null = null;
    for (var i = 0; i < this.length; i++)
        if ((<any>this[i]).ID == pID)
            r = this[i];
    return r;
};
Array.prototype.FirstOrNull = function <T>(pPredicate?: XFunc<any>): any
{
    for (var i = 0; i < this.length; i++)
    {
        var item = this[i];
        if (pPredicate == null || pPredicate(item))
            return item;
    }
    return null;
};
Array.prototype.LastOrNull = function <T>(pPredicate?: XFunc<any>): any
{
    for (var i = this.length - 1; i >= 0; i--)
    {
        var item = this[i];
        if (pPredicate == null || pPredicate(item))
            return item;
    }
    return null;
};
Array.prototype.OrderBy = function (pOrder: any): any
{
    var ar = this.slice();
    ar.Order = pOrder;
    XSort.Sort<any>(ar, XSort.Swap, ar._Comparer, <any>ar);
    return ar;
};

Array.prototype.OrderByEx = function <T>(pOrder: any): XArray<T>
{
    var ar = this.slice();
    ar.Order = pOrder;
    XSort.Sort<any>(ar, XSort.Swap, ar._Comparer, <any>ar);
    return ar;
};

Array.prototype._Comparer = function <T>(pLeft: T, pRigh: T): number
{
    var l = this.Order(pLeft);
    var r = this.Order(pRigh);
    if (l != null && l.localeCompare)
        return l.localeCompare(r);
    if (r > l)
        return -1;
    else
        if (r < l)
            return 1;
    return 0;
};

interface Element
{
    Object: XElement;
}

type EventMethod = { Event: any, Method: any };
interface HTMLElement
{
    Location(pReference: HTMLElement): XPoint;
    Name: string;
    GetTextWidth(pText: string, pFont: string): number;
    Swap(pLeft: number, pRight: number): any;
    GetRectRelative(pRelative: HTMLElement): XRect;
    GetRect(pInternal?: boolean): XRect;
    StyleValue(pItemName: string): number;
    StyleStrValue(pItemName: string): string;
    SetRect(pRect: XRect): void;
    Handlers: XArray<EventMethod> | null;
}

HTMLElement.prototype.SetRect = function (pRect: XRect): void
{
    this.style.left = `${pRect.Left}px`;
    this.style.top = `${pRect.Top}px`;
    this.style.width = `${pRect.Width}px`;
    this.style.height = `${pRect.Height}px`;
}

HTMLElement.prototype.StyleStrValue = function (pItemName: string): string
{
    var styleValue = "";
    if (document.defaultView && document.defaultView.getComputedStyle)
        styleValue = document.defaultView.getComputedStyle(this, "").getPropertyValue(pItemName);
    //else
    //    if (this.currentStyle)
    //    {
    //        pItemName = pItemName.replace(/\-(\w)/g, (strMatch, p1) => p1.toUpperCase());
    //        styleValue = this.currentStyle[pItemName];
    //    }
    return styleValue;
};

HTMLElement.prototype.StyleValue = function (pItemName: string): number
{
    return parseInt(this.StyleStrValue(pItemName));
};

HTMLElement.prototype.GetRect = function (pInternal: boolean = false): XRect
{
    let or = this.getBoundingClientRect();
    if (pInternal)
    {
        let r = new XRect(or)
        let bl = this.StyleValue("border-left");
        let bt = this.StyleValue("border-top");
        let br = this.StyleValue("border-right");
        let bb = this.StyleValue("border-bottom");
        if (Number.isNaN(bl) || Number.isNaN(bt) || Number.isNaN(br) || Number.isNaN(bb))
            return r;
        return new XRect(r.Left - bl, r.Top - bt, r.Width - bl - br, r.Height - bt - bb);
    }
    return new XRect(or);
}


HTMLElement.prototype.GetRectRelative = function (pRelative: HTMLElement): XRect
{
    let or = this.getBoundingClientRect();
    let rr = pRelative.getBoundingClientRect();
    return new XRect(or.left - rr.left, or.top - rr.top, or.width, or.height);
}

HTMLElement.prototype.GetTextWidth = function (pText: string, pFont: string): number
{
    var canvas = window.Canvas || (window.Canvas = document.createElement("canvas"));
    var context = canvas.getContext("2d");
    context.font = pFont;
    var metrics = context.measureText(pText);
    return metrics.width;
};
HTMLElement.prototype.Location = function (pReference: HTMLElement): XPoint
{
    var elm = <HTMLElement>this;
    var r1 = elm.getBoundingClientRect();
    var r2 = pReference.getBoundingClientRect();
    return new XPoint(r1.left - r2.left, r1.top - r2.top);
};
HTMLElement.prototype.Swap = function (pLeft: number, pRight: number): any
{
    this.insertBefore(this.childNodes[pLeft], this.childNodes[pRight]);
    this.insertBefore(this.childNodes[pRight], this.childNodes[pLeft]);
};

interface String
{
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

String.prototype.Count = function (pChar: string): number
{
    var cnt = 0;
    for (var i = 0; i < this.length; i++)
        if (this[i] == pChar)
            cnt++;
    return cnt;
}

String.prototype.Exchange = function (pPos: number, pChar: string): string
{
    var ret = "";
    for (var i = 0; i < this.length; i++)
    {
        if (i == pPos)
            ret += pChar;
        else
            ret += this[i];
    }
    return ret;
}

String.prototype.Split = function (pSeparator: string): XArray<String>
{
    var ret = new XArray<String>();
    let prts = <[]>this.split(pSeparator);
    for (var i = 0; i < prts.length; i++)
    {
        let str = prts[i];
        if (X.IsEmpty(prts))
            continue;
        ret[ret.length] = str;
    }
    return ret;
};
String.prototype.LPad = function (pCount: number, pChar?: any): string
{
    var str = this == null ? "" : this;
    if (str.length > pCount)
        return str.substr(str.length - pCount, pCount);
    if (X.IsEmpty(pChar))
        pChar = " ";
    return pChar.repeat(pCount - str.length) + str;
};
String.prototype.RPad = function (pCount: number, pChar?: any): string
{
    var str = this == null ? "" : this;
    if (str.length > pCount)
        return str.substr(0, pCount);
    if (X.IsEmpty(pChar))
        pChar = " ";
    return str.Add(pChar, pCount - str.length);
};
String.prototype.Add = function (pChar: any, pCount: number): string 
{
    var str = <string>(this == null ? "" : this);
    for (var i = 0; i < pCount; i++)
        str = str + pChar;
    return str;
};
String.prototype.ReplaceAll = function (pSearch: string, pValue: string): string
{
    return this.split(pSearch).join(pValue);
};
String.prototype.Exist = function (pValue: string): boolean
{
    return this.indexOf(pValue) != -1;
};
String.prototype.Clean = function (): string
{
    var str = <string>this;
    for (var i = 0; i < Maps.length; i++)
        str = str.replace(Maps[i].letters, Maps[i].base);
    return str;
};
String.prototype.IndexOf = function (pValue: string): number
{
    var str = <string>this.Clean();
    pValue = pValue.Clean();
    return str.indexOf(pValue);
};
String.prototype.IsEqual = function (pValue: string): boolean
{
    var str = this;
    if (X.IsEmpty(str) || X.IsEmpty(pValue))
        return false;
    return str == pValue;
};
String.prototype.Contains = function (pValue: string[]): boolean
{
    if (X.IsEmpty(pValue))
        return false;
    for (var x = 0; x < pValue.length; x++)
        if (this.indexOf(pValue[x].toLowerCase()) == -1)
            return false;
    return true;
};

class XDateTimeResult
{
    IsValid: boolean = false;
    Value = XDefault.NullDate;
    IsEmpty: boolean = true;
}

enum XDatePart
{
    Year = 'year',
    Month = 'month',
    Week = 'week',
    Day = 'day',
    Hour = 'hour',
    Minute = 'minute',
    Second = 'second',
}
interface Date
{
    FormatDateTime(pPattern: string): string
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

Date.prototype.Add = function (pPart: XDatePart, pValue: number): Date
{
    var ret = new Date(this);

    var CheckRollover = function (pThis: Date)
    {
        if (ret.getDate() != pThis.getDate())
            ret.setDate(0);
    };

    switch (pPart)
    {
        case 'year':
            ret.setFullYear(ret.getFullYear() + pValue);
            CheckRollover(this);
            break;
        case 'month':
            ret.setMonth(ret.getMonth() + pValue);
            CheckRollover(this);
            break;
        case 'week':
            ret.setDate(ret.getDate() + 7 * pValue);
            break;
        case 'day':
            ret.setDate(ret.getDate() + pValue);
            break;
        case 'hour':
            ret.setTime(ret.getTime() + pValue * 3600000);
            break;
        case 'minute':
            ret.setTime(ret.getTime() + pValue * 60000);
            break;
        case 'second':
            ret.setTime(ret.getTime() + pValue * 1000);
            break;
        default:
            ret = this;
            break;
    }
    return ret;
}

Date.prototype.IsToday = function ()
{
    let now = new Date();
    let it = <Date>this;
    return it.getUTCDate() == now.getUTCDate() && it.getUTCMonth() == now.getUTCMonth() && it.getUTCFullYear() == now.getUTCFullYear();
};

Date.prototype.IsLeapYear = function ()
{
    return Date.IsLeapYear(this.getFullYear());
};

Date.prototype.GetUTCDaysInMonth = function ()
{
    return Date.GetDaysInMonth(this.getUTCFullYear(), this.getUTCMonth());
};

Date.prototype.AddMonths = function (pValue: number): Date
{
    var n = this.getUTCDate();
    this.setUTCDate(1);
    this.setUTCMonth(this.getUTCMonth() + pValue);
    this.setUTCDate(Math.min(n, this.GetUTCDaysInMonth()));
    return this;
};

Date.prototype.WeekDay = function (): string
{
    return ['Domingo', 'Segunda-feira', 'Terça-feira', 'Quarta-feira', 'Quinta-feira', 'Sexta-feira', 'Sábado'][this.getDay()];
}

Date.prototype.Month = function (): string
{
    return ['Janeiro', 'Fevereiro', 'Março', 'Abril', 'Maio', 'Junho', 'Julho', 'Agosto', 'Setembro', 'Outubro', 'Novembro', 'Dezembro'][this.getMonth()];
}

Date.prototype.LocalDateTimeString = function (pShortY?: boolean, pShortH?: boolean, pShowDecimal?: boolean): string
{
    return this.LocalDateString(pShortY) + ' ' + this.LocalTimeString(pShortH, pShowDecimal);
}

Date.prototype.DateTimeString = function (): string
{
    return this.DateString() + ' ' + this.TimeString();
}

Date.prototype.LocalTimeString = function (pShort?: boolean, pShowDecimal?: boolean): string
{
    if (pShort)
        return X.PadStart(this.getHours(), 2, "0") + ":" + X.PadStart(this.getMinutes(), 2, "0");
    if (pShowDecimal)
        return X.PadStart(this.getHours(), 2, "0") + ":" + X.PadStart(this.getMinutes(), 2, "0") + ":" + X.PadStart(this.getSeconds(), 2, "0") + "," + X.PadStart(this.getMilliseconds(), 3, "0");
    return X.PadStart(this.getHours(), 2, "0") + ":" + X.PadStart(this.getMinutes(), 2, "0") + ":" + X.PadStart(this.getSeconds(), 2, "0");
}

Date.prototype.LocalDateString = function (pShort?: boolean): string
{
    if (pShort)
        return X.PadStart(this.getDate().toString(), 2, "0") + "/" + X.PadStart(this.getMonth() + 1, 2, "0") + "/" + this.getFullYear().toString().LPad(2, '0');
    return X.PadStart(this.getDate().toString(), 2, "0") + "/" + X.PadStart(this.getMonth() + 1, 2, "0") + "/" + this.getFullYear().toString();
}

Date.prototype.DateString = function (): string
{
    var dstr = X.PadStart(this.getUTCDate().toString(), 2, "0") + "/" + X.PadStart(this.getUTCMonth() + 1, 2, "0") + "/" + this.getUTCFullYear().toString();
    return dstr;
}

Date.prototype.TimeString = function (pShort: boolean = false): string
{
    if (!pShort)
        return X.PadStart(this.getUTCHours(), 2, "0") + ":" + X.PadStart(this.getUTCMinutes(), 2, "0") + ":" + X.PadStart(this.getUTCSeconds(), 2, "0");
    return X.PadStart(this.getUTCHours(), 2, "0") + ":" + X.PadStart(this.getUTCMinutes(), 2, "0");
}
Date.prototype.ToLocalISO = function (): string
{
    var dstr = this.getFullYear() + "-" + X.PadStart(this.getMonth() + 1, 2, "0") + "-" + X.PadStart(this.getDate(), 2, "0") + "T" +
        X.PadStart(this.getHours(), 2, "0") + ":" + X.PadStart(this.getMinutes().toString(), 2, "0") + ":" +
        X.PadStart(this.getSeconds(), 2, "0") + "." + X.PadStart(this.getMilliseconds(), 6, "0");
    return dstr;
}
Date.prototype.ToISO = function (): string
{
    var dstr = this.getUTCFullYear() + "-" + X.PadStart(this.getUTCMonth() + 1, 2, "0") + "-" + X.PadStart(this.getUTCDate(), 2, "0") + "T" +
        X.PadStart(this.getUTCHours(), 2, "0") + ":" + X.PadStart(this.getUTCMinutes().toString(), 2, "0") + ":" +
        X.PadStart(this.getUTCSeconds(), 2, "0") + "." + X.PadStart(this.getUTCMilliseconds(), 6, "0");
    return dstr;
}

Date.prototype.OnlyTime = function (): Date
{
    var dstr = "1755-01-01T" + X.PadStart(this.getUTCHours(), 2, "0") + ":" + X.PadStart(this.getUTCMinutes().toString(), 2, "0") + ":" +
        X.PadStart(this.getUTCSeconds(), 2, "0") + "." + X.PadStart(this.getUTCMilliseconds(), 6, "0");
    return Date.ToDateTime(dstr, true);
}

Date.prototype.OnlyDate = function (): Date
{
    var dstr = this.getUTCFullYear() + "-" + X.PadStart(this.getUTCMonth() + 1, 2, "0") + "-" + X.PadStart(this.getUTCDate(), 2, "0") + "T00:00:00.000000"
    return Date.ToDateTime(dstr, true);
}

Date.prototype.Full = function (): string
{
    var dstr = this.getUTCFullYear() + "-" + X.PadStart(this.getUTCMonth() + 1, 2, "0") + "-" + X.PadStart(this.getUTCDate(), 2, "0") + "T" +
        X.PadStart(this.getUTCHours(), 2, "0") + ":" + X.PadStart(this.getUTCMinutes().toString(), 2, "0") + ":" + X.PadStart(this.getUTCSeconds(), 2, "0") + " " + this.getUTCMilliseconds();
    return dstr;
}

Date.prototype.ToString = function (): string
{
    var dstr = X.PadStart(this.getUTCDate().toString(), 2, "0") + "/" + X.PadStart(this.getUTCMonth() + 1, 2, "0") + "/" + this.getUTCFullYear().toString() + " " +
        X.PadStart(this.getUTCHours(), 2, "0") + ":" + X.PadStart(this.getUTCMinutes(), 2, "0") + ":" + X.PadStart(this.getUTCSeconds(), 2, "0");
    return dstr;
}

Date.prototype.FormatDateTime = function (pPattern: string): string
{
    if (this.getFullYear() == 1755)
        return "";
    if (pPattern.length > 16)
        return this.LocalDateTimeString(false, false, true); // 01/01/0001 01:01
    if (pPattern.length == 16)
        return this.LocalDateTimeString(false, true); // 01/01/0001 01:01
    if (pPattern.length == 14)
        return this.LocalDateTimeString(true, true); // 01/01/01 01:01
    if (pPattern.length == 10)
        return this.LocalDateString(); // 01/01/0001
    if (pPattern.length == 8)
        if (pPattern.indexOf(':') == -1)
            return this.LocalDateString(true); // 01/01/01
        else
            return this.LocalTimeString(); // 01:01:01
    if (pPattern.length == 5)
        return this.LocalTimeString(true); // 01:01
    return this.LocalDateTimeString(); // 01/01/0001 01:01:01
}

interface DateConstructor
{
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

Date.IsEmpty = function (pValue: Date): boolean
{
    if (pValue == null || Date.IsNullDateOrTime(pValue))
        return true;
    return pValue == XDefault.NullDate;
}

Date.IsLeapYear = function (pYear: number): boolean
{
    return (((pYear % 4 === 0) && (pYear % 100 !== 0)) || (pYear % 400 === 0));
}

Date.GetDaysInMonth = function (pYear: number, pMonth: number): number
{
    return [31, (Date.IsLeapYear(pYear) ? 29 : 28), 31, 30, 31, 30, 31, 31, 30, 31, 30, 31][pMonth];
}

Date.Parse = function (pValue: string): XDateTimeResult
{
    var res = new XDateTimeResult();

    if (X.IsEmpty(pValue))
        return res;
    if (Date.IsBRDateTime(pValue))
        res = Date.FromBRDateTime(pValue);
    else
        if (Date.IsBRDate(pValue))
            res = Date.FromBRDate(pValue);
        else
            if (Date.IsBRTime(pValue))
                res = Date.FromBRTime(pValue);
    if (Date.IsDateTime(pValue))
        res = Date.FromDateTime(pValue);
    else
        if (Date.IsDate(pValue))
            res = Date.FromDate(pValue);
        else
            if (Date.IsTime(pValue))
                res = Date.FromTime(pValue);
    res.IsEmpty = res.IsValid && res.Value.getUTCFullYear() <= 1755;
    return res;
}

Date.IsBRDate = function (pValue: string): boolean
{
    return Date.FromBRDate(pValue).IsValid;
}

Date.FromBRDate = function (pValue: string): XDateTimeResult
{
    var res = new XDateTimeResult();
    try
    {
        if (X.IsEmpty(pValue))
            return res;
        pValue = pValue.trim();
        var strs1 = pValue.split(" ");
        if (strs1.length != 1)
            return res;
        var strs2: any = strs1[0].split("/");
        if (strs2.length != 3)
            return res;
        var d: any = new Number(strs2[0]);
        var m: any = new Number(strs2[1]);
        var y: any = new Number(strs2[2]);
        if (d < 0 || m <= 0 || y <= 1754)
            return res
        var iso = strs2[2] + "-" + strs2[1] + "-" + strs2[0] + "T00:00:00+0000";
        var dt = new Date(Date.parse(iso));
        if (dt.getUTCDate() != d || dt.getUTCMonth() + 1 != m || dt.getUTCFullYear() != y)
            return res;
        res.IsValid = true;
        res.Value = dt;
        return res;
    }
    catch (pError)
    {
        return res;
    }
}

Date.IsBRTime = function (pValue: string): boolean
{
    return Date.FromBRTime(pValue).IsValid;
}

Date.FromBRTime = function (pValue: string): XDateTimeResult
{
    var res = new XDateTimeResult();

    try
    {
        if (X.IsEmpty(pValue))
            return res;
        var strs1 = pValue.split(" ");
        var strs2: any;
        if (strs1.length != 2)
            strs2 = pValue.split(":");
        else
            strs2 = strs1[0].split(":");
        if (strs2.length < 2 || strs2.length > 3)
            return res;
        if (strs2.length == 2)
            strs2[2] = "00";
        var h = Number.parseInt(strs2[0]);
        var m = Number.parseInt(strs2[1]);
        var s = Number.parseInt(strs2[2]);
        if (s > 60 || m > 60 || h > 23)
            return res;
        var iso = "1755-01-01T" + strs2[0] + ":" + strs2[1] + ":" + strs2[2] + "+0000";
        var dt = new Date(Date.parse(iso));
        if (dt.getUTCHours() != h || dt.getUTCMinutes() != m || dt.getUTCSeconds() != s)
            return res;
        res.IsValid = true;
        res.Value = dt;
        return res;
    }
    catch (pError)
    {
        return res;
    }
}

Date.IsNullDateOrTime = function (pValue: any)
{
    if (X.IsEmpty(pValue))
        return true;
    if (pValue instanceof Date)
        if (pValue.toJSON() == XDefault.NullDate.toJSON() || pValue.getUTCFullYear() <= 1755)
            return true;
        else
            return false;
    if (!Date.IsDateOrTime(pValue))
        return true;
    if (pValue.indexOf('.') == -1)
        pValue = pValue + "+0000";
    var dt = new Date(pValue);

    return dt.toJSON() == XDefault.NullDate.toJSON() || dt.getUTCFullYear() <= 1755;
}

Date.IsDateOrTime = function (pValue: any)
{
    if (X.IsEmpty(pValue))
        return false;
    if (pValue.getUTCDate)
        return true;
    return Date.IsDate(pValue) || Date.IsTime(pValue) || Date.IsDateTime(pValue) || Date.IsBRDate(pValue) || Date.IsBRTime(pValue) || Date.IsBRDateTime(pValue);
}

Date.ToDateTime = function (pValue: string, pAsUTC?: boolean): Date
{
    var ret: Date | null = null;
    if (Date.IsDateTime(pValue))
        ret = this.FromDateTime(pValue).Value;
    else
        if (Date.IsTime(pValue))
            ret = this.FromTime(pValue).Value;
        else
            if (Date.IsDate(pValue))
                ret = this.FromDate(pValue).Value;

    if (Date.IsBRDateTime(pValue))
        ret = this.FromBRDateTime(pValue).Value;
    else
        if (Date.IsBRDate(pValue))
            ret = this.FromBRDate(pValue).Value;
        else
            if (Date.IsBRTime(pValue))
                ret = this.FromBRTime(pValue).Value;
    if (ret != null)
        return ret;
    return XDefault.NullDate;
}

Date.IsDate = function (pValue: string): boolean
{
    return this.FromDate(pValue).IsValid;
}

Date.FromDate = function (pValue: string): XDateTimeResult
{
    var res = new XDateTimeResult();
    try
    {
        if (X.IsEmpty(pValue))
            return res;
        var strs1 = pValue.split("T");
        if (strs1.length != 2)
            return res;

        var strs2: any = strs1[0].split("-");
        if (strs2.length != 3)
            return res;
        var y = Number.parseInt(strs2[0]);
        var m = Number.parseInt(strs2[1]);
        var d = Number.parseInt(strs2[2]);
        if (d < 0 || d > 32 || m < 0 || m > 11 || y <= 1754)
            return res;
        var iso = strs2[0] + "-" + strs2[1] + "-" + strs2[2] + "T00:00:00+0000";
        var dt = new Date(Date.parse(iso));
        if (dt.getUTCDate() != d || dt.getUTCMonth() + 1 != m || dt.getUTCFullYear() != y)
            return res;
        res.IsValid = true;
        res.Value = dt;
        return res;
    }
    catch (pError)
    {
        return res;
    }
}

Date.IsTime = function (pValue: string): boolean
{
    return this.FromTime(pValue).IsValid;
}

Date.FromTime = function (pValue: string): XDateTimeResult
{
    var res = new XDateTimeResult();
    try
    {
        if (X.IsEmpty(pValue))
            return res;
        var strs1 = pValue.split(" ");
        var strs2: any;
        if (strs1.length != 2)
            strs2 = pValue.split(":");
        else
            strs2 = strs1[0].split(":");
        if (strs2.length < 2 || strs2.length > 3)
            return res;
        if (strs2.length == 2)
            strs2[2] = "00";
        var h = Number.parseInt(strs2[0]);
        var m = Number.parseInt(strs2[1]);
        var s = Number.parseInt(strs2[2]);
        if (s > 60 || m > 60 || h > 23)
            return res;
        var iso = "1755-01-01T" + strs2[0] + ":" + strs2[1] + ":" + strs2[2] + "+0000";
        var dt = new Date(Date.parse(iso));
        if (dt.getUTCHours() != h || dt.getUTCMinutes() != m || dt.getUTCSeconds() != s)
            return res;
        res.IsValid = true;
        res.Value = dt;
        return res;
    }
    catch (pError)
    {
        return res;
    }
}
Date.IsDateTime = function (pValue: string): boolean
{
    return this.FromDateTime(pValue).IsValid;
}

Date.FromDateTime = function (pValue: string): XDateTimeResult
{
    var res = new XDateTimeResult();
    try
    {
        if (X.IsEmpty(pValue))
            return res;
        var strs1 = pValue.split("T");
        if (strs1.length != 2)
            return res;

        var strs2: any = strs1[0].split("-");
        if (strs2.length != 3)
            return res;
        var y = Number.parseInt(strs2[0]);
        var m = Number.parseInt(strs2[1]);
        var d = Number.parseInt(strs2[2]);
        if (d < 0 || d > 31 || m < 0 || m > 12 || y <= 1754)
            return res;
        var strs3: any = strs1[1].split(":");
        if (strs3.length < 2 || strs3.length > 3)
            return res;
        if (strs2.length == 2)
            strs3[2] = "00";
        var h = Number.parseInt(strs3[0]);
        var n = Number.parseInt(strs3[1]);
        var s = Number.parseInt(strs3[2]);
        if (s > 60 || n > 60 || h > 23)
            return res;
        var iso = "" + strs2[0] + "-" + strs2[1] + "-" + strs2[2] + "T" + strs3[0] + ":" + strs3[1] + ":" + strs3[2] + "+0000";
        var dt = new Date(Date.parse(iso));
        if (dt.getUTCDate() != d || dt.getUTCMonth() + 1 != m || dt.getUTCFullYear() != y || dt.getUTCHours() != h || dt.getUTCMinutes() != n || dt.getUTCSeconds() != s)
            return res;
        res.IsValid = true;
        res.Value = dt;
        return res;
    }
    catch (pError)
    {
        return res;
    }
}

Date.IsBRDateTime = function (pValue: string): boolean
{
    return this.FromBRDateTime(pValue).IsValid;
}

Date.FromBRDateTime = function (pValue: string): XDateTimeResult
{
    var res = new XDateTimeResult();

    try
    {
        if (X.IsEmpty(pValue))
            return res;
        var strs1 = pValue.split(" ");
        if (strs1.length != 2)
            return res;

        var strs2: any = strs1[0].split("/");
        if (strs2.length != 3)
            return res;
        var d = Number.parseInt(strs2[0]);
        var m = Number.parseInt(strs2[1]);
        var y = Number.parseInt(strs2[2]);
        if (d < 0 || m <= 0 || y <= 1754)
            return res;
        var strs3: any = strs1[1].split(":");
        if (strs3.length < 2 || strs3.length > 3)
            return res;
        if (strs3.length == 2)
            strs3[2] = "00";
        var h = Number.parseInt(strs3[0]);
        var n = Number.parseInt(strs3[1]);
        var s = Number.parseInt(strs3[2]);
        if (s > 60 || n > 60 || h > 23)
            return res;
        var iso = strs2[2] + "-" + strs2[1] + "-" + strs2[0] + "T" + strs3[0] + ":" + strs3[1] + ":" + strs3[2] + "+0000";
        var dt = new Date(Date.parse(iso));
        if (dt.getUTCDate() != d || dt.getUTCMonth() + 1 != m || dt.getUTCFullYear() != y || dt.getUTCHours() != h || dt.getUTCMinutes() != n || dt.getUTCSeconds() != s)
            return res;
        res.IsValid = true;
        res.Value = dt;
        return res;
    }
    catch (pError)
    {
        return res;
    }
}

class X
{
    static DataIsEmpty(pValue: string): boolean
    {
        return X.IsEmpty(pValue) || ["NI", "NA"].Contains(pValue);
    }
    static AddNL(pSource: string, ...pValues: string[])
    {
        if (!X.IsEmpty(pSource))
            return pValues + "\r\n" + pValues.join("\r\n");
        return pValues.join("\r\n");
    }

    static Lower(pString: string): string
    {
        if (X.IsEmpty(pString))
            return pString;
        return pString.toString().toLowerCase();
    }

    static Split(pValue: string, pSeparetor: string): Array<string>
    {
        if (X.IsEmpty(pValue))
            return [];
        var prts = pValue.split(pSeparetor);
        var ret = new Array<string>();
        for (var i = 0; i < prts.length; i++)
        {
            var prt = prts[i]
            if (!X.IsEmpty(prt))
                prt = prt.trim();
            if (X.IsEmpty(prt))
                continue;
            ret.Add(prt);
        }
        return ret;
    }

    static In<T>(pValue: T, ...pValues: T[]): boolean
    {
        for (var i = 0; i < pValues.length; i++)
            if (pValue == pValues[i])
                return true;
        return false;
    }

    static Exists(pData: string, ...pValues: string[]): boolean
    {
        if (X.IsEmpty(pData) || X.IsEmpty(pValues))
            return false;
        for (var i = 0; i < pValues.length; i++)
        {
            if (pData.IndexOf(pValues[i]) != -1)
                return true;
        }
        return false;
    }

    static ToDate(pValue: string): Date
    {
        return new Date(pValue);
    }

    static IsNumber(pValue: any): boolean
    {
        if (X.IsEmpty(pValue))
            return false;
        return !isNaN(Number(pValue.toString()));
    }

    static IsF5(pArg: KeyboardEvent): boolean
    {
        return ((pArg.which || pArg.keyCode) == XKey.K_F5);
    }

    static IsAlpha(pValue: string): boolean
    {
        return pValue >= "A" && pValue <= "Z" || pValue >= "a" && pValue <= "z";
    }

    static IsNum(pValue: string): boolean
    {
        return pValue >= "0" && pValue <= "9";
    }

    static Sleep(pTime: number)
    {
        var start = new Date().getTime();
        for (var i = 0; i < 1e7; i++)
            if ((new Date().getTime() - start) > pTime)
                break;
    }

    static Length(pValue: any)
    {
        if (pValue != null && pValue.length)
            return pValue.length;
        return -1;
    }

    static PadStart(pString: any, pSize: number, pAdd: string): string
    {
        pString = pString.toString();
        if (pString.padStart)
            return pString.padStart(pSize, pAdd);
        if (X.IsEmpty(pAdd) || pAdd == "undefined")
            pAdd = ' ';
        if (X.IsEmpty(pString))
            pString = " ";
        else
            pString = pString.toString();
        if (pString.length < pSize)
            pString = pAdd.repeat(pSize + 1) + pString;
        return pString.substring(pString.length - pSize, pString.length);
    }

    public static IfNull(pString: string, pValue: string): string
    {
        if (X.IsEmpty(pString))
            return pValue;
        return pString;
    }

    public static As(pValue: any): any
    {
        return pValue;
    }

    public static Void(pArg: any)
    {
        return false;
    }

    public static IsChar(pValue: string): boolean
    {
        if (X.IsEmpty(pValue) && pValue == " ")
            return false;
        return pValue.length == 1 && (pValue == " " || (pValue >= "0" && pValue <= "9") || (pValue >= "A" && pValue <= "Z") || (pValue >= "a" && pValue <= "z"));
    }

    public static IsEmpty(pValue: any): boolean
    {
        if (pValue == Guid.Empty)
            return true;
        return pValue == null || pValue == "undefined" || pValue.toString() == "" || (pValue.length != null && (pValue.length == 0 || pValue == " ".repeat(pValue.length)));
    }

    public static Contains(pArray: Array<any>, pValue: any): boolean
    {
        return !X.IsEmpty(pArray) && pArray.Contains(pValue);
    }
}

class XCall
{
    static AddEvent(pContext: any, pElement: any, pEvent: string, pMethod: any)
    {
        if (pElement.Method == null)
            pElement.Method = new Object();
        pElement.Method[pContext.UUID + pEvent] = (arg: any) => XCall.Call(pContext, pMethod, [pElement]);
        pElement.addEventListener(pEvent, pElement.Method[pContext.UUID + pEvent]);
    }

    static RemoveEvent(pContext: any, pElement: any, pEvent: string)
    {
        if (pElement.Method != null && pElement.Method[pContext.UUID + pEvent] != null)
            pElement.removeEventListener(pEvent, pElement.Method[pContext.UUID + pEvent]);
    }

    static RemoveAll(pElement: any)
    {
        if (pElement.Method != null)
            for (var vle in pElement.Method)
                pElement.removeEventListener(vle, pElement.Method[vle]);
    }

    static Call(pCallScope: any, pEvent: any, pArg?: any[])
    {
        try
        {
            pEvent.apply(pCallScope, pArg);
        }
        catch (pError)
        {
            if (pCallScope.Application != null)
                if (pCallScope.Application.ShowError == null)
                    throw (pError);
                else
                    pCallScope.Application.ShowError(pError);
            else
                if (window.ShowError != undefined)
                    window.ShowError(<any>pError);
                else
                    throw (pError);
        }
    }
}