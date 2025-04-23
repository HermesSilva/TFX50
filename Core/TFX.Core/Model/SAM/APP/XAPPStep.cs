using System;
using System.Collections.Generic;
using System.Drawing;

using TFX.Core.Model.APP;

namespace TFX.Core.Model.APP
{
    public class XPAMDecisionStep : XAPPStep
    {
        public XPAMDecisionStep()
        {
        }

        public XPAMDecisionStep(Guid pID, String pName, String pTitle, Guid pFormID, Guid pDataSourceID)
            : base(pID, pName, pTitle, pFormID, pDataSourceID)
        {
        }
    }

    public class XAPPStep : XContainerObject
    {
        public XAPPStep()
        {
        }

        public XAPPStep(Guid pID, String pName, String pTitle, Guid pFormID, Guid pDataSourceID)
        {
            ID = pID;
            Name = pName;
            Title = pTitle;
            FormID = pFormID;
            DataSourceID = pDataSourceID;
        }

        private Dictionary<Guid, Object> _Variables = new Dictionary<Guid, Object>();

        public Guid DataSourceID;
        public Guid DataSetID;
        public Guid[] NextStepsID = new Guid[0];
        public Guid[] PreviousStepsID;
        public Guid FormID;
        public Guid GroupID;
        public String[] Paths;
        public Rectangle Rect;
        public Guid SourceFieldID;
        public Int32 Flow;
        public Int64[] Values;
        public Guid[] AditionalFormsID;
        public Guid SourceLinkFieldID;
        public Guid TargetLinkFieldID;
        public Guid TitleFieldID;
        public Guid StateFieldID;
        public Boolean IsRepeatable;
        public String Previous;
        public String PreviousDescription;
        public String Next;
        public String NextDescription;
        public Boolean FlushService;

        public XAPPModel SAM
        {
            get
            {
                return (XAPPModel)Owner;
            }
        }
    }
}