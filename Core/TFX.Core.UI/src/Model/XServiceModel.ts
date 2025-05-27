
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
}

interface XDataViewModel
{
    Columns: XColumnModel[]
}

interface XServiceModel
{
    Forms: XFRMModel[]
    DataView: XDataViewModel
    SearchPath: string
}