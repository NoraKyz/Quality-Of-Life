using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using ZLinq;

namespace Quality.Core.Utilities
{
    public class IgnoreSOPropertiesResolver : DefaultContractResolver
    {
        private const string NAME_PROPERTY       = "name";
        private const string HIDE_FLAGS_PROPERTY = "hideFlags";
        
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            var properties = base.CreateProperties(type, memberSerialization);

            if (typeof(UnityEngine.ScriptableObject).IsAssignableFrom(type))
            {
                properties = properties.AsValueEnumerable()
                    .Where(p => p.PropertyName != NAME_PROPERTY && p.PropertyName != HIDE_FLAGS_PROPERTY).ToList();
            }

            return properties;
        }
    }
}
