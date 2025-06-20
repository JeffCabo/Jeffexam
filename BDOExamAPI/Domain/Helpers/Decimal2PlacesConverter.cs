﻿using System.Globalization;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace BDOExamAPI.Domain.Helpers
{
    public class Decimal2PlacesConverter : JsonConverter<decimal>
    {
        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            => throw new NotSupportedException("This converter is for serialization only.");

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString("0.00", CultureInfo.InvariantCulture));
    }
}