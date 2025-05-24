interface XData<T>
{
    ID: string
    Tuples: T[]
}

interface XResponse<T>
{
    Ok: boolean;
    Status: number;
    Data: T;
    Errors?: string
    Details?: string
}

interface XTuple
{
    IsReadOnly: boolean;
    IsSelected: boolean;
    IsChecked: boolean;
    State: number;
}

interface XField
{
    Value: string;
    State: number;
}

class XModelEditors 
{
    static XPromoteStringEditor = "29D1D781-89F4-4D05-876C-02016DE4ACC1"
    static XBoolean = "8165BCEE-65D3-4BDA-80AF-B4EF0B1590F0"
    static XComboString = "5B93450B-7432-4A60-8774-44755231C76F"
    static XDetailsDataGridEx = "4663FD81-BE86-4A8F-8676-B094A8304277"
    static XDetailsDataGridImp = "CEB96A0B-3856-44DE-9264-8D264940C893"
    static XDate = "00292032-E4A9-4A74-8B31-BF42D90A137A"
    static XDateTime = "D56E8E17-9D09-4FE9-9A30-A6AA5865ABC6"
    static XDBDataLabel = "E055611D-CE99-40B8-9A24-34E156D1401F"
    static XDecimal = "6FD76216-1D13-4438-B568-72556767B3DF"
    static XDescription = "1875318E-9350-4B21-A68D-1B2B0377170C"
    static XDetailScheduler = "763A1846-53AE-4C59-A479-6509CED9CFF6"
    static XDetailsDataGrid = "083C4AC6-6C57-4BE4-8EFB-11B6B442FA4C"
    static XDynamicForm = "0005A0E4-850E-481A-98D2-2C8A0548FA60"
    static XFileSelector = "F427FA36-C763-455B-9C58-182E347E01DC"
    static XFormAdhoc = "0D42C2CC-30AE-4486-8FD5-C449DD377E40"
    static XHTML = "C2D436A3-8683-4D5A-A770-186E0ADDCCCE"
    static XImageView = "F0C1A4EC-DBF7-4629-A369-74B42DA9D83A"
    static XInt32 = "B45FBE87-40A0-44EF-B2EC-CD6A3000DA3A"
    static XInt64 = "9CBDEBDC-C431-4D85-8EF8-FE89876D5548"
    static XLabel = "334E5334-CCC4-4B25-A546-36089F25FB9D"
    static XLookup = "66290387-F0AD-4746-A1F7-B0D8D4C5C2EE"
    static XLookupMultiSelect = "BB8C8BA5-3378-4253-92DF-1CCC774BD925"
    static XMainScheduler = "922B1689-6911-43C1-A52D-680259343CBC"
    static XMemo = "ADA993EB-4A42-440D-98E2-722EE0119959"
    static XPassword = "5CD44E65-DDA1-43A6-899D-19B08032044E"
    static XScheduler = "1414EAB6-E148-43D9-8EB5-8E508825F813"
    static XStaticCombo = "AC111493-AD7B-4F74-A0E8-DF0E0BB6B74A"
    static XStringDiscreet = "40540A3A-14D2-4E66-9A7F-7C32EA75D65A"
    static XStringDiscreetParagraph = "703C78D9-DB54-4D46-BD26-B470356C55AF"
    static XString = "F5982E79-BA45-40FB-85E1-9F2C8B90B6EF"
    static XTime = "0F94CC94-1D3E-41A6-850F-305695E46817"
    static XTreeView = "831CA1FB-78DA-4341-A635-F171ED7A33AF"
    static XValueSelector = "467003AD-148B-47D5-B5B1-D81E525EDF44"
    static XSearchBox = "AA2A0DB9-2C59-4833-994D-3DBF2AA0CB3C"
    static XQueryDesigner = "AFAB755F-D08E-4B3B-996D-A0D5EED370F0"
    static XInt16 = "19967A5D-6772-4B74-B25B-9D0F21FF9EEF"
    static XMapViewEditor = "4C8C9711-A99D-4779-93DD-8443AEC5F549"
    static XImageEditor = "BE361BF6-973B-44F4-AFB4-81CED3E1F848"
    static XCleanArea = "90F889A0-F4BC-4370-9BC5-7DE20F06A334"
    static XTabControlEditor = "59A28B08-01BF-4EAF-81CA-F317B3907B19"
    static XImageGridEditor = "2B717129-F9F5-48BD-9DE8-5CC0AF15B425"
    static XDocumentGridEditor = "C11A7B09-292C-4D86-9ADA-7907C1EF95BA"
    static XMemoGridEditor = "B2B78C3C-48B4-4B16-935C-045691B79E05"
    static XPanelEditor = "E939FB33-DC74-4BF1-A9B8-CDE54A2145B5"
    static XSearchDetailEditor = "C559C9FE-F950-40D8-8959-C2791775E652"
    static XToggleInativeEditor = "06A707A8-7E43-4C18-BD8D-CE6508F5FA39"
} 
