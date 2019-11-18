using System.Collections.Generic;
using AutoMapper;
using MyFoodDoc.Core.Mappings;
using Entity = MyFoodDoc.Application.Entites;

namespace MyFoodDoc.Api.Models
{
    public class DiaryEntry
    {
        public ICollection<DiaryMeal> Meals { get; set; }

        public DiaryExercise Exercise { get; set; }

        public DiaryLiquid Liquid { get; set; }
    }
}