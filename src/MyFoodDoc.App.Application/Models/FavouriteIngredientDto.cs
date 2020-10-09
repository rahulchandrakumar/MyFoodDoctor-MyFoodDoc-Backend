using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MyFoodDoc.Application.Entities.Diary;

namespace MyFoodDoc.App.Application.Models
{
    public class FavouriteIngredientDto
    {
        public DiaryEntryDtoIngredient Ingredient { get; set; }

        public decimal Amount { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(FavouriteIngredient), GetType());
    }
}
