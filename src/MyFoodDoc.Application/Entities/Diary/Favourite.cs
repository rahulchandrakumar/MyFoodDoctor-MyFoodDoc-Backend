using System;
using System.Collections.Generic;
using System.Text;
using MyFoodDoc.Application.Abstractions;

namespace MyFoodDoc.Application.Entities.Diary
{
    public class Favourite : AbstractAuditableEntity
    {
        public string UserId { get; set; }

        public string Title { get; set; }

        public bool IsGeneric { get; set; }

        public User User { get; set; }

        public ICollection<FavouriteIngredient> Ingredients { get; set; }

        public ICollection<MealFavourite> Meals { get; set; }
    }
}
