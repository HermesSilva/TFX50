using System;
using System.Collections.Generic;

namespace TFX.Core
{
	public class XEnvironment
	{
        public static IServiceProvider Services;

        public const String NewLine = "\r\n";
#if DEBUG
		public static bool IsDebug = true;
        public static bool AtivarScalar = true;
#else
        public static bool AtivarScalar = false;
		public static bool IsDebug = false;
#endif
        public static T Read<T>(string varName, T defaultValue = default, string varEmptyError = null)
			where T : IComparable, IConvertible
		{
			if (varName.IsEmpty())
				throw new ArgumentNullException(nameof(varName));
			bool hasDefault = !EqualityComparer<T>.Default.Equals(defaultValue, default);
			var value = Environment.GetEnvironmentVariable(varName);

			if (value.IsEmpty())
				return defaultValue;

			var type = typeof(T);
			if (type.IsEnum)
			{
				if (Enum.TryParse(type, value.ToString(), out object ret))
					return (T)ret;
			}
			T result = (T)Convert.ChangeType(value, typeof(T));
			if (!EqualityComparer<T>.Default.Equals(result, defaultValue))
				return result;
			return defaultValue;

		}
	}
}
