using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.OpenApi.Models;

using Swashbuckle.AspNetCore.SwaggerGen;

namespace TFX.Core.Extensions
{
    public class EnumSchemaFilter : ISchemaFilter
    {
        private static XDocument _XmlComments;

        public EnumSchemaFilter(string xmlPath)
        {
            if (_XmlComments == null && File.Exists(xmlPath))
                _XmlComments = XDocument.Load(xmlPath);
        }

        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            if (context.Type.IsEnum)
            {
                var enumType = context.Type;
                var enumNames = Enum.GetNames(enumType);
                var summaries = new List<string>();

                foreach (var enumName in enumNames)
                {
                    var memberName = $"F:{enumType.FullName}.{enumName}";
                    var summary = _XmlComments.Descendants("member")
                        .FirstOrDefault(m => m.Attribute("name")?.Value == memberName)?
                        .Descendants("summary").FirstOrDefault()?.Value.Trim();

                    summaries.Add(summary ?? enumName);
                }

                schema.Description += "\n\nValores:\n" + string.Join("\n", summaries.Select((s, i) => $"- {enumNames[i]}: {s}"));
            }
        }
    }
}
