using System;
using System.Collections.Generic;
using System.Drawing;

namespace TFX.Core.Model.APP
{
    public enum XPAMActionView : byte
    {
        None = 0,
        Search = 1,
        SearchByRow = 2,
        Edit = 3,
        View = 4,
        EditView = 5,
        AllTime = 6,
        New = 7,
    }

    public class XAddButton : XObject
    {
        public Int32 ServiceID
        {
            get; set;
        }
        public String IconURL
        {
            get; set;
        }
        public String AskMessage
        {
            get; set;
        }
        public String Hint
        {
            get; set;
        }
        public Boolean AllRecords
        {
            get; set;
        }
        public XPAMActionView ActionType
        {
            get; set;
        }
        public Guid[] Rights
        {
            get; set;
        }
    }
    public class XAPPModel : XContainerObject
    {
        public XAPPModel()
        {
            Steps = [];
        }

        public List<XAPPStep> Steps
        {
            get;
        }

        public Guid DataSourceID
        {
            get; set;
        }
        public Boolean FLushAtStep
        {
            get; set;
        }
        public Size Size
        {
            get; set;
        }
        public Boolean HasDetail
        {
            get; set;
        }
        public Guid SearchServiceID
        {
            get; set;
        }
        public Guid SearchPKID
        {
            get; set;
        }
        public Guid[] SubjectFields
        {
            get; set;
        }
        public String SubjectTitle
        {
            get; set;
        }
        public Guid MainViewID
        {
            get; set;
        }
        public Int16 MainMenuID
        {
            get; set;
        }
        public Int16 PlatformID
        {
            get; set;
        }
        public Boolean IsSystem
        {
            get; set;
        }
        public Guid ConfigFormID
        {
            get; set;
        }
        public Boolean IsDisabled
        {
            get; set;
        }
        public Int16[] Rights
        {
            get; set;
        }
        public String Icon
        {
            get; set;
        }
        public Int16 GroupID
        {
            get; set;
        }
        public Int32 AppTypeID
        {
            get; set;
        }
        public Int16[] UseTypeID
        {
            get; set;
        }
        public Boolean OnlyHolding
        {
            get; set;
        }
        public Boolean FreeForUsers
        {
            get; set;
        }
    }
}