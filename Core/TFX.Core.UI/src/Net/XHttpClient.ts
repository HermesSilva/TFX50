
type XOnLoad = (pData: JSON | any, pCallData: any | null, pEvent: ProgressEvent | null) => void;

class XHttpClient
{
    
    constructor()
    {
        this.Context = null;
        this.Method = "POST";
        this._Data = null;
        this._Xhr = new XMLHttpRequest();
        this.UID = XElement.NextID();
    }
    UID: number = 0;
    private _Xhr: XMLHttpRequest;
    private _Headers: Record<string, string> = {};
    private _Data?: any;
    private _CallBackData?: any;
    Context: any;
    private _Timeout: number = 0;
    public Method: string;
    public OnLoad?: (pData: JSON | any, pCallData: any | null, pEvent: ProgressEvent | null) => void;
    public OnError?: (pError: Error, pCallData: any | null, pEvent: ProgressEvent | null) => void;
    public OnProgress?: (pEvent: ProgressEvent, pCallData: any | null) => void;


    public SetTimeout(pMilliseconds: number): this
    {
        this._Timeout = pMilliseconds;
        return this;
    }

    public SetCallBackData(pData: any): this
    {
        this._CallBackData = pData;
        return this;
    }
    public SetData(pData: any): this
    {
        this._Data = pData;
        return this;
    }

    public SetHeader(pName: string, pValue: string): this
    {
        this._Headers[pName] = pValue;
        return this;
    }

    public SendAsync(pPath: string, pData: any = null, pOnLoad: XOnLoad | null = null): void
    {
        if (pData != null)
            this._Data = pData;
        if (this._Data == null)
            this._Data = {};
        this._Xhr.timeout = this._Timeout;
        this._Xhr.open(this.Method, pPath, true);
        this._Xhr.responseType = 'json';
        try
        {
            if (this._Xhr.readyState !== XMLHttpRequest.OPENED)
                throw new Error("XHR not initialized");

            this.SetupCommonHeaders();
            this._Xhr.ontimeout = (pEvent) =>
            {
                this.OnError?.apply(this.Context, [new Error('Request timeout'), this._CallBackData, pEvent]);
                this.OnError = undefined;
            }

            this._Xhr.onload = (pEvent) =>
            {
                if (this._Xhr.status >= 200 && this._Xhr.status < 300)
                {
                    if (pOnLoad != null)
                        pOnLoad.apply(this.Context, [this._Xhr.response, this._CallBackData, pEvent]);
                    else
                        this.OnLoad?.apply(this.Context, [this._Xhr.response, this._CallBackData, pEvent]);
                }
                else
                    this.OnError?.apply(this.Context, [new Error("Error status [" + this._Xhr.status + "], Response [" + this._Xhr.response + "]"), this._CallBackData, pEvent]);
                this.OnLoad = undefined;
            };

            this._Xhr.onerror = (pEvent) =>
            {
                this.OnError?.apply(this.Context, [new Error("Error status [" + this._Xhr.status + "], Response [" + this._Xhr.response + "]"), this._CallBackData, pEvent]);
                this.OnError = undefined;
            }

            if (this.OnProgress)
                this._Xhr.onprogress = (pEvent) =>
                {
                    this.OnProgress?.apply(this.Context, [pEvent, this._CallBackData]);
                    this.OnProgress = undefined;
                }

            this._Xhr.send(JSON.stringify(this._Data));
        }
        catch (pError)
        {
            this.OnError?.apply(this.Context, [<Error>pError, this._CallBackData, <any>null]);
            this.OnLoad = undefined;
            this.OnError = undefined;
            this.OnProgress = undefined;
        }
    }

    private SetupCommonHeaders(): void
    {
        const MergedHeaders: Record<string, string> = {
            'Content-Type': 'application/json',
            ...this._Headers
        };

        Object.keys(MergedHeaders).forEach(pKey =>
            this._Xhr.setRequestHeader(pKey, MergedHeaders[pKey])
        );
    }

    public Abort(): void
    {
        this._Xhr.abort();
        this.OnError?.apply(this.Context, [new Error('Request aborted'), this._Data, null]);
    }
}