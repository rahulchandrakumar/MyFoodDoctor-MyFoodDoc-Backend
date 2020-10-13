using MyFoodDoc.Application.Entities.Abstractions;
using System;

namespace MyFoodDoc.Application.Entities.Diary
{
    public class Exercise : AbstractDiaryEntity<string>
    {
        public int Duration { get; set; }

        public TimeSpan LastAdded { get; set; }
    }
}