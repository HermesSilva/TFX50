using System;

namespace TFX.Core.Model
{
	public abstract class XJobConfig
	{
		public int Interval
		{
			get; set;
		}
		public Guid ID
		{
			get; set;
		}
	}
}
