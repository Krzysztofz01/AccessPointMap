using Ganss.XSS;
using Microsoft.AspNetCore.Http;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace AccessPointMap.API.Converters
{
    public class AntiXssConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var sanitizer = new HtmlSanitizer();

            var rawProperty = reader.GetString();

            var sanitizedProperty = sanitizer.Sanitize(rawProperty);

            if (rawProperty == sanitizedProperty) return sanitizedProperty;

            throw new BadHttpRequestException("XSS injection detected.");
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
