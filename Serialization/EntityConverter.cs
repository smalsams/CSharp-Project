﻿using SamSer.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace SamSer.Serialization
{
    public class EntityConverter : JsonConverter<IEntity>
    {
        public override bool CanWrite => false;
        public override IEntity ReadJson(JsonReader reader, Type objectType, IEntity existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var jsonObject = JObject.Load(reader);
            var type = jsonObject["Class"].Value<Type>();
            if (!type.IsAssignableFrom(typeof(IEntity)))
            {
                throw new NotSupportedException();
            }
            var entity = (IEntity)Activator.CreateInstance(type);
            return entity;

        }

        public override void WriteJson(JsonWriter writer, IEntity value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
