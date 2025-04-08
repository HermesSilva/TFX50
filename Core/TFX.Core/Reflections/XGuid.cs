using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

namespace TFX.Core.Reflections
{
	public class XGuidAttribute : Attribute
	{
		public XGuidAttribute(string pID, Type pType)
		{
			ID = new Guid( pID);
			Type = pType;
		}

		public Type Type
		{
			get;
		}

		public Guid ID
		{
			get;
		}
	}
}
