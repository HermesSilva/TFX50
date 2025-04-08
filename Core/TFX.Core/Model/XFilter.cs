using System;

namespace TFX.Core.Model
{
    public class XFilter : XDataTuple
    {
        public Int32? TakeRows
        {
            get; set;
        }

        public Int32? SkipRows
        {
            get; set;
        }
    }
}
