class XHotkeyManager
{
    static OnKeyDown(pArg: KeyboardEvent)
    {
        let elm = <any>pArg.target;
        if (!X.IsEmpty(pArg.key) && pArg.key.length == 1 && pArg.altKey)
        {
            pArg.preventDefault();
            return false;
        }
        return true;
    }
}