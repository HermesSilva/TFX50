using System;

using TFX.Core.Model;

namespace TFX.Core.Model
{
	public class XInt64DataField : XDataField<Int64>
	{
		public XInt64DataField()
		{
		}

		public XInt64DataField(long pValue)
			: base(pValue)
		{
		}

		public XInt64DataField(XFieldState pState, long pValue)
			: base(pState, pValue)
		{
		}

		public XInt64DataField(XFieldState pState, Object pValue)
			: base(pState, pValue)
		{
		}

		public XInt64DataField(XFieldState pState, long pValue, Object pOldValue)
			: base(pState, pValue, pOldValue)
		{
		}
	}


    public class XInt64NullableDataField : XDataField<Int64?>
    {
        public XInt64NullableDataField()
        {
        }

        public XInt64NullableDataField(Int64? pValue)
            : base(pValue)
        {
        }

        public XInt64NullableDataField(XFieldState pState, Int64? pValue)
            : base(pState, pValue)
        {
        }

        public XInt64NullableDataField(XFieldState pState, Object pValue)
            : base(pState, pValue)
        {
        }

        public XInt64NullableDataField(XFieldState pState, Int64? pValue, Object pOldValue)
            : base(pState, pValue, pOldValue)
        {
        }
    }
}
