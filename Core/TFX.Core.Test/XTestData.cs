using System;
using System.Collections;
using System.Collections.Generic;

using TFX.Core.Model;

namespace TFX.Access
{
    public class XTestData<T> : IEnumerable<Object[]> where T : XServiceDataTuple
    {
        protected readonly List<Object[]> Data = new List<Object[]>();

        public IEnumerator<Object[]> GetEnumerator()
        {
            return Data.GetEnumerator();
        }

        public T this[int pIndex] => (T)Data[pIndex][1];


        public IEnumerable<XServiceDataTuple> Tuples()
        {
            if (Data.Count == 0)
                yield break;
            foreach (var item in Data)
                yield return (T)item[1];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
