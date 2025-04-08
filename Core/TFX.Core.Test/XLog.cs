using System.Diagnostics;

namespace System
{
    public static class XLog
    {
        private static Object _ToLock = new Object();

        private static bool _HasError;
        public static int Verbosity = 9;

        public static Boolean HasError => _HasError;

        public static Exception LastException
        {
            get; internal set;
        }

        public static void ClearError()
        {
            _HasError = false;
        }

        public static void AddException(Exception pEX)
        {
            lock (_ToLock)
                if (LastException != null)
                    LastException = new AggregateException(LastException, pEX);
                else
                    LastException = pEX;
            _HasError = true;
            Debug.WriteLine(pEX.Message);
            Debug.WriteLine(pEX.StackTrace);
        }

        public static void AddLog(string pMessage, int pVerbosity = 9)
        {
            if (Verbosity >= pVerbosity)
                Debug.WriteLine(pMessage);
        }
    }
}
