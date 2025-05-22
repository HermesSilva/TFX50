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
    private static _Providers = new Map<Function, XProviderEntry>()
    private static _CanonicalMap = new Map<Function, Function>()
    private static _Creating = new Set<Function>()

    static HasProvider(pToken: Function): boolean
    {
        return XObjectCache._Providers.has(XObjectCache.ResolveCanonical(pToken))
    }

    static AddProvider(pToken: new () => any, pLifetime: XLifetime = XLifetime.Transient): void
    {
        const vBaseToken = XObjectCache.ResolveCanonical(pToken) as new () => any

        if (!XObjectCache._Providers.has(vBaseToken))
        {
            XObjectCache._Providers.set(vBaseToken, { Token: vBaseToken, Lifetime: pLifetime })
            XObjectCache._CanonicalMap.set(pToken, vBaseToken)
        }
    }

    static Get<T>(pToken: new () => T, pContext?: Map<Function, any>): T
    {
        const vBaseToken = XObjectCache.ResolveCanonical(pToken) as new () => T
        const vProvider = XObjectCache._Providers.get(vBaseToken)

        if (!vProvider)
            throw new Error(`Provider for "${pToken.name}" not registered.`)

        if (vProvider.Lifetime === XLifetime.Singleton)
        {
            if (!vProvider.Instance)
                vProvider.Instance = XObjectCache.Create(vBaseToken)
            return vProvider.Instance
        }

        if (vProvider.Lifetime === XLifetime.Scoped)
        {
            if (!pContext)
                throw new Error(`No context provided for scoped resolution of "${pToken.name}"`)
            if (!pContext.has(vBaseToken))
                pContext.set(vBaseToken, XObjectCache.Create(vBaseToken))
            return pContext.get(vBaseToken)
        }

        return XObjectCache.Create(vBaseToken)
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
        const vContext = pContext ?? new Map<Function, any>()
        const vClasses = GetClassHierarchy(pInstance)

        for (const vCls of vClasses)
        {
            const vInjects = vCls.prototype?.__inject__ as XInjectionItem[] | undefined
            if (!vInjects)
                continue

            for (const vItem of vInjects)
            {
                if (pInstance[vItem.Key])
                    continue

                const vLifetime: XLifetime = vItem.Lifetime ?? XLifetime.Singleton

                if (vLifetime === XLifetime.Transient)
                    pInstance[vItem.Key] = XObjectCache.Get(vItem.Token, new Map()) // nova instância sempre
                else
                    if (vLifetime === XLifetime.Scoped)
                    {
                        if (!vContext.has(vItem.Token))
                            vContext.set(vItem.Token, XObjectCache.Get(vItem.Token, vContext))
                        pInstance[vItem.Key] = vContext.get(vItem.Token)
                    }
                    else
                        pInstance[vItem.Key] = XObjectCache.Get(vItem.Token)
            }
        }
    }


    static GetRegisteredLifetime(pToken: Function): XLifetime | undefined
    {
        return XObjectCache._Providers.get(XObjectCache.ResolveCanonical(pToken))?.Lifetime
    }

    private static ResolveCanonical(pToken: Function): Function
    {
        return XObjectCache._CanonicalMap.get(pToken) ?? pToken
    }
}

function Inject(pToken: new () => any, pLifetime?: XLifetime): PropertyDecorator
{
    return function (pTarget: any, pKey: string | symbol): void
    {
        if (!pTarget.__inject__)
            pTarget.__inject__ = []

        const vAlready = XObjectCache.HasProvider(pToken)
        if (!vAlready)
            XObjectCache.AddProvider(pToken, pLifetime ?? XLifetime.Singleton)

        const vFinalLifetime = pLifetime ?? XObjectCache.GetRegisteredLifetime(pToken) ?? XLifetime.Singleton

        pTarget.__inject__.push({
            Token: pToken,
            Key: pKey,
            Lifetime: vFinalLifetime
        })
    }
}

