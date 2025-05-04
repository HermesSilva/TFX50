using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using TFX.Core.IDs.Model;
using TFX.Core.Model.APP;
using TFX.Core.Model.Payload;

namespace TFX.Core.Cache
{
    public static class XMainCache
    {

        private static readonly Dictionary<Guid, Type> _Cache = new Dictionary<Guid, Type>();

        public static void Add<T>(Guid pID)
        {
            lock (_Cache)
                _Cache.Add(pID, typeof(T));
        }

        public static T Create<T>(Guid pID)
        {
            lock (_Cache)
            {
                _Cache.TryGetValue(pID, out Type tp);
                return tp.CreateInstance<T>();
            }
        }

        internal static async Task<XAPPModel> GetModel(XAppPayload pPayload)
        {
            var mdl = Create<XAPPModel>(pPayload.ID);
            return await Task.FromResult(mdl);
        }
    }
}
