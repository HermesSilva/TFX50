using System;

using TFX.Core.Model;

namespace TFX.Core.Model
{
    public class XBinaryDataField : XDataField<Byte[]>
    {
        public XBinaryDataField()
        {
        }

        public XBinaryDataField(byte[] pValue)
            : base(pValue)
        {
        }

        public XBinaryDataField(XFieldState pState, byte[] pValue)
            : base(pState, pValue)
        {
        }

        public XBinaryDataField(XFieldState pState, byte[] pValue, Object pOldValue)
            : base(pState, pValue, pOldValue)
        {
        }
    }
}
