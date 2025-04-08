using System;
using System.IO;
using System.Threading;

namespace TFX.Core.Test.Browsers
{
    public static class XWSUtils
    {
        private static Random _Rnd = new Random(DateTime.Now.Second);
        private static string _AppPath;
        public static void WaitHumanTick()
        {
            Thread.Sleep(_Rnd.Next(100, 500));
        }

        public static void WaitHumanClickTick()
        {
            Thread.Sleep(_Rnd.Next(500, 800));
        }

        public static String AppPath
        {
            get
            {
                if (!_AppPath.IsFull())
                {
                    String loc = typeof(XWSUtils).Assembly.Location;
                    _AppPath = Path.GetDirectoryName(loc);
                }
                return _AppPath;
            }
        }

    }
}
