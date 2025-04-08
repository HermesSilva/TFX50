using System;

using TFX.Core.Model;

namespace TFX.Core.Model
{
	public class XDecimalDataField : XDataField<Decimal>
	{
		public XDecimalDataField()
		{
		}

		public XDecimalDataField(decimal pValue)
			: base(pValue)
		{
		}
		public XDecimalDataField(decimal? pValue)
			: base(pValue.HasValue ? pValue.Value : 0M)
		{
		}

		public XDecimalDataField(XFieldState pState, decimal pValue)
			: base(pState, pValue)
		{
		}

		public XDecimalDataField(XFieldState pState, Object pValue)
			: base(pState, pValue)
		{
		}

		public XDecimalDataField(XFieldState pState, decimal pValue, Object pOldValue)
			: base(pState, pValue, pOldValue)
		{
		}
	}
}
