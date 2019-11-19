using MyFoodDoc.Application.Entites.Abstractions;
using System;

namespace MyFoodDoc.Application.Entites
{
    public class Liquid : AbstractDiaryEntity<string>
    {
        public int Amount { get; set; }

        public TimeSpan LastAdded { get; set; }
    }
}