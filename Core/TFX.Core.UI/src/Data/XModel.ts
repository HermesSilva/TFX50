interface XData<T>
{
    ID: string
    Tuples: T[]
}

interface XResponse<T>
{
    Ok: boolean;
    Status: number;
    Data: T;
}

interface XTuple
{
    IsReadOnly: boolean;
    IsSelected: boolean;
    IsChecked: boolean;
    State: number;
}

interface XField
{
    Value: string;
    State: number;
}