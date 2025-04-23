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
        public Int32 ServiceID;
        public String IconURL;
        public String AskMessage;
        public String Hint;
        public Boolean AllRecords;
        public XPAMActionView ActionType;
        public Int16[] Rights = new Int16[] { 1 };
    }
    public class XAPPModel : XContainerObject
    {
        public Guid DataSourceID;
        public Boolean FLushAtStep;
        public Size Size;
        private Dictionary<Guid, XAPPStep> Steps = new Dictionary<Guid, XAPPStep>();
        public Boolean HasDetail;
        public Guid SearchServiceID;
        public Guid SearchPKID;
        public Guid[] SubjectFields;
        public String SubjectTitle;
        public Guid MainViewID;
        public Int16 MainMenuID;
        public Int16 PlatformID;
        public Boolean IsSystem;
        public Guid ConfigFormID;
        public Boolean IsDisabled;
        public Int16[] Rights;
        public String Icon;
        public Int16 GroupID;
        public Int32 AppTypeID = 0;
        public Int16[] UseTypeID = new Int16[0];
        public Boolean OnlyHolding = false;
        public Boolean FreeForUsers = false;
        private Boolean _IsReadOnly = false;
    }
}