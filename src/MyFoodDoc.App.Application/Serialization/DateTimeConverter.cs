using Newtonsoft.Json.Converters;

namespace MyFoodDoc.App.Application.Serialization
{
    public class DateTimeConverter : IsoDateTimeConverter
    {
        /// <summary>
        /// Format: Year-Month-Day
        /// </summary>
        public const string DateTimeFormatString = @"yyyy-MM-dd";

        public DateTimeConverter()
        {
            base.DateTimeFormat = DateTimeFormatString;
        }
    }
}
