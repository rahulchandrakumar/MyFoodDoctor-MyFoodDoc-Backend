namespace MyFoodDoc.Application.Entities.Diary
{
    public class MealIngredient
    {
        public int MealId { get; set; }

        public int IngredientId { get; set; }

        public decimal Amount { get; set; }

        public Meal Meal { get; set; }

        public Ingredient Ingredient { get; set; }
    }
}