using Newtonsoft.Json;
using System;

namespace MyFoodDoc.App.Application.Serialization
{
    public class TimespanConverter : JsonConverter<TimeSpan?>
    {
        /// <summary>
        /// Format: Days.Hours:Minutes:Seconds:Milliseconds
        /// </summary>
        public const string TimeSpanFormatString = @"hh\:mm";

        public override void WriteJson(JsonWriter writer, TimeSpan? value, JsonSerializer serializer)
        {
            var timespanFormatted = $"{value?.ToString(TimeSpanFormatString)}";
            writer.WriteValue(timespanFormatted);
        }

        public override TimeSpan? ReadJson(JsonReader reader, Type objectType, TimeSpan? existingValue, bool hasExistingValue, JsonSerializer serializer)
        {
            var value = reader.Value;
            if (value != null)
            {
                TimeSpan.TryParseExact((string)value, TimeSpanFormatString, null, out TimeSpan parsedTimeSpan);
                return parsedTimeSpan;
            }
            return null;
        }
    }
}
