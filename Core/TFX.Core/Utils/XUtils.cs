using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;
using System.Linq;


using Microsoft.Data.SqlClient;

using Newtonsoft.Json;

using TFX.Core.Exceptions;
using TFX.Core.Model;
using TFX.Core;

namespace TFX.Core
{
    public static class XUtils
    {
        public static string SecretHashId => XEnvironment.Read("HASID_SECRET", "9ea96c82-20-8c-b5-d0fc7b736af1");

        private static readonly string SecurityKey = "http://www.2am5ana.c0m";

        public static void ProcessStream<T>(Stream stream, int batchSize, Action<List<T>> pCallback) where T : new()
        {
            using var reader = new StreamReader(stream);
            var batch = new List<T>();
            string line;
            // pula header e linhas em branco
            while ((line = reader.ReadLine()) != null)
            {
                if (string.IsNullOrWhiteSpace(line) || line.StartsWith("Abreviatura@"))
                    continue;
                var parts = line.Split('@');
                var item = new T();
                batch.Add(item);
                if (batch.Count >= batchSize)
                {
                    pCallback(batch);
                    batch = new List<T>();
                }
            }
            // processa remanescente
            if (batch.Count > 0)
                pCallback(batch);
        }
        public static Boolean Assign(Object pTarget, Object pSource)
        {
            var changed = false;
            if (pSource == null)
                return changed;
            var tgtppts = pTarget.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);
            var srcppts = pSource.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);
            foreach (var sppt in srcppts)
            {
                var tppt = tgtppts.FirstOrDefault(p => p.Name == sppt.Name && p.PropertyType.Name == sppt.PropertyType.Name);
                if (tppt == null || !sppt.CanRead || !tppt.CanWrite || tppt.GetIndexParameters().Length > 0)
                    continue;

                var svalue = sppt.GetValue(pSource);
                var tvalue = tppt.GetValue(pTarget);
                if (tppt.PropertyType.IsArray && tppt.PropertyType.GetElementType().Implemnts<XDataTuple>())
                {
                    var asvalue = svalue as object[];
                    if (asvalue.IsEmpty())
                        continue;
                    var cnt = asvalue.Length;
                    var atvalue = Array.CreateInstanceFromArrayType(tppt.PropertyType, cnt);
                    tppt.SetValue(pTarget, atvalue);
                    for (int i = 0; i < cnt; i++)
                    {
                        var otpl = (XDataTuple)asvalue[i];
                        var ntpl = (XDataTuple)tppt.PropertyType.GetElementType().CreateInstance();
                        Assign(otpl, ntpl);
                        atvalue.SetValue(ntpl, i);
                    }
                    continue;
                }
                changed = changed || !object.Equals(svalue, tvalue);
                tppt.SetValue(pTarget, svalue);
            }
            return changed;
        }

        public static StringBuilder GetValues(Object pObject)
        {
            var sb = new StringBuilder();
            if (pObject == null)
                return sb;
            var srcppts = pObject.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Default);
            foreach (var sppt in srcppts)
            {
                if (!sppt.CanRead)
                    continue;

                var svalue = sppt.GetValue(pObject);
                sb.AppendLine($"{sppt.Name}=[{svalue}]");
            }
            return sb;
        }

        private static readonly Dictionary<string, XmlSerializer> SerializersCache = new Dictionary<string, XmlSerializer>();
        public static XmlSerializer CreateXmlSerialize(Object pObject, string pNamespace)
        {
            var key = $"{pObject.GetType().FullName}";
            if (!SerializersCache.TryGetValue(key, out var serializer))
            {
                XmlRootAttribute xmlRoot = new XmlRootAttribute();
                xmlRoot.Namespace = pNamespace;
                serializer = new XmlSerializer(pObject.GetType(), xmlRoot);
            }
            return serializer;
        }

        public static XmlSerializer CreateXmlSerialize<T>(string pRootElementName = null, string pNamespace = null)
        {
            var tp = typeof(T);
            var key = $"{tp.FullName}_{pRootElementName}_{pNamespace}";
            if (!SerializersCache.TryGetValue(key, out var serializer))
            {
                XmlRootAttribute xmlRoot = new XmlRootAttribute(pRootElementName);
                xmlRoot.Namespace = pNamespace;
                serializer = new XmlSerializer(tp, xmlRoot);
            }
            return serializer;
        }

        public static Boolean CheckCPFCNPJ(String pData)
        {
            if (pData.IsEmpty() || pData != pData.OnlyNumbers() || (pData.Length != 11 && pData.Length != 14))
                return false;
            if (pData.Length == 11)
                return CheckCPF(pData);
            return CheckCNPJ(pData);
        }

        public static Boolean CheckCPF(String pCPF)
        {
            if (pCPF.IsEmpty() || pCPF != pCPF.OnlyNumbers() || pCPF.Length != 11)
                return false;
            String dig = CalculateCPF(pCPF.Substring(0, 9));
            return pCPF.EndsWith(dig);
        }

        public static Boolean CheckCNPJ(String pCNPJ)
        {
            if (pCNPJ.IsEmpty() || pCNPJ != pCNPJ.OnlyNumbers() || pCNPJ.Length != 14)
                return false;
            String dig = CalculateCNPJ(pCNPJ.Substring(0, 12));
            return pCNPJ.EndsWith(dig);
        }

        public static String CalculateCNPJ(String pCNPJ)
        {
            if (pCNPJ.IsEmpty() || pCNPJ != pCNPJ.OnlyNumbers() || pCNPJ.Length != 12)
                return "";
            Int32[] multiplicador1 = new Int32[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            Int32[] multiplicador2 = new Int32[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            Int32 soma;
            Int32 rst;
            String dig;
            String str = pCNPJ;
            soma = 0;
            for (int i = 0; i < 12; i++)
                soma += Int32.Parse(str[i].ToString()) * multiplicador1[i];
            rst = soma % 11;
            if (rst < 2)
                rst = 0;
            else
                rst = 11 - rst;
            dig = rst.ToString();
            str = str + dig;
            soma = 0;
            for (int i = 0; i < 13; i++)
                soma += Int32.Parse(str[i].ToString()) * multiplicador2[i];
            rst = soma % 11;
            if (rst < 2)
                rst = 0;
            else
                rst = 11 - rst;
            dig = dig + rst.ToString();
            return dig;
        }

        public static Int32 DACItau(String pNumero, String pAgencia = null, String pConta = null)
        {
            String str = pAgencia + pConta + pNumero;
            if (!String.IsNullOrEmpty(pAgencia) && str?.Length != 20)
                return -1;
            Int32 soma = 0;
            for (int i = 0; i < str.Length; i++)
            {
                Int32 cr = Int32.Parse(str[i].ToString());
                if (i % 2 != 0)
                {
                    cr = cr * 2;
                    if (cr > 10)
                        cr = Int32.Parse(cr.ToString()[0].ToString()) + Int32.Parse(cr.ToString()[1].ToString());
                }
                soma += cr;
            }
            return 10 - (soma % 7);
        }

        public static String CalculateCPF(String pCPF)
        {
            if (pCPF.IsEmpty() || pCPF != pCPF.OnlyNumbers() || pCPF.Length != 9)
                return "";
            Int32[] multiplicador1 = new Int32[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            Int32[] multiplicador2 = new Int32[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            String dig;
            Int32 soma;
            Int32 rst;
            String str = pCPF;
            soma = 0;
            for (int i = 0; i < 9; i++)
                soma += Int32.Parse(str[i].ToString()) * multiplicador1[i];
            rst = soma % 11;
            if (rst < 2)
                rst = 0;
            else
                rst = 11 - rst;
            dig = rst.ToString();
            str = str + dig;
            soma = 0;
            for (int i = 0; i < 10; i++)
                soma += Int32.Parse(str[i].ToString()) * multiplicador2[i];
            rst = soma % 11;
            if (rst < 2)
                rst = 0;
            else
                rst = 11 - rst;
            dig = dig + rst.ToString();
            return dig;
        }

        public static string GetCPFCNPJ(X509Certificate2 pCert)
        {
            string[] masks = [@"\d{3}\.\d{3}\.\d{3}-\d{2}", @"\d{2}\.\d{3}\.\d{3}/\d{4}-\d{2}", @"\b\d{3}\D?\d{3}\D?\d{3}\D?\d{2}\b", @"\b\d{2}\D?\d{3}\D?\d{3}\D?\d{4}\D?\d{2}\b"];
            string subject = pCert.Subject;
            foreach (var mask in masks)
            {
                Match doc = Regex.Match(subject, mask);
                if (doc.Success)
                    return doc.Value;
            }

            return string.Empty;
        }

        public static string GetCommonName(X509Certificate2 pCert)
        {
            var subject = pCert.Subject;
            foreach (var part in subject.SafeSplit(","))
                if (part.SafeTrim().StartsWith("CN="))
                    return part.SafeTrim().Substring(3).SafeBreak(":")[0];
            return string.Empty;
        }

        public static void Execute(out Process pProcess, String pExe, String pArgs, String pWorkingDirectory = null, DataReceivedEventHandler pConsole = null, DataReceivedEventHandler pError = null, EventHandler pExited = null)
        {
            ProcessStartInfo psi = new ProcessStartInfo(pExe);
            psi.ErrorDialog = false;
            psi.UseShellExecute = false;
            psi.Arguments = pArgs;
            psi.WorkingDirectory = pWorkingDirectory;

            pProcess = new Process();
            pProcess.StartInfo = psi;
            if (pConsole != null)
            {
                //psi.CreateNoWindow = true;
                psi.RedirectStandardOutput = true;
                pProcess.OutputDataReceived += pConsole;
            }
            if (pError != null)
            {
                // psi.CreateNoWindow = true;
                psi.RedirectStandardError = true;
                pProcess.ErrorDataReceived += pError;
                pProcess.BeginErrorReadLine();
            }
            pProcess.Start();
            pProcess.BeginOutputReadLine();
            if (pExited != null)
                pProcess.Exited += pExited;
            //pProcess.WaitForExit();
        }

        public static Object GetValue(Object pObject, String pProperty)
        {
            return GetValue<Object>(pObject, pProperty);
        }

        public static Boolean SetValue(Object pObject, String pProperty, Object pValue, Boolean pAllHierarchy = true)
        {
            return SetValue(pObject, pProperty, pValue, pAllHierarchy, null);
        }

        public static Boolean SetValue(Object pObject, String pProperty, Object pValue, Boolean pAllHierarchy, Object[] pIndex)
        {
            if (pObject == null || pProperty.IsEmpty())
                return false;
            Type tp = pObject.GetType();
            Type vletp = null;
            if (pValue != null)
                vletp = pValue.GetType();
            BindingFlags bd = pAllHierarchy ? BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance : BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;
            PropertyInfo targetpf = tp.GetProperty(pProperty, bd);
            if (targetpf == null)
            {
                FieldInfo target = tp.GetField(pProperty, bd);
                if (target == null)
                    return false;
                if (pValue == null || pValue == DBNull.Value)
                    pValue = tp.GetDefault();
                target.SetValue(pObject, pValue);
                return true;
            }
            if (targetpf.CanWrite)
            {
                targetpf.SetValue(pObject, pValue, pIndex);
                return true;
            }
            return false;
        }

        public static T GetFieldValue<T>(Object pObject, String pProperty, BindingFlags pFlgas = BindingFlags.Default)
        {
            if (pObject == null)
                return default(T);
            FieldInfo pf;
            if (pObject is Type)
                pf = ((Type)pObject).GetField(pProperty, pFlgas);
            else
                pf = pObject.GetType().GetField(pProperty);
            if (pf == null)
                return default(T);
            Object obj = pf.GetValue(pObject);
            if (obj == null)
                return default(T);
            return (T)obj;
        }

        public static T GetValue<T>(Object pObject, String pProperty)
        {
            if (pObject == null)
                return default(T);
            PropertyInfo pf = pObject.GetType().GetProperty(pProperty);
            if (pf == null)
                return GetFieldValue<T>(pObject, pProperty);
            Object obj = pf.GetValue(pObject, null);
            if (obj == null || !(obj is T))
                return default(T);
            return (T)obj;
        }
        public static byte[] GetResource(Assembly pAssembly, string pName)
        {
            var fname = $"{pAssembly.GetName().Name}.{pName}";

            using (MemoryStream ms = new MemoryStream())
            {
                var strm = pAssembly.GetManifestResourceStream(fname);
                strm.CopyTo(ms);
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        public static string ToHexa(this byte[] pData)
        {
            if (pData.IsEmpty())
                return null;
            StringBuilder hex = new StringBuilder(pData.Length * 2);
            foreach (byte b in pData)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString().SafeUpper();
        }

        public static string SerializeString(object pObject)
        {
            return Encoding.UTF8.GetString(Serialize(pObject));
        }

        public static Byte[] Serialize(object pObject)
        {
            if (pObject == null)
                return Encoding.UTF8.GetBytes("{}");
            using (MemoryStream ms = new MemoryStream())
            using (StreamWriter sr = new StreamWriter(ms))
            using (JsonTextWriter _Writer = new JsonTextWriter(sr))
            {
                sr.AutoFlush = true;
                JsonSerializer js = new JsonSerializer();
                js.Serialize(_Writer, pObject);
                ms.Position = 0;
                return ms.ToArray();
            }
        }

        public static void Deserialize(object pObject, string pData)
        {
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(pData)))
            using (TextReader sr = new StreamReader(ms))
            using (JsonTextReader _Reader = new JsonTextReader(sr))
            {
                JsonSerializer js = new JsonSerializer();
                js.Populate(_Reader, pObject);
            }
        }

        public static T Deserialize<T>(string pData)
        {
            return Deserialize<T>(Encoding.UTF8.GetBytes(pData));
        }

        public static T Deserialize<T>(Byte[] pData)
        {
            var obj = typeof(T).CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(pData))
            using (TextReader sr = new StreamReader(ms))
            using (JsonTextReader _Reader = new JsonTextReader(sr))
            {
                JsonSerializer js = new JsonSerializer();
                js.Populate(_Reader, obj);
            }
            return obj;
        }

        public static string GetMd5Hash(string input)
        {
            using MD5 md5Hasher = MD5.Create();
            byte[] data = md5Hasher.ComputeHash(Encoding.UTF8.GetBytes(input));
            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);
            if (useHashing)
            {
                using MD5 hashmd5 = MD5.Create();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
                hashmd5.Clear();
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(SecurityKey);
            using TripleDES tdes = TripleDES.Create();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            if (useHashing)
            {
                using MD5 hashmd5 = MD5.Create();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(SecurityKey));
                hashmd5.Clear();
            }
            else
            {
                keyArray = UTF8Encoding.UTF8.GetBytes(SecurityKey);
            }

            using TripleDES tdes = TripleDES.Create();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Encoding.UTF8.GetString(resultArray);
        }

        public static byte[] GetExceptionDetailsBytes(Exception pException, int pMaxSize = 0)
        {
            return Encoding.UTF8.GetBytes(GetExceptionDetails(pException, pMaxSize));
        }

        public static String GetExceptionDetails(Exception pException, int pMaxSize = 0)
        {
            if (pException == null)
                return null;
            Exception ex = pException;
            StringBuilder sb = new StringBuilder();

            sb.AppendLineEx(GetExceptionMessages(pException));
            sb.AppendLineEx(GetExceptionStack(pException));
            if (pMaxSize > 0 && sb.Length > pMaxSize)
                return sb.ToString().Substring(0, pMaxSize);
            return sb.ToString();
        }

        public static String GetExceptionStack(Exception pException)
        {
            if (pException == null)
                return "";
            Exception ex = pException;
            StringBuilder sb = new StringBuilder();
            while (ex != null)
            {
                if (ex is ReflectionTypeLoadException)
                {
                    ReflectionTypeLoadException rtle = (ReflectionTypeLoadException)ex;
                    foreach (Exception item in rtle.LoaderExceptions)
                        sb.AppendLineEx(GetExceptionStack(item));
                }
                else
                {
                    sb.AppendLineEx("***********************************************");
                    sb.AppendLineEx(ex.GetType().FullName + " " + ex.Message);
                    sb.AppendLineEx("***********************************************");
                    sb.AppendLineEx(ex.StackTrace);
                }
                if (pException is SqlException sqlex)
                    foreach (SqlError e in sqlex.Errors)
                        sb.AppendLineEx(ex.Message);
                ex = ex.InnerException;
            }
            return sb.ToString();
        }

        public static String GetExceptionMessage(Exception pException)
        {
            Exception ex = pException;
            while (ex != null)
            {
                if (!(ex is XThrowContainer))
                    return ex.Message;
                ex = ex.InnerException;
            }
            return pException?.Message;
        }

        public static String GetExceptionMessages(Exception pException, Int32 pCount = 0)
        {
            Exception ex = pException;
            StringBuilder sb = new StringBuilder();
            while (ex != null)
            {
                if (!(ex is XThrowContainer))
                {
                    if (ex is ReflectionTypeLoadException)
                    {
                        ReflectionTypeLoadException rtle = (ReflectionTypeLoadException)ex;
                        foreach (Exception item in rtle.LoaderExceptions)
                            sb.Append(GetExceptionMessages(item, pCount));
                    }
                    else
                    {
                        if (ex.Message.IsEmpty())
                        {
                            ex = ex.InnerException;
                            continue;
                        }
                        if (pCount == -1)
                            sb.Append(ex.Message);
                        else
                        {
                            pCount++;
                            sb.Append(" -> " + ex.Message);
                        }
                    }
                }
                if (pException is SqlException sqlex)
                    foreach (SqlError e in sqlex.Errors)
                        sb.Append(" -> " + e.Message);
                ex = ex.InnerException;
            }
            return sb.ToString();
        }

        public static string ByteToBase64(byte[] pValue)
        {
            if (pValue.IsEmpty())
                return null;
            return Convert.ToBase64String(pValue);
        }

        public static byte[] Base64Byte(string pValue)
        {
            if (pValue.IsEmpty())
                return null;
            return Convert.FromBase64String(pValue);
        }

    }
}
