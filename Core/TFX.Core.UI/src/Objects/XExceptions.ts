enum XErrorType
{
    None = 0,
    Error = 1,
    Warning = 2,
    Unconformity = 3,
    Message = 4,
}

class XException extends Error
{
    public static ShowStack()
    {
        try
        {
            var ix: any = "";
            ix.dont.exist += 0;
        }
        catch (e: any)
        {
            try
            {
                var strs = (<string>e.stack).split("\n");
                var str = "***********************  [  Begin  ]  ***********************\n";
                for (var i = 2; i < strs.length; i++)
                    str = str + strs[i] + "\n";
            }
            catch (ee: any)
            {
                console.log("Erro ao recuperar StackTrace\n" + ee.message);
            }
        }
    }

    constructor(pType: XErrorType, pMessage: string, pDetail: string | null = null, pStack: string | null = null, pCallBack: any = null)
    {
        super(pMessage);
        this.Type = pType;
        this.stack = <any>pStack;
        this.Detail = pDetail;
        this.CallBack = pCallBack;
    }

    public Type: XErrorType;
    CallBack: any;
    Detail: string | null;

    toString()
    {
        return this.message + "\r\n" + this.Detail + "\r\n" + this.stack;
    }
}

class XError extends XException
{
    constructor(pMessage: string, pDetail: string = "", pStack: string = "", pCallBack: any = "")
    {
        super(XErrorType.Error, pMessage, pDetail, pCallBack);
    }
}

class XWarning extends XException
{
    constructor(pMessage: string, pDetail: string | null = null, pStack: string | null = null, pCallBack: any = null)
    {
        super(XErrorType.Warning, pMessage, pDetail, pCallBack);
    }
}

class XUnconformity extends XException
{
    constructor(pMessage: string, pDetail: string | null = null, pStack: string | null = null, pCallBack: any = null)
    {
        super(XErrorType.Warning, pMessage, pDetail, pCallBack);
    }
}

class XMessage extends XException
{
    constructor(pMessage: string, pDetail: string | null = null, pStack: string | null = null, pCallBack: any = null)
    {
        super(XErrorType.Message, pMessage, pDetail, pCallBack);
    }
}