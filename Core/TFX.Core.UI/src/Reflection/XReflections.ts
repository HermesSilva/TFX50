
if (typeof (Reflect as any).defineMetadata !== 'function')
{
    const MetadataStore = new WeakMap<object, Map<string, any>>();
    (Reflect as any).defineMetadata = (key: string, value: any, target: object, propertyKey?: string) =>
    {
        let targetMap = MetadataStore.get(target)
        if (!targetMap)
        {
            targetMap = new Map<string, any>()
            MetadataStore.set(target, targetMap)
        }
        // usa “propertyKey:key” se for metadata de membro
        const metaKey = propertyKey ? `${propertyKey}:${key}` : key
        targetMap.set(metaKey, value)
    };

    (Reflect as any).getMetadata = (key: string, target: object, propertyKey?: string) =>
    {
        const targetMap = MetadataStore.get(target)
        if (!targetMap) return undefined
        const metaKey = propertyKey ? `${propertyKey}:${key}` : key
        return targetMap.get(metaKey)
    }
}

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
        const ctor = instance.constructor
        const injections = <XArray<XInjectionItem>>ctor.__inject__;
        const name = ctor.__name__;
        if (injections)
        {
            const injects = <XArray<XInjectionItem>>injections.Where(i => i.Class == name);
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
    const Wrapper = class extends target
    {
        constructor(...args: any[])
        {
            super(...args)
            if (this.constructor === Wrapper)
                XObjectCache.ResolveDependencies(this)
        }
    }
    return Wrapper as T
}

function Inject(token: Function): PropertyDecorator
{
    return function (target: Object, propertyKey: string | symbol): void
    {
        const ctor = (target.constructor as any);
        console.log(`Injecting [${<any>propertyKey}] into [${target.constructor.name}] into [${token.name}]`);

        ctor.__inject__ = <XArray<XInjectionItem>>ctor.__inject__ ?? new XArray<XInjectionItem>();
        ctor.__inject__ = (ctor.__inject__ as XArray<XInjectionItem>) ?? new XArray<XInjectionItem>();
        ctor.__inject__.Add({ Token: token, Class: target.constructor.name, Key: propertyKey });
        if (!ctor.__name__)
            ctor.__name__ = ctor.name;
    }
}
