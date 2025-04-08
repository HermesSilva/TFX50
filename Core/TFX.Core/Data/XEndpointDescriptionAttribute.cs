
using Microsoft.AspNetCore.Http.Metadata;

using System;
using System.Collections.Generic;
using System.Linq;

using TFX.Core;

namespace TFX.Core.Data
{
    [AttributeUsage(AttributeTargets.Method)]
    public class XEndpointDescriptionAttribute : Attribute, IEndpointDescriptionMetadata
    {
        public string Description
        {
            get;
        }

        public XEndpointDescriptionAttribute(params Type[] pTypes)
        {
            Description = string.Join("\r\n", XEntity.GetRules(pTypes).AsString().SafeBreak(Environment.NewLine).Select(l => $"<br>&#8226;{l}"));
        }

    }
}
