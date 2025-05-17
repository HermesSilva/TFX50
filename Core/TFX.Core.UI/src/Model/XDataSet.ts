class XDataTuple
{
    IsReadOnly!: boolean;
    IsSelected!: boolean;
    IsChecked!: boolean;
}

class XDataSet
{
    Tuples: XDataTuple[] = [];
    ID!: string;

    Fields(): string[]
    {
        if (this.Tuples.length === 0)
            return [];
        const keys = new Set<string>()
        let current = this.Tuples[0];

        while (current && current !== Object.prototype)
        {
            Object.getOwnPropertyNames(current).forEach(k => keys.add(k))
            current = Object.getPrototypeOf(current)
        }
        return [...keys]
    }
}
