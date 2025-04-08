
using System;
using System.Collections.Generic;
using System.Linq;

using TFX.Core.Exceptions;
using TFX.Core.Model;

namespace TFX.Core.Data
{
    public class XIndexedDataSet<T> where T : XDataTuple
    {
        public class xIndexField
        {
            public xIndexField(String[] pFields)
            {
                Fields = pFields;
            }

            internal Dictionary<String, T> Keys = new Dictionary<string, T>();
            internal string Name;
            internal Func<T, Boolean> Where;
            internal void Clear()
            {
                Keys.Clear();
            }

            public String[] Fields
            {
                get;
            }

            internal T Find(object[] pValue)
            {
                T tpl = default;
                String key = String.Join("$", pValue.Select(v => v.ToString()));
                if (Keys.TryGetValue(key, out tpl))
                    return tpl;
                return tpl;
            }

            internal void Add(XIndexedDataSet<T> pDataSet)
            {
                pDataSet.Tuples.ForEach(t => Add(t));
            }

            internal void Add(T pTuple)
            {
                if (Where != null && !Where(pTuple))
                    return;

                String values = String.Join("$", Fields.Select(f => pTuple[f].ToString()));
                if (Keys.ContainsKey(values))
                    XConsole.Debug($"Índice [{Name}] contem valor duplicado [{values}]");
                else
                    Keys.Add(values, pTuple);
            }
        }

        public XIndexedDataSet(params string[] pFields)
        {
            Fields = pFields;
            TupleType = typeof(T);
        }

        public readonly List<T> Tuples = new List<T>();
        public Dictionary<String, xIndexField> Indexes = new Dictionary<string, xIndexField>();

        public string[] Fields
        {
            get;
        }
        public int Count => Tuples.Count;

        protected internal Type TupleType;
        public int AddTuples(IEnumerable<T> pTuples)
        {
            pTuples.ForEach(t => AddTuple(t));
            return Count;
        }

        public T AddTuple(T pTuple)
        {
            Tuples.Add(pTuple);
            Indexes.ForEach(i => i.Value.Add(pTuple));
            return pTuple;
        }

        public T FindByIndex(String pIndexName, Object pValue, params Object[] pValues)
        {
            Object[] parm = [pValue];
            if (pValues.IsFull())
                parm = parm.Union(pValues).ToArray();
            return FindByIndex(pIndexName, parm);
        }

        private T FindByIndex(String pIndexName, params Object[] pValues)
        {
            if (!Indexes.TryGetValue(pIndexName, out xIndexField idx))
                throw new XError($"Não existe índice com nome [{pIndexName}].");
            return idx.Find(pValues);
        }

        public void RemoveIndex(String pIndexName = null)
        {
            if (pIndexName == null)
                Indexes.Clear();
            else
                Indexes.Remove(pIndexName);
        }

        public void AddIndex(String pIndexName, Func<T, Boolean> pWhere = null, params string[] pFields)
        {
            if (pFields.IsEmpty())
                pFields = [pIndexName];
            xIndexField idx = new xIndexField(pFields);
            idx.Where = pWhere;
            idx.Name = pIndexName;
            idx.Add(this);
            Indexes.Add(pIndexName, idx);
        }

        public void Clear()
        {
            Tuples.Clear();
            Indexes.Values.ForEach(v => v.Clear());
        }
    }
}
