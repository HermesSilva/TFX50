using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TFX.Core.Extensions
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class OpenApiAttribute : Attribute
    {
    }

    // Remove propriedades com atributo [OpenApiRequestIgnore] apenas no RequestBody.
    // Abordagem simples: qualquer propriedade marcada é removida de QUALQUER schema, independente do tipo.
    public sealed class HideRequestIgnoredPropertiesDocumentTransformer : IOpenApiDocumentTransformer
    {
        private HashSet<string> _hiddenPropertyNames;

        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            if (document?.Paths == null)
                return Task.CompletedTask;
            
            _hiddenPropertyNames = BuildHiddenPropertyNames();
            
            foreach (var path in document.Paths.Values)
            {
                foreach (var op in path.Operations.Values)
                {
                    var rb = op.RequestBody;
                    if (rb?.Content == null)
                        continue;
                    foreach (var kv in rb.Content.ToList())
                    {
                        var schema = kv.Value?.Schema;
                        if (schema == null)
                            continue;
                        var visited = new HashSet<OpenApiSchema>(ReferenceEqualityComparer<OpenApiSchema>.Instance);
                        kv.Value.Schema = CloneFiltered(schema, document, visited);
                    }
                }
            }

            return Task.CompletedTask;
        }

        private static HashSet<string> BuildHiddenPropertyNames()
        {
            var names = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try { types = asm.GetTypes(); } catch { continue; }
                foreach (var t in types)
                {
                    if (!t.IsClass)
                        continue;
                    var props = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance)
                                  .Where(p => Attribute.IsDefined(p, typeof(OpenApiAttribute), inherit: true));
                    foreach (var p in props)
                    {
                        names.Add(p.Name);
                        var lowerFirst = LowerFirst(p.Name);
                        if (!string.Equals(lowerFirst, p.Name, StringComparison.Ordinal))
                            names.Add(lowerFirst);
                    }
                }
            }
            return names;
        }

        private OpenApiSchema CloneFiltered(OpenApiSchema src, OpenApiDocument doc, HashSet<OpenApiSchema> visited)
        {
            if (src == null)
                return null;
            if (!visited.Add(src))
                return src; // evita ciclos mantendo referência

            // Resolve componente de referência, se houver
            OpenApiSchema target = src;
            if (src.Reference?.Id is string rid && doc.Components?.Schemas != null && doc.Components.Schemas.TryGetValue(rid, out var comp))
            {
                target = comp;
            }

            var clone = new OpenApiSchema
            {
                Type = target.Type,
                Format = target.Format,
                Nullable = target.Nullable,
                Title = target.Title,
                Description = target.Description,
                Deprecated = target.Deprecated,
                MaxItems = target.MaxItems,
                MinItems = target.MinItems,
                MaxLength = target.MaxLength,
                MinLength = target.MinLength,
                Pattern = target.Pattern,
                AdditionalPropertiesAllowed = target.AdditionalPropertiesAllowed,
                Default = target.Default,
                Example = target.Example
            };

            if (target.Enum?.Any() == true)
                foreach (var e in target.Enum)
                    clone.Enum.Add(e);

            // Items
            if (target.Items != null)
                clone.Items = CloneFiltered(target.Items, doc, visited);

            // AllOf / AnyOf / OneOf
            if (target.AllOf?.Any() == true)
                clone.AllOf = target.AllOf.Select(s => CloneFiltered(s, doc, visited)).Where(s => s != null).ToList();
            if (target.AnyOf?.Any() == true)
                clone.AnyOf = target.AnyOf.Select(s => CloneFiltered(s, doc, visited)).Where(s => s != null).ToList();
            if (target.OneOf?.Any() == true)
                clone.OneOf = target.OneOf.Select(s => CloneFiltered(s, doc, visited)).Where(s => s != null).ToList();

            // Properties: simplesmente remove se o nome estiver no conjunto global
            if (target.Properties?.Any() == true)
            {
                clone.Properties = new Dictionary<string, OpenApiSchema>();
                foreach (var kv in target.Properties)
                {
                    var propName = kv.Key;
                    if (_hiddenPropertyNames.Contains(propName))
                        continue; // oculta no request
                    var child = CloneFiltered(kv.Value, doc, visited);
                    if (child != null)
                        clone.Properties[propName] = child;
                }
            }

            // Additional properties
            if (target.AdditionalProperties is OpenApiSchema aps)
                clone.AdditionalProperties = CloneFiltered(aps, doc, visited);

            return clone;
        }

        private static string LowerFirst(string name)
        {
            if (string.IsNullOrEmpty(name)) return name;
            if (char.IsLower(name[0])) return name;
            return char.ToLowerInvariant(name[0]) + name.Substring(1);
        }
        
        private sealed class ReferenceEqualityComparer<T> : IEqualityComparer<T> where T : class
        {
            public static readonly ReferenceEqualityComparer<T> Instance = new ReferenceEqualityComparer<T>();
            public bool Equals(T x, T y) => ReferenceEquals(x, y);
            public int GetHashCode(T obj) => System.Runtime.CompilerServices.RuntimeHelpers.GetHashCode(obj);
        }
    }
}

