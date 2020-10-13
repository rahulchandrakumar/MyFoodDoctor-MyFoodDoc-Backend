using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.Application.Entities.Diary
{
    public class FavouriteIngredient
    {
        public int FavouriteId { get; set; }

        public int IngredientId { get; set; }

        public decimal Amount { get; set; }

        public Favourite Favourite { get; set; }

        public Ingredient Ingredient { get; set; }
    }
}
