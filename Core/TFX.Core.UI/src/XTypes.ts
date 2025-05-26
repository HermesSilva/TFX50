/// <reference path="Utils/XUtils.ts" />
class XSize
{
    constructor(pWidth: number = 0, pHeight: number = 0)
    {
        this.Width = pWidth;
        this.Height = pHeight;
    }

    public Width: number;
    public Height: number;

    Equal(pOther: XSize): boolean
    {
        return pOther != null && pOther.Width == this.Width && pOther.Height == this.Height;
    }
}

class XArray<T> extends Array<T>
{
    constructor(pArg?: number | T[] | any)
    {
        super();
        if (pArg != null)
        {
            if (pArg.length > 0)
            {
                for (var i = 0; i < pArg.length; i++)
                    this[i] = pArg[i];
            }
            else
            {
                if (pArg > 0)
                {
                    this.length = pArg;
                    for (var i = 0; i < this.length; i++)
                        this[i] = <T>null;
                }
            }
        }
    }
}

class XHashSet<T, I>
{
    public Items: any = new Object();
    public List = new XArray<any>();

    get Count(): number
    {
        if (this.List == null)
            return 0;
        return this.List.length;
    }

    Add(pItem: T, pID: I): T
    {
        this.Items[pID] = pItem;
        this.List.Add(pID);
        return pItem;
    }

    Contains(pID: I): boolean
    {
        return this.Items[pID] != null;
    }

    Get(pID: I): T
    {
        return this.Items[pID];
    }

    Remove(pID: I)
    {
        for (var i = 0; i < this.List.length; i++)
        {
            var v = this.List[i];
            if (v.ID == pID)
            {
                this.List.Remove(v);
                break;
            }
        }
        this.Items[pID] = null;
    }
}
enum XAction
{
    Save = 1,
    Close = 2,
    NewTuple = 3,
}

interface XActionEvent { (pAction: XAction): void; }

interface XFunc<T> { (pItem: T): Boolean; }
interface XFuncEx<T> { (pItem: T[]): Boolean; }

interface XFuncNumber<T> { (pItem: T): number; }

interface XMethod<T> { (pItem: T): void; }
interface XEvent { (): void; }
interface XOwnerEvent<T> { (pSender: T): void; }

interface XValue<T> { (pValue: T): any; }

enum XDragType
{
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

class XHSLColor
{
    constructor(pH: number, pS: number, pL: number)
    {
        this.H = pH;
        this.S = pS;
        this.L = pL;
    }

    H: number;
    S: number;
    L: number;
    A: number = 1;
    get RGB(): string { return XHSLColor.HSLToRGB(this.H, this.S, this.L, this.A); }

    static StringToRGB(pColor: string): XArray<number>
    {
        var c: any;
        if (/^#([A-Fa-f0-9]{3}){1,2}$/.test(pColor))
        {
            c = pColor.substring(1).split('');
            if (c.length == 3)
            {
                c = [c[0], c[0], c[1], c[1], c[2], c[2]];
            }
            c = '0x' + c.join('');
            return [(c >> 16) & 255, (c >> 8) & 255, c & 255];
        }
        return [0, 0, 0];
    }

    static RGBToHSL(pR: number, pG: number, pB: number): XHSLColor
    {
        pR /= 255, pG /= 255, pB /= 255;
        var max = Math.max(pR, pG, pB);
        var min = Math.min(pR, pG, pB);
        var h = 0;
        var s = 0;
        var l = (max + min) / 2;
        if (max == min)
            h = s = 0;
        else
        {
            var d = max - min;
            s = l > 0.5 ? d / (2 - max - min) : d / (max + min);
            switch (max)
            {
                case pR:
                    h = (pG - pB) / d + (pG < pB ? 6 : 0);
                    break;
                case pG:
                    h = (pB - pR) / d + 2;
                    break;
                case pB:
                    h = (pR - pG) / d + 4;
                    break;
            }
            h /= 6;
        }
        return new XHSLColor(h, s, l);
    }

    static HSLToRGB(pH: number, pS: number, pL: number, pA: number)
    {
        var r = 0;
        var g = 0;
        var b = 0;
        if (pS == 0)
            r = g = b = pL;
        else
        {
            var hue2rgb = function hue2rgb(p: number, q: number, t: number)
            {
                if (t < 0) t += 1;
                if (t > 1) t -= 1;
                if (t < 1 / 6) return p + (q - p) * 6 * t;
                if (t < 1 / 2) return q;
                if (t < 2 / 3) return p + (q - p) * (2 / 3 - t) * 6;
                return p;
            };
            var q = pL < 0.5 ? pL * (1 + pS) : pL + pS - pL * pS;
            var p = 2 * pL - q;
            r = hue2rgb(p, q, pH + 1 / 3);
            g = hue2rgb(p, q, pH);
            b = hue2rgb(p, q, pH - 1 / 3);
        }
        return "#" + Math.round(r * 255).toString(16).LPad(2, '0') + Math.round(g * 255).toString(16).LPad(2, '0') + Math.round(b * 255).toString(16).LPad(2, '0') + Math.round(pA * 255).toString(16).LPad(2, '0');
    }
}

class XPoint
{
    static _Empty = new XPoint(0, 0);

    static get Empty()
    {
        return this._Empty;
    }

    constructor(pX: number = Number.NaN, pY: number = Number.NaN)
    {
        this.X = pX;
        this.Y = pY;
    }

    public X: number;
    public Y: number;
    public Tag: any;

    get IsLessZero(): Boolean
    {
        return this.X < 0 || this.Y < 0;
    }

    Equals(pOther: XPoint): boolean
    {
        return this.X == pOther.X && this.Y == pOther.Y;
    }

    LocationType(pW: number, pH: number, pSize: number = 4, pDragArea: number = 35): XDragType
    {
        var x = this.X;
        var y = this.Y;
        if (x <= pSize && y <= pSize)
            return XDragType.LeftTop;
        else
            if (x >= pW - pSize && y <= pSize)
                return XDragType.RightTop;
            else
                if (x >= pW - pSize && y >= pH - pSize)
                    return XDragType.RightBottom;
                else
                    if (x <= pSize && y >= pH - pSize)
                        return XDragType.LeftBottom;
                    else
                        if (y <= pSize)
                            return XDragType.Top;
                        else
                            if (x >= pW - pSize)
                                return XDragType.Right;
                            else
                                if (y >= pH - pSize)
                                    return XDragType.Bottom;
                                else
                                    if (x <= pSize)
                                        return XDragType.Left;
                                    else
                                        if (y > pSize && y <= pDragArea)
                                            return XDragType.Drag;
        return XDragType.Error;
    }

    AsString(): string
    {
        return this.X + " " + this.Y;
    }

    toString(): string
    {
        return "X=" + this.X + " Y=" + this.Y;
    }
}

class XRect
{
    static _Empty = new XRect(0, 0, 0, 0);

    static get Empty()
    {
        return this._Empty;
    }

    static FromPoints(pLeftTop: XPoint, pRightBottom: XPoint): XRect
    {
        return new XRect(pLeftTop.X, pLeftTop.Y, pRightBottom.X - pLeftTop.X, pRightBottom.Y - pLeftTop.Y);
    }

    constructor(pLeft: number | any = 0, pTop: number = 0, pWidth: number = 0, pHeight: number = 0)
    {
        if (pLeft instanceof DOMRect)
        {
            let r = <DOMRect>pLeft;
            pLeft = r.left;
            pTop = r.top;
            pWidth = r.width;
            pHeight = r.height;
        }
        if (!XUtils.IsNumber(pLeft))
        {
            var pts = pLeft.split(';');
            pLeft = Number.parseInt(pts[0]);
            pTop = Number.parseInt(pts[1]);
            pWidth = Number.parseInt(pts[2]);
            pHeight = Number.parseInt(pts[3]);
        }
        this.SetValue(pLeft, pTop, pWidth, pHeight);
    }

    Left: number = 0;
    Top: number = 0;
    Width: number = 0;
    Height: number = 0;
    Bottom: number = 0;
    Right: number = 0;
    Size: XSize | undefined;

    get IsEmpty(): boolean { return this.Width <= 0 || this.Height <= 0; }
    get LeftTop(): XPoint { return new XPoint(this.Left, this.Top); }

    get RightTop(): XPoint { return new XPoint(this.Left + this.Width, this.Top); }

    get LeftBottom(): XPoint { return new XPoint(this.Left, this.Top + this.Height); }

    get RightBottom(): XPoint { return new XPoint(this.Left + this.Width, this.Top + this.Height); }
    get X(): number { return this.Left; }
    get Y(): number { return this.Top; }

    get AsPath(): string
    {
        var d = ["M", this.Left, this.Top, "L", this.Right, this.Top, this.Right, this.Bottom, this.Left, this.Bottom, this.Left, this.Top, "Z"].join(" ");
        return d;
    }

    toString()
    {
        return [this.Left, this.Top, this.Width, this.Height].join(" ");
    }

    IntersectsWith(pRect: XRect): boolean
    {
        if (this.IsEmpty || pRect.IsEmpty)
            return false;
        return (pRect.Left <= this.Right) && (pRect.Right >= this.Left) && (pRect.Top <= this.Bottom) && (pRect.Bottom >= this.Top);
    }

    Clone(): XRect
    {
        return new XRect(this.Left, this.Top, this.Width, this.Height);
    }

    ApplyStyle(pStyle: CSSStyleDeclaration)
    {
        pStyle.left = this.Left + "px";
        pStyle.top = this.Top + "px";
        pStyle.width = this.Width + "px";
        pStyle.height = this.Height + "px";
    }

    Union(pRect: XRect)
    {
        if (this.IsEmpty)
        {
            this.SetValue(pRect.Left, pRect.Top, pRect.Width, pRect.Height);
            return
        }
        var l = Math.min(this.Left, pRect.Left);
        var t = Math.min(this.Top, pRect.Top);
        var w = Math.max(this.Right, pRect.Right) - l;
        var h = Math.max(this.Bottom, pRect.Bottom) - t;
        this.SetValue(l, t, w, h);
    }

    private SetValue(pLeft: any, pTop: number, pWidth: number, pHeight: number)
    {
        this.Left = pLeft;
        this.Top = pTop;
        this.Width = pWidth;
        this.Height = pHeight;
        this.Bottom = pTop + pHeight;
        this.Right = pLeft + pWidth;
        this.Size = new XSize(pWidth, pHeight);
    }

    Inflate(pWidth: number, pHeight: number)
    {
        var l = this.Left - pWidth;
        var t = this.Top - pHeight;
        var w = this.Width + pWidth * 2;
        var h = this.Height + pHeight * 2;
        this.SetValue(l, t, w, h);
    }

    AsSelectPath(pValue: number = 2): string
    {
        var d = ["M", this.Left, this.Top, "L", this.Right, this.Top, this.Right, this.Bottom - pValue, this.Left, this.Bottom - pValue, "Z"].join(" ");
        return d;
    }

    Center(): XPoint
    {
        return new XPoint(this.Left + (this.Width / 2), this.Top + (this.Height / 2));
    }

    Contains(pPoint: XPoint): boolean
    {
        return ((pPoint.X >= this.Left) && (pPoint.X - this.Width <= this.Left) && (pPoint.Y >= this.Top) && (pPoint.Y - this.Height <= this.Top));
    }

    Postion(pTarget: XRect): XDragType
    {
        if (this.Right < pTarget.Left)
        {
            if (this.Top > pTarget.Bottom)
                return XDragType.RightTop;
            if (this.Bottom < pTarget.Top)
                return XDragType.RightBottom;
            return XDragType.Right;
        }
        if (pTarget.Right < this.Left)
        {
            if (pTarget.Bottom < this.Top)
                return XDragType.LeftTop;
            if (pTarget.Top > this.Bottom)
                return XDragType.LeftBottom;
            return XDragType.Left;
        }
        if (this.Top < pTarget.Bottom)
            return XDragType.Bottom;
        if (this.Bottom > pTarget.Top)
            return XDragType.Top;
        return XDragType.Error;
    }
}

