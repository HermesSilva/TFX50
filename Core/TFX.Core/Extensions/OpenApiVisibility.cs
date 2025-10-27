using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi.Models;

namespace TFX.Core.Extensions
{
    public enum OpenApiVisibility
    {
        /// <summary>Padrão: visível em Request e Response</summary>
        Default = 0,
        /// <summary>Oculto apenas no Request (payload)</summary>
        HiddenInRequest = 1,
        /// <summary>Oculto apenas no Response (resultado)</summary>
        HiddenInResponse = 2,
        /// <summary>Oculto em Request e Response</summary>
        Hidden = 3
    }

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class OpenApiAttribute : Attribute
    {
        public OpenApiVisibility Visibility { get; set; } = OpenApiVisibility.Default;
        public string Description { get; set; }
    }

    // Aplica visibilidade e descrições definidas por [OpenApi] em propriedades
    public sealed class ApplyOpenApiAttributesDocumentTransformer : IOpenApiDocumentTransformer
    {
        private Dictionary<string, OpenApiPropertyMetadata> _propertyMetadata;

        public Task TransformAsync(OpenApiDocument document, OpenApiDocumentTransformerContext context, CancellationToken cancellationToken)
        {
            if (document?.Paths == null)
                return Task.CompletedTask;
            
            _propertyMetadata = BuildPropertyMetadata();
            
            // Aplica descrições nos componentes (compartilhados)
            if (document.Components?.Schemas != null)
            {
                foreach (var kv in document.Components.Schemas)
                {
                    ApplyDescriptions(kv.Value);
                }
            }
            
            // Aplica visibilidade nos requests e responses
            foreach (var path in document.Paths.Values)
            {
                foreach (var op in path.Operations.Values)
                {
                    // REQUEST: oculta HiddenInRequest e Hidden
                    if (op.RequestBody?.Content != null)
                    {
                        foreach (var kv in op.RequestBody.Content.ToList())
                        {
                            var schema = kv.Value?.Schema;
                            if (schema == null)
                                continue;
                            var visited = new HashSet<OpenApiSchema>(ReferenceEqualityComparer<OpenApiSchema>.Instance);
                            kv.Value.Schema = CloneFiltered(schema, document, visited, filterRequest: true);
                        }
                    }
                    
                    // RESPONSE: oculta HiddenInResponse e Hidden
                    foreach (var response in op.Responses.Values)
                    {
                        if (response.Content == null)
                            continue;
                        foreach (var kv in response.Content.ToList())
                        {
                            var schema = kv.Value?.Schema;
                            if (schema == null)
                                continue;
                            var visited = new HashSet<OpenApiSchema>(ReferenceEqualityComparer<OpenApiSchema>.Instance);
                            kv.Value.Schema = CloneFiltered(schema, document, visited, filterRequest: false);
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }

        private static Dictionary<string, OpenApiPropertyMetadata> BuildPropertyMetadata()
        {
            var metadata = new Dictionary<string, OpenApiPropertyMetadata>(StringComparer.OrdinalIgnoreCase);
            
            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
            {
                Type[] types;
                try { types = asm.GetTypes(); } catch { continue; }
                
                foreach (var t in types)
                {
                    if (!t.IsClass)
                        continue;
                    
                    var props = t.GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);
                    foreach (var p in props)
                    {
                        var attr = p.GetCustomAttributes(typeof(OpenApiAttribute), inherit: true)
                                    .OfType<OpenApiAttribute>()
                                    .FirstOrDefault();
                        if (attr == null)
                            continue;
                        
                        var meta = new OpenApiPropertyMetadata
                        {
                            Visibility = attr.Visibility,
                            Description = attr.Description
                        };
                        
                        // Registra tanto PascalCase quanto camelCase
                        metadata[p.Name] = meta;
                        var lowerFirst = LowerFirst(p.Name);
                        if (!string.Equals(lowerFirst, p.Name, StringComparison.Ordinal))
                            metadata[lowerFirst] = meta;
                    }
                }
            }
            return metadata;
        }

        private void ApplyDescriptions(OpenApiSchema schema)
        {
            if (schema?.Properties == null)
                return;
            
            foreach (var kv in schema.Properties)
            {
                var propName = kv.Key;
                if (_propertyMetadata.TryGetValue(propName, out var meta) && !string.IsNullOrWhiteSpace(meta.Description))
                {
                    kv.Value.Description = meta.Description;
                }
            }
        }

        private OpenApiSchema CloneFiltered(OpenApiSchema src, OpenApiDocument doc, HashSet<OpenApiSchema> visited, bool filterRequest)
        {
            if (src == null)
                return null;
            if (!visited.Add(src))
                return src; // evita ciclos

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
                clone.Items = CloneFiltered(target.Items, doc, visited, filterRequest);

            // AllOf / AnyOf / OneOf
            if (target.AllOf?.Any() == true)
                clone.AllOf = target.AllOf.Select(s => CloneFiltered(s, doc, visited, filterRequest)).Where(s => s != null).ToList();
            if (target.AnyOf?.Any() == true)
                clone.AnyOf = target.AnyOf.Select(s => CloneFiltered(s, doc, visited, filterRequest)).Where(s => s != null).ToList();
            if (target.OneOf?.Any() == true)
                clone.OneOf = target.OneOf.Select(s => CloneFiltered(s, doc, visited, filterRequest)).Where(s => s != null).ToList();

            // Properties: aplica regra de visibilidade
            if (target.Properties?.Any() == true)
            {
                clone.Properties = new Dictionary<string, OpenApiSchema>();
                foreach (var kv in target.Properties)
                {
                    var propName = kv.Key;
                    
                    // Verifica se deve ocultar baseado no contexto (request ou response)
                    if (_propertyMetadata.TryGetValue(propName, out var meta))
                    {
                        bool mustHide = false;
                        if (filterRequest)
                        {
                            // Filtrando Request: oculta HiddenInRequest e Hidden
                            mustHide = meta.Visibility == OpenApiVisibility.HiddenInRequest || 
                                      meta.Visibility == OpenApiVisibility.Hidden;
                        }
                        else
                        {
                            // Filtrando Response: oculta HiddenInResponse e Hidden
                            mustHide = meta.Visibility == OpenApiVisibility.HiddenInResponse || 
                                      meta.Visibility == OpenApiVisibility.Hidden;
                        }
                        
                        if (mustHide)
                            continue;
                    }
                    
                    var child = CloneFiltered(kv.Value, doc, visited, filterRequest);
                    if (child != null)
                        clone.Properties[propName] = child;
                }
            }

            // Additional properties
            if (target.AdditionalProperties is OpenApiSchema aps)
                clone.AdditionalProperties = CloneFiltered(aps, doc, visited, filterRequest);

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
        
        private sealed class OpenApiPropertyMetadata
        {
            public OpenApiVisibility Visibility { get; set; }
            public string Description { get; set; }
        }
    }
}
