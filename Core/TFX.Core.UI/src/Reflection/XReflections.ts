
interface XInjectionItem
{
    Class: string;
    Token: Function;
    Key: string;
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
        if (instance.__diInjected)
            return;
        instance.__diInjected = true;
        const ctor = instance.constructor
        const injections = <XArray<XInjectionItem>>ctor.__inject__;
        const name = ctor.__name__;
        if (injections)
        {
            const injects = <XArray<XInjectionItem>>injections;//.Where(i => i.Class == name);
            for (var i = 0; i < injects.length; i++)
            {
                const item = injects[i];
                console.log(`Resolving [${item.Key}] into [${item.Class}] into [${item.Token.name}]`);
                instance[item.Key] = XObjectCache.Get(<any>item.Token)
            }
        }
    }
}

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

function Inject(token: Function): PropertyDecorator
{
    return function (target: Object, propertyKey: string | symbol): void
    {
        const ctor = (target.constructor as any);
        console.log(`Injecting [${<any>propertyKey}] into [${target.constructor.name}] into [${token.name}]`);

        ctor.__inject__ = <XArray<XInjectionItem>>ctor.__inject__ ?? new XArray<XInjectionItem>();
        ctor.__inject__.Add({ Token: token, Class: target.constructor.name, Key: propertyKey });
        ctor.__name__ = target.constructor.name;
    }
}
