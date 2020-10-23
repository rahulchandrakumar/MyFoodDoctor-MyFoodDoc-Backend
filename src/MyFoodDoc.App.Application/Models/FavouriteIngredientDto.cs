using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities.Diary;

namespace MyFoodDoc.App.Application.Models
{
    public class FavouriteIngredientDto : IMapFrom<FavouriteIngredient>
    {
        public IngredientDto Ingredient { get; set; }

        public decimal Amount { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(FavouriteIngredient), GetType());
    }
}
