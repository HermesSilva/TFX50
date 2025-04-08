using System;
using System.Collections.Generic;

using TFX.Core.IDs.Model;

namespace TFX.Core.Cache
{
    public static class XSessionCache
    {
        private static Dictionary<String, XUserSession> _Users = new Dictionary<String, XUserSession>();

        private static Dictionary<Guid, XUserSession> _Cache = new Dictionary<Guid, XUserSession>();

        public static int Count => _Cache.Count;

        public static XUserSession AddSession(XUserSession pLogin, Boolean pIndexName = false)
        {
            lock (_Cache)
            {
                _Cache.TryAdd(pLogin.SessionID, pLogin);
                if (pIndexName)
                    _Users.TryAdd(pLogin.Login, pLogin);
            }
            return pLogin;
        }

        public static void Wash()
        {
            lock (_Cache)
                _Cache.Clear();
        }

        public static XUserSession GetSession(Guid pSessionID)
        {
            lock (_Cache)
            {
                _Cache.TryGetValue(pSessionID, out XUserSession login);
                return login;
            }
        }
        public static XUserSession GetSession(String pLogin)
        {
            lock (_Cache)
            {
                _Users.TryGetValue(pLogin, out XUserSession login);
                return login;
            }
        }

        public static void Invalidate(Guid pSessionID)
        {
            lock (_Cache)
                _Cache.Remove(pSessionID);
        }
    }
}
