using System;
using System.Collections.Generic;

using TFX.Core.Cache;
using TFX.Core.Data;


namespace TFX.Core.Model.FRM
{
    public delegate void XValidateAction(XFRMModel pForm, Boolean pClear = true);

    public enum XFRMEditorType
    {
        AdhocEditor = 1,
        AnswerStringEditor = 2,
        BooleanEditor = 3,
        ButtonEditor = 4,
        ComboStringEditor = 5,
        ConstantLabelBox = 6,
        DateEditor = 7,
        DateTimeEditor = 8,
        DBDataLabelBox = 9,
        DecimalEditor = 10,
        DescriptionEditor = 11,
        DetailBinaryBox = 12,
        DetailSchedulerEditor = 13,
        DetailsDataGridEditor = 14,
        DynamicFormBox = 15,
        FileUpload = 16,
        FingerprintEditor = 17,
        HTMLEditor = 18,
        ImageFileEditor = 19,
        Int32Editor = 20,
        Int64Editor = 21,
        LabelBox = 22,
        MemoEditor = 23,
        PasswordEditor = 24,
        RepeatableDetailEditor = 25,
        SchedulerBoxEditor = 26,
        ServiceSelectorEditor = 27,
        StaticCrossDataGridEditor = 28,
        StaticSelectorEditor = 29,
        StringDiscreetEditor = 30,
        StringDiscreetParagraphEditor = 31,
        StringEditor = 32,
        TimeEditor = 33,
        TreeViewEditor = 34
    }


    public enum XFRMStyle
    {
        Normal = 0,
        Document = 1,
    }

    public enum XFRMType
    {
        None = 0,
        Activity = 1,
        SVCFilter = 2,
        DetailGrid = 3,
        Config = 4,
        StandAlone = 5,
        PAMAdditionalForm = 6,
        RPTFilter = 7,
    }

    public class XFRMModel : XObject
    {
        public XFRMModel()
        {
            Fields = [];
        }

        public List<XFRMField> Fields
        {
            get;
        }

        protected XFRMField AddField(XFRMField pField)
        {
            Fields.Add(pField);
            return pField;
        }

        public Boolean IsLineForm
        {
            get; set;
        }
        public Int32 MinRows
        {
            get; set;
        }
        public Guid ConfigCID
        {
            get; set;
        }
        public Boolean ConfigPKFilter
        {
            get; set;
        }
        public Boolean CompanyFilter
        {
            get; set;
        }
        public XFRMStyle Style
        {
            get; set;
        }
        public XFRMType Type
        {
            get; set;
        }
    }
}