using System;
using System.Text.Json.Serialization;

using TFX.Core.Interfaces;

using TFX.Core.Model;

namespace TFX.Core.Model
{

    public class XDataField
    {
        public void Set<TValue, T>(TValue pField, T Value)
        {
        }

    }

    public class XDataField<T> : XDataField, XIDataField 
    {

        //public static Boolean operator !=(XDataField<T> pLeft, XDataField<T> pRight)
        //{
        //    return !(pLeft == pRight);
        //}

        //public static Boolean operator ==(XDataField<T> pLeft, XDataField<T> pRight)
        //{
        //    if ((Object)pLeft == (Object)pRight && (Object)pRight == null)
        //        return true;
        //    if (((Object)pLeft == null && (Object)pRight != null) || ((Object)pLeft != null && (Object)pRight == null) || ((Object)pLeft == null && (Object)pRight == null))
        //        return false;
        //    return Object.Equals(pLeft.Value, pRight.Value);
        //}

        public XDataField()
        {
        }

        public XDataField(T pValue)
        {
            Value = pValue;
            State = XFieldState.Unchanged;
        }

        public XDataField(XFieldState pState, T pValue)
        {
            Value = pValue;
            State = pState;
        }

        public XDataField(XFieldState pState, Object pValue)
        {
            if (pValue is T vlr)
                Value = (T)vlr;
            else
                Value = default(T);
            State = pState;
        }

        public XDataField(XFieldState pState, T pValue, Object pOldValue)
        {
            Value = pValue;
            OldValue = pOldValue;
            State = pState;
        }

        public XDataField<T> Clone(XDataField<T> pSource)
        {
            var fld = this.GetType().CreateInstance<XDataField<T>>();
            fld.Value = Value;
            fld.OldValue = OldValue;
            fld.State = State;
            return fld;
        }

        private T _Value;

        public void SetValue(T pValue)
        {
            Value = pValue; 
        }

        public T Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (!Object.Equals(_Value, value))
                {
                    _Value = value;
                    if (State != XFieldState.Modified)
                        State = XFieldState.Modified;
                }
            }
        }

        [JsonIgnore]
        public Object? OldValue
        {
            get; set;
        }

        public XFieldState State
        {
            get; set;
        }

        public override Boolean Equals(Object pObject)
        {
            if (GetHashCode() == pObject?.GetHashCode())
                return true;
            var other = pObject as XDataField<T>;
            if (other != null && Object.Equals(other.Value, Value))
                return true;
            return false;
        }

        public override Int32 GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}

