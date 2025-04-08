using System;
using System.Collections.Generic;

using TFX.Core.IDs.Model;

namespace TFX.Core.Cache
{
	public class XBaseCache<Key, Data>
	{
		private Dictionary<Key, Data> _Cache = new Dictionary<Key, Data>();

		public int Count
		{
			get
			{
				lock (this)
					return _Cache.Count;
			}
		}

		public Data Add(Key pKey, Data pData)
		{
			lock (this)
				_Cache.TryAdd(pKey, pData);
			return pData;
		}

		public void Wash()
		{
			lock (this)
				_Cache.Clear();
		}

		public void Swap(Dictionary<Key, Data> pData)
		{
			lock (this)
				_Cache = pData;
		}

		public Data Get(Key pKey)
		{
			lock (this)
			{
				_Cache.TryGetValue(pKey, out Data data);
				return data;
			}
		}
	}
}
