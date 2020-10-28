using System;
using System.Collections.Generic;
using System.Text;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class MealFavouritePayload : FavouritePayload
    {
        public int? Id { get; set; }
    }
}
