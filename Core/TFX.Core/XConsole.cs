using System;
using System.Diagnostics;


namespace TFX.Core
{
    public static class XConsole
    {
        public static Boolean IsEnabled => true;
        public static String SNow
        {
            get
            {
                return DateTime.Now.ToString(XDefault.FullDateTimeFormat).PadRight(27, '0');
            }
        }

        [Conditional("DEBUG")]
        public static void Write(String pText, ConsoleColor pForeground = ConsoleColor.White, ConsoleColor pBackground = ConsoleColor.Black)
        {
            Console.BackgroundColor = pBackground;
            Console.ForegroundColor = pForeground;
            Console.Write(pText);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = ConsoleColor.White;
        }

        [Conditional("DEBUG")]
        internal static void WritePause(String pValue)
        {
            Console.WriteLine(pValue);
            Console.ReadLine();
        }

        [Conditional("DEBUG")]
        public static void WriteLine(String pText, ConsoleColor pForeground = ConsoleColor.White, ConsoleColor pBackground = ConsoleColor.Black)
        {
            XConsole.Write(pText, pForeground, pBackground);
            Console.WriteLine();
        }

        [Conditional("DEBUG")]
        public static void WriteLine(params Object[] pData)
        {
            InternalWriteLine(pData);
        }

        private static void InternalWriteLine(params object[] pData)
        {
            if (!XConsole.IsEnabled)
                return;
            foreach (Object obj in pData)
            {
                if (obj is ConsoleColor)
                    Console.ForegroundColor = (ConsoleColor)obj;
                else
                    Console.Write(obj);
            }
            Console.WriteLine();
        }

        [Conditional("DEBUG")]
        public static void Write(params Object[] pData)
        {
            if (!XConsole.IsEnabled)
                return;
            foreach (Object obj in pData)
            {
                if (obj is ConsoleColor)
                    Console.ForegroundColor = (ConsoleColor)obj;
                else
                    Console.Write(obj);
            }
        }

        [Conditional("DEBUG")]
        public static void WriteLine()
        {
            if (!XConsole.IsEnabled)
                return;
            Console.WriteLine();
        }

        private static String MSG(Object pValue)
        {
            String msg = SNow;
            if (pValue is Exception)
                msg = msg + " - " + XUtils.GetExceptionStack((Exception)pValue);
            else
                msg = msg + " - " + pValue;
            return msg;
        }

        public static void Error(String pMessage, Exception pEx = null)
        {
            InternalWriteLine(ConsoleColor.DarkRed, MSG(pMessage + " " + XUtils.GetExceptionStack(pEx)), ConsoleColor.White);
        }

        public static void Error(Exception pEx)
        {
            InternalWriteLine(ConsoleColor.DarkRed, MSG(pEx), ConsoleColor.White);
        }

        public static void Warn(Object pMSG)
        {
            InternalWriteLine(ConsoleColor.Yellow, MSG(pMSG), ConsoleColor.White);
        }

        [Conditional("DEBUG")]
        public static void Debug(params Object[] pMSG)
        {
            if (pMSG.IsFull())
                pMSG.ForEach(v => WriteLine(ConsoleColor.White, MSG(v), ConsoleColor.White));
        }

        [Conditional("DEBUG")]
        public static void Hint(params Object[] pMSG)
        {
            if (pMSG.IsFull())
                pMSG.ForEach(v => WriteLine(ConsoleColor.White, MSG(v), ConsoleColor.White));
        }
    }
}
