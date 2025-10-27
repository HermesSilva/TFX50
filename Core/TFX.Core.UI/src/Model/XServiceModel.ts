
interface XColumnModel
{
    Name: string;
    Visible: boolean;
    Width: number;
    Title: string;
    Align: XAlign;
    Mask: string;
    IsFreeSearch: boolean;
    Operator: XOperator;
    MaxLenght: number;
    Type: string;
}

interface XDataViewModel
{
    Columns: XColumnModel[]
}

interface XServiceModel
{
    ID: string;
    Forms: XFRMModel[]
    DataView: XDataViewModel
    SearchPath: string
}