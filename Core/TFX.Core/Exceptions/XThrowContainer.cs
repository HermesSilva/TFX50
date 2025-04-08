using System;

namespace TFX.Core.Exceptions
{
    [Serializable]
    public class XThrowContainer : Exception
    {
        private static Exception Check(Exception pEx)
        {
            if (!(pEx is XError))
                return new XError(pEx.Message, pEx);
            return pEx;
        }

        public XThrowContainer(Exception pException)
          : base("", Check(pException))
        {
        }

        public XThrowContainer(String pMessage, Exception pException)
          : base(pMessage, Check(pException))
        {
        }

    }
}
