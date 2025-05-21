/// <reference path="../XDiv.ts" />
interface XSizeableElementSizeEvent { (pSender: XElement): void; }

class XSizeableElement extends XDiv 
{
    constructor(pOwner: XElement, pClass: string)
    {
        super(pOwner, pClass);
        this.PrepareEvents();
    }

    protected _Location: XPoint = XPoint.Empty;
    protected _Start: XPoint = XPoint.Empty;
    protected _StartPos: XPoint = XPoint.Empty;
    DragType: XDragType = XDragType.Error;
    IsCaptured: boolean = false;
    protected AutoPosition: boolean = true;
    CustomSizePosition: boolean = false;
    ParentConstraint: boolean = true;
    DragPanelSizeEvent: XSizeableElementSizeEvent | any;
    DragRect: XRect = XRect.Empty;
    private _CanDrag: boolean = true;
    private _CanResize: boolean = true;
    
    get CanDrag(): boolean { return this._CanDrag; }
    get CanResize(): boolean { return this._CanResize; }
    set CanDrag(pValue: boolean) { this._CanDrag = pValue; }
    set CanResize(pValue: boolean) { this._CanResize = pValue; }

    PrepareEvents()
    {
        if (!this.CanDrag && !this.CanResize)
            return;
        XEventManager.AddEvent(this, this.HTML, XEventType.MouseDown, this.OnMouseDown, true);
        XEventManager.AddEvent(this, this.HTML, XEventType.MouseMove, this.OnMouseMove);
        XEventManager.AddEvent(this, this.HTML, XEventType.MouseLeave, this.OnMouseLeave, true);
        XEventManager.AddEvent(this, this.HTML, XEventType.MouseUp, this.OnMouseUp, true);
    }

    SelectionChanged()
    {
        //if (this.IsSelected)
        //    this.HTML.setAttribute("IsSelected", "true");
        //else
        //    this.HTML.removeAttribute("IsSelected");
    }

    get IsDraging(): boolean
    {
        return XDragUtils.HasDrag == this;
    }

    Focus()
    {
    }

    Resize()
    {
        if (this.AutoPosition)
            this.DoAutoPosition();
        if (this.DragPanelSizeEvent != null)
            this.DragPanelSizeEvent(this);
        this.Focus();
    }

    protected DoAutoPosition()
    {
        if (this.HTML.parentElement == null)
            return;
        if (this.HTML.clientWidth > 0 && this.HTML.clientHeight > 0)
        {
            this.HTML.style.left = ((this.HTML.parentElement.clientWidth - this.HTML.clientWidth) / 2) + "px";
            this.HTML.style.top = ((this.HTML.parentElement.clientHeight - this.HTML.clientHeight) / 2) + "px";
        }
    }

    GetLineCount(pRect: XRect): number[]
    {
        var cnt = new XArray<number>([0, 0, 0, 0, 0]);
        if (cnt[0] > 0)
            cnt[0] = pRect.Height / (cnt[0] + 1);
        if (cnt[1] > 0)
            cnt[1] = pRect.Width / (cnt[1] + 1);
        if (cnt[2] > 0)
            cnt[2] = pRect.Height / (cnt[2] + 1);
        if (cnt[3] > 0)
            cnt[3] = pRect.Width / (cnt[3] + 1);
        return cnt;
    }

    get DragHeight(): number
    {
        return this.DragRect.Height;
    }

    get ResizeWidth(): number
    {
        return 4;
    }

    OnMouseLeave(pArg: MouseEvent)
    {
        if (!XDragUtils.HasDrag && this.HTML.parentElement != null)
        {
            pArg.stopPropagation();
            this.HTML.parentElement.style.cursor = "unset";
        }
    }

    OnMouseDown(pArg: MouseEvent)
    {
        this.StartMouseDown(pArg);
        if (this.AutoIncZIndex)
            this.IncZIndex();
        if (!pArg.ctrlKey && pArg.button == 0)
        {
            XPopupManager.HideAll(pArg);
            var rect = this.HTML.GetRect();
            this._Start = new XPoint(pArg.pageX - rect.Left, pArg.pageY - rect.Top);
            this._StartPos = new XPoint(pArg.pageX, pArg.pageY);
            this.DragType = this.CanExecute(this.GetLocationType(this._Start, this.HTML.offsetWidth, this.HTML.offsetHeight, this.ResizeWidth, this.DragHeight));
            if (this.DragType != XDragType.Error)
            {
                pArg.stopPropagation();
                XEventManager.AddEvent(this,<any> window, XEventType.MouseUp, this.OnMouseUp);
                XDragUtils.HasDrag = this;
                XEventManager.AddEvent(this, document.body, XEventType.MouseMove, this.DragMouseMove);
                XDragUtils.HasDrag.IsCaptured = true;
                this._Location = XUtils.Location(this.HTML);
            }
        }
    }

    StartMouseDown(pArg: MouseEvent)
    {
    }

    GetLocationType(pStart: XPoint, pClientWidth: number, pClientHeight: number, pResizeWidth: number, pDragHeight: number): XDragType
    {
        let dt = pStart.LocationType(pClientWidth, pClientHeight, pResizeWidth, pDragHeight);
        if (dt == XDragType.Drag && this.DragRect != null && !this.DragRect.Contains(pStart))
        {
            this.DragRect.Contains(pStart);
            return XDragType.Error;
        }
        return dt;
    }

    GetPos(pSource: XRect, pTarget: XRect): XDragType
    {
        return pSource.Postion(pTarget);
    }

    OnMouseUp(pArg: MouseEvent)
    {
        this.StartMouseDown(pArg);
        if (XDragUtils.HasDrag != null && XDragUtils.HasDrag == this && this.HTML.parentElement != null)
        {
            pArg.stopPropagation();
            XEventManager.RemoveEvent(XDragUtils.HasDrag, window, XEventType.MouseUp);
            XEventManager.RemoveEvent(XDragUtils.HasDrag, document.body, XEventType.MouseMove);
            XDragUtils.HasDrag.IsCaptured = false;
            var oldtype = XDragUtils.HasDrag.DragType;
            this.HTML.parentElement.style.cursor = "unset";
            XDragUtils.HasDrag = null;
            if (oldtype == XDragType.Drag)
                this.EndDrag();
            else
                if (oldtype != XDragType.Error)
                    this.EndSize();
        }
    }

    EndDrag()
    { }

    EndSize()
    {
    }

    OnMouseMove(pArg: MouseEvent)
    {
        if (this.HTML.parentElement == null)
            return;
        if (pArg.target == this.HTML && XDragUtils.HasDrag == null && !pArg.ctrlKey)
        {
            pArg.stopPropagation();
            var rect = this.HTML.GetRect();
            var pt = new XPoint(pArg.pageX - rect.Left, pArg.pageY - rect.Top);
            this.DragType = this.CanExecute(this.GetLocationType(pt, this.HTML.offsetWidth, this.HTML.offsetHeight, this.ResizeWidth, this.DragHeight));
            XUtils.SetCursor(this.HTML.parentElement, this.DragType);
            if (this.IsCaptured)
                this.DragMouseMove(pArg);
        }
        else
            XUtils.SetCursor(this.HTML.parentElement, XDragType.Error);
    }

    CanExecute(pType: XDragType): XDragType
    {
        if (!this.CanDrag && pType == XDragType.Drag)
            return XDragType.Error;
        if (!this.CanResize && pType != XDragType.Drag)
            return XDragType.Error;
        return pType;
    }

    DragMouseMove(pArg: MouseEvent)
    {
        if (this.HTML.parentElement == null)
            return;
        pArg.stopPropagation();
        var x = this.HTML.parentElement.scrollLeft;
        var y = this.HTML.parentElement.scrollTop;
        var mx = pArg.pageX - this._StartPos.X;
        var my = pArg.pageY - this._StartPos.Y;
        this._StartPos = new XPoint(pArg.pageX, pArg.pageY);
        switch (this.DragType)
        {
            case XDragType.LeftTop:
                this._Location = new XPoint(this._Location.X + mx, this._Location.Y + my);
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth - mx), (this.HTML.clientHeight - my)))
                    break;
                this.HTML.style.left = x + this._Location.X + "px";
                this.HTML.style.top = y + this._Location.Y + "px";
                this.HTML.style.width = (this.HTML.clientWidth - mx) + "px";
                this.HTML.style.height = (this.HTML.clientHeight - my) + "px";
                break;
            case XDragType.Top:
                this._Location = new XPoint(this._Location.X, this._Location.Y + my);
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth - mx), (this.HTML.clientHeight - my)))
                    break;
                this.HTML.style.top = y + this._Location.Y + "px";
                this.HTML.style.height = (this.HTML.clientHeight - my) + "px";
                break;
            case XDragType.RightTop:
                this._Location = new XPoint(this._Location.X + mx, this._Location.Y + my);
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth + mx), (this.HTML.clientHeight - my)))
                    break;
                this.HTML.style.top = y + this._Location.Y + "px";
                this.HTML.style.width = (this.HTML.clientWidth + mx) + "px";
                this.HTML.style.height = (this.HTML.clientHeight - my) + "px";
                break;
            case XDragType.Right:
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth + mx), (this.HTML.clientHeight - my)))
                    break;
                this.HTML.style.width = (this.HTML.clientWidth + mx) + "px";
                break;
            case XDragType.RightBottom:
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth + mx), (this.HTML.clientHeight + my)))
                    break;
                this.HTML.style.width = (this.HTML.clientWidth + mx) + "px";
                this.HTML.style.height = (this.HTML.clientHeight + my) + "px";
                break;
            case XDragType.Bottom:
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth + mx), (this.HTML.clientHeight + my)))
                    break;
                this.HTML.style.height = (this.HTML.clientHeight + my) + "px";
                break;
            case XDragType.LeftBottom:
                this._Location = new XPoint(this._Location.X + mx, this._Location.Y + my);
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth - mx), (this.HTML.clientHeight - my)))
                    break;
                this.HTML.style.left = x + this._Location.X + "px";
                this.HTML.style.width = (this.HTML.clientWidth - mx) + "px";
                this.HTML.style.height = (this.HTML.clientHeight + my) + "px";
                break;
            case XDragType.Left:
                this._Location = new XPoint(this._Location.X + mx, this._Location.Y);
                if (this.ParentConstraint && XUtils.IsOut(this.HTML.parentElement.getBoundingClientRect(), this._Location, (this.HTML.clientWidth - mx), (this.HTML.clientHeight - my)))
                    break;
                this.HTML.style.left = x + this._Location.X + "px";
                this.HTML.style.width = (this.HTML.clientWidth - mx) + "px";
                break;
            case XDragType.Drag:
                this._Location = new XPoint(this._Location.X + mx, this._Location.Y + my);
                this.HTML.style.left = x + this._Location.X + "px";
                this.HTML.style.top = y + this._Location.Y + "px";
                break;
        }
        this.CustomSizePosition = true;
        this.Draging();
    }

    Draging()
    { }
}