using System;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class ExercisePayload
    {
        public DateTime Date { get; set; }

        public TimeSpan Time { get; set; }

        public int Duration { get; set; }
    }
}