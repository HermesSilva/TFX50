using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Linq;

namespace TFX.Core.Model
{
    public enum XTupleState
    {
        Detached = 0,
        Unchanged = 1,
        Deleted = 2,
        Modified = 3,
        Added = 4,
        Insert = 5
    }

    public enum XFieldState
    {
        Empty = 0,
        Unchanged = 1,
        NotEmpty = 2,
        Modified = 3,
    }

    public class XDataTuple
    {

        public Boolean Assign(Object pSource)
        {
            var changed = false;
            if (pSource == null)
                return changed;
            var tgtppts = GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);
            var srcppts = pSource.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);
            foreach (var sppt in srcppts)
            {
                var tppt = tgtppts.FirstOrDefault(p => p.Name == sppt.Name && p.PropertyType.Name == sppt.PropertyType.Name);
                if (tppt == null || !sppt.CanRead || !tppt.CanWrite || tppt.GetIndexParameters().Length > 0)
                    continue;

                var svalue = sppt.GetValue(pSource);
                var tvalue = tppt.GetValue(this);
                if (tppt.PropertyType.IsArray && tppt.PropertyType.GetElementType().Implemnts<XDataTuple>())
                {
                    var asvalue = svalue as object[];
                    if (asvalue.IsEmpty())
                        continue;
                    var cnt = asvalue.Length;
                    var atvalue = Array.CreateInstanceFromArrayType(tppt.PropertyType, cnt);
                    tppt.SetValue(this, atvalue);
                    for (int i = 0; i < cnt; i++)
                    {
                        var otpl = (XDataTuple)asvalue[i];
                        var ntpl = (XDataTuple)tppt.PropertyType.GetElementType().CreateInstance();
                        ntpl.Assign(otpl);
                        atvalue.SetValue(ntpl, i);
                    }
                    continue;
                }
                changed = changed || !object.Equals(svalue, tvalue);               
                tppt.SetValue(this, svalue);
            }
            return changed;
        }

        public XTupleState State
        {
            get; set;
        }
        public Object this[string pField]
        {
            get
            {
                var vlr = XUtils.GetValue(this, pField);
                return vlr;
            }
            set
            {
                XUtils.SetValue(this, pField, value);
            }
        }
    }
}
