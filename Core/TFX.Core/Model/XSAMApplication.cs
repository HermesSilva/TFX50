using System;

namespace TFX.Core.Model
{
    public abstract class XSAMApplication
    {
        public String BtnNewID => $"NW-BTN{MenuID}";
        public String BtnCloseID => $"CL-BTN{MenuID}";
        public String BtnSearchID => $"SR-BTN{MenuID}";
        public String BtnPreviewID => $"PR-BTN{MenuID}";
        public String BtnEditID => $"ED-BTN{MenuID}";
        public String BtnSaveID => $"SV-BTN{MenuID}";
        public String BtnDeleteID => $"DL-BTN{MenuID}";
        public String BtnRecycleID => $"RC-BTN{MenuID}";

        public String Title
        {
            get;
            set;
        }
        public String Module
        {
            get;
            set;
        }
        public String Icon
        {
            get;
            set;
        }
        public Guid MenuID
        {
            get;
            set;
        }
        public int Ordem
        {
            get;
            set;
        }
    }
}
