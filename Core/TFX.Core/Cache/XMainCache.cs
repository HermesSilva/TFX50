using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using TFX.Core.IDs.Model;
using TFX.Core.Model.APP;
using TFX.Core.Model.Payload;
using TFX.Core.Model.Service;
using TFX.Core.Services;

namespace TFX.Core.Cache
{
    public static class XMainCache
    {

        private static readonly Dictionary<Guid, Type> _Cache = new Dictionary<Guid, Type>();
        public static readonly Dictionary<Guid, XAPPModel> Apps = new Dictionary<Guid, XAPPModel>();

        public static void Add<T>(Guid pID) where T : class, new()
        {
            lock (_Cache)
            {
                var tp = typeof(T);
                _Cache.Add(pID, tp);
                if (tp.Implemnts<XAPPModel>())
                    Apps.Add(pID, new T() as XAPPModel);
            }
        }

        public static T Create<T>(Guid pID)
        {
            lock (_Cache)
            {
                _Cache.TryGetValue(pID, out Type tp);
                return tp.CreateInstance<T>();
            }
        }

        internal static async Task<XAPPModel> GetAppModel(XModelPayload pPayload)
        {
            var mdl = Create<XAPPModel>(pPayload.ID);
            return await Task.FromResult(mdl);
        }

        internal static async Task<XServiceModel> GetServiceModel(XModelPayload pPayload)
        {
            var mdl = Create<XServiceModel>(pPayload.ID);
            return await Task.FromResult(mdl);
        }
    }
}
