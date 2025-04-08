using System;

namespace TFX.Core.IDs.Model
{
    public record XUserSession
    {
        public Guid SessionID
        {
            get; set;
        }
        public Guid UserID
        {
            get; set;
        }
        public String Login
        {
            get; set;
        }
        public string Token
        {
            get;
            set;
        }
    }
}
