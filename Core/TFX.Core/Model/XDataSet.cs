using System;
using System.Collections.Generic;

using TFX.Core.Cache;

namespace TFX.Core.Model
{
    public class XDataSet<T> where T : XServiceDataTuple, new()
    {
        public XDataSet()
        {
            Tuples = new List<T>();
        }

        public void Assign(XDataSet<T> pSource)
        {
            Tuples.Clear();
            foreach (var stpl in pSource.Tuples)
            {
                var ttpl = AddTuple();
                ttpl.Assign((XDataTuple)stpl);
            }
        }

        public T AddTuple()
        {
            var tpl = new T();
            tpl.Initialize();
            Tuples.Add(tpl);
            return tpl;
        }

        public List<T> Tuples
        {
            get; set;
        }

        protected virtual T NewTuple()
        {
            return default;
        }

        public virtual Guid ID
        {
            get;
        }

        protected XJobConfig InternalGetConfig(Guid pID)
        {
            return XCacheManager.GetConfig(pID);
        }

    }
}
