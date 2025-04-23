using System;
using System.Collections.Generic;

using TFX.Core.Cache;
using TFX.Core.Data;


namespace TFX.Core.Model.FRM
{
    public delegate void XValidateAction(XFRMModel pForm, Boolean pClear = true);

    public static class XEditorType
    {
        public const Int32 Int32Editor = 16777383;
        public const Int32 BooleanEditor = 16777382;
        public const Int32 StaticCrossDataGridEditor = 16778930;
        public const Int32 DateTimeEditor = 16777385;
        public const Int32 DateEditor = 497479;
        public const Int32 TimeEditor = 497480;
        public const Int32 DetailsDataGridEditor = 16777387;
        public const Int32 Int64Editor = 16777384;
        public const Int32 MemoEditor = 16780528;
        public const Int32 PasswordEditor = 16777386;
        public const Int32 ServiceSelectorEditor = 16778774;
        public const Int32 StringEditor = 16777381;
        public const Int32 DecimalEditor = 464859;
        public const Int32 AdhocEditor = 469549;
        public const Int32 HTMLEditor = 470136;
        public const Int32 StaticSelectorEditor = 16777388;
        public const Int32 ImageFileEditor = 473207;
        public const Int32 FingerprintEditor = 473501;
        public const Int32 RepeatableDetailEditor = 497262;
        public const Int32 FileUpload = 516781;
        public const Int32 ButtonEditor = 516783;
        public const Int32 TreeViewEditor = 594204;
        public const Int32 SchedulerBoxEditor = 599657;
        public const Int32 DetailSchedulerEditor = 600166;
        public const Int32 ComboStringEditor = 600395;
        public const Int32 DescriptionEditor = 600397;
        public const Int32 AnswerStringEditor = 600913;
        public const Int32 ConstantLabelBox = 601922;
        public const Int32 LabelBox = 601918;
        public const Int32 StringDiscreetEditor = 601919;
        public const Int32 StringDiscreetParagraphEditor = 601920;
        public const Int32 DynamicFormBox = 601956;
        public const Int32 DBDataLabelBox = 602288;
        public const Int32 DetailBinaryBox = 603763;
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

    public class XFRMModel : XContainerObject
    {
        public XFRMModel()
        {
        }

        public Boolean IsLineForm;
        public Int32 MinRows;
        public Guid ConfigCID;
        public Boolean ConfigPKFilter;
        public Boolean CompanyFilter;
        public XFRMStyle Style = XFRMStyle.Normal;
        public event XValidateAction ValidateAction;
        public XFRMType FRMType = XFRMType.None;
    }
}