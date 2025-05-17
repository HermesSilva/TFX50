using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore.Update.Internal;

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
        }
        public XDataViewModel DataView
        {
            get;
        }
    }
}
