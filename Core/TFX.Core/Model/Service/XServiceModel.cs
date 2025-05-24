using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Update.Internal;

using TFX.Core.Model.FRM;
using TFX.Core.Services;

namespace TFX.Core.Model.Service
{

    public class XColumnModel
    {
        public string Name
        {
            get; set;
        }
        public string Description
        {
            get; set;
        }
        public string Type
        {
            get; set;
        }
        public string Title
        {
            get; set;
        }
        public string Value
        {
            get; set;
        }
    }

    public class XDataViewModel
    {
        public XDataViewModel()
        {
            Columns = [];
        }

        public List<XColumnModel> Columns
        {
            get; set;
        }
    }


    public class XServiceModel
    {
        public XServiceModel()
        {
            DataView = new XDataViewModel();
            Forms = [];
        }

        public string SearchPath
        {
            get;
            set;
        }
        public List<XFRMModel> Forms
        {
            get;
        }

        public XDataViewModel DataView
        {
            get;
        }
    }
}
