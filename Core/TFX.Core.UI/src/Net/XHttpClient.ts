
class XHttpClient
{
    private _Xhr: XMLHttpRequest;
    private _Headers: Record<string, string> = {};
    private _Url: string;
    private _Data?: any;
    private _Context: any;
    public Method: string;
    public OnLoad?: (pData: JSON, pCallData: any, pEvent: ProgressEvent) => void;
    public OnError?: (pError: any, pCallData: any, pEvent: ProgressEvent) => void;
    public OnProgress?: (pEvent: ProgressEvent, pCallData: any) => void;

    constructor(pContex: any, pUrl: string, pData: any = null)
    {
        this._Context = pContex;
        this._Url = pUrl;
        this.Method = "POST";
        this._Data = pData;
        this._Xhr = new XMLHttpRequest();
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

    public SendAsync(pData: any = null): void
    {
        if (pData != null)
            this._Data = pData;
        this._Xhr.open(this.Method, this._Url, true);
        this._Xhr.responseType = 'json';
        try
        {
            if (this._Xhr.readyState !== XMLHttpRequest.OPENED)
                throw new Error("XHR not initialized");

            this.SetupCommonHeaders();

            this._Xhr.onload = (pEvent) =>
            {
                if (this._Xhr.status >= 200 && this._Xhr.status < 300)
                    this.OnLoad?.apply(this._Context, [this._Xhr.response, this._Data, pEvent]);
                else
                    this.OnError?.apply(this._Context, [this._Xhr, this._Data, pEvent]);
            };

            this._Xhr.onerror = (pEvent) => this.OnError?.apply(this._Context, [pEvent, this._Data, pEvent]);

            if (this.OnProgress)
                this._Xhr.onprogress = (pEvent) => this.OnProgress?.apply(this._Context, [pEvent, this._Data]);

            this._Xhr.send(JSON.stringify(this._Data));
        }
        catch (pError)
        {
            this.OnError?.apply(this._Context, [pError, this._Data, <any>null]);
        }
    }

    public Send(pData: any = null): void
    {
        this._Xhr.open(this.Method, this._Url, false);
        this._Xhr.responseType = 'json';
        if (pData != null)
            this._Data = pData;
        try
        {
            if (this._Xhr.readyState !== XMLHttpRequest.OPENED)
                throw new Error("XHR not initialized");
            this.SetupCommonHeaders();
            this._Xhr.onload = (pEvent) =>
            {
                if (this._Xhr.status >= 200 && this._Xhr.status < 300)
                    this.OnLoad?.apply(this._Context, [this._Xhr.response, this._Data, pEvent]);
                else
                    this.OnError?.apply(this._Context, [this._Xhr, this._Data, pEvent]);
            };
            this._Xhr.onerror = (pEvent) => this.OnError?.apply(this._Context, [pEvent, this._Data, pEvent]);
            const JsonData = JSON.stringify(this._Data);
            this._Xhr.send(JsonData);
        }
        catch (pError)
        {
            this.OnError?.apply(this._Context, [pError, this._Data, <any>null]);
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
    }
}