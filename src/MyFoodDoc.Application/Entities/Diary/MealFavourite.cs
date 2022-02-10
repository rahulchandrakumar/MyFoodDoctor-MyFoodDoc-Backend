namespace MyFoodDoc.Application.Entities.Diary
{
    public class MealFavourite
    {
        public int MealId { get; set; }

        public int FavouriteId { get; set; }

        public Meal Meal { get; set; }

        public Favourite Favourite { get; set; }
    }
}
