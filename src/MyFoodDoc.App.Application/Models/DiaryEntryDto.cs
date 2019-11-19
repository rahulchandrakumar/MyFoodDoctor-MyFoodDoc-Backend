using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Models
{
    public class DiaryEntryDto
    {
        public ICollection<DiaryEntryDtoMeal> Meals { get; set; }

        public DiaryEntryDtoExercise Exercise { get; set; }

        public DiaryEntryDtoLiquid Liquid { get; set; }
    }
}