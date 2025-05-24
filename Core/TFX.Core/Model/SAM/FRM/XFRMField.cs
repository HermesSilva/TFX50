using System;
using System.Collections.Generic;

using TFX.Core.Data;
using TFX.Core.DB;

namespace TFX.Core.Model.FRM
{
    public enum XFontStyle
    {
        Unset = 10,
        Regular = 0,
        Bold = 1,
        Italic = 2,
        BoldItalic = 3,
        Underline = 4,
        Strikeout = 8,
        Normal = 11
    }

    public enum XTextAlignment
    {
        Unset = 0,
        BaseLineLeft = 1,
        TopLeft = 2,
        CenterLeft = 3,
        BottomLeft = 4,
        BaseLineCenter = 5,
        TopCenter = 6,
        Center = 7,
        BottomCenter = 8,
        BaseLineRight = 9,
        TopRight = 10,
        CenterRight = 11,
        BottomRight = 12,
    }

    public enum XHorizontalAlignment
    {
        None = 0,
        Left = 1,
        Right = 2,
        Center = 3,
        Stretch = 4,
    }

    public enum XVerticalAlignment
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Center = 3,
        Stretch = 4,
    }
    public class XGeneratorInfo
    {
        public Guid GeneratorID;
        public Object Values;
        public Guid DataSourceID;
        public Boolean FilterInactive;
        public Boolean AllowEmpty;
        public Boolean AllowDuplicity;
        public Object ConstantValue;
        public Guid TestFieldsID;
        public Guid DataSourceFieldsID;
    }

    public enum XFRMFieldFilterType : int
    {
        Contains = 1,
        NotContains = 2,
    }

    public class XFRMField : XObject
    {

        public XGeneratorInfo GeneratorInfo = new XGeneratorInfo();
        public XFRMEditorType EditorType
        {
            get; set;
        }

        public Guid DataSourceID
        {
            get; set;
        }
        public Int32 Location
        {
            get; set;
        }
        public Guid EditorCID
        {
            get; set;
        }
        public Object DefaultValue
        {
            get; set;
        }
        public Boolean IsNullable
        {
            get; set;
        }
        public Boolean AllowEmpty
        {
            get; set;
        }
        public Int32 RowCount
        {
            get; set;
        }
        public Int32 ColCount
        {
            get; set;
        }
        public Guid OwnerID
        {
            get; set;
        }
        public Guid ParentID
        {
            get; set;
        }
        public Guid[] TargetDisplayFieldID
        {
            get; set;
        }
        public Guid[] SourceDisplayFieldID
        {
            get; set;
        }
        public Guid[] TargetFilterFieldID
        {
            get; set;
        }
        public Guid[] SourceFilterFieldID
        {
            get; set;
        }
        public Guid GridFormCID
        {
            get; set;
        }
        public Guid RowsServiceID
        {
            get; set;
        }
        public Guid ColsServiceID
        {
            get; set;
        }
        public Boolean JustifyHeight
        {
            get; set;
        }
        public Boolean IsRequired
        {
            get; set;
        }
        public Guid[] AdditionalFieldsID
        {
            get; set;
        }
        public Guid[] AdditionalDataFieldsID
        {
            get; set;
        }
        public Int32 Order
        {
            get; set;
        }
        public XFRMModel Model
        {
            get; set;
        }
        public Boolean IsFreeSearch
        {
            get; set;
        }
        public String Mask
        {
            get; set;
        }
        public Guid LookupPKFieldID
        {
            get; set;
        }
        public Boolean FormImplace
        {
            get; set;
        }
        public Boolean IsReadOnly
        {
            get; set;
        }
        public Guid FormFieldID
        {
            get; set;
        }
        public Guid TypeID
        {
            get; set;
        }
        public Int32 Length
        {
            get; set;
        }
        public Int32 Scale
        {
            get; set;
        }
        public Guid PAMID
        {
            get; set;
        }
        public Guid SourceFieldID
        {
            get; set;
        }
        public Boolean AutoLoad
        {
            get; set;
        }
        public Guid RowFieldID
        {
            get; set;
        }
        public Boolean ForceRW
        {
            get; set;
        }
        public Int32[] FilterData
        {
            get; set;
        }
        public XFRMFieldFilterType FilterType
        {
            get; set;
        }
        public Boolean CanInsert
        {
            get; set;
        }
        public Boolean CanUpdate
        {
            get; set;
        }
        public XOperator Operator
        {
            get; set;
        }
        public Boolean FilterInative
        {
            get; set;
        }
        public Guid SearchServiceID
        {
            get; set;
        }
        public Guid SearchPKFieldID
        {
            get; set;
        }
        public Int32[] SearchFilterFieldsID
        {
            get; set;
        }
        public String ValueMath
        {
            get; set;
        }
        public String FontColor
        {
            get; set;
        }
        public XFontStyle FontStyle
        {
            get; set;
        }
        public Boolean ShowFooter
        {
            get; set;
        }
        public String[] ValueItems
        {
            get; set;
        }
        public Boolean IsAnswer
        {
            get; set;
        }
        public Boolean AllowMultiSelect
        {
            get; set;
        }
        public Boolean NewLine
        {
            get; set;
        }
        public Int32 FontSize
        {
            get; set;
        }
        public Guid ViewSAM
        {
            get; set;
        }
        public Boolean IsHidden
        {
            get; set;
        }
        public Boolean AllwaysPrint
        {
            get; set;
        }
        public List<String> Rules = new List<String>();

        public virtual Object Value
        {
            get;
            set;
        }
    }
}