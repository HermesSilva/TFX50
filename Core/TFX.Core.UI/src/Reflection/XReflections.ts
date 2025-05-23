const AutoInitMarker = Symbol("AutoInitClass")
const CreatingInstances = new Set<Function>()

enum XLifetime
{
    Singleton = 1,
    Scoped = 2,
    Transient = 3
}

interface XProviderEntry
{
    Lifetime: XLifetime
    Token: new () => any
    Instance?: any
}

interface XInjectionItem
{
    Token: new () => any
    Key: string
    Lifetime: XLifetime
}

function AutoInit<T extends new (...pArgs: any[]) => any>(pActual: T): T
{
    let current = pActual.prototype
    while (current && current !== Object.prototype)
    {
        if ((current.constructor as any)[AutoInitMarker])
            throw new Error(`The class "${pActual.name}" cannot use @AutoInit because a base class is already decorated.`)

        current = Object.getPrototypeOf(current)
    }

    const XDerived = class extends pActual
    {
        constructor(...pArgs: any[])
        {
            if (CreatingInstances.has(pActual))
                throw new Error(`Circular dependency detected for class "${pActual.name}"`)

            CreatingInstances.add(pActual)
            try
            {
                super(...pArgs)
                XObjectCache.ResolveDependencies(this)
            }
            finally
            {
                CreatingInstances.delete(pActual)
            }
        }
    }

    Object.defineProperty(XDerived, AutoInitMarker, { value: true, enumerable: false, configurable: false, writable: false })
    return XDerived as T
}

function GetClassHierarchy(pInstance: any): Function[]
{
    const vHierarchy: Function[] = []
    let vCurrent = Object.getPrototypeOf(pInstance)

    while (vCurrent && vCurrent !== Object.prototype)
    {
        vHierarchy.push(vCurrent.constructor)
        vCurrent = Object.getPrototypeOf(vCurrent)
    }

    return vHierarchy
}

class XObjectCache
{
    private static _Providers: Record<string, XProviderEntry> = {}
    private static _Creating = new Set<Function>()

    static HasProvider(pToken: Function): boolean
    {
        return XObjectCache._Providers["__" + pToken.name + "__"] !== undefined
    }

    static AddProvider(pToken: new () => any, pLifetime: XLifetime = XLifetime.Transient): void
    {
        if (!XObjectCache.HasProvider(pToken))
            XObjectCache._Providers["__" + pToken.name + "__"] = { Token: pToken, Lifetime: pLifetime }
    }

    static Get<T>(pToken: new () => T, pContext?: Map<Function, any>, pLifetime?: XLifetime): T
    {
        const vProvider = XObjectCache._Providers["__" + pToken.name + "__"]

        if (!vProvider)
            throw new Error(`Provider for "${pToken.name}" not registered.`)

        if (pLifetime === XLifetime.Singleton || (vProvider.Lifetime === XLifetime.Singleton && pLifetime == null))
        {
            if (!vProvider.Instance)
                vProvider.Instance = XObjectCache.Create(pToken)
            return vProvider.Instance
        }

        if (pLifetime === XLifetime.Scoped || (vProvider.Lifetime === XLifetime.Scoped && pLifetime == null))
        {
            if (!pContext)
                throw new Error(`No context provided for scoped resolution of "${pToken.name}"`)
            if (!pContext.has(pToken))
                pContext.set(pToken, XObjectCache.Create(pToken))
            return pContext.get(pToken)
        }

        return XObjectCache.Create(pToken)
    }

    private static Create<T>(pToken: new () => T): T
    {
        if (XObjectCache._Creating.has(pToken))
            throw new Error(`Circular resolution detected for class "${pToken.name}"`)

        try
        {
            XObjectCache._Creating.add(pToken)
            return new pToken()
        }
        finally
        {
            XObjectCache._Creating.delete(pToken)
        }
    }

    static ResolveDependencies(pInstance: any, pContext?: Map<Function, any>): void
    {
        const ctx: any = pContext ?? new Map<Function, any>()
        const classes = GetClassHierarchy(pInstance)

        for (const vCls of classes)
        {
            const vInjects = vCls.prototype?.__inject__ as XInjectionItem[] | undefined
            if (!vInjects)
                continue

            for (const vItem of vInjects)
            {
                if (pInstance[vItem.Key])
                    continue
                let key = "__" + vItem.Token.name + "__";
                if (vItem.Lifetime === XLifetime.Scoped)
                {
                    if (ctx[key] == undefined)
                        ctx[key] = XObjectCache.Get(vItem.Token, ctx, vItem.Lifetime)
                    pInstance[vItem.Key] = ctx[key]
                }
                else
                    pInstance[vItem.Key] = XObjectCache.Get(vItem.Token, ctx, vItem.Lifetime)
            }
        }
    }
}

function Inject(pToken: new () => any, pLifetime?: XLifetime): PropertyDecorator
{
    return function (pTarget: any, pKey: string | symbol): void
    {
        if (!pTarget.__inject__)
            pTarget.__inject__ = []

        if (!XObjectCache.HasProvider(pToken))
            XObjectCache.AddProvider(pToken, XLifetime.Scoped)

        pTarget.__inject__.push({ Token: pToken, Key: pKey, Lifetime: pLifetime })
    }
}

