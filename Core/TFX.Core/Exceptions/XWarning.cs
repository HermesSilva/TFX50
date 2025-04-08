using System;

namespace TFX.Core.Exceptions
{
    [Serializable]
    public class XWarning : XException
    {
        public XWarning(String pMSG)
          : base(pMSG)
        {
        }

        public XWarning(String pMSG, String pDetail)
         : base(pMSG, pDetail)
        {
        }

        public override XExceptionType Type
        {
            get
            {
                return XExceptionType.Warning;
            }
        }
    }
}
