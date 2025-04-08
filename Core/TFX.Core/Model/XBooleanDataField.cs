using System;

using TFX.Core.Model;

namespace TFX.Core.Model
{
    public class XBooleanDataField : XDataField<Boolean>
    {
        public XBooleanDataField()
        {
        }

        public XBooleanDataField(bool pValue) : base(pValue)
        {
        }

        public XBooleanDataField(XFieldState pState, bool pValue)
            : base(pState, pValue)
        {
        }

        public XBooleanDataField(XFieldState pState, object pValue)
            : base(pState, pValue)
        {
        }

        public XBooleanDataField(XFieldState pState, bool pValue, object pOldValue)
            : base(pState, pValue, pOldValue)
        {
        }
    }
}
