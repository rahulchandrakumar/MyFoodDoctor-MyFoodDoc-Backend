using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MyFoodDoc.App.Application.Mappings;
using MyFoodDoc.Application.Entities.Diary;

namespace MyFoodDoc.App.Application.Models
{
    public class MealFavouriteDto: IMapFrom<MealFavourite>
    {
        public FavouriteDto Favourite { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(MealFavourite), GetType());
    }
}
