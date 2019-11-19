using System;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class LiquidPayload
    {
        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public int Amount { get; set; }
    }
}