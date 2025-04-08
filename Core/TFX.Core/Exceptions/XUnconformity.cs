using System;
using System.Text;

namespace TFX.Core.Exceptions
{
    [Serializable]
    public class XUnconformity : XException
    {
        public static Boolean InClietSide = false;

        public XUnconformity()
        {
        }

        public XUnconformity(StringBuilder pBuilder)
            : this(pBuilder.ToString())
        {
        }

        public XUnconformity(Exception pException)
          : base("", pException)
        {
        }

        public XUnconformity(String pMessage, Exception pException)
          : base(pMessage, pException)
        {
        }

        public XUnconformity(String pMSG, String pDetail)
          : base(pMSG, pDetail)
        {
        }

        public XUnconformity(String pMSG)
          : base(pMSG)
        {
        }

        public XUnconformity(String pMessage, Exception pException, String pDetail)
          : base(pMessage, pException, pDetail)
        {
        }

        public override XExceptionType Type
        {
            get
            {
                return XExceptionType.Unconformity;
            }
        }
    }
}
