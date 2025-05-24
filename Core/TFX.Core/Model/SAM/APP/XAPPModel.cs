using System;
using System.Collections.Generic;
using System.Drawing;

using TFX.Core.Model.FRM;

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

    public enum XScenes : byte
    {
        SearchGrid = 1,
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
    public class XAPPModel : XObject
    {

        public XAPPModel()
        {
            Forms = [];
        }

        public void AddForm(XFRMModel pModel)
        {
            Forms.Add(pModel);
        }

        public Guid SearchServiceID
        {
            get;
            set;
        }

        public virtual String SearchPath
        {
            get;
            set;
        }

        public XScenes FisrtScene
        {
            get;
            set;
        }
        public List<XFRMModel> Forms
        {
            get;
            private set;
        }
    }
}