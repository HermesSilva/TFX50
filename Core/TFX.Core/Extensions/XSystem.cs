using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;

using Microsoft.AspNetCore.Http;

using Newtonsoft.Json;

using TFX.Core;
using TFX.Core.Exceptions;
using System.Security.Claims;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;
using TFX.Core.Extensions;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Hosting;
using AspNetCore.Scalar;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Mvc;
using TFX.Core.Controllers;

namespace System
{
    public delegate void XEvent();

    public class XAutoResetEvent
    {
        private Int32 _TimeOut;
        private DateTime _Start;
        private Boolean _Set = false;
        private AutoResetEvent _Reset = new AutoResetEvent(false);

        public void Wait(Int32 pTimeOut)
        {
            _TimeOut = pTimeOut;
            _Start = DateTime.Now;
            while (_TimeOut > Elapsed)
            {
                _Reset.WaitOne(100);
                if (_Set)
                    return;
            }
            IsTimedOut = true;
        }

        public void RefreshTimeOut(Int32 pTimeOut)
        {
            _TimeOut = Elapsed + pTimeOut;
        }

        public void Set()
        {
            _Set = true;
        }

        private Int32 Elapsed
        {
            get
            {
                return (Int32)(DateTime.Now - _Start).TotalMilliseconds;
            }
        }

        public Boolean IsTimedOut
        {
            get;
            private set;
        }
    }

    public class XWait
    {
        public XWait(String pName, Boolean pRaiseTrouOnTimeout = true)
        {
            _RaiseTrouOnTimeout = pRaiseTrouOnTimeout;
            _Name = pName;
        }

        private XAutoResetEvent _WaitEvent = new XAutoResetEvent();
        private Boolean _RaiseTrouOnTimeout;
        private String _Name;

        public void Wait(Int32 pTimeOut)
        {
            _WaitEvent.Wait(pTimeOut);
            if (_RaiseTrouOnTimeout && _WaitEvent.IsTimedOut)
                throw new XError($"Aconteceu time out durante [{_Name}]");
        }

        public void Set()
        {
            _WaitEvent.Set();
        }

        public void RefreshTimeOut(Int32 pTimeOut)
        {
            _WaitEvent.RefreshTimeOut(pTimeOut);
        }
    }

    public class XUniqueComparer<T> : IEqualityComparer<T> where T : IEquatable<T>
    {
        public Boolean Equals(T pLeft, T pRight)
        {
            return pLeft.Equals(pRight);
        }

        public Int32 GetHashCode(T pValue)
        {
            return pValue.GetHashCode();
        }
    }

    public struct XTimeSpan
    {
        public XTimeSpan(Int32 years, Int32 months, Int32 days, Int32 hours, Int32 minutes, Int32 seconds, Int32 milliseconds)
        {
            Years = years;
            Months = months;
            Days = days;
            Hours = hours;
            Minutes = minutes;
            Seconds = seconds;
            Milliseconds = milliseconds;
        }

        public String LongForm
        {
            get
            {
                String ret = "";
                if (Years >= 1)
                {
                    ret = Years.ToString();
                    if (Years > 1)
                        ret += " anos";
                    else
                        ret += " ano";
                }
                if (Months >= 1)
                {
                    if (ret.IsFull())
                    {
                        if (Days > 1)
                            ret += ", ";
                        else
                            ret += " e ";
                    }
                    ret += Months.ToString();
                    if (Months > 1)
                        ret += " meses";
                    else
                        ret += " mês";
                }
                if (Days >= 1)
                {
                    if (ret.IsFull())
                        ret += " e ";
                    ret += Days.ToString();
                    if (Days > 1)
                        ret += " dias";
                    else
                        ret += " dia";
                }
                if (Months == 0 && Days == 0 && Years > 1)
                    ret += " (aniversário)";
                return ret;
            }
        }

        public Int32 Years
        {
            get;
        }

        public Int32 Months
        {
            get;
        }

        public Int32 Days
        {
            get;
        }

        public Int32 Hours
        {
            get;
        }

        public Int32 Minutes
        {
            get;
        }

        public Int32 Seconds
        {
            get;
        }

        public Int32 Milliseconds
        {
            get;
        }
    }

    public static class X
    {
        private static MethodInfo _Mi = null;
        private static Random _Random = new Random();

        public static void Deserialize(this byte[] pData, object pObject)
        {
            Deserialize(pObject, pData);
        }

        public static void Deserialize(object pObject, byte[] pData)
        {
            using (MemoryStream ms = new MemoryStream(pData))
            using (TextReader sr = new StreamReader(ms))
            using (JsonTextReader _Reader = new JsonTextReader(sr))
            {
                JsonSerializer js = new JsonSerializer();
                js.Populate(_Reader, pObject);
            }
        }

        private enum xPhase
        {
            Years, Months, Days, Done
        }

        private static Boolean IsWordChar(Char pChar, String pAceptAsChar) => Char.IsLetterOrDigit(pChar) || pAceptAsChar.Contains(pChar);

        public static Boolean TryGetItem<T>(this List<T> pList, Func<T, Boolean> pWhere, out T pItem)
        {
            pItem = pList.FirstOrDefault(i => pWhere(i));
            return pItem != null;
        }

        public static String ReplaceWholeWordX(this String pText, String pFind, String pReplace, String pAceptAsChar = "_")
        {
            StringBuilder sb = null;
            Int32 p = 0;
            Int32 j = 0;
            while (j < pText.Length && (j = pText.IndexOf(pFind, j, StringComparison.Ordinal)) >= 0)
            {
                if ((j == 0 || !IsWordChar(pText[j - 1], pAceptAsChar)) && (j + pFind.Length == pText.Length || !IsWordChar(pText[j + pFind.Length], pAceptAsChar)))
                {
                    if (sb == null)
                        sb = new StringBuilder();
                    sb.Append(pText, p, j - p);
                    sb.Append(pReplace);
                    j += pFind.Length;
                    p = j;
                }
                else
                    j++;
            }
            if (sb == null)
                return pText;
            sb.Append(pText, p, pText.Length - p);
            return sb.ToString();
        }

        public static List<TResult> SelectManyEx<TSource, TResult>(this IEnumerable<TSource> pSource, Func<TSource, IEnumerable<TResult>> pSelector)
        {
            List<TResult> list = new List<TResult>();
            if (pSource == null || pSelector == null)
                return list;
            foreach (var item in pSource)
            {
                var ret = pSelector(item);
                if (ret != null)
                    list.AddRange(ret);
            }
            return list;
        }

        public static TOut FirstOrDefault<TOut>(this IEnumerable pSource, Func<TOut, Boolean> pPredicate = null)
        {
            foreach (Object vlr in pSource)
                if (vlr is TOut && (pPredicate == null || pPredicate((TOut)vlr)))
                    return (TOut)vlr;
            return default(TOut);
        }

        public static StringBuilder AppendLineEx(this StringBuilder pBuilder)
        {
            return pBuilder.Append(XEnvironment.NewLine);
        }

        public static StringBuilder AppendLineEx(this StringBuilder pBuilder, String pValue)
        {
            pBuilder.Append(pValue);
            return pBuilder.Append(XEnvironment.NewLine);
        }

        public static String SafeReplaceInsensitive(this String pThis, String pSearch, String pReplacement)
        {
            if (String.IsNullOrEmpty(pSearch) || String.IsNullOrEmpty(pSearch))
                return pThis;
            return Regex.Replace(pThis, Regex.Escape(pSearch), pReplacement.Replace("$", "$$"), RegexOptions.IgnoreCase);
        }

        public static Boolean SafeCompareInsensitive(this String pThis, String pValue)
        {
            if (pThis.IsEmpty() && pValue.IsEmpty())
                return true;
            if (pThis.IsEmpty() || pValue.IsEmpty())
                return false;
            return String.Compare(pThis, pValue, StringComparison.OrdinalIgnoreCase) == 0;
        }

        public static XTimeSpan Compare(this DateTime pDate1, DateTime pDate2)
        {
            if (pDate2 < pDate1)
            {
                DateTime sub = pDate1;
                pDate1 = pDate2;
                pDate2 = sub;
            }

            DateTime current = pDate1;
            Int32 years = 0;
            Int32 months = 0;
            Int32 days = 0;

            xPhase phase = xPhase.Years;
            XTimeSpan span = new XTimeSpan();
            Int32 officialDay = current.Day;

            while (phase != xPhase.Done)
            {
                switch (phase)
                {
                    case xPhase.Years:
                        if (current.AddYears(years + 1) > pDate2)
                        {
                            phase = xPhase.Months;
                            current = current.AddYears(years);
                        }
                        else
                        {
                            years++;
                        }
                        break;

                    case xPhase.Months:
                        if (current.AddMonths(months + 1) > pDate2)
                        {
                            phase = xPhase.Days;
                            current = current.AddMonths(months);
                            if (current.Day < officialDay && officialDay <= DateTime.DaysInMonth(current.Year, current.Month))
                                current = current.AddDays(officialDay - current.Day);
                        }
                        else
                        {
                            months++;
                        }
                        break;

                    case xPhase.Days:
                        if (current.AddDays(days + 1) > pDate2)
                        {
                            current = current.AddDays(days);
                            TimeSpan timespan = pDate2 - current;
                            span = new XTimeSpan(years, months, days, timespan.Hours, timespan.Minutes, timespan.Seconds, timespan.Milliseconds);
                            phase = xPhase.Done;
                        }
                        else
                        {
                            days++;
                        }
                        break;
                }
            }
            return span;
        }

        public static T[] CheckParams<T>(this T[] pArray)
        {
            if (pArray.IsEmpty() || pArray.SafeLength() > 1)
                return pArray;
            if (pArray.SafeLength() == 1 && !(pArray[0] is String) && (pArray[0] is IEnumerable))
                return ((IEnumerable)pArray[0]).Cast<T>().ToArray();
            return pArray;
        }

        public static Type GetType(this Type pType, String pName)
        {
            BindingFlags bd = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.FlattenHierarchy;
            PropertyInfo targetpf = pType.GetProperty(pName, bd);
            if (targetpf == null)
            {
                FieldInfo target = pType.GetField(pName, bd);
                if (target == null)
                    return null;
                return target.FieldType;
            }
            if (targetpf != null)
                return targetpf.PropertyType;
            return null;
        }

        public static DateTime StartOfWeek(this DateTime pDate)
        {
            return pDate.AddDays(-(Int32)pDate.DayOfWeek);
        }

        public static void Add<T>(this List<T> pThis, params T[] pValues)
        {
            if (pValues.IsEmpty() || pThis == null)
                return;
            foreach (T item in pValues)
                pThis.Add(item);
        }

        public static DateTime NearestDate(this DateTime pDateTime, Int32 pDay)
        {
            return new DateTime(pDateTime.Year, pDateTime.Month, Math.Min(DateTime.DaysInMonth(pDateTime.Year, pDateTime.Month), pDay),
                pDateTime.Hour, pDateTime.Minute, pDateTime.Second);
        }

        public static Boolean In<T>(this T pThis, params T[] pValue)
        {
            return !pValue.IsEmpty() && pValue.Any(v => Object.Equals(v, pThis));
        }

        public static DateTime LastDayPreviousMonth(this DateTime pThis)
        {
            DateTime d = pThis.AddMonths(-1);
            return new DateTime(d.Year, d.Month, DateTime.DaysInMonth(d.Year, d.Month));
        }

        public static Int32 TotalHours(this DateTime pThis)
        {
            TimeSpan ts = new TimeSpan(pThis.Ticks);
            return (Int32)ts.TotalHours;
        }

        public static T As<T>(this Object pValue)
        {
            return (T)pValue;
        }

        public static String FromCamelCase(this String pString)
        {
            if (pString == null)
                throw new XError("Null is not allowed for Utils.FromCamelCase");

            StringBuilder sb = new StringBuilder(pString.Length + 10);
            Boolean first = true;
            Char lastChar = '\0';

            foreach (Char ch in pString)
            {
                if (!first && (Char.IsUpper(ch) || Char.IsDigit(ch)) && !Char.IsDigit(lastChar))
                    sb.Append(' ');

                sb.Append(ch);
                first = false;
                lastChar = ch;
            }
            return sb.ToString();
        }

        public static String[] SafeSplit(this StringBuilder pValue)
        {
            return pValue.AsString().SafeSplit(XEnvironment.NewLine);
        }

        public static String[] SafeSplit(this String pValue, String pSeparator)
        {
            return pValue.IsEmpty() ? new String[0] : pValue.Split(new String[] { pSeparator }, StringSplitOptions.RemoveEmptyEntries);
        }

        //public static Int32 Count(this IEnumerable pValue)
        //{
        //    return pValue == null ? 0 : Enumerable.Count(pValue.Cast<Object>());
        //}

        public static T Random<T>(this T[] pValues)
        {
            Int32 idx = _Random.Next(pValues.Length);
            return pValues[idx];
        }

        public static String AsString(this Boolean pValue)
        {
            return Convert.ToString(pValue).ToLower();
        }

        public static String AsString(this Enum pValue, Boolean pToLower = false)
        {
            String result = Convert.ToString(pValue);
            if (pToLower)
                result = result.ToLower();
            return result;
        }

        public static String AlphaNumber(this String pValue)
        {
            if (String.IsNullOrEmpty(pValue))
                return null;
            StringBuilder sb = new StringBuilder();
            pValue = pValue.RemoveAccent().SafeUpper();
            foreach (Char ch in pValue)
                if ((ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'Z'))
                    sb.Append(ch);
            return sb.ToString();
        }

        public static String GetEnumMemberAttrValue(this Enum pValue)
        {
            MemberInfo[] memInfo = pValue.GetType().GetMember(pValue.ToString());
            EnumMemberAttribute attr = memInfo[0].GetCustomAttributes(false).OfType<EnumMemberAttribute>().FirstOrDefault();
            return attr?.Value;
        }

        public static String AsCode(this Guid pValue, String pPrefix = null)
        {
            return $"{pPrefix}{pValue.AsString().AlphaNumber()}";
        }

        public static String AsString(this Object pValue, String pFormat = null)
        {
            if (pValue == null || pValue is DBNull)
                return null;
            if (pValue is Guid)
                return ((Guid)pValue).AsString();
            //switch (pValue)
            //{
            //    case Guid _:
            //        return ((Guid)pValue).AsString();

            //    case null:
            //    case DBNull _:
            //        return null;
            //}

            return pFormat.IsEmpty() ? pValue.ToString() : String.Format("{0:" + pFormat + "}", pValue);
        }

        public static void ForEach<TSource>(this IEnumerable<TSource> pSource, Action<TSource> pAction)
        {
            foreach (TSource source in pSource)
                pAction(source);
        }

        public static String ToName(this String pThis)
        {
            if (pThis.IsEmpty())
                return "NoName";
            String nm = pThis.RemoveAccent().CleanString(' ');
            return nm.Replace(' ', '_');
        }

        public static T GetSafeValue<T>(this T[] pThis, Int32 pIndex)
        {
            return (pIndex < 0 || pIndex + 1 >= pThis.Length) ? default(T) : pThis[pIndex];
        }

        public static String AsString(this Object pThis)
        {
            return pThis?.ToString();
        }

        public static String AsString(this Guid pThis)
        {
            return pThis.ToString().SafeUpper();
        }

        public static String RemoveAccent(this String pValue)
        {
            if (pValue.IsEmpty())
                return "";
            Char[] ret = pValue.ToCharArray();
            for (Int32 i = 0; i < ret.Length; i++)
                switch (ret[i])
                {
                    case 'á':
                    case 'à':
                    case 'ã':
                    case 'â':
                    case 'ä':
                        ret[i] = 'a';
                        break;

                    case 'À':
                    case 'Â':
                    case 'Ã':
                    case 'Á':
                    case 'Ä':
                        ret[i] = 'A';
                        break;

                    case 'é':
                    case 'ê':
                    case 'ë':
                    case 'è':
                        ret[i] = 'e';
                        break;

                    case 'É':
                    case 'Ê':
                    case 'Ë':
                    case 'È':
                        ret[i] = 'E';
                        break;

                    case 'í':
                    case 'ì':
                    case 'ï':
                    case 'î':
                        ret[i] = 'i';
                        break;

                    case 'Í':
                    case 'Ì':
                    case 'Ï':
                    case 'Î':
                        ret[i] = 'I';
                        break;

                    case 'ó':
                    case 'ô':
                    case 'õ':
                    case 'ö':
                    case 'ò':
                        ret[i] = 'o';
                        break;

                    case 'Ó':
                    case 'Ô':
                    case 'Õ':
                    case 'Ö':
                    case 'Ò':
                        ret[i] = 'O';
                        break;

                    case 'ú':
                    case 'ü':
                    case 'ù':
                    case 'û':
                        ret[i] = 'u';
                        break;

                    case 'Ú':
                    case 'Ü':
                    case 'Ù':
                    case 'Û':
                        ret[i] = 'U';
                        break;

                    case 'ç':
                        ret[i] = 'c';
                        break;

                    case 'Ç':
                        ret[i] = 'C';
                        break;
                }
            return new String(ret);
        }

        public static String OnlyNumbers(this String pValue)
        {
            if (String.IsNullOrEmpty(pValue))
                return null;
            StringBuilder sb = new StringBuilder();
            foreach (Char ch in pValue)
                if (ch >= '0' && ch <= '9')
                    sb.Append(ch);
            return sb.ToString();
        }

        public static String CleanString(this String pValue, Char? pIgnore = null)
        {
            if (String.IsNullOrEmpty(pValue))
                return null;
            StringBuilder sb = new StringBuilder();
            if (pValue[0] >= '0' && pValue[0] <= '9')
                pValue = "_" + pValue;
            foreach (Char ch in pValue)
                if (ch == '_' || (ch >= '0' && ch <= '9') || (ch >= 'A' && ch <= 'Z') || (ch >= 'a' && ch <= 'z') || !pIgnore.HasValue || pIgnore.Value == ch)
                    sb.Append(ch);
            return sb.ToString();
        }

        public static String ToHighLevelText(this TimeSpan pTimeSpan, Boolean pLegend = false)
        {
            String ret = pTimeSpan.TotalDays > 1 ? pTimeSpan.ToString(@"dd\:hh\:mm\:ss\:fff") : (pTimeSpan.TotalHours > 1 ? pTimeSpan.ToString(@"hh\:mm\:ss\:fff") : (pTimeSpan.TotalMinutes > 1 ? pTimeSpan.ToString(@"mm\:ss\:fff") : pTimeSpan.ToString(@"ss\:fff")));
            if (pLegend)
                ret += String.Format(" ({0})", pTimeSpan.TotalDays > 1 ? @"dd:hh:mm:ss:fff" : (pTimeSpan.TotalHours > 1 ? @"hh:mm:ss:fff" : (pTimeSpan.TotalMinutes > 1 ? @"mm:ss:fff" : @"ss:fff")));
            return ret;
        }

        public static Boolean IsEqual<TSource>(this IEnumerable pSource, IEnumerable pOther, Func<TSource, TSource, Boolean> pPredicate) where TSource : class
        {
            IEnumerator s = pSource.GetEnumerator();
            IEnumerator o = pOther.GetEnumerator();
            while (s.MoveNext())
            {
                if (!o.MoveNext())
                    return false;
                TSource source1 = o.Current as TSource;
                TSource source = s.Current as TSource;
                if (!(source != null && source1 != null && pPredicate(source, source1)))
                    return false;
            }

            while (o.MoveNext())
            {
                if (!s.MoveNext())
                    return false;
                TSource source1 = o.Current as TSource;
                TSource source = s.Current as TSource;
                if (!(source != null && source1 != null && pPredicate(source, source1)))
                    return false;
            }
            return true;
        }

        public static Boolean Exist<T>(this IEnumerable<T> pThis, T pValue)
        {
            return pThis != null && pThis.Any(f => Object.Equals(f, pValue));
        }

        public static String Display(this Double pValue, Int32 pDec = 4)
        {
            return pDec > 0 ? pValue.ToString("#,##0." + new String('0', pDec)) : pValue.ToString("#,##0");
        }

        public static String Display(this Decimal pValue, Int32 pDec = 4)
        {
            return pDec > 0 ? pValue.ToString("#,##0." + new String('0', pDec)) : pValue.ToString("#,##0");
        }

        public static String Display(this Int16 pValue)
        {
            return pValue.ToString("#,##0");
        }

        public static String Display(this Int64 pValue)
        {
            return pValue.ToString("#,##0");
        }

        public static String Display(this Int32 pValue)
        {
            return pValue.ToString("#,##0");
        }

        public static String[] SafeBreak(this String pThis, String pTag, StringSplitOptions pOtion)
        {
            if (pThis.IsEmpty())
                return new String[0];
            String[] strs = pThis.Split(new String[] { pTag }, pOtion);
            return strs;
        }

        public static String[] SafeBreak(this String pThis, String pTag)
        {
            if (pThis.IsEmpty())
                return new String[0];
            String[] strs = pThis.Split(new String[] { pTag }, StringSplitOptions.RemoveEmptyEntries);
            return strs;
        }

        public static String[] SafeBreak(this String pThis, Char pTag)
        {
            if (pThis.IsEmpty())
                return new String[0];
            String[] strs = pThis.Split(new Char[] { pTag }, StringSplitOptions.RemoveEmptyEntries);
            return strs;
        }

        public static Object CreateInstance(this Type pType)
        {
            Type[] noparams = { };
            return CreateInstance(pType, noparams, noparams);
        }

        public static Type GetNestedType<TType>(this Type pType)
        {
            Type[] types = pType.GetNestedTypes(BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);
            return types.FirstOrDefault(tp => tp.Implemnts(typeof(TType)));
        }

        public static T CreateInstanceNP<T>(this Type pType)
        {
            return (T)Activator.CreateInstance(pType, true);
        }

        public static T CreateInstance<T>(this Type pType)
        {
            return (T)Activator.CreateInstance(pType);
        }

        public static T CreateInstance<T>(this Type pType, Type[] pParamTypes, Object[] pParamValues)
        {
            return (T)CreateInstance(pType, pParamTypes, pParamValues);
        }

        public static T CreateInstance<T>(this Type pType, params Object[] pParams)
        {
            return (T)CreateInstance(pType, pParams);
        }

        public static Object CreateInstance(this Type pType, Type[] pParamTypes, Object[] pParamValues)
        {
            ConstructorInfo cti = pType.GetConstructors().FirstOrDefault(c => c.GetParameters().Count() == pParamValues.Length && c.GetParameters().All(p => pParamTypes.Any(px => p.ParameterType.Implemnts(px))));
            return cti.Invoke(pParamValues);
        }

        public static Object CreateInstance(this Type pType, Object[] pParamValues)
        {
            return Activator.CreateInstance(pType, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance, null, pParamValues, null);
        }

        public static Object DefaultValue(this Type pThis)
        {
            return GetDefault(pThis);
        }

        private static ConcurrentDictionary<Type, Object> _Defs = new ConcurrentDictionary<Type, Object>();

        public static Object GetDefault(this Type pThis)
        {
            if (pThis.Name == "Byte[]")
                return new Byte[0];
            if (_Defs.ContainsKey(pThis))
                return _Defs[pThis];
            if (_Mi == null)
                _Mi = typeof(X).GetMethod("GetDefaultImp", BindingFlags.Public | BindingFlags.Static);
            Object def = _Mi.MakeGenericMethod(pThis).Invoke(null, new Type[0]);
            _Defs.TryAdd(pThis, def);
            return def;
        }

        public static Int32 FirstIndexOf<TSource>(this IEnumerable<TSource> pSource, Func<TSource, Boolean> pPredicate)
        {
            Int32 idx = -1;
            foreach (TSource item in pSource)
            {
                idx++;
                if (pPredicate(item))
                    return idx;
            }
            return -1;
        }

        public static Int32 FirstIndexOfType<TSource, TTarget>(this IEnumerable<TSource> pSource, Func<TTarget, Boolean> pPredicate) where TTarget : TSource
        {
            Int32 idx = -1;
            foreach (Object item in pSource)
            {
                if (!(item is TTarget))
                    continue;
                idx++;
                if (pPredicate((TTarget)item))
                    return idx;
            }
            return -1;
        }

        public static TSource FirstOr<TSource>(this IEnumerable<TSource> pSource, Object pValue = null)
        {
            if (pSource == null)
                return (TSource)(pValue ?? default(TSource));
            else
            {
                TSource ret = pSource.FirstOrDefault();
                if (ret == null)
                    return (TSource)(pValue ?? default(TSource));
                return ret;
            }
        }

        public static String AttributeValue(this XmlNode pElement, String pTagName, String pAttributeName)
        {
            XmlElement elm = pElement.FirstOrDefault<XmlElement>(e => e.Name == pTagName && e.HasAttribute(pAttributeName));
            if (elm != null)
                return elm.GetAttribute(pAttributeName);
            return null;
        }

        public static String TextValue(this XmlNode pElement, Func<XmlElement, Boolean> pPredicade)
        {
            XmlElement elm = pElement.FirstOrDefault<XmlElement>(e => pPredicade(e));
            if (elm != null)
                return elm.InnerText;
            return null;
        }

        public static T FirstOrDefault<T>(this XmlNode pElement, Func<T, Boolean> pPredicade)
        {
            foreach (T ndl in pElement.ChildNodes)
            {
                if (ndl is T && pPredicade(ndl))
                    return ndl;
            }
            return default(T);
        }

        public static Object FirstOrNull<TType>(this Array pArray, Func<TType, Boolean> pPredicade)
        {
            for (Int32 i = 0; i < pArray.Length; i++)
            {
                Object vlr = pArray.GetValue(i);
                if (vlr is TType && pPredicade((TType)vlr))
                    return (TType)vlr;
            }
            return null;
        }

        public static T GetDefaultImp<T>()
        {
            return default(T);
        }

        public static String GetFolder(this Type pThis)
        {
            String nspc = pThis.Namespace;
            String asmn = pThis.Assembly.GetName().Name;
            return asmn.Length >= nspc.Length ? String.Empty : nspc.Remove(0, asmn.Length + 1).Replace('.', '/');
        }

        public static Boolean Implemnts<T>(this Type pThis, Boolean pOrIs = true)
        {
            if (pThis == null)
                return false;
            var thi = pThis;
            while (thi != null && thi != typeof(Object))
            {
                if (thi.Implemnts(typeof(T)))
                    return true;
                thi = thi.BaseType;
            }
            return false;
        }

        public static Boolean Implemnts(this Type pThis, Type pIntance, Boolean pOrIs = true)
        {
            if (pOrIs && Object.Equals(pThis, pIntance))
                return true;
            return pThis.IsSubclassOf(pIntance) || pThis.GetInterfaces().Any(t => Object.Equals(t, pIntance));
        }

        public static Boolean Is<T>(this Exception pThis) where T : Exception
        {
            Exception ex = pThis;
            while (ex != null)
            {
                if (ex is T)
                    return true;
                ex = ex.InnerException;
            }
            return false;
        }

        public static Boolean IsEmpty(this Object[] pArray)
        {
            return pArray == null || pArray.Length == 0;
        }

        public static Boolean IsEmpty(this Guid pValue)
        {
            return pValue == Guid.Empty;
        }

        public static Boolean IsFull(this Guid pValue)
        {
            return !pValue.IsEmpty();
        }

        public static String AsTitleCase(this String pValue)
        {
            String[] preps = new[]{"e", "di", "o","os","a","as","um","uns","uma","umas","a","ao","aos","à","às","de","do","dos","da","das",
                                  "dum","duns","duma","dumas","em","no","nos","na","nas","num","nuns","numa","numas","por",
                                  "per","pelo","pelos","pela","pelas"};
            String[] vlrs = pValue.SafeLower().SafeBreak(" ");
            StringBuilder sb = new StringBuilder();

            foreach (String wd in vlrs)
            {
                if (sb.Length > 0)
                    sb.Append(" ");
                if (Array.IndexOf<String>(preps, wd) == -1)
                {
                    sb.Append(Char.ToUpper(wd[0]));
                    sb.Append(wd.Substring(1));
                }
                else
                    sb.Append(wd);
            }
            return sb.ToString();
        }

        public static Boolean IsEmpty(this Array pValue)
        {
            return pValue == null || pValue.Length == 0;
        }

        public static Boolean IsFull(this Array pValue)
        {
            return !pValue.IsEmpty();
        }

        public static String AsQuoted(this String pValue)
        {
            return $"\"{pValue}\"";
        }

        public static Boolean IsEmpty(this String pValue)
        {
            return String.IsNullOrEmpty(pValue) || String.IsNullOrWhiteSpace(pValue);
        }

        public static Boolean IsEmpty(this DateTime? pValue)
        {
            return !pValue.HasValue || pValue.Value.Year <= 1755;
        }

        public static Boolean IsFull(this String pValue)
        {
            return !pValue.IsEmpty();
        }

        public static String SafeCopyLast(this String pValue, params String[] pText)
        {
            if (pText.IsEmpty() || pText.IsEmpty())
                return "";
            String value = pValue.ToLower();
            foreach (String txt in pText)
            {
                if (txt.IsEmpty())
                    continue;
                Int32 idx = value.LastIndexOf(txt.ToLower());
                if (idx != -1)
                    return pValue.Substring(idx, txt.Length);
            }
            return null;
        }

        public static Boolean SafeContains(this String pValue, String pText)
        {
            if (pValue.IsEmpty() || pText.IsEmpty())
                return false;
            return pValue.ToLower().Contains(pText.ToLower());
        }

        public static Boolean SafeContains(this String pValue, Char pChar)
        {
            return pValue.IsEmpty() || pValue.Any(c => c == pChar);
        }

        public static Boolean IsInner(this String pThis, String pValue)
        {
            if (pValue == null && pThis == null)
                return true;
            if (pValue == null || pThis == null)
                return false;
            return String.CompareOrdinal(pThis.ToLower(), pValue.ToLower()) == 0 || pThis.ToLower().Contains(pValue.ToLower());
        }

        public static String SafeLower(this String pThis)
        {
            return pThis.IsEmpty() ? pThis : pThis.ToLower();
        }

        public static void SafeDispose(this IDisposable pDisposable)
        {
            pDisposable?.Dispose();
        }

        public static Int32 SafeLength(this Array pValue)
        {
            return pValue.IsEmpty() ? 0 : pValue.Length;
        }

        public static IEnumerable<T> SafeEnumerable<T>(this T[] pThis)
        {
            if (pThis.IsEmpty())
                yield break;
            foreach (T item in pThis)
                yield return item;
        }

        public static Boolean SafeAny<T>(this IEnumerable<T> pThis, Func<T, Boolean> pFunc)
        {
            if (pThis == null || pFunc == null)
                return false;
            return pThis.Any(v => pFunc(v));
        }

        public static Boolean SafeAny<T>(this IEnumerable pThis, Func<T, Boolean> pFunc)
        {
            if (pThis == null || pFunc == null)
                return false;
            foreach (T tpl in pThis)
            {
                if (pFunc(tpl))
                    return true;
            }
            return false;
        }

        public static TResult SafeMax<TSource, TResult>(this IEnumerable<TSource> pThis, Func<TSource, TResult> pSelector)
        {
            if (pThis == null || pSelector == null || pThis.Count() == 0)
                return default(TResult);
            return pThis.Max(pSelector);
        }

        public static Boolean SafeBoxedEquals(this Object pValue1, Object pValue2)
        {
            if (Object.Equals(pValue1, pValue2))
                return true;
            if (pValue2 == null || pValue1 == null)
                return false;
            return pValue1.ToString() == pValue2.ToString();
        }

        public static Boolean SafeContains<T>(this T[] pArray, params T[] pValues)
        {
            if (pArray.IsEmpty())
                return false;
            foreach (T elm in pValues)
                if (Array.IndexOf<T>(pArray, elm) != -1)
                    return true;
            return false;
        }

        public static Int32 SafeLength(this String pValue)
        {
            return pValue?.Length ?? 0;
        }

        public static Boolean SafeStartsWith(this String pThis, String pValue)
        {
            return pThis?.StartsWith(pValue) == true;
        }

        public static String SafeTrim(this String pValue)
        {
            return pValue.IsEmpty() ? null : pValue.Trim();
        }

        public static String SafePad(this String pValue, Int32 pSize)
        {
            if (pValue.IsEmpty())
                return null;
            return pValue.Length > pSize ? pValue.Substring(0, pSize) : pValue;
        }

        public static String SafePadLef(this String pValue, Int32 pSize)
        {
            if (pValue.IsEmpty())
                return null;
            String nstr = new String(' ', pSize) + pValue;
            return nstr.Substring(nstr.Length - pSize);
        }

        public static String SafePadRight(this String pValue, Int32 pSize)
        {
            if (pValue.IsEmpty())
                return null;
            if (pValue.Length > pSize)
                return pValue.Substring(0, pSize);
            return pValue + new String(' ', pSize - pValue.Length);
        }

        public static String SafePadLeft(this String pValue, Int32 pSize, Char pChar)
        {
            String nstr = new String(pChar, pSize) + pValue;
            return nstr.Substring(nstr.Length - pSize);
        }

        public static String SafeReplace(this String pValue, Char pOld, Char pNew)
        {
            return pValue.IsEmpty() ? pValue : pValue.Replace(pOld, pNew);
        }

        public static String SafeReplaceLast(this String pSource, String pFind, String pReplace)
        {
            Int32 place = pSource.LastIndexOf(pFind);

            if (place == -1)
                return pSource;

            String result = pSource.Remove(place, pFind.Length).Insert(place, pReplace);
            return result;
        }

        public static String SafeReplace(this String pValue, String pOld, String pNew)
        {
            return pValue.IsEmpty() ? pValue : pValue.Replace(pOld, pNew);
        }

        public static String SafeReplaceNL(this String pValue, String pNewValue)
        {
            return pValue.IsEmpty() ? pValue : pValue.Replace(XEnvironment.NewLine, pNewValue).Replace(Environment.NewLine, pNewValue);
        }

        public static Boolean Swap<T>(this IList<T> pList, T pFirst, Int32 pToPosition)
        {
            T second = pList[pToPosition];
            return pList.Swap<T>(pFirst, second);
        }

        public static Boolean Swap<T>(this IList<T> pList, T pElement, Boolean pUp)
        {
            Int32 idx = pList.IndexOf(pElement);
            Int32 newidx = idx + (pUp ? -1 : 1);
            if ((pUp && newidx < 0) || (!pUp && idx >= pList.Count - 1))
                return false;
            return pList.Swap(pElement, newidx);
        }

        public static Boolean Swap<T>(this IList<T> pList, T pFirst, T pSecond)
        {
            Int32 idxst = pList.IndexOf(pFirst);
            Int32 idxnd = pList.IndexOf(pSecond);
            if (idxnd == -1 || idxst == -1)
                return false;
            T tmp = pList[idxst];
            pList[idxst] = pList[idxnd];
            pList[idxnd] = tmp;
            return true;
        }

        public static String ToQuoted(this String pValue, String pQuote = "'")
        {
            return String.Format("{0}{1}{0}", pQuote, pValue);
        }

        public static String SafeUpper(this String pThis)
        {
            return pThis.IsEmpty() ? pThis : pThis.ToUpper();
        }

        public static String GetValueOrEmpty(this String pObject)
        {
            return pObject ?? String.Empty;
        }
    }
}
