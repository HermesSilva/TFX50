using System;

using TFX.Core;
using TFX.Core.Exceptions;
using TFX.Core.Service.Data;

namespace Projecao.Core.ITG.Recebe
{
    public abstract class XClientMessageRule
    {
        protected String[] Fields;
        public Byte[] Data;
        public Byte[] ToDeleteData;
        public Int32 Count = 0;
        protected XILog Log = XLog.GetLogFor<XClientMessageRule>();

        public abstract void Execute(XExecContext pContext);

        protected T GetValue<T>(String[] pData, String pField, T pDefault = default(T))
        {
            String vlr = GetValue(pData, pField);
            if (vlr.IsEmpty())
                return pDefault;
            return XConvert.FromString<T>(vlr);
        }

        protected String GetValue(String[] pData, String pField, String pDefault = "")
        {
            Int32 idx = Array.IndexOf(Fields, pField);
            if (idx == -1)
                throw new XError($"Campo [{pField}] não existe.");
            if (idx > pData.SafeLength() - 1)
                return pDefault;
            String vlr = pData[idx];
            if (vlr.IsEmpty())
                return pDefault;
            return vlr;
        }
    }
}