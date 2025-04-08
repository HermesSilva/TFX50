using System;

using Newtonsoft.Json.Linq;

using TFX.Core.Model;

namespace TFX.Core.Model
{
    public class XGuidDataField : XDataField<Guid>
    {

        public XGuidDataField()
        {
        }

        public XGuidDataField(Guid pValue)
            : base(pValue)
        {
        }

        public XGuidDataField(Object pValue)
            : this(XFieldState.Unchanged, pValue)
        {
        }

        public XGuidDataField(XFieldState pState, Guid pValue)
            : base(pState, pValue)
        {
        }

        public XGuidDataField(XFieldState pState, object pValue)
            : base(pState, pValue)
        {
        }

        public XGuidDataField(XFieldState pState, Guid pValue, object pOldValue)
            : base(pState, pValue, pOldValue)
        {
        }
    }
    public class XGuidNullableDataField : XDataField<Guid?>
    {

        public XGuidNullableDataField()
        {
        }

        public XGuidNullableDataField(Guid? pValue)
            : base(pValue)
        {
        }

        public XGuidNullableDataField(Object pValue)
            : this(XFieldState.Unchanged, pValue)
        {
        }

        public XGuidNullableDataField(XFieldState pState, Guid? pValue)
            : base(pState, pValue)
        {
        }

        public XGuidNullableDataField(XFieldState pState, object pValue)
            : base(pState, pValue)
        {
        }

        public XGuidNullableDataField(XFieldState pState, Guid? pValue, object pOldValue)
            : base(pState, pValue, pOldValue)
        {
        }
    }
}
