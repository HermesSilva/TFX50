using System;

namespace TFX.Core.IDs.Model
{
    public record XUser
    {
        public Guid? ID
        {
            get; set;
        }
        public Guid SessionID
        {
            get; set;
        }
        public string Login
        {
            get; set;
        }
    }
}
