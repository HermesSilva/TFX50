
type XAppLoad = (App: XAPPModel) => void;

class XMainCache
{
    private static Cache: { [Key: string]: XAPPModel } = {}

    static Get(AppID: string, pContex: any, pCallBack: XAppLoad)
    {
        let app = this.Cache[AppID];
        if (app)
        {
            pCallBack.apply(pContex, [app]);
            return;
        }
        var clt = new XHttpClient(this, Routes.AppModel);
        clt.SetCallBackData([pCallBack, pContex])
        clt.OnLoad = this.LoadCallBack
        clt.SendAsync({ ID: AppID });
    }

    static LoadCallBack(pData: XResponse<XAPPModel>, pCallData: any | null, pEvent: ProgressEvent | null)
    {
        XMainCache.Add(pData.Data.ID, pData.Data);
        pCallData[0].apply(pCallData[1], [pData.Data]);
    }

    static Add(pID: string, Model: XAPPModel)
    {
        this.Cache[pID] = Model;
    }
}