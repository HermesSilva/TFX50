

interface XColumnModel
{
    Name: string
    Description: string
    Type: string
    Value: string
}

interface XDataViewModel
{
    Columns: XColumnModel[]
}

interface XServiceModel
{
    Forms: XFRMModel[]
    DataView: XDataViewModel
}