using System;

using TFX.Core.Model;

namespace TFX.Core.Model
{
    public class XStringDataField : XDataField<String>
    {
        public XStringDataField()
        {
        }

        public XStringDataField(String pValue)
            : base(pValue)
        {
        }

        public XStringDataField(XFieldState pState, String pValue)
            : base(pState, pValue)
        {
        }

        public XStringDataField(XFieldState pState, String pValue, Object pOldValue)
            : base(pState, pValue, pOldValue)
        {
        }
    }
}
