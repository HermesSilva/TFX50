using System;

using TFX.Core.Model;

namespace TFX.Core.Model
{
	public class XDateTimeDataField : XDataField<DateTime>
	{
		public XDateTimeDataField()
		{
		}

		public XDateTimeDataField(DateTime pValue)
			: base(pValue)
		{
		}
		public XDateTimeDataField(DateTime? pValue)
					: base(pValue.HasValue ? pValue.Value : XDefault.NullDateTime)
		{
		}

		public XDateTimeDataField(XFieldState pState, DateTime pValue)
			: base(pState, pValue)
		{
		}

		public XDateTimeDataField(XFieldState pState, Object pValue)
			: base(pState, pValue)
		{
		}

		public XDateTimeDataField(XFieldState pState, DateTime pValue, Object pOldValue)
			: base(pState, pValue, pOldValue)
		{
		}
	}
}
