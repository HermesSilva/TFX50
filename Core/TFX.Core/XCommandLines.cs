using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

using TFX.Core.Exceptions;


namespace TFX.Core
{
    public class XCommandLines
    {
        public static XCommandLines Parse(String[] pArgs)
        {
            XCommandLines cl = new XCommandLines();
            cl._OrinalArgs = pArgs;
            if (pArgs.IsFull())
            {
                StringBuilder argvlr = new StringBuilder();
                String arg = null;
                String lastarg = null;
                for (int i = 0; i < pArgs.Length; i++)
                {
                    lastarg = pArgs[i];
                    if (lastarg.StartsWith("-"))
                    {
                        if (arg.IsFull())
                            cl._Args[arg.SafeUpper()] = argvlr.ToString().SafeTrim();
                        argvlr = new StringBuilder();
                        arg = lastarg;
                        continue;
                    }
                    else
                        argvlr.Append(lastarg + " ");
                    if (i == 0)
                    {
                        cl._Errors.AppendLineEx("Todo argumento deve começar com [-] e separados por espaço.");
                        break;
                    }
                }

                if (arg.IsFull() && arg.StartsWith("-") && !cl._Args.ContainsKey(arg.SafeUpper()))
                    cl._Args[arg.SafeUpper()] = argvlr.ToString().SafeTrim();
            }
            return cl;
        }

        private StringBuilder _Errors = new StringBuilder();
        private Dictionary<String, String> _Args = new Dictionary<String, String>();
        private List<String> _RequiredArgValue = new List<String>();
        private List<String> _RequiredArg = new List<String>();
        private List<String> _KnownArg = new List<String>();
        private String[] _OrinalArgs;

        public void AddKnown(String pArg, Boolean pRequired = false, Boolean pRequiredArgValue = false)
        {
            if (pRequiredArgValue && !_RequiredArgValue.Contains(pArg.SafeUpper()))
                _RequiredArgValue.Add(pArg.SafeUpper());
            if (pRequired && !_RequiredArg.Contains(pArg.SafeUpper()))
                _RequiredArg.Add(pArg.SafeUpper());
            if (!_KnownArg.Contains("-" + pArg.SafeUpper()))
                _KnownArg.Add("-" + pArg.SafeUpper());
        }

        public T Get<T>(String pArg)
        {
            String vlr = GetValue(pArg);
            switch (typeof(T).Name)
            {
                case "DateTime":
                    if (vlr.SafeLength() != 10 && vlr.SafeLength() != 19)
                        throw new XError($"No argumento [{pArg}] é esperado, encontrado [{vlr}],  uma data válida nos formatos [dd/MM/yyyy] ou [dd/MM/yyyy HH:mm:ss].");

                    if (vlr.SafeLength() == 10)
                        return (T)(Object)DateTime.ParseExact(vlr, "dd/MM/yyyy", null);
                    return (T)(Object)DateTime.ParseExact(vlr, "dd/MM/yyyy HH:mm:ss", null);

                default:
                    return (T)Convert.ChangeType(vlr, typeof(T));
            }
        }

        public Boolean IsOk
        {
            get
            {
                foreach (String arg in _Args.Keys)
                    if (!KnownContains(arg))
                        _Errors.AppendLineEx($"O Parâmetro [{arg}] não é conhecido.");
                foreach (String arg in _RequiredArgValue)
                    if (Contains(arg) && GetValue(arg).IsEmpty())
                        _Errors.AppendLineEx($"O Parâmetro [{arg}] deve ter argumento.");
                foreach (String arg in _RequiredArg)
                    if (!Contains(arg))
                        _Errors.AppendLineEx($"O Parâmetro [{arg}] é obrigatório.");
                if (_Errors.Length > 0)
                {
                    Console.WriteLine("ERROR DE PARÂMETROS");
                    StringReader sr = new StringReader(_Errors.ToString());
                    while (true)
                    {
                        String str = sr.ReadLine();
                        if (str.IsEmpty())
                            break;
                        Console.WriteLine(str);
                    }
                }
                return _Errors.Length == 0;
            }
        }

        public String[] OrinalArgs => _OrinalArgs;

        public void ShowAllParameters(String pName = null)
        {
            if (pName.IsFull())
                Console.WriteLine($"Show Params Name[{pName}]");
            if (_Args.Count == 0)
                Console.WriteLine($"No Params");
            foreach (KeyValuePair<String, String> item in _Args)
                Console.WriteLine($"Param=[{item.Key}] Value=[{item.Value}]");
        }

        public String GetValue(String pArg)
        {
            if (pArg.IsEmpty())
                return null;
            pArg = pArg.SafeUpper();
            if (_Args.ContainsKey("-" + pArg))
                return _Args["-" + pArg];
            if (_Args.ContainsKey(pArg))
                return _Args[pArg];
            return null;
        }

        public Boolean KnownContains(String pArg)
        {
            pArg = pArg.SafeUpper();
            return !pArg.IsEmpty() && _KnownArg.Contains(pArg);
        }

        public Boolean Contains(String pArg)
        {
            pArg = pArg.SafeUpper();
            return !pArg.IsEmpty() && _Args.ContainsKey("-" + pArg);
        }
    }
}
