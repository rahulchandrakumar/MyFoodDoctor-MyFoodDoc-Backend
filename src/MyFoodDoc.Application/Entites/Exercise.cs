using MyFoodDoc.Application.Entites.Abstractions;
using System;

namespace MyFoodDoc.Application.Entites
{
    public class Exercise : AbstractDiaryEntity<string>
    {
        public int Duration { get; set; }

        public TimeSpan LastAdded { get; set; }
    }
}