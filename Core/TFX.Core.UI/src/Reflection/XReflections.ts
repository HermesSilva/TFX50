const AutoInitMarker = Symbol("AutoInitClass")
const CreatingInstances = new Set<Function>()

function AutoInit<T extends new (...args: any[]) => any>(Actual: T): T
{
    let current = Actual.prototype
    while (current && current !== Object.prototype)
    {
        if (current.constructor && (current.constructor as any)[AutoInitMarker])
            throw new Error(`The class "${Actual.name}" cannot use @AutoInit because a base class is already decorated.`)

        current = Object.getPrototypeOf(current)
    }

    const Derived = class extends Actual
    {
        constructor(...args: any[])
        {
            if (CreatingInstances.has(Actual))
                throw new Error(`Circular dependency detected for class "${Actual.name}"`)

            CreatingInstances.add(Actual)
            try
            {
                super(...args)
                XObjectCache.ResolveDependencies(this)
            }
            finally
            {
                CreatingInstances.delete(Actual)
            }
        }
    }

    Object.defineProperty(Derived, AutoInitMarker, { value: true, enumerable: false, configurable: false, writable: false })

    return Derived as T
}

interface XInjectionItem
{
    Token: Function
    Key: string
}

function GetClassHierarchy(obj: any): Function[]
{
    const hierarchy: Function[] = []
    let current = Object.getPrototypeOf(obj)

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

    static AddProvider(token: Function)
    {
        XObjectCache._Providers.set(token, null)
    }

    static Get<T>(token: new () => T): T
    {
        if (XObjectCache._Creating.has(token))
            throw new Error(`Circular resolution detected for class "${token.name}"`)

        let instance = XObjectCache._Providers.get(token)
        if (!instance)
        {
            try
            {
                XObjectCache._Creating.add(token)
                instance = new (token as any)()
                XObjectCache._Providers.set(token, instance)
            }
            finally
            {
                XObjectCache._Creating.delete(token)
            }
        }
        return instance as T
    }

    static ResolveDependencies(instance: any): void
    {
        const classes = GetClassHierarchy(instance)
        for (const cls of classes)
        {
            const injects = cls.prototype?.__inject__ as XInjectionItem[] | undefined
            if (!injects) continue

            for (const item of injects)
            {
                if (instance[item.Key]) continue
                instance[item.Key] = XObjectCache.Get(item.Token as any)
            }
        }
    }
}

function Inject(token: Function): PropertyDecorator
{
    return function (target: any, propertyKey: string | symbol): void
    {
        if (!target.__inject__)
            target.__inject__ = []

        target.__inject__.push({ Token: token, Key: propertyKey })
    }
}
