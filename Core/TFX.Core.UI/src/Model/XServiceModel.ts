class XColumnModel
{
    Name!: string
    Description!: string
    Type!: string
    Value!: string
}

class XDataViewModel
{
    Columns: XColumnModel[] = []
}

class XServiceModel
{
    ID!: string;
    readonly DataView: XDataViewModel = new XDataViewModel()
}
