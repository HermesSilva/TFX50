
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

function GetClassHierarchy(obj: any): string[]
{
    const hierarchy: any[] = []
    let current = Object.getPrototypeOf(obj)

    while (current && current !== Object.prototype)
    {
        const name = current.constructor?.name ?? "(anonymous)"
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
                const injects = <XArray<XInjectionItem>>item.prototype.__inject__;
                for (var j = 0; i < injects.length; i++)
                {
                    const item = injects[i];
                    let vlr = instance[item.Key];
                    if (vlr)
                        continue;
                    console.log(`Resolving [${item.Key}] into [${item.Token.name}] UUID=[${instance.UID}]`);
                    instance[item.Key] = XObjectCache.Get(<any>item.Token)
                }
            }
        }
    }
}

function Inject(token: Function): PropertyDecorator
{
    return function (target: Object, propertyKey: string | symbol): void
    {
        Object.getPrototypeOf(target)
        const ctor = <any>target;//(target.constructor as any);
        console.log(`Injecting [${<any>propertyKey}] into [${token.name}]`);

        ctor.__inject__ = <XArray<XInjectionItem>>ctor.__inject__ ?? new XArray<XInjectionItem>();
        ctor.__inject__ = (ctor.__inject__ as XArray<XInjectionItem>) ?? new XArray<XInjectionItem>();
        ctor.__inject__.Add({ Token: token, Class: target.constructor.name, Key: propertyKey });
        if (!ctor.__name__)
            ctor.__name__ = ctor.name;
    }
}
