/// <reference path="../Elements/Base/XBaseInput.ts" />
class XEditorFactory 
{
    static DataTypeToEditorType(pDataType: string): string
    {
        switch (pDataType.toLowerCase())
        {
            case "date":
                return XModelEditors.XDate;
            case "datetime":
                return XModelEditors.XDateTime;
            case "decimal":
                return XModelEditors.XDecimal;
            case "int32":
                return XModelEditors.XInt32;
            case "int64":
                return XModelEditors.XInt64;
            case "string":
                return XModelEditors.XString;
            case "boolean":
                return XModelEditors.XBoolean;
            default:
                return XModelEditors.XString; 
        }

    }
    static CreateEditor(pOwner: XForm, pField: XFRMField): XIEditor
    {
        var editor: XIEditor = XEditorFactory.NewEditor(pField.EditorCID, pOwner);
        editor.AdditionalDataFieldsID = pField.AdditionalDataFieldsID;
        editor.AdditionalFieldsID = pField.AdditionalFieldsID;
        editor.AllowEmpty = pField.AllowEmpty;
        editor.Cols = pField.ColCount;
        editor.ColsServiceID = pField.ColsServiceID;
        editor.DataSourceID = pField.DataSourceID;
        editor.Description = pField.Description;
        editor.Description = pField.Description;
        editor.GeneratorInfo = pField.GeneratorInfo;
        editor.GeneratorInfo = pField.GeneratorInfo;
        editor.GridFormCID = pField.GridFormCID;
        editor.ID = pField.ID;
        editor.IsChecked = pField.IsChecked;
        editor.IsFormInplace = pField.FormImplace;
        editor.IsFreeSearch = pField.IsFreeSearch;
        editor.IsJustifyHeight = pField.JustifyHeight;
        editor.IsNullable = pField.IsNullable;
        editor.IsReadOnly = pField.IsReadOnly;
        editor.IsRequired = pField.IsRequired;
        editor.IsSelected = pField.IsSelected;
        editor.LookupPKFieldID = pField.LookupPKFieldID;
        editor.Mask = pField.Mask;
        editor.Name = pField.Name;
        editor.Order = pField.Order;
        editor.OrderIndex = pField.Location;
        editor.OwnerID = pField.OwnerID;
        editor.ParentID = pField.ParentID;
        editor.Rows = pField.RowCount;
        editor.RowsServiceID = pField.RowsServiceID;
        editor.SourceDisplayFieldID = pField.SourceDisplayFieldID;
        editor.SourceFilterFieldID = pField.SourceFilterFieldID;
        editor.State = pField.State;
        editor.TargetDisplayFieldID = pField.TargetDisplayFieldID;
        editor.TargetFilterFieldID = pField.TargetFilterFieldID;
        editor.Title = pField.Title;
        editor.Type = pField.Type;
        editor.Value = pField.Value;
        return editor;
    }

    static NewEditor(pEditorCID: string, pOwner: XElement | HTMLElement | null): XIEditor
    {
        var editor!: XIEditor;
        switch (pEditorCID)
        {
            case XModelEditors.XDate:
            case XModelEditors.XDateTime:
                editor = new XDatePickerEditor(pOwner);
                break;
            case XModelEditors.XDecimal:
                editor = new XDecimalEditor(pOwner);
                break;
            case XModelEditors.XInt32:
                editor = new XInt32Editor(pOwner);
                break;
            case XModelEditors.XInt64:
                editor = new XInt64Editor(pOwner);
                break;
            case XModelEditors.XMemo:
                editor = new XMemoEditor(pOwner);
                break;
            case XModelEditors.XString:
                editor = new XStringEditor(pOwner);
                break;
            case XModelEditors.XSearchBox:
                editor = new XSearchBoxEditor(pOwner);
                break;

            case XModelEditors.XTime:
            case XModelEditors.XBoolean:
            case XModelEditors.XDescription:
            case XModelEditors.XPromoteStringEditor:
            case XModelEditors.XComboString:
            case XModelEditors.XDetailsDataGridEx:
            case XModelEditors.XDetailsDataGridImp:
            case XModelEditors.XDBDataLabel:
            case XModelEditors.XDetailScheduler:
            case XModelEditors.XDetailsDataGrid:
            case XModelEditors.XDynamicForm:
            case XModelEditors.XFileSelector:
            case XModelEditors.XFormAdhoc:
            case XModelEditors.XHTML:
            case XModelEditors.XImageView:
            case XModelEditors.XLabel:
            case XModelEditors.XLookup:
            case XModelEditors.XLookupMultiSelect:
            case XModelEditors.XMainScheduler:
            case XModelEditors.XPassword:
            case XModelEditors.XScheduler:
            case XModelEditors.XStaticCombo:
            case XModelEditors.XStringDiscreet:
            case XModelEditors.XStringDiscreetParagraph:
            case XModelEditors.XTreeView:
            case XModelEditors.XValueSelector:
            case XModelEditors.XQueryDesigner:
            case XModelEditors.XInt16:
            case XModelEditors.XMapViewEditor:
            case XModelEditors.XImageEditor:
            case XModelEditors.XCleanArea:
            case XModelEditors.XTabControlEditor:
            case XModelEditors.XImageGridEditor:
            case XModelEditors.XDocumentGridEditor:
            case XModelEditors.XMemoGridEditor:
            case XModelEditors.XPanelEditor:
            case XModelEditors.XSearchDetailEditor:
            case XModelEditors.XToggleInativeEditor:
            default:
                editor = new XStringEditor(pOwner);
                break;
        }
        return editor;
    }
}