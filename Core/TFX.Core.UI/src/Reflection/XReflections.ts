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
    Token: Function
    Instance?: any
}

interface XInjectionItem
{
    Token: Function
    Key: string
    Lifetime: XLifetime
}

function AutoInit<T extends new (...pArgs: any[]) => any>(pActual: T): T
{
    let current = pActual.prototype
    while (current && current !== Object.prototype)
    {
        if (current.constructor && (current.constructor as any)[AutoInitMarker])
            throw new Error(`The class "${pActual.name}" cannot use @AutoInit because a base class is already decorated.`)

        current = Object.getPrototypeOf(current)
    }

    const Derived = class extends pActual
    {
        constructor(...args: any[])
        {
            if (CreatingInstances.has(pActual))
                throw new Error(`Circular dependency detected for class "${pActual.name}"`)

            CreatingInstances.add(pActual)
            try
            {
                super(...args)
                XObjectCache.ResolveDependencies(this)
            }
            finally
            {
                CreatingInstances.delete(pActual)
            }
        }
    }

    Object.defineProperty(Derived, AutoInitMarker, { value: true, enumerable: false, configurable: false, writable: false })

    return Derived as T
}

function GetClassHierarchy(pInstance: any): Function[]
{
    const hierarchy: Function[] = []
    let current = Object.getPrototypeOf(pInstance)

    while (current && current !== Object.prototype)
    {
        hierarchy.push(current.constructor)
        current = Object.getPrototypeOf(current)
    }

    return hierarchy
}

class XObjectCache
{
    private static _Providers = new Map<Function, any>()
    private static _Creating = new Set<Function>()

    static HasProvider(pToken: Function): boolean
    {
        return XObjectCache._Providers.has(pToken)
    }

    static AddProvider(pToken: Function, pLifetime: XLifetime = XLifetime.Transient)
    {
        if (!XObjectCache._Providers.has(pToken))
            XObjectCache._Providers.set(pToken, { Token: pToken, Lifetime: pLifetime })
    }

    static Get<T>(pToken: new () => T, pContext?: Map<Function, any>): T
    {
        const provider = XObjectCache._Providers.get(pToken)
        if (!provider)
            throw new Error(`Provider for "${pToken.name}" not registered.`)

        if (provider.Lifetime === XLifetime.Singleton)
        {
            if (!provider.Instance)
                provider.Instance = XObjectCache.Create(pToken)
            return provider.Instance
        }

        if (provider.Lifetime === XLifetime.Scoped)
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
            return new (pToken as any)()
        }
        finally
        {
            XObjectCache._Creating.delete(pToken)
        }
    }

    static ResolveDependencies(pInstance: any, pContext?: Map<Function, any>): void
    {
        const context = pContext ?? new Map<Function, any>()

        const classes = GetClassHierarchy(pInstance)
        for (const cls of classes)
        {
            const injects = cls.prototype?.__inject__ as XInjectionItem[] | undefined
            if (!injects) continue

            for (const item of injects)
            {
                if (pInstance[item.Key]) continue

                const lifetime = item.Lifetime ?? XLifetime.Singleton

                if (lifetime === XLifetime.Scoped)
                {
                    if (!context.has(item.Token))
                        context.set(item.Token, XObjectCache.Get(item.Token as any, context))
                    pInstance[item.Key] = context.get(item.Token)
                }
                else
                {
                    const useContext = lifetime === XLifetime.Transient ? new Map() : undefined
                    pInstance[item.Key] = XObjectCache.Get(item.Token as any, useContext)
                }
            }
        }
    }
}

function Inject(pToken: Function, pLifetime: XLifetime = XLifetime.Singleton): PropertyDecorator
{
    return function (target: any, propertyKey: string | symbol): void
    {
        if (!target.__inject__)
            target.__inject__ = []

        if (!XObjectCache.HasProvider(pToken))
            XObjectCache.AddProvider(pToken, pLifetime)

        target.__inject__.push({ Token: pToken, Key: propertyKey, Lifetime: pLifetime })
    }
}
