/// <reference path="../Net/XHttpClient.ts" />

type XAppLoad = (App: XAPPModel) => void;

class XMainCache
{
    private static AppCache: { [Key: string]: XAPPModel } = {}
    private static ServiceCache: { [Key: string]: XServiceModel } = {}
    private static _Client = new XHttpClient(XMainCache);

    static GetApp(AppID: string, pContex: any, pCallBack: XAppLoad)
    {
        let app = this.AppCache[AppID];
        if (app)
        {
            pCallBack.apply(pContex, [app]);
            return;
        }
        this._Client.SetCallBackData([pCallBack, pContex])
        this._Client.SendAsync(Paths.AppModel, { ID: AppID }, (pData: XResponse<XAPPModel>, pCallData: any | null, pEvent: ProgressEvent | null) =>
        {
            XMainCache.AppCache[pData.Data.ID] = pData.Data;
            pCallData[0].apply(pCallData[1], [pData.Data]);
        });
    }

    static GetService(AppID: string, pContex: any, pCallBack: XAppLoad)
    {
        let app = this.AppCache[AppID];
        if (app)
        {
            pCallBack.apply(pContex, [app]);
            return;
        }
        this._Client.SetCallBackData([pCallBack, pContex])
        this._Client.SendAsync(Paths.AppModel, { ID: AppID }, (pData: XResponse<XServiceModel>, pCallData: any | null, pEvent: ProgressEvent | null) =>
        {
            XMainCache.ServiceCache[pData.Data.ID] = pData.Data;
            pCallData[0].apply(pCallData[1], [pData.Data]);
        });
    }
}