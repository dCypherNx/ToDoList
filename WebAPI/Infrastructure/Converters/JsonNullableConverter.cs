using System.Reflection;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Diagnostics.CodeAnalysis;

namespace WebAPI.Infrastructure.Converters
{
    [ExcludeFromCodeCoverage]
    public class JsonNullableConverter : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            return Nullable.GetUnderlyingType(typeToConvert) != null;
        }

        public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            Type nullableType = Nullable.GetUnderlyingType(typeToConvert);
            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                typeof(JsonNullableConverterInner<>).MakeGenericType(new Type[] { nullableType }),
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null);

            return converter;
        }

        private class JsonNullableConverterInner<T> : JsonConverter<T?>
            where T : struct
        {
            public JsonNullableConverterInner(JsonSerializerOptions options)
            {
            }

            public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.Null)
                {
                    return null;
                }

                return JsonSerializer.Deserialize<T>(ref reader, options);
            }

            public override void Write(Utf8JsonWriter writer, T? value, JsonSerializerOptions options)
            {
                if (value.HasValue)
                {
                    JsonSerializer.Serialize(writer, value.Value, options);
                }
                else
                {
                    writer.WriteNullValue();
                }
            }
        }
    }
}
