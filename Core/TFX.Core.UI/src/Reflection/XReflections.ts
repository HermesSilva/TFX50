
function Injectable<T extends new (...args: any[]) => any>(target: T): T
{
    XObjectCache.AddProvider(target)
    return class extends target
    {
        constructor(...args: any[])
        {
            super(...args)
            XObjectCache.ResolveDependencies(this)
        }
    } as T
}

const Inject= (token: Function): PropertyDecorator => (target, key) => ((target.constructor as any).__inject__ ??= {})[key as string] = token

class XObjectCache
{
    private static _Providers = new Map<Function, any>()

    static AddProvider(token: Function)
    {
        XObjectCache._Providers.set(token, null)
    }

    static Get<T>(token: new () => T): T
    {
        let instance = XObjectCache._Providers.get(token)
        if (!instance)
        {
            instance = new (token as any)()
            XObjectCache._Providers.set(token, instance)
        }
        return instance as T
    }

    static ResolveDependencies<T>(instance: T): void
    {
        const ctor = (instance as any).constructor
        const injections = (ctor as any).__inject__ as Record<string, Function>
        if (injections)
            for (const key in injections)
                (instance as any)[key] = XObjectCache.Get(<any>injections[key])
    }
}