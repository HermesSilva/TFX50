using System;

using TFX.Core.Exceptions;

namespace TFX.Core.Model
{
    public class XEndPointMessage
    {
        public static XEndPointMessage Erro(Exception pEx)
        {
            var msg = new XEndPointMessage();
            msg.Status = 0;
            var type = XUtils.GetValue(pEx, "Type");
            if (type is XExceptionType extype)
                msg.Type = extype;
            msg.Message = pEx.Message.SafeBreak(Environment.NewLine);
#if DEBUG
            msg.Details = XUtils.GetExceptionDetails(pEx).SafeBreak(Environment.NewLine);
#endif
            return msg;
        }

        public static XEndPointMessage Ok
        {
            get
            {
                var msg = new XEndPointMessage();
                msg.Status = 1;
                msg.Message = ["Sucesso"];
                return msg;
            }
        }

        public XExceptionType Type
        {
            get; set;
        }
        public string[] Message
        {
            get; set;
        }
        public int Status
        {
            get; set;
        }
#if DEBUG
        public string[] Details
        {
            get; set;
        }
#endif
    }
    public class XEndPointData
    {
    }
    public class XServiceDataTuple : XDataTuple
    {
        public Boolean IsReadOnly
        {
            get; set;
        }

        public Boolean IsSelected
        {
            get; set;
        }

        public Boolean IsChecked
        {
            get; set;
        }
		public virtual void Initialize()
		{
		}
    }
}
