using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace luftborn.Presistance.Infrastructure
{
    public class JsonSerializationContractResolver : DefaultContractResolver
    {
        private Type _typeToIgnore;
        public JsonSerializationContractResolver(Type typeToIgnore)
        {
            _typeToIgnore = typeToIgnore;
        }
        protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
        {
            IList<JsonProperty> properties = base.CreateProperties(type, memberSerialization);

            properties = properties.Where(p => p.PropertyType != _typeToIgnore).ToList();

            return properties;
        }
    }
}
