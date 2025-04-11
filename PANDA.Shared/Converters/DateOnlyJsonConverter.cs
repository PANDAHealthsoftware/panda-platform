using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace PANDA.Shared.Converters
{
    public class DateOnlyJsonConverter : JsonConverter<DateTime>
    {
        private const string DateFormat = "dd/MM/yyyy";

        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var value = reader.GetString();

            // Accept ISO format first
            if (DateTime.TryParse(value, out var parsed))
                return parsed;

            // Fallback to UK format
            return DateTime.ParseExact(value!, DateFormat, CultureInfo.InvariantCulture);
        }
        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString("O")); // "O" = ISO 8601 round-trip format
        }
    }
}