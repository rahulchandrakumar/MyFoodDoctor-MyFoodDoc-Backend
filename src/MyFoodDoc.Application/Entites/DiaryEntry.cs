using MyFoodDoc.Application.Abstractions;
using System.Collections.Generic;

namespace MyFoodDoc.Application.Entites
{
    public class DiaryEntry : AbstractAuditEntity<int>
    {
        public ICollection<DiaryMeal> Meals { get; set; }

        public DiaryExercise Exercise { get; set; }

        public DiaryLiquid Liquid { get; set; }
    }
}