using System;

using TFX.Core.Model;

namespace TFX.Core.Model
{
	public class XInt32DataField : XDataField<Int32>
	{
		public XInt32DataField()
		{
		}

		public XInt32DataField(int pValue)
			: base(pValue)
		{
		}

		public XInt32DataField(XFieldState pState, int pValue)
			: base(pState, pValue)
		{
		}

		public XInt32DataField(XFieldState pState, Object pValue)
			: base(pState, pValue)
		{
		}

		public XInt32DataField(XFieldState pState, int pValue, Object pOldValue)
			: base(pState, pValue, pOldValue)
		{
		}
	}

    public class XInt32NullableDataField : XDataField<Int32?>
    {
        public XInt32NullableDataField()
        {
        }


        public XInt32NullableDataField(Int32? pValue)
            : base(pValue)
        {
        }

        public XInt32NullableDataField(XFieldState pState, Int32? pValue)
            : base(pState, pValue)
        {
        }

        public XInt32NullableDataField(XFieldState pState, Object pValue)
            : base(pState, pValue)
        {
        }

        public XInt32NullableDataField(XFieldState pState, Int32? pValue, Object pOldValue)
            : base(pState, pValue, pOldValue)
        {
        }
    }
}
