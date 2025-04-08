using System;

namespace TFX.Core.Exceptions
{
    public delegate void XCathException(Object pSender, Exception pException);

    public enum XExceptionType : byte
    {
        Error = 1,
        Warning = 2,
        Unconformity = 3,
    }

    [Serializable]
    public abstract class XException : Exception
    {
        public XException()
        {
        }

        public XException(String pMessage, Exception pException)
          : base(pMessage, pException)
        {
        }

        public XException(String pMSG)
          : base(pMSG)
        {
        }

        public XException(String pMSG, String pDetail)
          : base(pMSG)
        {
            Detail = pDetail;
        }

        public XException(String pMessage, Exception pException, String pDetail)
        : base(pMessage, pException)
        {
            Detail = pDetail;
        }

        public String Detail;

        public virtual XExceptionType Type
        {
            get;
        }

        public static Exception Deserialize(Byte[] pException)
        {
            throw new NotImplementedException();
        }

        public static Byte[] Serialize(Exception pEx)
        {
            throw new NotImplementedException();
        }
    }
}
