using System;

namespace TFX.Core.Exceptions
{
    [Serializable]
    public class XError : XException
    {
        public static Boolean HasError = false;

        public XError(String pMSG)
          : base(pMSG)
        {
            HasError = true;
        }

        public XError(String pMessage, Exception pException)
          : base(pMessage, pException)
        {
            HasError = true;
        }

        public XError(String pMessage, String pDetail)
            : base(pMessage, pDetail)
        {
        }


        public XError(String pMessage, Exception pException, String pDetail)
          : base(pMessage, pException, pDetail)
        {
        }

        public override XExceptionType Type
        {
            get
            {
                return XExceptionType.Error;
            }
        }
    }
}
