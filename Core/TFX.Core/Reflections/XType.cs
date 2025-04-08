using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace TFX.Core.Reflections
{
	public class XType
	{
		public XType(Guid pID, Type pType, Type pTypeEx = null)
		{
			ID = pID;
			Type = pType;
			TypeEx = pTypeEx;
		}

		public Type Type
		{
			get;
		}
		public Type TypeEx
		{
			get;
		}
		public Guid ID
		{
			get;
		}
	}
}
