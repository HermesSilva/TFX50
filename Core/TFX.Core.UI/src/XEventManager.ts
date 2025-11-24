enum XEventType
{
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
    DblClick = "dblclick",
    FocusIn = "focusin",
    Blur = "blur",
    Scroll = "scroll",
    Change = "change"
}

class XCallOnce
{
    constructor(pUUID: string, pEvent: any)
    {
        this.UUID = pUUID;
        this.Event = pEvent;
    }
    UUID: string;
    Event: any;

    Execute()
    {
        this.Event.apply(this);
    }
}

type XChangeHandler<T> = (Object: T, OldValue: any, NewValue: any) => void;

class XBinding
{
    static Bind(pOSource: any, pOTarget: any, pPSource: string, pPTarget: string)
    {
        pOSource.TargetList = new XArray<{ Object: any, Property: any }>();
        pOTarget.TargetList = new XArray<{ Object: any, Property: any }>();
        XBinding.InternalBind(pOSource, pOTarget, pPSource, pPTarget);
    }

    static BindSource(pOSource: any, pOTarget: any, pPSource: string, pPTarget: string)
    {
        pOSource.TargetList = new XArray<{ Object: any, Property: any }>();
        XBinding.InternalBind(pOSource, pOTarget, pPSource, pPTarget);
    }

    static BindTarget(pOSource: any, pOTarget: any, pPSource: string, pPTarget: string)
    {
        pOTarget.TargetList = new XArray<{ Object: any, Property: any }>();
        XBinding.InternalBind(pOSource, pOTarget, pPSource, pPTarget);
    }

    private static InternalBind(pOSource: any, pOTarget: any, pPSource: string, pPTarget: string)
    {
        if (pOSource?.TargetList && pOTarget.BindNotify)
            pOSource?.TargetList.Add({ Object: pOTarget, Property: pPTarget });
        if (pOTarget?.TargetList && pOSource.BindNotify)
            pOTarget?.TargetList.Add({ Object: pOSource, Property: pPSource });

        XBinding.TrackChange(pOSource, pPSource, (obj, oldVal, newVal) =>
        {
            if (oldVal == newVal)
                return;
            if (pOTarget[pPTarget] !== newVal)
            {
                pOTarget[pPTarget] = newVal;
                if (pOTarget.Input instanceof HTMLInputElement)
                    pOTarget.Input.dispatchEvent(new Event('input', { bubbles: true }));
                if (pOTarget.TargetList)
                {
                    for (let i = 0; i < pOTarget.TargetList.length; i++)
                    {
                        const t = pOTarget.TargetList[i];
                        if (t.Object.BindNotify)
                            t.Object.BindNotify(obj, t.Property, newVal);
                    }
                }
            }
        });

        XBinding.TrackChange(pOTarget, pPTarget, (obj, oldVal, newVal) =>
        {
            if (oldVal == newVal)
                return;
            if (pOSource[pPSource] !== newVal)
            {
                pOSource[pPSource] = newVal;
                if (pOSource.Input instanceof HTMLInputElement)
                    pOSource.Input.dispatchEvent(new Event('input', { bubbles: true }));
                if (pOSource.TargetList)
                {
                    for (let i = 0; i < pOSource.TargetList.length; i++)
                    {
                        const t = pOSource.TargetList[i];
                        if (t.Object.BindNotify)
                            t.Object.BindNotify(obj, t.Property, newVal);
                    }
                }
            }
        });
        pOTarget[pPTarget] = pOSource[pPSource];
    }

    static TrackChange<T extends object, K extends keyof T>(pObjct: T, pProperty: K, pOnChange: XChangeHandler<T>)
    {
        const desc = Object.getOwnPropertyDescriptor(pObjct, pProperty);
        let ivlr = pObjct[pProperty];

        Object.defineProperty(pObjct, pProperty,
            {
                configurable: true, enumerable: true,
                get()
                {
                    return desc?.get ? desc.get.call(pObjct) : ivlr;
                },
                set(nvlr: T[K])
                {
                    const vlr = desc?.get ? desc.get.call(pObjct) : ivlr;

                    if (vlr !== nvlr)
                    {
                        if (desc?.set)
                            desc.set.call(pObjct, nvlr);
                        else
                            ivlr = nvlr;
                        pOnChange(pObjct, vlr, nvlr);
                    }
                }
            });
    }
}
class XEventManager
{
    private static _CallOnce = new Array<XCallOnce>();



    static AddExecOnce(pUUID: string, pEvent: any)
    {
        const co = new XCallOnce(pUUID, pEvent);
        XEventManager._CallOnce.Add(co);
    }

    static ExecOnce(pUUID: string)
    {
        const co = XEventManager._CallOnce.FirstOrNull(c => c.UUID == pUUID);
        if (co != null)
        {
            XEventManager._CallOnce.Remove(co);
            co.Execute();
        }
    }

    static AddObserver(pContext: any, pConfig: any, pEvent: any)
    {
        const observer = new MutationObserver(() => pEvent.apply(pContext));
        observer.observe(pContext.HTML, pConfig);
    }

    static RemoveEvent(pContext: any, pElement: any, pEvent: string)
    {
        if (pElement.Method != null && pElement.Method[pContext.UUID + "-" + pEvent] != null)
        {
            pElement.removeEventListener(pEvent, pElement.Method[pContext.UUID + "-" + pEvent]);
            pElement.Method[pContext.UUID + "-" + pEvent] = null;
        }
    }

    static Remove(pElement: HTMLElement)
    {
        if (pElement.Handlers)
        {
            for (let i = 0; i < pElement.Handlers.length; i++)
            {
                const em = pElement.Handlers[i];
                pElement.removeEventListener(em.Event, em.Method);
            }
        }
        for (let i = 0; i < pElement.childNodes.length; i++)
            this.Remove(<HTMLElement>pElement.childNodes[i])
    }

    static AddEvent(pContext: any, pElement: HTMLElement, pEvent: XEventType, pMethod: any, pCheckSource: boolean = false)
    {
        const elm: any = pElement;
        if (elm.Method == null)
            elm.Method = new Object();
        if (pElement.Handlers == null)
            pElement.Handlers = new XArray<EventMethod>();

        const method = (arg: any) => XEventManager.Call(pContext, pMethod, pElement, pCheckSource, arg);
        elm.Method[pContext.UUID + "-" + pEvent] = method;
        pElement.removeEventListener(pEvent, method);
        pElement.Handlers.Add({ Event: pEvent, Method: method });
        pElement.addEventListener(pEvent, method);
    }

    static Call(pCallScope: any, pEvent: any, pHTM: any, pCheckSource: boolean, pArg: any)
    {
        try
        {
            if (!pCheckSource || pHTM == pArg.srcElement)
                pEvent.apply(pCallScope, [pArg]);
        }
        catch (pError)
        {
            if (pCallScope.Application != null && pCallScope.Application.ShowError != null)
                pCallScope.Application.ShowError(pError);
            else
                if (window.ShowError != null)
                    window.ShowError(<any>pError);
                else
                    throw pError;
        }
    }

    static DelayedEvent(pContext: any, pEvent: any, pTime: number = 100)
    {
        if (pContext._Timer != null && pContext._Timer != -1)
            window.clearTimeout(pContext._Timer);
        pContext._Timer = setTimeout(() => pEvent.apply(pContext, []), pTime);
    }

    static SetTiemOut(pContext: any, pEvent: any, pTime: number = 100)
    {
        this.DelayedEvent(pContext, pEvent, pTime);
    }
}