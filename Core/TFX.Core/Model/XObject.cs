using System;
using System.Reflection;

namespace TFX.Core
{
    public class XObject
    {
        public XObject()
        {
        }

        protected Int32? PCID;
        public String Description;
        private String _Alias;

        public Boolean SuppressFromDB
        {
            get;
            set;
        }

        public Boolean HasOwner<T>() where T : XObject
        {
            if (Owner is T)
                return true;
            if (Owner == null)
                return false;
            return Owner.HasOwner<T>();
        }

        public virtual Int32 SortedBy
        {
            get;
        }

        public String Alias
        {
            get
            {
                return _Alias ?? Name;
            }
            set
            {
                _Alias = value;
            }
        }

        public virtual XObject Owner
        {
            get;
            set;
        }

        public virtual Guid ID
        {
            get;
            set;
        }

        public virtual String Name
        {
            get;
            set;
        }

        public virtual String Title
        {
            get;
            set;
        }
    }
}