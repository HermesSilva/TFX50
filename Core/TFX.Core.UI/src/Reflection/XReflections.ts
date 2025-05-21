
const AutoInitMarker = Symbol("AutoInitClass")

function AutoInit<T extends new (...args: any[]) => any>(Actual: T): T
{
    let current = Actual.prototype;
    while (current && current !== Object.prototype)
    {
        if (current.constructor && (current.constructor as any)[AutoInitMarker])
            throw new Error(`The Class "${Actual.name}" cannot use @AutoInit because a base class is already decorated.`);

        current = Object.getPrototypeOf(current);
    }

    const Derived = class extends Actual
    {
        constructor(...args: any[])
        {
            super(...args)
            XObjectCache.ResolveDependencies(this)
        }
    };

    Object.defineProperty(Derived, AutoInitMarker, { value: true, enumerable: false, configurable: false, writable: false });
    return Derived as T
}

interface XInjectionItem
{
    Token: Function;
    Key: string;
}

function GetClassHierarchy(obj: any): Function[]
{
    const hierarchy: Function[] = []
    let current = Object.getPrototypeOf(obj)

    while (current && current !== Object.prototype)
    {
        if (current.constructor?.name)
            hierarchy.push(current.constructor)
        current = Object.getPrototypeOf(current)
    }
    return hierarchy
}

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

    static ResolveDependencies<T>(instance: any): void
    {
        let x = GetClassHierarchy(instance);
        for (var i = 0; i < x.length; i++)
        {
            const item = <any>x[i];
            if (item == null)
                continue;
            if (item.prototype.__inject__)
            {
                const injects = <XInjectionItem[]>item.prototype.__inject__;
                for (var j = 0; j < injects.length; j++)
                {
                    const item = injects[j];
                    let vlr = instance[item.Key];
                    if (vlr)
                        continue;
                    instance[item.Key] = XObjectCache.Get(<any>item.Token)
                }
            }
        }
    }
}

function Inject(token: Function): PropertyDecorator
{
    return function (target: any, propertyKey: string | symbol): void
    {
        if (!target.__inject__)
            target.__inject__ = [];

        target.__inject__.push({ Token: token, Key: propertyKey });
        if (!target.__name__)
            target.__name__ = target.name;
    }
}
