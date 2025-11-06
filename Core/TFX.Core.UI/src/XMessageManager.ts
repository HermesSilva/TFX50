
class XMessageManager
{
    static ContexError(pContext: XElement, pError: any, pCallData: any | null, pEvent: ProgressEvent | null)
    {
        try
        {
            var ctx = pContext.GetDialogContainer();
            if (ctx != null && ctx.Dialog != null)
                ctx.Dialog.ShowError("Erro Não Previsto.", pError.message, pError.stack);
            else
                window.Dialog.ShowError("Erro Não Previsto.", pError.message, pError.stack);
        }
        catch (e)
        {
            console.error("Failed to show global error:", e);
        }
    }

    static ShowGlobalError(pError: Error | any) 
    {
        try
        {
            window.Dialog.ShowError("Erro Não Previsto.", pError.message, pError.stack);
        }
        catch (e)
        {
            console.error("Failed to show global error:", e);
        }
    }
}
