using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

using Castle.Components.DictionaryAdapter;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;

using TFX.Core.IDs.Model;
using TFX.Core.Model;
using TFX.Core.Reflections;
using TFX.Core.Services;

namespace TFX.Core.Cache
{
	public static class XCacheManager
	{
		private static bool _Initialized;
		private static Dictionary<Guid, XJobConfig> _JobCache = new Dictionary<Guid, XJobConfig>();
		private static Dictionary<Guid, List<XType>> _ConfigTypeCache = new Dictionary<Guid, List<XType>>();

		public static void Initialize()
		{
			LoadTypes();
			WashCaches();
			_Initialized = true;
		}

		private static void LoadTypes()
		{
			foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
			{
				foreach (var tp in asm.GetTypes())
				{
					var att = tp.GetCustomAttribute<XGuidAttribute>(true);
					if (att == null)
						continue;
					if (!_ConfigTypeCache.TryGetValue(att.ID, out List<XType> lst))
					{
						lst = new List<XType>();
						_ConfigTypeCache.Add(att.ID, lst);
					}
					lst.Add(new XType(att.ID, tp, att.Type));
				}
			}
		}

		public static IEnumerable<XType> GetTypes<T>()
		{
			foreach (var key in _ConfigTypeCache)
				foreach (var tp in key.Value.Where(t => t.TypeEx.Implemnts<T>()))
					yield return tp;
		}


		public static XJobConfig GetConfig(Guid pID)
		{
			lock (_JobCache)
			{
				_JobCache.TryGetValue(pID, out XJobConfig config);
				return config;
			}
		}

		public static List<XType> GetTypes(Guid pID)
		{
			_ConfigTypeCache.TryGetValue(pID, out var lst);
			return lst;
		}


		private static void WashCaches()
		{
			lock (_JobCache)
			{
				_JobCache.Clear();
				//using (var cfgs = XService.GetService<IConfiguracaoJobService>())
				//{
				//	var dst = cfgs.Select(true);
				//	foreach (var tpl in dst.Tuples)
				//	{
				//		var cfgtype = GetTypes(tpl.CORxJobID.Value).FirstOrDefault(t => t.Type.Implemnts<XJobConfig>());
				//		if (cfgtype == null)
				//			continue;
				//		var cfg = cfgtype.Type.CreateInstance<XJobConfig>();
				//		tpl.Dados.Value.Deserialize(cfg);
				//		_JobCache.Add(tpl.CORxJobID.Value, cfg);
				//	}
				//}
			}
		}
	}
}
