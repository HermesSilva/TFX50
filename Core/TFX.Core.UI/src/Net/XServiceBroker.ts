enum XServiceBrokerCommand
{
    None = 0,
    GetData = 1,
    GetModel = 2,
    PostData = 3,
    Logon = 4,
    Custom = 5,
    Print = 6,
}
 
enum XSVCBrokerCommand
{
    None = 0,
    Get = 1,
    Put = 2,
    NewPK = 3,
    Revoke = 4,
    Recycle = 5
}

class XSVCBrokerFilter
{
    constructor(pData: any)
    {
        this._Data = pData;
    }

    private _Data: any;

    Add(pFieldID: number, pValue: any)
    {
        this._Data.Data[this._Data.Data.length] = pFieldID;
        this._Data.Data[this._Data.Data.length] = pValue;
        this._Data.Count = this._Data.Data.length;
    }
}

class XSVCBroker
{
    static Create(): XSVCBroker
    {
        var bk = new XSVCBroker();
        return bk;
    }

    constructor()
    {
        this._Data = JSON.parse('{}');
        this._Data.RowCount = 75;
        this._Data.LoadChildren = false;
        this._Data.FilterZero = true;
        this._Data.FilterInactive = true;
        this._Data.IsPKGet = false;
        this._Data.LateLoad = false;
        this._Data.SVCCommand = XSVCBrokerCommand.None;
        this._Data.PKValue = null;
        this._Data.SearchData = JSON.parse('{}');
        this._Data.DataSet = JSON.parse('{}');
        this._Data.Filter = JSON.parse('{"Count":0,"Data":[]}');
        this._Filter = new XSVCBrokerFilter(this._Data.Filter);
    }

    private _Data: any;
    private _Filter: XSVCBrokerFilter;

    get RowCount(): number { return this._Data.RowCount; }

    get LoadChildren(): boolean { return this._Data.LoadChildren; }

    get FilterZero(): boolean { return this._Data.FilterZero; }

    get FilterInactive(): boolean { return this._Data.FilterInactive; }

    get IsPKGet(): boolean { return this._Data.IsPKGet; }

    get LateLoad(): boolean { return this._Data.LateLoad; }

    get SVCCommand(): XSVCBrokerCommand { return this._Data.SVCCommand; }

    get PKValue(): any { return this._Data.PKValue; }

    get SearchData(): any { return this._Data.SearchData; }

    get DataSet(): any { return this._Data.DataSet; }

    get Filter(): XSVCBrokerFilter { return this._Filter; }

    set RowCount(pValue: number) { this._Data.RowCount = pValue; }

    set LoadChildren(pValue: boolean) { this._Data.LoadChildren = pValue; }

    set FilterZero(pValue: boolean) { this._Data.FilterZero = pValue; }

    set FilterInactive(pValue: boolean) { this._Data.FilterInactive = pValue; }

    set IsPKGet(pValue: boolean) { this._Data.IsPKGet = pValue; }

    set LateLoad(pValue: boolean) { this._Data.LateLoad = pValue; }

    set SVCCommand(pValue: XSVCBrokerCommand) { this._Data.SVCCommand = pValue; }

    set PKValue(pValue: any) { this._Data.PKValue = pValue; }

    set SearchData(pValue: any) { this._Data.SearchData = pValue; }

    set DataSet(pValue: any) { this._Data.DataSet = pValue; }

    get Data(): any
    {
        return this._Data;
    }
}

class XAPPBroker
{
    static Create(): XAPPBroker
    {
        var bk = new XAPPBroker();
        return bk;
    }

    constructor()
    {
        this._Data = JSON.parse('{}');
    }
    private _Data: any;
    set ID(pValue: string) { this._Data.ID = pValue; }
    get ID(): any { return this._Data.ID; }
    get Data(): any { return this._Data; }
}

class XServiceBroker
{

    static CreateSVCSearch(pID: string, pOwnerID: string = "", pData: any = null, pRowCount: number = 75, pLoadChildren: boolean = false, pFilterZero: boolean = true, pFilterInactive: boolean = true): XServiceBroker
    {
        var sb = new XServiceBroker();
        sb.Path = "SVCSearch";
        sb.ID = pID;
        sb.OwnerID = pOwnerID;
        sb.Command = XServiceBrokerCommand.GetData;
        sb.SVCBroker = XSVCBroker.Create();
        sb.SVCBroker.FilterInactive = pFilterInactive;
        sb.SVCBroker.RowCount = pRowCount;
        sb.SVCBroker.LoadChildren = pLoadChildren;
        sb.SVCBroker.FilterZero = pFilterZero;
        sb.SVCBroker.SearchData = pData;
        return sb;
    }


    constructor()
    {
        this._Data = JSON.parse('{}');
        this.Path = "";
        this.ID = "";
        this.OwnerID = "";
        this.AuxServiceID = "";
        this.AuxData = new XArray<any>();
        this.Command = XServiceBrokerCommand.None;
        this._Data.FRMFieldID = Guid.Empty;
        this.SVCBroker = null;
    }

    private _Data: any;
    private _SVCBroker: XSVCBroker | null = null;

    get URLID(): string
    {
        return this.Path + "-" + this.ID + "-" + this.OwnerID + "-" + this.Command;
    }

    set SVCBroker(pValue: XSVCBroker | null) { this._SVCBroker = pValue; }
    get SVCBroker(): XSVCBroker | null { return this._SVCBroker; }

    get Path(): string { return this._Data.Path; }

    get ID(): string { return this._Data.ID; }

    get OwnerID(): string { return this._Data.OwnerID; }

    get AuxServiceID(): string { return this._Data.AuxServiceID; }

    get AuxData(): XArray<any> { return this._Data.AuxData; }

    get Command(): XServiceBrokerCommand { return this._Data.Command; }

    set Path(pValue: string) { this._Data.Path = pValue; }

    set ID(pValue: string) { this._Data.ID = pValue; }

    set OwnerID(pValue: string) { this._Data.OwnerID = pValue; }

    set AuxServiceID(pValue: string) { this._Data.AuxServiceID = pValue; }

    set AuxData(pValue: XArray<any>) { this._Data.AuxData = pValue; }

    set Command(pValue: XServiceBrokerCommand) { this._Data.Command = pValue; }

    UseCache: boolean = false;



    public get Data(): any
    {
        if (this.Command == XServiceBrokerCommand.None)
            throw new XError("Não é permitido chamada com Comando=[None]");
        return this._Data;
    }

    AddWhere(pFieldID: any, pValue: any)
    {
        if (this.SVCBroker != null)
        {
            if (this.SVCBroker.SearchData == null)
            {
                this.SVCBroker.SearchData = new Object();
                this.SVCBroker.SearchData["ID"] = -1;
                this.SVCBroker.SearchData["Data"] = new XArray<any>();
            }
            var data = <XArray<any>>this.SVCBroker.SearchData.Data;
            if (pValue == null)
            {
                data.Add(pFieldID);
                return;
            }
            data.Add("FieldID:" + pFieldID);
            if (pValue != null)
                data.Add("Value:" + pValue);
        }
    }
}