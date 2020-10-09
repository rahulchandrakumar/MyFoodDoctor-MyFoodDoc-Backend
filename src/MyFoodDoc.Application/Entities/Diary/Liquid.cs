using MyFoodDoc.Application.Entities.Abstractions;
using System;

namespace MyFoodDoc.Application.Entities.Diary
{
    public class Liquid : AbstractDiaryEntity<string>
    {
        public int Amount { get; set; }

        public TimeSpan LastAdded { get; set; }
    }
}