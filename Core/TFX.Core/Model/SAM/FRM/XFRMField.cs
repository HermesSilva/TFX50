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

    public class XFRMField : XContainerObject
    {
       
        public XFRMField()
        {
        }

        public XFRMField(Guid pID, String pName, String pTitle, Int32 pType, Int32 pRowCount, Int32 pColCount, Boolean pIsNullable, Guid pOwnerID, Object pDefaultValue = null, Guid pParentID = default)
        {
            ID = pID;
            Name = pName;
            Title = pTitle;
            Type = pType;
            RowCount = pRowCount;
            ColCount = pColCount;
            IsNullable = pIsNullable;
            OwnerID = pOwnerID;
            ParentID = pParentID;
            DefaultValue = pDefaultValue;
        }

        public XGeneratorInfo GeneratorInfo = new XGeneratorInfo();
        public Guid DataSourceID;
        public Int32 Location;
        public Guid EditorCID;
        public Object DefaultValue;
        public Boolean IsNullable;
        public Boolean AllowEmpty;
        public Int32 RowCount;
        public Int32 ColCount;
        public Guid OwnerID;
        public Guid ParentID;
        public Guid[] TargetDisplayFieldID;
        public Guid[] SourceDisplayFieldID;
        public Guid[] TargetFilterFieldID;
        public Guid[] SourceFilterFieldID;

        public Guid GridFormCID;
        public Guid RowsServiceID;
        public Guid ColsServiceID;
        public Boolean JustifyHeight;
        public Boolean IsRequired;
        public Guid[] AdditionalFieldsID;
        public Guid[] AdditionalDataFieldsID;
        public Int32 Order;
        public XFRMModel Model;
        public Boolean IsFreeSearch;
        public String Mask;
        public Guid LookupPKFieldID;
        public Boolean FormImplace;
        public Boolean IsReadOnly;
        public Guid FormFieldID;
        public Int32 TypeID;
        public Int32 Length;
        public Int32 Scale;
        public Guid PAMID;
        public Guid SourceFieldID;
        public Boolean AutoLoad;
        public Guid RowFieldID;
        public Boolean ForceRW;
        public Int32[] FilterData;
        public XFRMFieldFilterType FilterType;
        public Boolean CanInsert;
        public Boolean CanUpdate;
        public XOperator Operator;
        public Boolean FilterInative;
        public Guid SearchServiceID;
        public Guid SearchPKFieldID;
        public Int32[] SearchFilterFieldsID;
        public String ValueMath;
        public String FontColor;
        public XFontStyle FontStyle;
        public Boolean ShowFooter;
        public String[] ValueItems;
        public Boolean IsAnswer;
        public Boolean AllowMultiSelect;
        public Boolean NewLine;
        public Int32 FontSize = 13;
        public Guid ViewSAM;
        public Boolean IsHidden;
        public Boolean AllwaysPrint;
        public List<String> Rules = new List<String>();

        public virtual Object Value
        {
            get;
            set;
        }

        public XFRMModel Form
        {
            get
            {
                return (XFRMModel)Owner;
            }
        }

        public Int32 Type
        {
            get
            {
                return TypeID;
            }
            set
            {
                TypeID = value;
            }
        }
    }
}