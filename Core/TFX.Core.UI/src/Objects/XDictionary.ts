class XDictionary<TKey extends string | number, TValue>
{
    private items: Record<TKey, TValue>;

    constructor()
    {
        this.items = {} as Record<TKey, TValue>;
    }

    public Add(key: TKey, value: TValue): void
    {
        if (this.ContainsKey(key))
            throw new Error(`A chave '${key}' já existe.`);
        this.items[key] = value;
    }

    public Remove(key: TKey): boolean
    {
        if (this.ContainsKey(key))
        {
            delete this.items[key];
            return true;
        }
        return false;
    }

    public ContainsKey(key: TKey): boolean
    {
        return Object.prototype.hasOwnProperty.call(this.items, key);
    }

    public TryGetValue(key: TKey): TValue | undefined
    {
        return this.items[key];
    }

    public Set(key: TKey, value: TValue): void
    {
        this.items[key] = value;
    }

    public Get(key: TKey): TValue
    {
        if (!this.ContainsKey(key))
            throw new Error(`A chave '${key}' não foi encontrada.`);
        return this.items[key];
    }

    public Clear(): void
    {
        this.items = {} as Record<TKey, TValue>;
    }

    public Keys(): TKey[]
    {
        return Object.keys(this.items) as TKey[];
    }

    public Values(): TValue[]
    {
        return Object.values(this.items);
    }

    public Count(): number
    {
        return this.Keys().length;
    }

    public ForEach(action: (key: TKey, value: TValue) => void): void
    {
        for (const key of this.Keys())
            action(key, this.items[key]);
    }
}
