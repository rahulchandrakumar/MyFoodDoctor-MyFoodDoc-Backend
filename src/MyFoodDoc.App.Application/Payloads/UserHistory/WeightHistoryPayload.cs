using System;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class WeightHistoryPayload
    {
        public DateTime Date { get; set; }

        public decimal Value { get; set; }
    }
}