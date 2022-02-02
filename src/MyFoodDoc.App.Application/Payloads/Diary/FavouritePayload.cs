using System.Collections.Generic;

namespace MyFoodDoc.App.Application.Payloads.Diary
{
    public class FavouritePayload
    {
        public string Title { get; set; }

        public IEnumerable<IngredientPayload> Ingredients { get; set; }
    }
}
