using System;

using TFX.Core.Model;

namespace TFX.Core.Model
{
    public class XInt16DataField : XDataField<Int16>
    {
        public XInt16DataField()
        {
        }

        public XInt16DataField(short pValue) : base(pValue)
        {
        }

        public XInt16DataField(XFieldState pState, short pValue)
            : base(pState, pValue)
        {
        }

        public XInt16DataField(XFieldState pState, Object pValue)
            : base(pState, pValue)
        {
        }

        public XInt16DataField(XFieldState pState, short pValue, Object pOldValue)
            : base(pState, pValue, pOldValue)
        {
        }
    }

    public class XInt16NullableDataField : XDataField<Int16?>
    {
        public XInt16NullableDataField()
        {
        }

        public XInt16NullableDataField(Int16? pValue) : base(pValue)
        {
        }

        public XInt16NullableDataField(XFieldState pState, Int16? pValue)
            : base(pState, pValue)
        {
        }

        public XInt16NullableDataField(XFieldState pState, Object pValue)
            : base(pState, pValue)
        {
        }

        public XInt16NullableDataField(XFieldState pState, Int16? pValue, Object pOldValue)
            : base(pState, pValue, pOldValue)
        {
        }
    }
}
