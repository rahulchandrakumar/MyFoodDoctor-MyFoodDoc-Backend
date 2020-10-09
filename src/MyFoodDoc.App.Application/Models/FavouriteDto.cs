using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using MyFoodDoc.Application.Entities.Diary;

namespace MyFoodDoc.App.Application.Models
{
    public class FavouriteDto
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public decimal Fat { get; set; }

        public decimal Protein { get; set; }

        public decimal Sugar { get; set; }

        public decimal Carbohydrate { get; set; }

        public ICollection<FavouriteIngredientDto> Ingredients { get; set; }

        public void Mapping(Profile profile) => profile.CreateMap(typeof(Favourite), GetType());
    }
}
