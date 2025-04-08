using System.Collections.Generic;

using TFX.Core.IDs.Model;

namespace TFX.Core.Access.Service
{
    public class XCacheUser
    {
        private static Dictionary<string, XUser> _Users = new Dictionary<string, XUser>();

        public static int Count => _Users.Count;

        public void Swap(Dictionary<string, XUser> pUsers)
        {
            lock (this)
                _Users = pUsers;
        }

        public XUser Add(XUser pUser)
        {
            lock (this)
                _Users.TryAdd(pUser.Login, pUser);
            return pUser;
        }

        public void Wash()
        {
            lock (this)
                _Users.Clear();
        }

        public XUser GetUser(string pLogin)
        {
            lock (this)
            {
                _Users.TryGetValue(pLogin, out XUser user);
                return user;
            }
        }

        public void Remove(string pLogin)
        {
            lock (this)
                _Users.Remove(pLogin);
        }
    }
}
