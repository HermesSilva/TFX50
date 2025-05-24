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

    public class XAPPStep : XObject
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


        public Guid DataSourceID
        {
            get; set;
        }
        public Guid DataSetID
        {
            get; set;
        }
        public Guid[] NextStepsID
        {
            get; set;
        }
        public Guid[] PreviousStepsID
        {
            get; set;
        }
        public Guid FormID
        {
            get; set;
        }
        public Guid GroupID
        {
            get; set;
        }
        public String[] Paths
        {
            get; set;
        }
        public Rectangle Rect
        {
            get; set;
        }
        public Guid SourceFieldID
        {
            get; set;
        }
        public Int32 Flow
        {
            get; set;
        }
        public Int64[] Values
        {
            get; set;
        }
        public Guid[] AditionalFormsID
        {
            get; set;
        }
        public Guid SourceLinkFieldID
        {
            get; set;
        }
        public Guid TargetLinkFieldID
        {
            get; set;
        }
        public Guid TitleFieldID
        {
            get; set;
        }
        public Guid StateFieldID
        {
            get; set;
        }
        public Boolean IsRepeatable
        {
            get; set;
        }
        public String Previous
        {
            get; set;
        }
        public String PreviousDescription
        {
            get; set;
        }
        public String Next
        {
            get; set;
        }
        public String NextDescription
        {
            get; set;
        }
        public Boolean FlushService
        {
            get; set;
        }

        public XAPPModel SAM
        {
            get
            {
                return (XAPPModel)Owner;
            }
        }
    }
}