using System;
using System.Collections.Generic;
using System.Linq;

namespace TFX.Core
{
    public class XContainerObject : XObject
    {
        public readonly Dictionary<Guid, XObject> Items = new Dictionary<Guid, XObject>();

        public virtual XObject AddItem(XObject pItem)
        {
            Items.Add(pItem.ID, pItem);
            return pItem;
        }

        public Int32 Count<T>()
        {
            return 0;
        }

        public T GetItem<T>(Guid pID) where T : XObject
        {
            return (T)Items[pID];
        }

        public T GetItem<T>(Func<T, Boolean> pPredicate) where T : XObject
        {
            return (T)Items.Values.FirstOrDefault(i => i is T && pPredicate((T)i));
        }

        public virtual IEnumerable<T> GetItems<T>(Func<T, Boolean> pPredicate = null) where T : XObject
        {
            return Items.Values.Where(i => i is T && (pPredicate == null || pPredicate((T)i))).Select(i => (T)i);
        }
    }
}